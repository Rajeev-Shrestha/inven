using System;
using System.Linq;
using Hrevert.Common.Constants.Address;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly IAddressQueryProcessor _addressQueryProcessor;
        private readonly ITitleTypesQueryProcessor _titleTypesQueryProcessor;
        private readonly ISuffixTypesQueryProcessor _suffixTypesQueryProcessor;
        private readonly IAddressTypesQueryProcessor _addressTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressQueryProcessor addressQueryProcessor,
            IDbContext context, ILoggerFactory factory, 
            ITitleTypesQueryProcessor titleTypesQueryProcessor,
            ISuffixTypesQueryProcessor suffixTypesQueryProcessor,
            IAddressTypesQueryProcessor addressTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserQueryProcessor userQueryProcessor,
            ICompanyQueryProcessor companyQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _addressQueryProcessor = addressQueryProcessor;
            _titleTypesQueryProcessor = titleTypesQueryProcessor;
            _suffixTypesQueryProcessor = suffixTypesQueryProcessor;
            _addressTypesQueryProcessor = addressTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<AddressController>();
        }

        [HttpGet]
        [Route("getalladdresses")]
        public ObjectResult GetAll()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewAddresses))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new AddressToAddressViewModelMapper();
                return Ok(_addressQueryProcessor.GetActiveAddresses().Select(address => mapper.Map(address)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.ViewAddresses, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getaddress/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewAddresses))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _addressQueryProcessor.GetAddress(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("createaddress")]
        public ObjectResult Create([FromBody] AddressViewModel addressViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddAddress))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            addressViewModel.CompanyId = currentUserCompanyId;
            addressViewModel.FirstName = addressViewModel.FirstName.Trim();
            addressViewModel.MiddleName = addressViewModel.MiddleName?.Trim();
            addressViewModel.LastName = addressViewModel.LastName.Trim();
            addressViewModel.AddressLine1 = addressViewModel.AddressLine1.Trim();
            addressViewModel.AddressLine2 = addressViewModel.AddressLine2?.Trim();
            addressViewModel.City = addressViewModel.City.Trim();
            addressViewModel.CountryId = addressViewModel.CountryId;
            addressViewModel.Telephone = addressViewModel.Telephone.Trim();
            addressViewModel.MobilePhone = addressViewModel.MobilePhone.Trim();
            addressViewModel.Email = addressViewModel.Email.Trim();

            var model = addressViewModel;
            if (_addressQueryProcessor.Exists(a => a.Email == model.Email && a.Id != model.Id && a.CompanyId == model.CompanyId))
            {
                return BadRequest(AddressConstants.AddressControllerConstants.EmailAlreadyExists);
            }            

            var mapper = new AddressToAddressViewModelMapper();
            var newAddress = mapper.Map(addressViewModel);
            try
            {
                var savedAddress = _addressQueryProcessor.Save(newAddress);
                addressViewModel = mapper.Map(savedAddress);                
                return Ok(addressViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddAddress, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteAddress))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _addressQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteAddress, ex, ex.Message);
                BadRequest(ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateaddress")]
        public ObjectResult Put([FromBody] AddressViewModel addressViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateAddress))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            addressViewModel.FirstName = addressViewModel.FirstName.Trim();
            addressViewModel.MiddleName = addressViewModel.MiddleName?.Trim();
            addressViewModel.LastName = addressViewModel.LastName.Trim();
            addressViewModel.AddressLine1 = addressViewModel.AddressLine1.Trim();
            addressViewModel.AddressLine2 = addressViewModel.AddressLine2?.Trim();
            addressViewModel.City = addressViewModel.City.Trim();
            addressViewModel.CountryId = addressViewModel.CountryId;
            addressViewModel.Telephone = addressViewModel.Telephone.Trim();
            addressViewModel.MobilePhone = addressViewModel.MobilePhone.Trim();
            addressViewModel.Email = addressViewModel.Email.Trim();
            

            //TODO: What kind of checks should be introduced here so that we can make sure no duplicates of addresses gets saved into the database

            var model = addressViewModel;
            if (_addressQueryProcessor.Exists(a => a.Email == model.Email && a.Id != model.Id && a.CompanyId == model.CompanyId))
            {
                return BadRequest(AddressConstants.AddressControllerConstants.EmailAlreadyExists);
            }

            var mapper = new AddressToAddressViewModelMapper();
            var newAddress = mapper.Map(addressViewModel);
            try
            {
                var savedAddress = _addressQueryProcessor.Update(newAddress);
                addressViewModel = mapper.Map(savedAddress); 
                return Ok(addressViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateAddress, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("gettitletypes")]
        public ObjectResult GetTitleTypes()
        {
            return Ok(_titleTypesQueryProcessor.GetActiveTitleTypes());
        }

        [HttpGet]
        [Route("getsuffixtypes")]
        public ObjectResult GetSuffixTypes()
        {
            return Ok(_suffixTypesQueryProcessor.GetActiveSuffixTypes());
        }

        [HttpGet]
        [Route("getaddresstypes")]
        public ObjectResult GetAddressTypes()
        {
            return Ok(_addressTypesQueryProcessor.GetActiveAddressTypes());
        }

        [HttpGet]
        [Route("activateaddress/{id}")]
        public ObjectResult ActivateAddressById(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewAddresses))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _addressQueryProcessor.ActivateAddress(id);

            var mapper = new AddressToAddressViewModelMapper();

            return Ok(mapper.Map(result));
         
        }
    }
}
