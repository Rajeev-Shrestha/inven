using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Enums;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartQueryProcessor _shoppingCartQueryProcessor;
        private readonly IShoppingCartDetailQueryProcessor _shoppingCartDetailQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(IShoppingCartQueryProcessor shoppingCartQueryProcessor,
            IShoppingCartDetailQueryProcessor shoppingCartDetailQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor)
        {
            _shoppingCartQueryProcessor = shoppingCartQueryProcessor;
            _shoppingCartDetailQueryProcessor = shoppingCartDetailQueryProcessor; 
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _logger = factory.CreateLogger<ShoppingCartController>();
        }
        
        [HttpGet]
        [Route("getcart/{id}/{customerId}")]
        public ObjectResult Get(int id, int customerId) //Get Includes Full ShoppingCart data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewShoppingCarts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (id == 0 && customerId == 0) return BadRequest("There is no such cart");
            var result = _shoppingCartQueryProcessor.GetShoppingCartViewModel(id, customerId);
            if (result == null)  return BadRequest("There is no items in the cart.");
            return Ok(result);
        }

        [HttpGet]
        [Route("getshoppingcart/{id}/{customerId}/{guid}")]
        public ObjectResult Get(int id, int customerId, Guid guid) //Get Includes Full ShoppingCart data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewShoppingCarts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (id == 0 && customerId == 0) return BadRequest("There is no such cart");
            var result = _shoppingCartQueryProcessor.GetCartViewModel(id, customerId, guid);
            if (result == null) return BadRequest("There is no items in the cart.");
            return Ok(result);
        }


        [HttpGet]
        [Route("activateShoppingCart/{id}")]
        public ObjectResult ActivateShoppingCart(int id)
        {
            var mapper = new ShoppingCartToShoppingCartViewModelMapper();
            return Ok(mapper.Map(_shoppingCartQueryProcessor.ActivateShoppingCart(id)));
        }

        [HttpPost]
        [Route("createshoppingcart")]
        public ObjectResult Create([FromBody] ShoppingCartViewModel shoppingCartViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddShoppingCart))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            
            shoppingCartViewModel.HostIp = shoppingCartViewModel.HostIp?.Trim();

            var mapper = new ShoppingCartToShoppingCartViewModelMapper();
            var newShoppingCart = mapper.Map(shoppingCartViewModel);
            var shoppingCartDetailList = newShoppingCart.ShoppingCartDetails;
            try
            {
                if (shoppingCartDetailList.Count > 0)
                {
                    _shoppingCartDetailQueryProcessor.SaveAll(shoppingCartDetailList.ToList());
                }
                var savedShoppingCart = _shoppingCartQueryProcessor.Save(newShoppingCart);
                shoppingCartViewModel = mapper.Map(savedShoppingCart);

                return Ok(shoppingCartViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddShoppingCart, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteShoppingCart))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _shoppingCartQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteShoppingCart, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateshoppingcart")]
        public ObjectResult Put([FromBody] ShoppingCartViewModel shoppingCartViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateShoppingCart))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            shoppingCartViewModel.HostIp = shoppingCartViewModel.HostIp?.Trim();

            var mapper = new ShoppingCartToShoppingCartViewModelMapper();
            var newShoppingCart = mapper.Map(shoppingCartViewModel);
            var shoppingCartDetailList = newShoppingCart.ShoppingCartDetails;
            try
            {
                if (shoppingCartDetailList.Count > 0)
                {
                    _shoppingCartDetailQueryProcessor.SaveAll(shoppingCartDetailList.ToList());
                }
                var updatedShoppingCart = _shoppingCartQueryProcessor.Update(newShoppingCart);
                shoppingCartViewModel = mapper.Map(updatedShoppingCart);
                return Ok(shoppingCartViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateShoppingCart, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpPost]
        [Route("ConvertCartToOrder")]
        public ObjectResult ConvertCartToOrder([FromBody] ConvertCartToOrderViewModel convertCartToOrderViewModel)
        {
            if (convertCartToOrderViewModel.ShippingAddressViewModel.Id == null)
            {
                convertCartToOrderViewModel.ShippingAddressViewModel.Title = TitleType.None;
                convertCartToOrderViewModel.ShippingAddressViewModel.Suffix = SuffixType.None;
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var cartSalesOrder =_shoppingCartQueryProcessor.ConvertCartToOrder(convertCartToOrderViewModel);
            if (cartSalesOrder == null)
            {
                return BadRequest("There is not any item in Cart. Please add Items to cart First!");
            }
            return Ok("Success");
        }

        [HttpPost]
        [Route("SendEmailToCustomer")]
        public void SendEmailToCustomer([FromBody] SendOrderToCustomerViaEmailViewModel sendOrderToCustomerViaEmailViewModel)
        {
            _shoppingCartQueryProcessor.SendEmailToCustomer(sendOrderToCustomerViaEmailViewModel);
        }

        [HttpPost]
        [Route("addtocart")]
        public ObjectResult AddToCart([FromBody] ShoppingCartDetailViewModel shoppingCartDetailViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var mapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();
            var shoppingCartDetail = mapper.Map(shoppingCartDetailViewModel);

            var result = _shoppingCartQueryProcessor.AddProductToCart(shoppingCartDetail);
            return Ok(result);
        }

        [HttpPost]
        [Route("removeproductfromcart/{id}")]
        public ObjectResult RemoveProductFromCart(int id)
        {
            return Ok(_shoppingCartQueryProcessor.RemoveProductFromCart(id));
        }

        [HttpPost]
        [Route("UpdateShoppingCartDetail")]
        public ObjectResult UpdateShoppingCartDetail([FromBody] ShoppingCartDetailViewModel shoppingCartDetailViewModel)
        {
            var cartDetailMapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();
            var result = _shoppingCartQueryProcessor.UpdateCartDetail(cartDetailMapper.Map(shoppingCartDetailViewModel));
            return Ok(result);
        }

        [HttpPost]
        [Route("updatecartwithcustomerdetails/{cartId}/{customerId}")]
        public ObjectResult UpdateShoppingCart(int cartId, int customerId)
        {
            if (cartId == 0 || customerId == 0) return Ok("Invalid Values. CartId and CustomerId cannot be null");
            var result = _shoppingCartQueryProcessor.UpdateCartWithCustomerDetails(cartId, customerId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.ShoppingCart);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("getShoppingCarts")]
        public ObjectResult GetShoppingCarts()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewShoppingCarts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.ShoppingCart);
                return Ok(_shoppingCartQueryProcessor.GetShoppingCarts(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewShoppingCarts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getTrendingProducts")]
        public ObjectResult GetTrendingProducts()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewShoppingCarts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _shoppingCartDetailQueryProcessor.GetTrendingProducts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewShoppingCarts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getTopCategories")]
        public ObjectResult GetTopCategories()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewShoppingCarts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _shoppingCartDetailQueryProcessor.GetTopCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewShoppingCarts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getHotThisWeek")]
        public ObjectResult GetHotThisWeek()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewShoppingCarts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _shoppingCartDetailQueryProcessor.GetHotThisWeek();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewShoppingCarts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}

