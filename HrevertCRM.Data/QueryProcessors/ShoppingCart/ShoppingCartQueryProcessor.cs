using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.ShoppingCart;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.Mapper.Company;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.ShoppingCartViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ShoppingCartQueryProcessor : QueryBase<ShoppingCart>, IShoppingCartQueryProcessor
    {
        private readonly IProductQueryProcessor _productQueryProcessor;
        private readonly IShoppingCartDetailQueryProcessor _shoppingCartDetailQueryProcessor;
        private readonly IFiscalPeriodQueryProcessor _fiscalPeriodQueryProcessor;
        private readonly IAddressQueryProcessor _addressQueryProcessor;
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;
        private readonly IEmailSenderQueryProcessor _emailSenderQueryProcessor;
        private readonly ISalesOrderQueryProcessor _salesOrderQueryProcessor;

        public ShoppingCartQueryProcessor(IUserSession userSession, IDbContext dbContext,
            IProductQueryProcessor productQueryProcessor,
            IShoppingCartDetailQueryProcessor shoppingCartDetailQueryProcessor,
            IFiscalPeriodQueryProcessor fiscalPeriodQueryProcessor,
            IAddressQueryProcessor addressQueryProcessor,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor,
            IEmailSenderQueryProcessor emailSenderQueryProcessor,
            ISalesOrderQueryProcessor salesOrderQueryProcessor) : base(userSession, dbContext)
        {
            _productQueryProcessor = productQueryProcessor;
            _shoppingCartDetailQueryProcessor = shoppingCartDetailQueryProcessor;
            _fiscalPeriodQueryProcessor = fiscalPeriodQueryProcessor;
            _addressQueryProcessor = addressQueryProcessor;
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
            _emailSenderQueryProcessor = emailSenderQueryProcessor;
            _salesOrderQueryProcessor = salesOrderQueryProcessor;
        }

        public ShoppingCartViewModel AddProductToCart(ShoppingCartDetail detailLine)
        {
            var shoppingCartMapper = new ShoppingCartToShoppingCartViewModelMapper();
            detailLine.CompanyId = LoggedInUser.CompanyId;
            if (detailLine.ShoppingCartId == 0 || (detailLine.CustomerId == 0 || detailLine.CustomerId == null))
            {
                //CustomerId ra shoppingCartId null cha vane ShoppingCart create garera, ShoppingCartDetail save garne
                if ((detailLine.CustomerId == 0 || detailLine.CustomerId == null) && detailLine.ShoppingCartId == 0)
                {
                    var sCart = new ShoppingCart
                    {
                        CompanyId = LoggedInUser.CompanyId,
                        Active = true
                    };
                    var shoppingCart = Save(sCart);
                    detailLine.ShoppingCartId = shoppingCart.Id;
                    _dbContext.Set<ShoppingCartDetail>().Add(detailLine);
                }
                //Chainna vane, Guid bata Shopping Cart Details tanne ra tesma customerId ko value update garne ani Guid null garne
                else
                {
                    ShoppingCart shoppingCart;
                    if (detailLine.ShoppingCartId == 0)
                    {
                        var sCart = new ShoppingCart
                        {
                            CompanyId = LoggedInUser.CompanyId,
                            Active = true
                        };
                        shoppingCart = Save(sCart);
                        detailLine.ShoppingCartId = shoppingCart.Id;
                    }
                    else
                    {
                        shoppingCart =
                        _dbContext.Set<ShoppingCart>().Include(x => x.ShoppingCartDetails).AsNoTracking().FirstOrDefault(x => x.Id == detailLine.ShoppingCartId);
                    }

                    if (detailLine.CustomerId != null)
                    {
                        var cartDetails =
                        _dbContext.Set<ShoppingCartDetail>().AsNoTracking().Where(x => x.Guid == detailLine.Guid && x.CustomerId == null).ToList();
                        if (cartDetails != null && cartDetails.Count > 0)
                        {
                            foreach (var shoppingCartDetail in cartDetails)
                            {
                                shoppingCartDetail.CustomerId = detailLine.CustomerId;
                                shoppingCartDetail.Guid = null;
                                shoppingCart.ShoppingCartDetails.Add(shoppingCartDetail);
                            }
                        }
                    }

                    // If the added cartDetail is already in the cart, we increment the quantity of the product
                    // rather than creating a new cartDetail
                    #region Increasing the Quantity of already existing Product
                    var isDetailGotModified = false;

                    if (shoppingCart.ShoppingCartDetails != null && shoppingCart.ShoppingCartDetails.Count > 0)
                    {
                        foreach (var cartDetail in shoppingCart.ShoppingCartDetails)
                        {
                            if (cartDetail.ProductId != detailLine.ProductId) continue;
                            cartDetail.Quantity = detailLine.Quantity + cartDetail.Quantity;
                            _shoppingCartDetailQueryProcessor.Update(cartDetail);
                            isDetailGotModified = true;
                            break;
                        }
                        if (isDetailGotModified)
                        {
                            var modifiedCart = shoppingCartMapper.Map(
                            _dbContext.Set<ShoppingCart>().AsNoTracking()
                                .Include(x => x.ShoppingCartDetails)
                                .SingleOrDefault(x => x.Id == detailLine.ShoppingCartId));
                            if (modifiedCart.ShoppingCartDetails == null || modifiedCart.ShoppingCartDetails.Count <= 0) return modifiedCart;

                            foreach (var detail in modifiedCart.ShoppingCartDetails)
                            {
                                var product = _productQueryProcessor.GetProduct(detail.ProductId);
                                detail.ProductName = product.Name;
                                detail.ProductCode = product.Code;
                                detail.ProductImageUrls = product.ProductMetadatas.Select(x => x.MediaUrl).ToList();
                            }
                            return modifiedCart;
                        }
                    }
                    #endregion

                    _dbContext.Set<ShoppingCartDetail>().Add(detailLine);
                }
            }
            else
            {
                var shoppingCart = _dbContext.Set<ShoppingCart>().Include(inc => inc.ShoppingCartDetails).AsNoTracking()
                    .FirstOrDefault(x => x.Id == detailLine.ShoppingCartId);

                var cartDetails =
                        _dbContext.Set<ShoppingCartDetail>().Where(x => x.Guid == detailLine.Guid && x.CustomerId == null).AsNoTracking().ToList();
                if (cartDetails != null && cartDetails.Count > 0)
                {
                    foreach (var shoppingCartDetail in cartDetails)
                    {
                        shoppingCartDetail.CustomerId = detailLine.CustomerId;
                        shoppingCartDetail.Guid = null;
                        shoppingCart.ShoppingCartDetails.Add(shoppingCartDetail);
                    }
                }

                // If the added cartDetail is already in the cart, we increment the quantity of the product
                // rather than creating a new cartDetail
                #region Increasing the Quantity of already existing Product
                var isDetailGotModified = false;

                if (shoppingCart.ShoppingCartDetails != null && shoppingCart.ShoppingCartDetails.Count > 0)
                {
                    foreach (var cartDetail in shoppingCart.ShoppingCartDetails)
                    {
                        if (cartDetail.ProductId != detailLine.ProductId) continue;
                        cartDetail.Quantity = detailLine.Quantity + cartDetail.Quantity;
                        _shoppingCartDetailQueryProcessor.Update(cartDetail);
                        isDetailGotModified = true;
                        break;
                    }
                    if (isDetailGotModified)
                    {
                        var modifiedCart = shoppingCartMapper.Map(
                            _dbContext.Set<ShoppingCart>().AsNoTracking()
                                .Include(x => x.ShoppingCartDetails)
                                .SingleOrDefault(x => x.Id == detailLine.ShoppingCartId));
                        if (modifiedCart.ShoppingCartDetails == null || modifiedCart.ShoppingCartDetails.Count <= 0) return modifiedCart;

                        foreach (var detail in modifiedCart.ShoppingCartDetails)
                        {
                            var product = _productQueryProcessor.GetProduct(detail.ProductId);
                            detail.ProductName = product.Name;
                            detail.ProductCode = product.Code;
                            detail.ProductImageUrls = product.ProductMetadatas.Select(x => x.MediaUrl).ToList();
                        }
                        return modifiedCart;
                    }
                }
                #endregion

                // Else, we save the cartDetail
                _dbContext.Set<ShoppingCartDetail>().Add(detailLine);
            }
            _dbContext.SaveChanges();

            var cart =  shoppingCartMapper.Map(_dbContext.Set<ShoppingCart>().Include(x => x.ShoppingCartDetails).AsNoTracking().FirstOrDefault(x => x.Id == detailLine.ShoppingCartId));
            if (cart.ShoppingCartDetails == null || cart.ShoppingCartDetails.Count <= 0) return cart;

            foreach (var detail in cart.ShoppingCartDetails)
            {
                var product = _productQueryProcessor.GetProduct(detail.ProductId);
                detail.ProductName = product.Name;
                detail.ProductCode = product.Code;
                detail.ProductImageUrls = product.ProductMetadatas.Select(x => x.MediaUrl).ToList();
            }
            return cart;
        }

        public bool Delete(int shoppingCartId)
        {
            var shoppingCartDelete = _dbContext.Set<ShoppingCart>().FirstOrDefault(sc => sc.Id == shoppingCartId);
            _dbContext.Set<ShoppingCart>().Remove(shoppingCartDelete);
            _dbContext.SaveChanges();
            return false;
        }

        public bool Exists(Expression<Func<ShoppingCart, bool>> where)
        {
            return _dbContext.Set<ShoppingCart>().Any(@where);
        }

        public ShoppingCart Get(int shoppingCartId)
        {
            var shoppingCart = _dbContext.Set<ShoppingCart>().FirstOrDefault(d => d.Id == shoppingCartId);
            return shoppingCart;
        }

        public ShoppingCart GetAll(int customerId)
        {
            var shoppingCart = _dbContext.Set<ShoppingCart>().FirstOrDefault(d => d.CustomerId == customerId);
            return shoppingCart;
        }

        public bool RemoveProductFromCart(int id)
        {
            var removedProduct = _dbContext.Set<ShoppingCartDetail>().FirstOrDefault(sc => sc.Id == id);
            if(removedProduct != null)
                    _dbContext.Set<ShoppingCartDetail>().Remove(removedProduct);
            return _dbContext.SaveChanges() > 1;
        }

        public ShoppingCart Save(ShoppingCart shoppingCart)
        {
            shoppingCart.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ShoppingCart>().Add(shoppingCart);
            _dbContext.SaveChanges();
            return shoppingCart;
        }

        public ShoppingCart Update(ShoppingCart shoppingCart)
        {
            var original = GetValidShoppingCartOrder(shoppingCart.Id);
            ValidateAuthorization(shoppingCart);

            original.CustomerId = shoppingCart.CustomerId;
            original.Amount = shoppingCart.Amount;
            original.HostIp = shoppingCart.HostIp;
            original.IsCheckedOut = shoppingCart.IsCheckedOut;
            original.BillingAddressId = shoppingCart.BillingAddressId;
            original.ShippingAddressId = shoppingCart.ShippingAddressId;
            //original.DeliveryMethodId = shoppingCart.DeliveryMethodId;
            //original.PaymentTermId = shoppingCart.PaymentTermId;
            original.Active = shoppingCart.Active;
            original.WebActive = shoppingCart.WebActive;
            original.CompanyId = shoppingCart.CompanyId;

            _dbContext.Set<ShoppingCart>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual ShoppingCart GetValidShoppingCartOrder(int shoppingCartId)
        {
            var shoppingCartOrder = _dbContext.Set<ShoppingCart>().FirstOrDefault(sc => sc.Id == shoppingCartId);
            if (shoppingCartOrder == null)
                throw new RootObjectNotFoundException(ShoppingCartConstants.ShoppingCartQueryProcessorConstants.ShoppingCartNotFound);
            return shoppingCartOrder;
        }

        public virtual ShoppingCartDetail GetValidShoppingCartDetailOrder(int id)
        {
            var shoppingCartDetailOrder = _dbContext.Set<ShoppingCartDetail>().FirstOrDefault(sc => sc.Id == id);
            if (shoppingCartDetailOrder == null)
                throw new RootObjectNotFoundException(ShoppingCartConstants.ShoppingCartDetailQueryProcessorConstants.ShoppingCartDetailNotFound);
            return shoppingCartDetailOrder;
        }
        public ShoppingCartDetailViewModel UpdateCartDetail(ShoppingCartDetail detailLine)
        {
            var original = GetValidShoppingCartDetailOrder(detailLine.Id);
            ValidateAuthorization(original);

            original.CustomerId = detailLine.CustomerId;
            original.ProductId = detailLine.ProductId;
            original.ProductCost = detailLine.ProductCost;
            original.Quantity = detailLine.Quantity;
            original.Discount = detailLine.Discount;
            original.ShoppingCartId = detailLine.ShoppingCartId;
            original.TaxAmount = detailLine.TaxAmount;
            original.WebActive = detailLine.WebActive;
            original.Active = detailLine.Active;

            _dbContext.Set<ShoppingCartDetail>().Update(original);
            _dbContext.SaveChanges();
            var mapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();
            var cartViewModel = mapper.Map(original);
            cartViewModel.ProductsRefByAssembledAndKit = GetProductsRefByAssembledAndKit(mapper.Map(original),
                detailLine.ShoppingCartId);
            return cartViewModel;
        }

        public virtual ShoppingCart GetValidShoppingCart(int shoppingCartId)
        {
            var shoppingCart = _dbContext.Set<ShoppingCart>().FirstOrDefault(sc => sc.Id == shoppingCartId);
            if (shoppingCart == null)
                throw new RootObjectNotFoundException(ShoppingCartConstants.ShoppingCartQueryProcessorConstants.ShoppingCartNotFound);
            return shoppingCart;
        }

        public ShoppingCartViewModel GetShoppingCartViewModel(int shoppingCartId, int customerId)
        {
            var shoppingCartViewModel = new ShoppingCartViewModel();
            ShoppingCart shoppingCart;
            var cartDetailViewModelHolderList = new List<ShoppingCartDetail>();
            var cartDetailMapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();
            var cartMapper = new ShoppingCartToShoppingCartViewModelMapper();

            if (shoppingCartId == 0)
            {
                var pendingCartDetails =
                    _dbContext.Set<ShoppingCartDetail>().AsNoTracking()
                        .Where(x => x.Active && x.CompanyId == LoggedInUser.CompanyId && x.CustomerId == customerId).ToList();

                if (pendingCartDetails != null && pendingCartDetails.Count > 0)
                {
                    var sCart = new ShoppingCart
                    {
                        CompanyId = LoggedInUser.CompanyId,
                        Active = true
                    };

                    var tempShoppingCart = Save(sCart);
                    shoppingCartViewModel = cartMapper.Map(tempShoppingCart);
                    shoppingCartViewModel.ShoppingCartDetails = new List<ShoppingCartDetailViewModel>();

                    foreach (var pendingCartDetail in pendingCartDetails)
                    {
                        if (
                            _dbContext.Set<ShoppingCart>()
                                .AsNoTracking()
                                .SingleOrDefault(x => x.Id == pendingCartDetail.ShoppingCartId) == null ||
                            _dbContext.Set<ShoppingCart>()
                                .AsNoTracking()
                                .SingleOrDefault(x => x.Id == pendingCartDetail.ShoppingCartId)
                                .IsCheckedOut) continue;
                        pendingCartDetail.ShoppingCartId = sCart.Id;
                        shoppingCartViewModel.ShoppingCartDetails.Add(cartDetailMapper.Map(pendingCartDetail));
                        _dbContext.Set<ShoppingCartDetail>().Update(pendingCartDetail);
                    }
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                shoppingCart = _dbContext.Set<ShoppingCart>().Include(x => x.ShoppingCartDetails).FirstOrDefault(d => d.Id == shoppingCartId && d.Active && d.CompanyId == LoggedInUser.CompanyId);
                if (shoppingCart.ShoppingCartDetails != null && shoppingCart.ShoppingCartDetails.Count > 0)
                {
                    if (customerId != 0)
                    {
                        foreach (var cartDetail in shoppingCart.ShoppingCartDetails.ToList())
                        {
                            cartDetail.CustomerId = customerId;
                            cartDetail.Guid = null;
                        }

                        var pendingCartDetails = _dbContext.Set<ShoppingCartDetail>().Where(x => x.Active && x.CompanyId == LoggedInUser.CompanyId && x.CustomerId == customerId && x.ShoppingCartId != shoppingCart.Id);
                        if (pendingCartDetails != null)
                        {
                            foreach (var pendingCartDetail in pendingCartDetails)
                            {
                                var cart =
                                    _dbContext.Set<ShoppingCart>()
                                        .AsNoTracking()
                                        .FirstOrDefault(
                                            x =>
                                                x.Id == pendingCartDetail.ShoppingCartId &&
                                                x.CompanyId == LoggedInUser.CompanyId);
                                if (cart == null || cart.IsCheckedOut)
                                    continue;
                                var isDuplicatedItemFound = false;
                                if (shoppingCart.ShoppingCartDetails != null &&
                                    shoppingCart.ShoppingCartDetails.Count > 0)
                                {
                                    foreach (var cartDetail in shoppingCart.ShoppingCartDetails)
                                    {
                                        if (cartDetail.ProductId != pendingCartDetail.ProductId) continue;
                                        if (cartDetail.Id == pendingCartDetail.Id)
                                        {
                                            cartDetail.Quantity += pendingCartDetail.Quantity;
                                            _dbContext.Set<ShoppingCartDetail>().Update(cartDetail);
                                        }

                                        else
                                        {
                                            cartDetail.Quantity += pendingCartDetail.Quantity;
                                            _dbContext.Set<ShoppingCartDetail>().Update(cartDetail);
                                            pendingCartDetail.Active = false;
                                            _dbContext.Set<ShoppingCartDetail>().Update(pendingCartDetail);
                                        }
                                        isDuplicatedItemFound = true;
                                        break;
                                    }
                                }
                                if (!isDuplicatedItemFound)
                                {
                                    cartDetailViewModelHolderList.Add(pendingCartDetail);
                                }
                            }
                        }
                    }
                    shoppingCartViewModel = cartMapper.Map(shoppingCart);
                    shoppingCartViewModel.ShoppingCartDetails.AddRange(cartDetailViewModelHolderList.Select(x => cartDetailMapper.Map(x)));
                    shoppingCart.ShoppingCartDetails.ToList().ForEach(x => x.ShoppingCartId = shoppingCartId);
                    cartDetailViewModelHolderList.ForEach(x => x.ShoppingCartId = shoppingCartId);
                    _dbContext.SaveChanges();
                }
            }
            if (shoppingCartViewModel.ShoppingCartDetails == null || shoppingCartViewModel.ShoppingCartDetails.Count <= 0) return null;

            foreach (var detail in shoppingCartViewModel.ShoppingCartDetails)
            {
                var product = _productQueryProcessor.GetProduct(detail.ProductId);
                var productMapper = new ProductToProductViewModelMapper();
                var productViewModelsList = new List<ProductViewModel>() { productMapper.Map(product) };
                var productViewModels = _productQueryProcessor.GetProductsWithDiscount(customerId, productViewModelsList);
                if (productViewModels != null && productViewModels.Count > 0)
                    detail.Discount = (decimal)(productViewModels[0].DiscountPrice);
                detail.ProductName = product.Name;
                detail.ProductCode = product.Code;
                detail.ProductType = product.ProductType;
                detail.ProductImageUrls = product.ProductMetadatas.Select(x => x.MediaUrl).ToList();
                if (product.ProductMetadatas != null)
                    detail.ProductImageUrls = product.ProductMetadatas.Select(x => x.MediaUrl).ToList();
                if (detail.ProductType != ProductType.Regular)
                    detail.ProductsRefByAssembledAndKit = GetProductsRefByAssembledAndKit(detail, shoppingCartId);
                GetTaxesOfDetail(detail, shoppingCartId);
            }
            return shoppingCartViewModel;
        }

        public ShoppingCartViewModel GetCartViewModel(int shoppingCartId, int customerId, Guid guid)
        {
            var shoppingCartViewModel = new ShoppingCartViewModel();
            ShoppingCart shoppingCart;
            var cartDetailViewModelHolderList = new List<ShoppingCartDetail>();
            var cartDetailMapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();
            var cartMapper = new ShoppingCartToShoppingCartViewModelMapper();

            if (shoppingCartId == 0)
            {
                var pendingCartDetails =
                    _dbContext.Set<ShoppingCartDetail>().AsNoTracking()
                        .Where(x => x.Active && x.CompanyId == LoggedInUser.CompanyId && x.CustomerId == customerId).ToList();

                if (pendingCartDetails != null && pendingCartDetails.Count > 0)
                {
                    var sCart = new ShoppingCart
                    {
                        CompanyId = LoggedInUser.CompanyId,
                        Active = true
                    };

                    var tempShoppingCart = Save(sCart);
                    shoppingCartViewModel = cartMapper.Map(tempShoppingCart);
                    shoppingCartViewModel.ShoppingCartDetails = new List<ShoppingCartDetailViewModel>();

                    foreach (var pendingCartDetail in pendingCartDetails)
                    {
                        if (
                            _dbContext.Set<ShoppingCart>()
                                .AsNoTracking()
                                .SingleOrDefault(x => x.Id == pendingCartDetail.ShoppingCartId) == null ||
                            _dbContext.Set<ShoppingCart>()
                                .AsNoTracking()
                                .SingleOrDefault(x => x.Id == pendingCartDetail.ShoppingCartId)
                                .IsCheckedOut) continue;
                        pendingCartDetail.ShoppingCartId = sCart.Id;
                        shoppingCartViewModel.ShoppingCartDetails.Add(cartDetailMapper.Map(pendingCartDetail));
                        _dbContext.Set<ShoppingCartDetail>().Update(pendingCartDetail);
                    }
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                shoppingCart = _dbContext.Set<ShoppingCart>().Include(x => x.ShoppingCartDetails).FirstOrDefault(d => d.Id == shoppingCartId && d.Active && d.CompanyId == LoggedInUser.CompanyId);
                if (shoppingCart.ShoppingCartDetails != null && shoppingCart.ShoppingCartDetails.Count > 0)
                {
                    if (customerId != 0)
                    {
                        foreach (var cartDetail in shoppingCart.ShoppingCartDetails.ToList())
                        {
                            cartDetail.CustomerId = customerId;
                            cartDetail.Guid = null;
                        }

                        var pendingCartDetails = _dbContext.Set<ShoppingCartDetail>().Where(x => x.Active && x.CompanyId == LoggedInUser.CompanyId && x.CustomerId == customerId && x.ShoppingCartId != shoppingCart.Id);
                        if (pendingCartDetails != null)
                        {
                            foreach (var pendingCartDetail in pendingCartDetails)
                            {
                                var cart =
                                    _dbContext.Set<ShoppingCart>()
                                        .AsNoTracking()
                                        .FirstOrDefault(
                                            x =>
                                                x.Id == pendingCartDetail.ShoppingCartId &&
                                                x.CompanyId == LoggedInUser.CompanyId);
                                if (cart == null || cart.IsCheckedOut)
                                    continue;
                                var isDuplicatedItemFound = false;
                                if (shoppingCart.ShoppingCartDetails != null &&
                                    shoppingCart.ShoppingCartDetails.Count > 0)
                                {
                                    foreach (var cartDetail in shoppingCart.ShoppingCartDetails)
                                    {
                                        if (cartDetail.ProductId != pendingCartDetail.ProductId) continue;
                                        if (cartDetail.Id == pendingCartDetail.Id)
                                        {
                                            cartDetail.Quantity += pendingCartDetail.Quantity;
                                            _dbContext.Set<ShoppingCartDetail>().Update(cartDetail);  
                                        }

                                        else
                                        {
                                            cartDetail.Quantity += pendingCartDetail.Quantity;
                                            _dbContext.Set<ShoppingCartDetail>().Update(cartDetail);
                                            pendingCartDetail.Active = false;
                                            _dbContext.Set<ShoppingCartDetail>().Update(pendingCartDetail);
                                        }
                                        isDuplicatedItemFound = true;
                                        break;
                                    }
                                }
                                if (!isDuplicatedItemFound)
                                {
                                    cartDetailViewModelHolderList.Add(pendingCartDetail);
                                }
                            }
                        }
                    }
                    shoppingCartViewModel = cartMapper.Map(shoppingCart);
                    shoppingCartViewModel.ShoppingCartDetails.AddRange(cartDetailViewModelHolderList.Select(x => cartDetailMapper.Map(x)));
                    shoppingCart.ShoppingCartDetails.ToList().ForEach(x => x.ShoppingCartId = shoppingCartId);
                    cartDetailViewModelHolderList.ForEach(x => x.ShoppingCartId = shoppingCartId);
                    _dbContext.SaveChanges();
                }
            }
            if (shoppingCartViewModel.ShoppingCartDetails == null || shoppingCartViewModel.ShoppingCartDetails.Count <= 0) return null;

            foreach (var detail in shoppingCartViewModel.ShoppingCartDetails)
            {
                var product = _productQueryProcessor.GetProduct(detail.ProductId);
                var productMapper = new ProductToProductViewModelMapper();
                var productViewModelsList = new List<ProductViewModel> { productMapper.Map(product) };
                var productViewModels = _productQueryProcessor.GetProductsWithDiscount(customerId, productViewModelsList);
                if (productViewModels != null && productViewModels.Count > 0)
                    detail.Discount = (decimal) (productViewModels[0].DiscountPrice);
                detail.ProductName = product.Name;
                detail.ProductCode = product.Code;
                detail.ProductType = product.ProductType;
                if(product.ProductMetadatas != null)
                    detail.ProductImageUrls = product.ProductMetadatas.Select(x => x.MediaUrl).ToList();
                if (detail.ProductType != ProductType.Regular)
                    detail.ProductsRefByAssembledAndKit = GetProductsRefByAssembledAndKit(detail, shoppingCartId);
                if (product.TaxesInProducts != null)
                    detail.Taxes = GetTaxViewModels(product.TaxesInProducts.Select(x => x.TaxId));

                GetTaxesOfDetail(detail, shoppingCartId);
            }
            return shoppingCartViewModel;
        }

        private void GetTaxesOfDetail(ShoppingCartDetailViewModel detail, int shoppingCartId)
        {
            switch (detail.ProductType)
            {
                case ProductType.Assembled:
                case ProductType.Kit:
                {
                        if (detail.ProductsRefByAssembledAndKit == null) break;
                        foreach (var cartDetail in detail.ProductsRefByAssembledAndKit)
                        {
                            var refProduct = _productQueryProcessor.GetProduct(cartDetail.ProductId);
                            cartDetail.ProductName = refProduct.Name;
                            cartDetail.ProductCode = refProduct.Code;
                            cartDetail.ProductType = refProduct.ProductType;
                            cartDetail.ProductDescription = refProduct.ShortDescription;
                            if (refProduct.ProductMetadatas != null)
                                cartDetail.ProductImageUrls = refProduct.ProductMetadatas.Select(x => x.MediaUrl).ToList();
                            if (detail.ProductType != ProductType.Regular)
                                cartDetail.ProductsRefByAssembledAndKit = GetProductsRefByAssembledAndKit(cartDetail, shoppingCartId);
                            if (refProduct.TaxesInProducts != null)
                                cartDetail.Taxes = GetTaxViewModels(refProduct.TaxesInProducts.Select(x => x.TaxId));
                            if (cartDetail.Taxes == null) continue;
                            {
                                cartDetail.TaxAndAmounts = new List<TaxNameAndAmountList>();
                                foreach (var taxViewModel in cartDetail.Taxes)
                                {
                                    cartDetail.TaxAndAmounts.Add(new TaxNameAndAmountList
                                    {
                                        TaxName = taxViewModel.TaxCode,
                                        TaxAmount = taxViewModel.TaxType == TaxCaculationType.Fixed ? taxViewModel.TaxRate : (taxViewModel.TaxRate * (decimal)refProduct.UnitPrice / 100)
                                    });
                                }
                            }
                        }
                    break;
                }
                case ProductType.Regular:
                    {
                        if (detail.Taxes == null) break;
                        var product = _productQueryProcessor.GetProduct(detail.ProductId);
                        detail.ProductName = product.Name;
                        detail.ProductCode = product.Code;
                        detail.ProductType = product.ProductType;
                        detail.ProductDescription = product.ShortDescription;
                        detail.TaxAndAmounts = new List<TaxNameAndAmountList>();
                        foreach (var taxViewModel in detail.Taxes)
                        {
                            detail.TaxAndAmounts.Add(new TaxNameAndAmountList
                            {
                                TaxName = taxViewModel.TaxCode,
                                TaxAmount = taxViewModel.TaxType == TaxCaculationType.Fixed ? taxViewModel.TaxRate : (taxViewModel.TaxRate * (decimal)product.UnitPrice / 100)
                            });
                        }
                        break;
                    }
                default:
                    // Yaha samma na aaune parne ho
                    throw new ArgumentOutOfRangeException($"Product Type Error");
            }
        }

        private List<TaxViewModel> GetTaxViewModels(IEnumerable<int> taxIds)
        {
            var taxMapper = new TaxToTaxViewModelMapper();
            var taxViewModels = new List<TaxViewModel>();
            foreach (var taxId in taxIds)
            {
                var tax =
                    _dbContext.Set<Tax>().FirstOrDefault(x => x.Active && x.CompanyId == LoggedInUser.CompanyId && x.Id == taxId);
                if (tax == null) continue;
                taxViewModels.Add(taxMapper.Map(tax));
            }
            return taxViewModels;
        }

        public SalesOrderViewModel ConvertCartToOrder(ConvertCartToOrderViewModel convertCartToOrderViewModel)
        {
            var sCart =
                _dbContext.Set<ShoppingCart>()
                    .Include(x => x.ShoppingCartDetails)
                    .ThenInclude(x => x.Product).ThenInclude(x => x.TaxesInProducts)
                    .FirstOrDefault(s => s.Id == convertCartToOrderViewModel.CartId);

            foreach (var cartDetail in sCart.ShoppingCartDetails)
            {
                cartDetail.ProductType = cartDetail.Product.ProductType;
            }
            if (sCart.ShoppingCartDetails.Count <= 0 || sCart.ShoppingCartDetails == null) return null;

            #region Code for regular and assembled types references

            var refCartDetailsList = new List<ShoppingCartDetail>();
            foreach (var cartDetail in sCart.ShoppingCartDetails)
            {
                if (cartDetail.Product.ProductType == ProductType.Assembled &&
                    cartDetail.Product.ProductType != ProductType.Kit) continue;
                var refProductIdList =
                    _dbContext.Set<ProductsRefByKitAndAssembledType>()
                        .Where(x => x.ProductId == cartDetail.ProductId)
                        .Select(x => x.ProductRefId);
                foreach (var productId in refProductIdList)
                {
                    var product = _dbContext.Set<Product>().Include(x => x.TaxesInProducts).FirstOrDefault(x => x.Id == productId);
                    var shoppingCartDetail = new ShoppingCartDetail
                    {
                        ShoppingCartId = convertCartToOrderViewModel.CartId,
                        CustomerId = convertCartToOrderViewModel.CustomerId,
                        Product = product,
                        ProductId = product.Id,
                        ProductCost = (decimal)product.UnitPrice,
                        ProductType = product.ProductType,
                        Quantity = cartDetail.Quantity,
                        Weight = cartDetail.Weight,
                        Discount = cartDetail.Discount,
                        TaxAmount = cartDetail.TaxAmount,
                        ShoppingDateTime = cartDetail.ShoppingDateTime,
                        ShippingCost = cartDetail.ShippingCost,
                        CompanyId = cartDetail.CompanyId,
                        WebActive = cartDetail.WebActive,
                        Active = cartDetail.Active,
                        Version = cartDetail.Version
                    };
                    refCartDetailsList.Add(shoppingCartDetail);
                }
            }
            foreach (var shoppingCartDetail in refCartDetailsList)
            {
                sCart.ShoppingCartDetails.Add(shoppingCartDetail);
            }
            #endregion

            var mapper = new CartToOrderMapper();
            var order = mapper.Map(sCart);

            // Calculating ShippingCost
            var shippingCostByCartDetailUnit = CalculateShippingCostByEachCartDetail(sCart, convertCartToOrderViewModel);
            var shippingCostByDocTotal = CalculateShippingCostByDocTotal(convertCartToOrderViewModel, GetTotalAmount(sCart.ShoppingCartDetails));
            var shippingCostByProductCategory = CalculateShippingCostByProductCategory(convertCartToOrderViewModel, sCart);

            //Save Order
            var savedOrder = SaveOrder(convertCartToOrderViewModel, sCart, order, shippingCostByCartDetailUnit, shippingCostByDocTotal, shippingCostByProductCategory);
            order.ShippingCost = savedOrder.ShippingCost; // We can use saveOrder in place or order below this line
            //Update Shopping Cart
            UpdateShoppingCart(sCart, order);

            //Update Quantity of Products
            var ecommerceSetting = _dbContext.Set<EcommerceSetting>()
                .FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
            UpdateQuantityOfProduct(order, ecommerceSetting);

            // Send Email
            //SendEmail(convertCartToOrderViewModel.BillingAddressId, convertCartToOrderViewModel.FileBase64);

            var salesOrderMapper = new SalesOrderToSalesOrderViewModelMapper();
            if (order.SalesOrderLines == null || order.SalesOrderLines.Count <= 0) return salesOrderMapper.Map(order);

            foreach (var salesOrderLine in order.SalesOrderLines)
            {
                var productViewModel = _productQueryProcessor.GetProductViewModel(salesOrderLine.ProductId);
                if (productViewModel == null)
                {
                    salesOrderLine.Discount = 0;
                    salesOrderLine.DiscountType = DiscountType.None;
                }
                var productViewModelWithDiscount = _productQueryProcessor.GetProductsWithDiscount(sCart.CustomerId ?? 0,
                    new List<ProductViewModel> {productViewModel});
                if (productViewModelWithDiscount == null || productViewModelWithDiscount.Count <= 0) continue;
                salesOrderLine.Discount = (decimal)productViewModelWithDiscount[0].UnitPrice - (decimal)productViewModelWithDiscount[0].DiscountPrice;
                salesOrderLine.DiscountType = productViewModelWithDiscount[0].DiscountType;
            }


            var assembledAndKitOrderLinesList = order.SalesOrderLines.Where(orderLine => orderLine.Product.ProductType != ProductType.Regular).ToList();
            if (assembledAndKitOrderLinesList == null || assembledAndKitOrderLinesList.Count <= 0)
                return salesOrderMapper.Map(order);
            foreach (var salesOrderLine in assembledAndKitOrderLinesList)
            {
                order.SalesOrderLines.Remove(salesOrderLine);
            }
            return salesOrderMapper.Map(order);
        }

        public ShoppingCartViewModel UpdateCartWithCustomerDetails(int cartId, int customerId)
        {
            var cartViewModel = GetCartViewModel(cartId, customerId, Guid.Empty);
            var billingAddressId = _dbContext.Set<Address>().FirstOrDefault(x => x.CustomerId == customerId && x.AddressType == AddressType.Billing).Id;
            var shippingAddress =
                _dbContext.Set<Address>()
                    .FirstOrDefault(
                        x => x.CustomerId == customerId && x.AddressType == AddressType.Shipping && x.IsDefault);
            cartViewModel.BillingAddressId = billingAddressId;
            cartViewModel.ShippingAddressId = shippingAddress?.Id ?? 0;
            cartViewModel.CustomerId = customerId;
            cartViewModel.CompanyId = LoggedInUser.CompanyId;
            var cartMapper = new ShoppingCartToShoppingCartViewModelMapper();
            var cart = cartMapper.Map(cartViewModel);
            Update(cart);
            return cartViewModel;
        }

        private List<ShoppingCartDetailViewModel> GetProductsRefByAssembledAndKit(ShoppingCartDetailViewModel cartDetailViewModel, int shoppingCartId)
        {
            var refCartDetailsViewModelList = new List<ShoppingCartDetailViewModel>();
            var refProductIdList =
                        _dbContext.Set<ProductsRefByKitAndAssembledType>()
                            .Where(x => x.ProductId == cartDetailViewModel.ProductId)
                            .Select(x => x.ProductRefId);
            foreach (var productId in refProductIdList)
            {
                var product = _dbContext.Set<Product>().Include(x => x.ProductMetadatas).FirstOrDefault(x => x.Id == productId);
                var shoppingCartDetail = new ShoppingCartDetailViewModel
                {
                    Id = product.Id,
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    ProductCost = (decimal)product.UnitPrice,
                    ProductType = product.ProductType,
                    Quantity = cartDetailViewModel.Quantity,
                    Weight = cartDetailViewModel.Weight,
                    Discount = cartDetailViewModel.Discount,
                    TaxAmount = cartDetailViewModel.TaxAmount,
                    ShoppingDateTime = cartDetailViewModel.ShoppingDateTime,
                    ShippingCost = cartDetailViewModel.ShippingCost,
                    CompanyId = cartDetailViewModel.CompanyId,
                    WebActive = cartDetailViewModel.WebActive,
                    Active = cartDetailViewModel.Active,
                    Version = cartDetailViewModel.Version,
                    ShoppingCartId = shoppingCartId,
                    ProductImageUrls = product.ProductMetadatas.Select(x => x.MediaUrl).ToList()
                };
                refCartDetailsViewModelList.Add(shoppingCartDetail);
            }
            return refCartDetailsViewModelList;
        }

        public void SendEmailToCustomer(SendOrderToCustomerViaEmailViewModel sendOrderToCustomerViaEmailViewModel)
        {
            if (sendOrderToCustomerViaEmailViewModel == null) return;
            var emailTo = _dbContext.Set<Address>().SingleOrDefault(x => x.Id == sendOrderToCustomerViaEmailViewModel.BillingAddressId).Email;

            const string subject = "Your Order";
            const string message = "Your order is as follows";

            _emailSenderQueryProcessor.EmailCartDetail(null, new EmailSenderViewModel
            {
                MailTo = emailTo,
                Subject = subject,
                Message = message
            }, sendOrderToCustomerViaEmailViewModel.FileBase64);
        }

        public PagedTaskDataInquiryResponse GetShoppingCarts(PagedDataRequest requestInfo, Expression<Func<ShoppingCart, bool>> @where = null)
        {
            var query = _dbContext.Set<ShoppingCart>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            if (requestInfo.Active)
                query = query.Where(x => x.Active);
            return FormatResultForPaging(requestInfo, query);
        }

        private decimal? CalculateShippingCostByProductCategory(ConvertCartToOrderViewModel convertCartToOrderViewModel, ShoppingCart cart)
        {
            // First get the CategoryId of Product and If the CategoryId of the product is more than one then
            // calculate the shipping cost for each category and return the max shipping cost after comparing.
            // Finally, after adding the shipping costs of each productCategory, we return the totalCost.

            // Also, this method returns the overall shipping cost of product categories
            var shippingAddressId = GetShippingAddressId(convertCartToOrderViewModel);
            decimal? totalcost = 0;
            var costList = new List<decimal>();
            foreach (var cartDetail in cart.ShoppingCartDetails.Where(x => x.ProductType == ProductType.Regular))
            {
                // Retrieving the categories (which may be one or more) of a product
                var listOfCategoryId =
                    _dbContext.Set<ProductInCategory>()
                        .Where(
                            pic => pic.ProductId == cartDetail.ProductId && pic.CompanyId == LoggedInUser.CompanyId)
                        .Select(x => x.CategoryId);

                foreach (var categoryId in listOfCategoryId)
                {
                    // Getting shipping cost of each category of that product, 
                    // Here, if there are more than one category of product then we return the max of them. 
                    var value = GetShippingCostOfProductCategory(categoryId, (int) cartDetail.Quantity,
                        shippingAddressId, convertCartToOrderViewModel.DeliveryMethodId) ?? (decimal)0.0;
                    // Comparing the costs, initially the categoryCost is set to 0 and the value is assigned to it
                    // If the value is greater than the previous one, we replace the value else keep the previous one.

                    //TODO: We can add all the values into a list and then get the max value
                    //categoryCost = categoryCost >= value ? categoryCost : value;

                    costList.Add(value);
                }

                var shippingCalculationType = _companyWebSettingQueryProcessor.Get(LoggedInUser.CompanyId).ShippingCalculationType;
                decimal? categoryCost;
                switch (shippingCalculationType)
                {
                    case ShippingCalculationType.Minimum:
                        categoryCost = costList.Min();
                        break;
                    case ShippingCalculationType.Maximum:
                        categoryCost = costList.Max();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                totalcost += categoryCost;
            }
            return totalcost;
        }

        private decimal? GetShippingCostOfProductCategory(int categoryId, int quantity, int shippingAddressId, int deliveryMethodId)
        {
            // This method returns the shipping cost of a single category

            var zoneId = GetZoneIdByShippingAddressId(shippingAddressId);
            // GetDeliverRateIdByProductCategoryQuantity first finds a deliverRate, and 
            // then compares the quantities and returns appropriate deliveryRate
            var deliveryRate =
                _dbContext.Set<DeliveryRate>()
                    .SingleOrDefault(x => x.DeliveryMethodId == deliveryMethodId && x.DeliveryZoneId == zoneId &&
                                          x.ProductCategoryId == categoryId &&
                                          x.Id == GetDeliveryRateIdByProductCategoryQuantity(quantity, categoryId));
            if (deliveryRate == null) return 0;
            return quantity < deliveryRate.UnitFrom ? deliveryRate.MinimumRate : deliveryRate.Rate;
        }

        private int GetDeliveryRateIdByProductCategoryQuantity(int quantity, int categoryId)
        {
            // This method first finds a deliverRate, and then compares the quantities and returns appropriate deliveryRate
            // If the quantity is less than minimum value of the deliveryRate then we return the Id of minimum rate value
            // Else if the quantity is greater than the value of maxdeliveryRate then we return the Id of the maximum rate value
            var deliveryRates = _dbContext.Set<DeliveryRate>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.ProductCategoryId == categoryId && x.Active).Select(dr => new { dr.Id, dr.UnitFrom, dr.UnitTo });
            var minDeliveryRateValue = deliveryRates.Min(x => x.UnitFrom);
            var maxDeliveryRateValue = deliveryRates.Max(x => x.UnitTo);
            var deliveryRateId = 0;

            // If the quantity lies in between min and max deliveryRate value, then we find the appropriate deliveryRate and returns its Id
            if (minDeliveryRateValue <= quantity && quantity <= maxDeliveryRateValue)
            {
                foreach (var deliveryRate in deliveryRates)
                {
                    if (deliveryRate.UnitFrom <= quantity && deliveryRate.UnitTo >= quantity)
                        deliveryRateId = deliveryRate.Id;
                }
            }
            else if (quantity < minDeliveryRateValue)
            {
                // If the quantity is less than the min deliveryRateValue,
                // then we find the Id of the deliveryRate which equals the minDeliveryRate
                foreach (var deliveryRate in deliveryRates)
                {
                    if (minDeliveryRateValue == deliveryRate.UnitFrom)
                        deliveryRateId = deliveryRate.Id;
                }
            }
            else
            {
                // If the quantity is greater than the max deliveryRateValue,
                // then we find the Id of the deliveryRate which equals the maxDeliveryRate
                foreach (var deliveryRate in deliveryRates)
                {
                    if (maxDeliveryRateValue == deliveryRate.UnitTo)
                        deliveryRateId = deliveryRate.Id;
                }
            }
            return deliveryRateId;
        }

        private SalesOrder SaveOrder(ConvertCartToOrderViewModel convertCartToOrderViewModel, ShoppingCart sCart, SalesOrder order, decimal? shippingCostByCartDetail, decimal? shippingCostByDocTotal, decimal? shippingCostByProductCategory)
        {
            var dueDatePeriod = _dbContext.Set<EcommerceSetting>()
                .FirstOrDefault(o => o.CompanyId == LoggedInUser.CompanyId && o.Active).DueDatePeriod;
            var shippingCost = GetShippingCost(shippingCostByDocTotal, shippingCostByCartDetail,
                shippingCostByProductCategory);

            order.ShippingAddressId = GetShippingAddressId(convertCartToOrderViewModel);
            order.TotalAmount = GetTotalAmount(sCart.ShoppingCartDetails) + (shippingCost ?? (decimal)0.0);
            order.ShippingCost = shippingCost;
            order.PaymentTermId = convertCartToOrderViewModel.PaymentTermId;
            order.FiscalPeriodId = _fiscalPeriodQueryProcessor.GetFiscalPeriodIdByCurrentDate();
            order.DeliveryMethodId = convertCartToOrderViewModel.DeliveryMethodId;
            order.PaymentMethodId = convertCartToOrderViewModel.PaymentMethodId;
            order.CustomerId = convertCartToOrderViewModel.CustomerId;
            order.CompanyId = LoggedInUser.CompanyId;
            order.BillingAddressId = convertCartToOrderViewModel.BillingAddressId;
            order.SalesRepId = LoggedInUser.Id;
            order.OrderType = SalesOrderType.Order;
            order.InvoicedDate = DateTime.Now;
            order.PurchaseOrderNumber = "Web Order";
            order.Status = SalesOrderStatus.SalesInvoice;
            order.IsWebOrder = true;
            order.SalesOrderCode = _salesOrderQueryProcessor.GenerateSalesOrderCode();
            order.DueDate = DateTime.Now.AddDays(dueDatePeriod);
            _dbContext.Set<SalesOrder>().Add(order);
            _dbContext.SaveChanges();
            return order;
        }

        

        private static decimal? GetShippingCost(decimal? shippingCostByDocTotal, decimal? shippingCostByCartDetail, decimal? shippingCostByProductCategory)
        {
            if (shippingCostByDocTotal == 0 && shippingCostByCartDetail == 0)
            {
                return shippingCostByProductCategory > 0 ? shippingCostByProductCategory : 0;
            }
            return shippingCostByCartDetail > shippingCostByDocTotal ? shippingCostByCartDetail : shippingCostByDocTotal;
        }

        private void UpdateShoppingCart(ShoppingCart sCart, SalesOrder order)
        {
            sCart.Amount = order.TotalAmount;
            sCart.BillingAddressId = order.BillingAddressId;
            sCart.CustomerId = order.CustomerId;
            sCart.DeliveryMethodId = order.DeliveryMethodId;
            sCart.PaymentTermId = order.PaymentTermId;
            sCart.ShippingAddressId = order.ShippingAddressId;
            sCart.IsCheckedOut = true;
            Update(sCart);
        }

        private decimal? CalculateShippingCostByDocTotal(ConvertCartToOrderViewModel convertCartToOrderViewModel, decimal totalAmount)
        {
            var shippingAddressId = GetShippingAddressId(convertCartToOrderViewModel);
            var zoneId = GetZoneIdByShippingAddressId(shippingAddressId);
            var shippingCost = _dbContext.Set<DeliveryRate>()
                .SingleOrDefault(
                    x =>
                        x.DeliveryMethodId == convertCartToOrderViewModel.DeliveryMethodId && x.DeliveryZoneId == zoneId &&
                        x.Id == GetDeliveryRateIdDocTotal(totalAmount));
            return shippingCost == null ? 0 : shippingCost.Rate;
        }

        private int GetDeliveryRateIdDocTotal(decimal totalAmount)
        {
            var deliveryRates = _dbContext.Set<DeliveryRate>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(dr => new { dr.Id, dr.DocTotalFrom, dr.DocTotalTo });
            var minDeliveryRateValue = deliveryRates.Min(x => x.DocTotalFrom);
            var maxDeliveryRateValue = deliveryRates.Max(x => x.DocTotalTo);
            var deliveryRateId = 0;

            if (minDeliveryRateValue <= totalAmount && totalAmount <= maxDeliveryRateValue)
            {
                foreach (var deliveryRate in deliveryRates)
                {
                    if (deliveryRate.DocTotalFrom <= totalAmount && deliveryRate.DocTotalTo >= totalAmount)
                    {
                        deliveryRateId = deliveryRate.Id;
                    }
                }
            }
            else if (totalAmount < minDeliveryRateValue)
            {
                foreach (var deliveryRate in deliveryRates)
                {
                    if (minDeliveryRateValue == deliveryRate.DocTotalFrom)
                        deliveryRateId = deliveryRate.Id;
                }
            }
            else
            {
                foreach (var deliveryRate in deliveryRates)
                {
                    if (maxDeliveryRateValue == deliveryRate.DocTotalTo)
                        deliveryRateId = deliveryRate.Id;
                }
            }
            return deliveryRateId;
        }

        private decimal? CalculateShippingCostByEachCartDetail(ShoppingCart sCart, ConvertCartToOrderViewModel convertCartToOrderViewModel)
        {
            var shippingAddressId = GetShippingAddressId(convertCartToOrderViewModel);
            foreach (var cartDetail in sCart.ShoppingCartDetails.Where(x => x.ProductType == ProductType.Regular))
            {
                cartDetail.ShippingCost = GetShippingCostOfCartDetail(cartDetail, shippingAddressId, convertCartToOrderViewModel.DeliveryMethodId);
            }
            return sCart.ShoppingCartDetails.Sum(x => x.ShippingCost);
        }

        private decimal? GetShippingCostOfCartDetail(ShoppingCartDetail cartDetail, int shippingAddressId, int deliveryMethodId)
        {
            var zoneId = GetZoneIdByShippingAddressId(shippingAddressId);
            var shippingCost =
                _dbContext.Set<DeliveryRate>()
                    .SingleOrDefault(
                        x =>
                            x.DeliveryMethodId == deliveryMethodId && x.DeliveryZoneId == zoneId &&
                            x.ProductId == cartDetail.ProductId && x.Id == GetDeliveryRateIdByProductQuantity((int)cartDetail.Quantity, cartDetail.ProductId));
            if (shippingCost == null) return 0;
            return cartDetail.Quantity < shippingCost.UnitFrom ? shippingCost.MinimumRate : shippingCost.Rate;
        }

        private int? GetZoneIdByShippingAddressId(int shippingAddressId)
        {
            return
                _dbContext.Set<Address>()
                    .Where(x => x.Id == shippingAddressId)
                    .Select(x => x.DeliveryZoneId)
                    .SingleOrDefault();
        }

        private void UpdateQuantityOfProduct(SalesOrder order, EcommerceSetting ecommerceSetting)
        {
            if (order.SalesOrderLines == null || order.SalesOrderLines.Count <= 0) return;
            foreach (var salesOrderLine in order.SalesOrderLines)
            {
                var product = _dbContext.Set<Product>().FirstOrDefault(x => x.Id == salesOrderLine.ProductId && x.Active && x.CompanyId == LoggedInUser.CompanyId);
                if (ecommerceSetting.DecreaseQuantityOnOrder)
                {
                    product.QuantityOnHand = product.QuantityOnHand - (int)salesOrderLine.ItemQuantity;
                }
                else
                {
                    product.QuantityOnOrder = product.QuantityOnOrder + (int)salesOrderLine.ItemQuantity;
                }
                _dbContext.Set<Product>().Update(product);
                _dbContext.SaveChanges();
            }
        }

        private int GetShippingAddressId(ConvertCartToOrderViewModel convertCartToOrderViewModel)
        {
            if (convertCartToOrderViewModel.ShippingAddressViewModel.Id != null)
                return (int)convertCartToOrderViewModel.ShippingAddressViewModel.Id;

            convertCartToOrderViewModel.ShippingAddressViewModel.CustomerId = convertCartToOrderViewModel.CustomerId;
            convertCartToOrderViewModel.ShippingAddressViewModel.AddressType = AddressType.Shipping;
            var addressMapper = new AddressToAddressViewModelMapper();
            return _addressQueryProcessor.Save(addressMapper.Map(convertCartToOrderViewModel.ShippingAddressViewModel)).Id;
        }

        private static decimal GetTotalAmount(ICollection<ShoppingCartDetail> sCartShoppingCartDetails)
        {
            if (sCartShoppingCartDetails == null) return 0;
            var totalAmount = sCartShoppingCartDetails.Aggregate<ShoppingCartDetail, decimal>(0, (current, cartDetail) => (current + cartDetail.ProductCost * cartDetail.Quantity));
            return totalAmount;
        }

        public ShoppingCart GetShoppingCart(int shoppingCartId)
        {
            var shoppingCart = _dbContext.Set<ShoppingCart>().FirstOrDefault(d => d.Id == shoppingCartId);
            return shoppingCart;
        }

        public void SaveAllShoppingCart(List<ShoppingCart> shoppingCarts)
        {
            _dbContext.Set<ShoppingCart>().AddRange(shoppingCarts);
            _dbContext.SaveChanges();
        }

        public int SaveAll(List<ShoppingCart> shoppingCarts)
        {
            _dbContext.Set<ShoppingCart>().AddRange(shoppingCarts);
            return _dbContext.SaveChanges();
        }

        public ShoppingCart ActivateShoppingCart(int id)
        {
            var original = GetValidShoppingCart(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ShoppingCart>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<ShoppingCart> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ShoppingCartToShoppingCartViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<ShoppingCartViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public ShoppingCart[] GetShoppingCarts(Expression<Func<ShoppingCart, bool>> @where = null)
        {
            var query = _dbContext.Set<ShoppingCart>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        private int GetDeliveryRateIdByProductQuantity(int productQuantity, int productId)
        {
            var deliveryRates = _dbContext.Set<DeliveryRate>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.ProductId == productId && x.Active).Select(dr => new { dr.Id, dr.UnitFrom, dr.UnitTo });
            var minDeliveryRateValue = deliveryRates.Min(x => x.UnitFrom);
            var maxDeliveryRateValue = deliveryRates.Max(x => x.UnitTo);
            var value = 0;

            if (minDeliveryRateValue <= productQuantity && productQuantity <= maxDeliveryRateValue)
            {
                foreach (var deliveryRate in deliveryRates)
                {
                    if (deliveryRate.UnitFrom <= productQuantity && deliveryRate.UnitTo >= productQuantity)
                    {
                        value = deliveryRate.Id;
                    }
                }
            }
            else if (productQuantity < minDeliveryRateValue)
            {
                foreach (var deliveryRate in deliveryRates)
                {
                    if (minDeliveryRateValue == deliveryRate.UnitFrom)
                        value = deliveryRate.Id;
                }
            }
            else
            {
                foreach (var deliveryRate in deliveryRates)
                {
                    if (maxDeliveryRateValue == deliveryRate.UnitTo)
                        value = deliveryRate.Id;
                }
            }
            return value;
        }
    }
}



