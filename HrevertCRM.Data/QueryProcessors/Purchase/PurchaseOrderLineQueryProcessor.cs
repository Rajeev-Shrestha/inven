using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Purchase;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using PagedTaskDataInquiryResponse =
    HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.PurchaseOrderLineViewModel>;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class PurchaseOrderLineQueryProcessor : QueryBase<PurchaseOrderLine>, IPurchaseOrderLineQueryProcessor
    {
        public PurchaseOrderLineQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public PurchaseOrderLine Update(PurchaseOrderLine purchaseOrderLine)
        {
            var original = GetValidPurchaseOrderLine(purchaseOrderLine.Id);
            ValidateAuthorization(purchaseOrderLine);
            CheckVersionMismatch(purchaseOrderLine, original);

            original.Description = purchaseOrderLine.Description;
            original.DescriptionType = purchaseOrderLine.DescriptionType;
            original.ItemPrice = purchaseOrderLine.ItemPrice;
            original.Shipped = purchaseOrderLine.Shipped;
            original.ItemQuantity = purchaseOrderLine.ItemQuantity;
            original.ShippedQuantity = purchaseOrderLine.ShippedQuantity;
            original.LineOrder = purchaseOrderLine.LineOrder;
            original.Discount = purchaseOrderLine.Discount;
            original.TaxAmount = purchaseOrderLine.TaxAmount;
            original.DiscountType = purchaseOrderLine.DiscountType;
            original.TaxId = purchaseOrderLine.TaxId;
            original.PurchaseOrderId = purchaseOrderLine.PurchaseOrderId;
            original.ProductId = purchaseOrderLine.ProductId;
            original.CompanyId = purchaseOrderLine.CompanyId;
            original.Active = purchaseOrderLine.Active;
            original.WebActive = purchaseOrderLine.WebActive;

            _dbContext.Set<PurchaseOrderLine>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual PurchaseOrderLine GetValidPurchaseOrderLine(int purchaseOrderLineId)
        {
            var purchaseOrderLine = _dbContext.Set<PurchaseOrderLine>().FirstOrDefault(sc => sc.Id == purchaseOrderLineId);
            if (purchaseOrderLine == null)
                throw new RootObjectNotFoundException(PurchaseOrderConstants.PurchaseOrderLineQueryProcessorConstants.PurchaseOrderLineNotFound);
            return purchaseOrderLine;
        }

        public PurchaseOrderLine GetPurchaseOrderLine(int purchaseOrderLineId)
        {
            var purchaseOrderLine = _dbContext.Set<PurchaseOrderLine>().FirstOrDefault(d => d.Id == purchaseOrderLineId);
            return purchaseOrderLine;
        }

        public void SaveAllPurchaseOrderLine(List<PurchaseOrderLine> purchaseOrderLines)
        {
            purchaseOrderLines.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<PurchaseOrderLine>().AddRange(purchaseOrderLines);
            _dbContext.SaveChanges();
        }

        public PurchaseOrderLine Save(PurchaseOrderLine purchaseOrderLine)
        {
            purchaseOrderLine.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<PurchaseOrderLine>().Add(purchaseOrderLine);
            _dbContext.SaveChanges();
            return purchaseOrderLine;
        }

        public int SaveAll(ICollection<PurchaseOrderLine> purchaseOrderLines)
        {
            _dbContext.Set<PurchaseOrderLine>().AddRange(purchaseOrderLines);
            return _dbContext.SaveChanges();
        }

        public void SaveAllViewModels(ICollection<PurchaseOrderLineViewModel> purchaseOrderLineViewModels)
        {
            var mapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
            _dbContext.Set<PurchaseOrderLine>().AddRange(purchaseOrderLineViewModels.Select(x => mapper.Map(x)));
            _dbContext.SaveChanges();
        }

        public PurchaseOrderLine ActivatePurchaseOrderLine(int id)
        {
            var original = GetValidPurchaseOrderLine(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<PurchaseOrderLine>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public PurchaseOrderLineViewModel GetPurchaseOrderLineViewModel(int id)
        {
            var purchaseOrderLine = _dbContext.Set<PurchaseOrderLine>().Single(s => s.Id == id);
            var mapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
            return mapper.Map(purchaseOrderLine);
        }

        public PagedTaskDataInquiryResponse SearchPurchaseOrderLines(PagedDataRequest requestInfo, Expression<Func<PurchaseOrderLine, bool>> @where = null)
        {
            var query = _dbContext.Set<PurchaseOrderLine>().Include(x => x.Product).Where(c => c.CompanyId == LoggedInUser.CompanyId);
            if (requestInfo.Active)
                query = query.Where(x => x.Active);
            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query
                : query.Where(x => x.Product.Name.ToUpper().Contains( requestInfo.SearchText.ToUpper()) ||
                                   x.Product.Code.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, result);
        }

        public void UpdateQuantityOnOrderInProduct(PurchaseOrderLineViewModel purchaseOrderLineViewModel, PurchaseOrderType orderType)
        {
            var product = _dbContext.Set<Product>().FirstOrDefault(x => x.Id == purchaseOrderLineViewModel.ProductId && x.Active && x.CompanyId == LoggedInUser.CompanyId);
            if (purchaseOrderLineViewModel.Id == 0 || purchaseOrderLineViewModel.Id == null)
            {
                switch (orderType)
                {
                    case PurchaseOrderType.Order:
                        //No effect on Stock or QOO
                        break;
                    case PurchaseOrderType.Invoice:
                        product.QuantityOnHand = product.QuantityOnHand + (int)purchaseOrderLineViewModel.ItemQuantity;
                        break;
                    case PurchaseOrderType.Quote:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null);
                }
            }
            else
            {
                var purchaOrderType = _dbContext.Set<PurchaseOrderLine>().Include(x => x.PurchaseOrder)
                    .FirstOrDefault(x => x.Id == purchaseOrderLineViewModel.Id).PurchaseOrder.OrderType;
                if (purchaOrderType == PurchaseOrderType.Invoice)
                {
                    product.QuantityOnHand = product.QuantityOnHand + (int)purchaseOrderLineViewModel.ItemQuantity;
                }
            }
            _dbContext.Set<Product>().Update(product);
            _dbContext.SaveChanges();
        }

        public bool Delete(int purchaseOrderLineId)
        {
            var doc = GetPurchaseOrderLine(purchaseOrderLineId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<PurchaseOrderLine>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<PurchaseOrderLine, bool>> where)
        {
            return _dbContext.Set<PurchaseOrderLine>().Any(where);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<PurchaseOrderLine> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(s => mapper.Map(s))
                    .ToList();

            var queryResult = new QueryResult<PurchaseOrderLineViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public PurchaseOrderLine[] GetPurchaseOrderLines(Expression<Func<PurchaseOrderLine, bool>> where = null)
        {
            var query = _dbContext.Set<PurchaseOrderLine>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        //public IQueryable<PurchaseOrderLine> GetActivePurchaseOrderLines()
        //{
        //    return _dbContext.Set<PurchaseOrderLine>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        //}

        //public IQueryable<PurchaseOrderLine> GetDeletedPurchaseOrderLines()
        //{
        //    return _dbContext.Set<PurchaseOrderLine>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        //}
        //public IQueryable<PurchaseOrderLine> GetAllPurchaseOrderLines()
        //{
        //    return _dbContext.Set<PurchaseOrderLine>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
        //}
    }
}
