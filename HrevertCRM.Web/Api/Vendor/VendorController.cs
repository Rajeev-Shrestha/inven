using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Vendor;
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
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class VendorController : Controller
    {
        private readonly IVendorQueryProcessor _vendorQueryProcessor;
        private readonly IDbContext _dbContext;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ILogger<VendorController> _logger;
        private readonly IAddressQueryProcessor _addressQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public VendorController(IVendorQueryProcessor vendorQueryProcessor,
            IDbContext context, IAddressQueryProcessor addressQueryProcessor,
            IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _vendorQueryProcessor = vendorQueryProcessor;
            _dbContext = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _addressQueryProcessor = addressQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<VendorController>();
        }

        [HttpGet]
        [Route("getvendorbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full Vendor data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.ViewVendors))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _vendorQueryProcessor.GetVendorViewModel(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("activatevendor/{id}")]
        public ObjectResult ActivateVendor(int id)
        {
            var mapper = new VendorToVendorViewModelMapper();
            return Ok(mapper.Map(_vendorQueryProcessor.ActivateVendor(id)));
        }

        [HttpPost]
        [Route("createvendor")]
        public ObjectResult Create([FromBody] VendorViewModel vendorViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.AddVendor))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (vendorViewModel.Code == null)
                    {
                        vendorViewModel.Code = _vendorQueryProcessor.GenerateVendorCode();
                    }

                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    vendorViewModel.CompanyId = currentUserCompanyId;
                    vendorViewModel.Code = vendorViewModel.Code?.Trim();
                    vendorViewModel.ContactName = vendorViewModel.ContactName?.Trim();
                    var addresses = vendorViewModel.Addresses;
                    var billingAddress = vendorViewModel.BillingAddress;

                    var model = vendorViewModel;
                    if (_vendorQueryProcessor.Exists(p => p.Code == model.Code && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExists);
                    }

                    if (_addressQueryProcessor.Exists(p => p.Email == model.BillingAddress.Email && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExistsWithThisEmail);
                    }
                    if (_addressQueryProcessor.Exists(p => p.Email == model.Addresses.SingleOrDefault(x => x.AddressType == AddressType.Shipping).Email 
                        && p.AddressType == AddressType.Billing && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExistsWithThisEmail);
                    }
                    if (_addressQueryProcessor.Exists(p => p.Email == model.Addresses.SingleOrDefault(x => x.AddressType == AddressType.Contact).Email 
                        && p.AddressType == AddressType.Billing && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExistsWithThisEmail);
                    }

                    var mapper = new VendorToVendorViewModelMapper();
                    var addressMapper = new AddressToAddressViewModelMapper();
                    var newVendor = mapper.Map(vendorViewModel);
                    newVendor.Addresses = null;
                    newVendor.BillingAddress = null;
                    var savedVendor = _vendorQueryProcessor.Save(newVendor);
                    billingAddress.VendorId = savedVendor.Id;
                    billingAddress.AddressType = AddressType.Billing;
                    billingAddress.IsDefault = true;
                    foreach (var address in addresses)
                    {
                        address.VendorId = savedVendor.Id;
                    }
                    _addressQueryProcessor.Save(addressMapper.Map(billingAddress));
                    _addressQueryProcessor.SaveAll(addresses.ToList());
                    trans.Commit();
                    vendorViewModel = mapper.Map(savedVendor);
                    return Ok(vendorViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int) SecurityId.AddVendor, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.DeleteVendor))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _vendorQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.DeleteVendor, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatevendor")]
        public ObjectResult Put([FromBody] EditVendorViewModel vendorViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.UpdateVendor))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    vendorViewModel.Code = vendorViewModel.Code?.Trim();
                    vendorViewModel.ContactName = vendorViewModel.ContactName?.Trim();

                    var model = vendorViewModel;
                    if (_vendorQueryProcessor.Exists(p => p.Code == model.Code && p.Id != model.Id && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExists);
                    }
                    if (_addressQueryProcessor.Exists(p => p.Email == model.BillingAddress.Email 
                        && p.VendorId != model.Id && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExistsWithThisEmail);
                    }
                    if (_addressQueryProcessor.Exists(p => p.Email == model.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Shipping).Email 
                        && p.VendorId != model.Id && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExistsWithThisEmail);
                    }
                    if (_addressQueryProcessor.Exists(p => p.Email == model.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Contact).Email
                        && p.VendorId != model.Id && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(VendorConstants.VendorControllerConstants.VendorAlreadyExistsWithThisEmail);
                    }
                    var mapper = new VendorToEditVendorViewModelMapper();
                    var newVendor = mapper.Map(vendorViewModel);

                    var updatedVendor = _vendorQueryProcessor.Update(newVendor);
                    
                    trans.Commit();

                    vendorViewModel = mapper.Map(updatedVendor);
                    return Ok(vendorViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateVendor, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpPost]
        [Route("checkcode/{vendorCode}")]
        public bool CheckIfVendorCodeExistsOrNot(string vendorCode)
        {
            var result = _vendorQueryProcessor.CheckIfVendorCodeExistsOrNot(vendorCode);
            return result;
        }

        [HttpGet]
        [Route("getcode")]
        public string GetCode()
        {
            var result = _vendorQueryProcessor.GenerateVendorCode();
            return result;
        }

        [HttpGet]
        [Route("activevendorwithoutpagaing")]
        public ObjectResult GetActiveVendorsWithoutPaging()
        {
            var mapper = new VendorToVendorViewModelMapper();
            var result = _vendorQueryProcessor.GetActiveVendorsWithoutPaging().Select(x => mapper.Map(x));
            return Ok(result);
        }

        [HttpGet]
        [Route("getVendorAllAddresses/{vendorId}")]
        public ObjectResult GetVendorAllAddresses(int vendorId)
        {
            var result = _vendorQueryProcessor.GetVendorAllAddresses(vendorId);

            var mapper = new AddressToAddressViewModelMapper();

            return Ok(result.Select(x => mapper.Map(x)).ToList());
        }

        [HttpGet]
        [Route("getVendorNames")]
        public ObjectResult GetVendorNames()
        {
            return Ok(_vendorQueryProcessor.GetVendorNames());
        }

        [HttpGet]
        [Route("CheckIfDeletedVendorWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedVendorWithSameCodeExists(string code)
        {
            var vendor = _vendorQueryProcessor.CheckIfDeletedVendorWithSameCodeExists(code);
            var vendorMapper = new VendorToEditVendorViewModelMapper();
            if (vendor != null)
            {
                vendorMapper.Map(vendor);
            }
            return Ok(vendor);
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.Vendor);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("getVendors")]
        public ObjectResult GetVendors()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewVendors))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.Vendor);
                return Ok(_vendorQueryProcessor.GetVendors(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewVendors, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> vendorsId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteVendor))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (vendorsId == null || vendorsId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _vendorQueryProcessor.DeleteRange(vendorsId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteVendor, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
