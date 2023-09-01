using System;
using System.Linq;
using System.Threading.Tasks;
using Hrevert.Common.Constants.Company;
using Hrevert.Common.Constants.User;
using HrevertCRM.Common;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.Seed;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyQueryProcessor _companyQueryProcessor;
        private readonly IDbContext _context;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderQueryProcessor _emailSenderQueryProcessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _env;
        private readonly IUserQueryProcessor _userQueryProcessor;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyQueryProcessor companyQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory, 
            ILoggerFactory factory, IUserSettingQueryProcessor userSettingQueryProcessor,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor,
            UserManager<ApplicationUser> userManager,
            IEmailSenderQueryProcessor emailSenderQueryProcessor,
            IServiceProvider serviceProvider,
            IHostingEnvironment env,
            IUserQueryProcessor userQueryProcessor
            )
        {
            _companyQueryProcessor = companyQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
            _userManager = userManager;
            _emailSenderQueryProcessor = emailSenderQueryProcessor;
            _serviceProvider = serviceProvider;
            _env = env;
            _userQueryProcessor = userQueryProcessor;
            _logger = factory.CreateLogger<CompanyController>();
        }

        private void ForPaging(PagedDataRequest requestInfo)
        {
            if (requestInfo.PageSize == 0)
            {
                requestInfo.PageSize = _userSettingQueryProcessor.GetPageSize(EntityId.Company);
            }
            else
            {
                _userSettingQueryProcessor.Save(new UserSetting
                {
                    EntityId = EntityId.Company,
                    PageSize = requestInfo.PageSize
                });
            }
        }

        [HttpGet]
        [Route("getall")]
        public ObjectResult GetAll()
        {
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                ForPaging(requestInfo);
                return Ok(_companyQueryProcessor.GetCompanies(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.ViewCompanies, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getactivecompanies")]
        public ObjectResult GetActiveCompanies()
        {
            try
            {
                var result = _companyQueryProcessor.GetActiveCompanies();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanies, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getdeletedcompanies")]
        public ObjectResult GetDeletedCompanies()
        {
            try
            {
                return Ok(_companyQueryProcessor.GetDeletedCompanies());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanies, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getallcompanies")]
        public ObjectResult GetAllCompanies()
        {
            try
            {
                return Ok(_companyQueryProcessor.GetAllCompanies());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanies, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("searchactivecompanies/{searchText}")]
        public ObjectResult SearchActive(string searchText)
        {
            try
            {
                return Ok(_companyQueryProcessor.SearchActive(searchText));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanies, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getcompanybyid/{id}")]
        public ObjectResult Get(int id)
        {
            return Ok(_companyQueryProcessor.GetCompany(id));
        }

        [HttpGet]
        [Route("getcompany")]
        public ObjectResult GetCompanyByLoggedInUserId()
        {
            return Ok(_companyQueryProcessor.GetCompanyByLoggedInUserId());
        }

        [HttpPost]
        [Route("createcompany")]
        public ObjectResult Create([FromBody] CompanyViewModel companyViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            companyViewModel.Name = companyViewModel.Name.Trim();
            var model = companyViewModel;
            if (_companyQueryProcessor.Exists(p => p.Name == model.Name && p.Id != model.Id))
            {
                return BadRequest(CompanyConstants.CompanyControllerConstants.CompanyAlreadyExists);
            }

            var mapper = new CompanyToCompanyViewModelMapper();
            var newCompany = mapper.Map(companyViewModel);
            try
            {
                var savedCompany = _companyQueryProcessor.Save(newCompany);
                return Ok(mapper.Map(savedCompany));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddCompany, ex, ex.Message);
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
            try
            {
                return Ok(_companyQueryProcessor.Delete(id));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteCompany, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updatecompany")]
        public ObjectResult Put([FromBody] CompanyViewModel companyViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            companyViewModel.Name = companyViewModel.Name.Trim();
            var model = companyViewModel;
            if (_companyQueryProcessor.Exists(p => p.Name == model.Name && p.Id != model.Id))
            {
                return BadRequest(CompanyConstants.CompanyControllerConstants.CompanyAlreadyExists);
            }

            var mapper = new CompanyToCompanyViewModelMapper();
            var newCompany = mapper.Map(companyViewModel);
            try
            {
                newCompany.CompanyId = companyViewModel.Id ?? 0;
                newCompany.IsCompanyInitialized = true;
                newCompany.Active = true;
                return Ok(mapper.Map(_companyQueryProcessor.Update(newCompany)));
            }
                catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateCompany, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpPost]
        [Route("GetUserEmailAndCompanyNameAfterPurchase/{Email}/{CompanyName}")]
        public async Task<ObjectResult> CreateNewCompanyAndEmailUserNameAndPassToCustomer(string email, string companyName)
        {
            if (email == null || companyName == null) return BadRequest("Please Enter Email and Company Name");
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    #region Save Company and Company Web Setting
                   
                    var savedCompany = _companyQueryProcessor.Save(new Company
                    {
                        IsCompanyInitialized = false,
                        Name = companyName
                    });

                    _companyWebSettingQueryProcessor.SaveCompanyWebSetting(new CompanyWebSetting
                    {
                        IsEstoreInitialized = false,
                        CompanyId = savedCompany.Id
                    });

                    #endregion

                    #region Generate Random Password and Save User

                    var newUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        CompanyId = savedCompany.Id,
                        Address = "N/A",
                        FirstName = "Admin",
                        LastName = "User",
                        Gender = 1,
                        UserType = UserType.CompanyAdmin,
                        EmailConfirmed = true
                    };

                    var oldUser = await _userManager.FindByEmailAsync(newUser.Email);
                    if (oldUser != null)
                    {
                        return Ok(BadRequest(UserConstants.UserControllerConstants.UserAlreadyExists));
                    }

                    var password = GetRandomHexPassword(24);
                    var res = await _userManager.CreateAsync(newUser, password);
                    if (!res.Succeeded) return BadRequest(res.Errors);

                    #endregion

                    //Seed Required Datas like Securities, Security Group, Security Rights etc.
                    await SeedRequiredData.Seed(savedCompany, newUser, _userManager, _serviceProvider, _env.ContentRootPath);

                    //Send Email with UserName and Password to the Buyer
                    //_emailSenderQueryProcessor.SendUsernamePassToCustomer(email, newUser.UserName, password);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    var user = _userQueryProcessor.Get(email);
                    _userQueryProcessor.DeleteUser(user.Id, user.CompanyId);
                    _logger.LogCritical((int)SecurityId.AddUser, ex, ex.StackTrace);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
            return Ok("Company Purchased Successfully!");
        }

        [HttpPost]
        [Route("CheckIfCompanyInitialized/userId")]
        public ObjectResult CheckIfCompanyIsInitialized(string userId)
        {
            return Ok(_companyQueryProcessor.CheckIfCompanyIsInitialized(userId));
        }

        [HttpPost]
        [Route("CheckIfEstoreInitialized/userId")]
        public ObjectResult CheckIfEstoreIsInitialized(string userId)
        {
            return Ok(_companyWebSettingQueryProcessor.CheckIfEstoreIsInitialized(userId));
        }

        private static string GetRandomHexPassword(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var password = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            while (true)
            {
                if (password.Any(char.IsDigit))
                {
                    break;
                }
                password = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return password;
        }
    }
}
 