using System;
using System.Collections.Generic;
using System.Security;
using Hrevert.Common.Constants.Account;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging;
using System.Linq;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountQueryProcessor _accountQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IAccountTypesQueryProcessor _accountTypesQueryProcessor;
        private readonly IAccountCashFlowTypesQueryProcessor _accountCashFlowTypesQueryProcessor;
        private readonly IAccountDetailTypesQueryProcessor _accountDetailTypesQueryProcessor;
        private readonly IAccountLevelTypesQueryProcessor _accountLevelTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountQueryProcessor accountQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IAccountTypesQueryProcessor accountTypesQueryProcessor,
            IAccountCashFlowTypesQueryProcessor accountCashFlowTypesQueryProcessor,
            IAccountDetailTypesQueryProcessor accountDetailTypesQueryProcessor,
            IAccountLevelTypesQueryProcessor accountLevelTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _accountQueryProcessor = accountQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _accountTypesQueryProcessor = accountTypesQueryProcessor;
            _accountCashFlowTypesQueryProcessor = accountCashFlowTypesQueryProcessor;
            _accountDetailTypesQueryProcessor = accountDetailTypesQueryProcessor;
            _accountLevelTypesQueryProcessor = accountLevelTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [Route("accounttree")]
        public ObjectResult GetAccountTreeViewModel()
        {
            //if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewAccounts))
            //    return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var accountNodes = _accountQueryProcessor.GetAccountsInTree();

            var accountsTreeViewModel = new List<AccountTreeViewModel>();
            var mapper = new AccountToAccountTreeViewModelMapper();

            foreach (var parent in accountNodes)
            {
                var account = parent.Source;
                var vm = mapper.Map(account);
                accountsTreeViewModel.Add(vm);
                WalkAccountTree(vm, parent.Children);
            }
            return Ok(accountsTreeViewModel);
        }

        [HttpGet]
        [Route("getallaccounttree")]
        public ObjectResult GetAllAccountTreeViewModel()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewAccounts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var accountNodes = _accountQueryProcessor.GetAllAccountsInTree();

            var accountsTreeViewModel = new List<AccountTreeViewModel>();
            var mapper = new AccountToAccountTreeViewModelMapper();

            foreach (var parent in accountNodes)
            {
                var account = parent.Source;
                var vm = mapper.Map(account);
                accountsTreeViewModel.Add(vm);
                WalkAccountTree(vm, parent.Children);
            }
            return Ok(accountsTreeViewModel);
        }


        [HttpPost]
        [Route("accounttree")]
        public ObjectResult SaveAccountTree([FromBody] List<AccountTreeViewModel> accounts)
        {
            short categoryRank = 1;
            var root = new AccountTreeViewModel();
            SaveAccountTreeChildren(root, categoryRank);
            return Ok(categoryRank);
        }

        private void SaveAccountTreeChildren(AccountTreeViewModel accountTreeViewModel, short categoryRank)
        {
            var parentId = accountTreeViewModel.Id == 0 ? null : accountTreeViewModel.Id;
            var mapper = new AccountToAccountTreeViewModelMapper();
            foreach (var accountVm in accountTreeViewModel.Children)
            {
                accountVm.ParentAccountId = parentId;
                var updatedAccount = mapper.Map(accountVm);

                if (updatedAccount.Id == 0)
                {
                    _accountQueryProcessor.Save(updatedAccount);

                }
                else
                {
                    _accountQueryProcessor.Update(updatedAccount);
                }
                categoryRank++;
                SaveAccountTreeChildren(accountVm, categoryRank);
            }
        }

        private void WalkAccountTree(AccountTreeViewModel accountTreeViewModel, List<AccountNode> parentChildren)
        {
            var mapper = new AccountNodeAndAccountTreeViewModelMapper();
            foreach (var accountNode in parentChildren)
            {
                var vm = mapper.Map(accountNode);
                accountTreeViewModel.Children.Add(vm);
                WalkAccountTree(vm, accountNode.Children);
            }
        }

        [HttpGet]
        [Route("getaccountbyid/{id}")]

        public ObjectResult Get(int id) //Get Includes Full Account data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewAccounts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var result = _accountQueryProcessor.GetAccountViewModel(id);
            return Ok(result);
        }


        [HttpGet()]
        [Route("activateaccount/{id}")]
        public ObjectResult ActivateAccount(int id)
        {
             var accountData = _accountQueryProcessor.ActivateAccount(id);
            var mapper = new AccountToAccountViewModelMapper();
            var mappedData = mapper.Map(accountData);
            return Ok(mappedData);
        }

        [HttpPost]
        [Route("createaccount")]
        public ObjectResult Create([FromBody] AccountViewModel accountViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddAccount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            accountViewModel.CompanyId = currentUserCompanyId;
            accountViewModel.AccountCode = accountViewModel.AccountCode?.Trim();
            accountViewModel.AccountDescription = accountViewModel.AccountDescription?.Trim();

            var model = accountViewModel;
            if (_accountQueryProcessor.Exists(p => p.AccountCode == model.AccountCode  && p.CompanyId == model.CompanyId))
            {
                return BadRequest(AccountConstants.AccountControllerConstants.AccountCodeExists);
            }

            if (_accountQueryProcessor.Exists(p => p.AccountDescription == model.AccountDescription && p.CompanyId == model.CompanyId))
            {
                return BadRequest(AccountConstants.AccountControllerConstants.AccountDescriptionExists);
            }

            var mapper = new AccountToAccountViewModelMapper();
            var newAccount = mapper.Map(accountViewModel);
            try
            {
                var savedAccount = _accountQueryProcessor.Save(newAccount);
                accountViewModel = mapper.Map(savedAccount);
                return Ok(accountViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddAccount, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }
        [HttpDelete()]
        [Route("deleteOnlyAccount/{id}")]
        public ObjectResult DeleteOnlyAccount(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteAccount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var isDeleted = false;
            try
            {
                isDeleted = _accountQueryProcessor.DeleteOnlyAccount(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteAccount, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteAccount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _accountQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteAccount, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateaccount")]
        public ObjectResult Put([FromBody] AccountViewModel accountViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateAccount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var model = accountViewModel;
            if (_accountQueryProcessor.Exists(p => p.AccountCode == model.AccountCode && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(AccountConstants.AccountControllerConstants.AccountCodeExists);
            }

            var mapper = new AccountToAccountViewModelMapper();
            var newAccount = mapper.Map(accountViewModel);
            try
            {
                var updatedAccount = _accountQueryProcessor.Update(newAccount);
                accountViewModel = mapper.Map(updatedAccount);
                return Ok(accountViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateAccount, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getchildrenaccounts/{id}")]
        public ObjectResult GetChildrenAccounts(int id)
        {
            var childrenAccounts = _accountQueryProcessor.GetChildrenAccounts(id);
            return Ok(childrenAccounts);
        }

        [HttpGet]
        [Route("getaccounttypes")]
        public ObjectResult GetAccountTypes()
        {
            return Ok(_accountTypesQueryProcessor.GetActiveAccountTypes());
        }

        [HttpGet]
        [Route("getaccountcashflowtypes")]
        public ObjectResult GetAccountCashFlowTypes()
        {
            return Ok(_accountCashFlowTypesQueryProcessor.GetActiveAccountCashFlowTypes());
        }

        [HttpGet]
        [Route("getaccountdetailtypes")]
        public ObjectResult GetAccountDetailTypes()
        {
            return Ok(_accountDetailTypesQueryProcessor.GetActiveAccountDetailTypes());
        }

        [HttpGet]
        [Route("getaccountleveltypes")]
        public ObjectResult GetAccountLevels()
        {
            return Ok(_accountLevelTypesQueryProcessor.GetAllAccountLevelTypes());
        }

        //[HttpGet]
        //[Route("searchactiveaccounts/{searchText}")]
        //public ObjectResult SearchActive(string searchText)
        //{
        //    var accountNodes = _accountQueryProcessor.SearchActive(searchText);

        //    var accountsTreeViewModel = new List<AccountTreeViewModel>();
        //    var mapper = new AccountToAccountTreeViewModelMapper();

        //    foreach (var parent in accountNodes)
        //    {
        //        var account = parent.Source;
        //        var vm = mapper.Map(account);
        //        accountsTreeViewModel.Add(vm);
        //        WalkAccountTree(vm, parent.Children);
        //    }
        //    return Ok(accountsTreeViewModel);
        //}

        //[HttpGet]
        //[Route("searchallaccounts/{searchText}")]
        //public ObjectResult SearchAll(string searchText)
        //{
        //    var accountNodes = _accountQueryProcessor.SearchAll(searchText);

        //    var accountsTreeViewModel = new List<AccountTreeViewModel>();
        //    var mapper = new AccountToAccountTreeViewModelMapper();

        //    foreach (var parent in accountNodes)
        //    {
        //        var account = parent.Source;
        //        var vm = mapper.Map(account);
        //        accountsTreeViewModel.Add(vm);
        //        WalkAccountTree(vm, parent.Children);
        //    }
        //    return Ok(accountsTreeViewModel);
        //}

        [HttpGet]
        [Route("getallactiveaccounts")]
        public ObjectResult GetAllActiveAccounts()
        {

            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewAccounts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new AccountToAccountViewModelMapper();
                var accounts = _accountQueryProcessor.GetAllActiveAccounts().Select(f => mapper.Map(f)).ToList();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProducts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("CheckIfDeletedAccountWithSameDescriptionExists/{description}")]
        public ObjectResult CheckIfDeletedAccountWithSameDescriptionExists(string description)
        {
            var account = _accountQueryProcessor.CheckIfDeletedAccountWithSameDescriptionExists(description);
            var accountMapper = new AccountToAccountViewModelMapper();
            if (account != null)
            {
                accountMapper.Map(account);
            }
            return Ok(account);

        }

        [HttpGet]
        [Route("CheckIfDeletedAccountWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedAccountWithSameCodeExists(string code)
        {
            var account = _accountQueryProcessor.CheckIfDeletedAccountWithSameCodeExists(code);
            var accountMapper = new AccountToAccountViewModelMapper();
            if (account != null)
            {
                accountMapper.Map(account);
            }
            return Ok(account);

        }

        [HttpGet]
        [Route("searchAccounts")]
        public ObjectResult SearchAccounts()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.Product);
                var result = _accountQueryProcessor.SearchAccounts(requestInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewAccounts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
