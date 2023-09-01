using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.JournalMaster;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class JournalMasterController : Controller
    {
        private readonly IJournalMasterQueryProcessor _journalMasterQueryProcessor;
        private readonly IFiscalPeriodQueryProcessor _fiscalPeriodQueryProcessor;
        private readonly IJournalTypesQueryProcessor _journalTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ILogger<JournalMasterController> _logger;

        public JournalMasterController(IJournalMasterQueryProcessor journalMasterQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IFiscalPeriodQueryProcessor fiscalPeriodQueryProcessor,
            IJournalTypesQueryProcessor journalTypesQueryProcessor, 
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor)
        {
            _journalMasterQueryProcessor = journalMasterQueryProcessor;
            _fiscalPeriodQueryProcessor = fiscalPeriodQueryProcessor;
            _journalTypesQueryProcessor = journalTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _logger = factory.CreateLogger<JournalMasterController>();
        }
        

        [HttpGet]
        [Route("getjournalmasterbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full JournalMaster data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewJournalMasters))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_journalMasterQueryProcessor.GetJournalMasterViewModel(id));
        }

        [HttpGet()]
        [Route("activatejournalmaster/{id}")]
        public ObjectResult ActivateJournalMaster(int id)
        {
            var mapper = new JournalMasterToJournalMasterViewModelMapper();
            return Ok(mapper.Map(_journalMasterQueryProcessor.ActivateJournalMaster(id)));
        }

        [HttpPost]
        [Route("createjournalmaster")]
        public ObjectResult Create([FromBody] JournalMasterViewModel journalMasterViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddJournalMaster))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            journalMasterViewModel.FiscalPeriodId = _fiscalPeriodQueryProcessor.GetFiscalPeriodIdByCurrentDate();
            journalMasterViewModel.Description = journalMasterViewModel.Description?.Trim();
            journalMasterViewModel.Note = journalMasterViewModel.Note?.Trim();
            journalMasterViewModel.Credit = journalMasterViewModel.Credit;
            journalMasterViewModel.Debit = journalMasterViewModel.Debit;
            journalMasterViewModel.PostedDate = journalMasterViewModel.PostedDate;
            journalMasterViewModel.JournalType = journalMasterViewModel.JournalType;
            journalMasterViewModel.Cancelled = journalMasterViewModel.Cancelled ?? false;
            journalMasterViewModel.Posted = journalMasterViewModel.Posted ?? false;
            journalMasterViewModel.Closed = journalMasterViewModel.Closed ?? false;
            journalMasterViewModel.Printed = journalMasterViewModel.Printed ?? false;

            var mapper = new JournalMasterToJournalMasterViewModelMapper();
            var newJournalMaster = mapper.Map(journalMasterViewModel);
            try
            {
                var savedJournalMaster = _journalMasterQueryProcessor.Save(newJournalMaster);
                journalMasterViewModel = mapper.Map(savedJournalMaster);
                return Ok(journalMasterViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddJournalMaster, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteJournalMaster))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var isDeleted = false;
            try
            {
                isDeleted = _journalMasterQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteJournalMaster, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatejournalmaster")]
        public ObjectResult Put([FromBody] JournalMasterViewModel journalMasterViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateJournalMaster))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            journalMasterViewModel.FiscalPeriodId = _fiscalPeriodQueryProcessor.GetFiscalPeriodIdByCurrentDate();
            journalMasterViewModel.Description = journalMasterViewModel.Description?.Trim();
            journalMasterViewModel.Note = journalMasterViewModel.Note?.Trim();
            journalMasterViewModel.Credit = journalMasterViewModel.Credit;
            journalMasterViewModel.Debit = journalMasterViewModel.Debit;
            journalMasterViewModel.PostedDate = journalMasterViewModel.PostedDate;
            journalMasterViewModel.JournalType = journalMasterViewModel.JournalType;
            journalMasterViewModel.Cancelled = journalMasterViewModel.Cancelled ?? false;
            journalMasterViewModel.Posted = journalMasterViewModel.Posted ?? false;
            journalMasterViewModel.Closed = journalMasterViewModel.Closed ?? false;
            journalMasterViewModel.Printed = journalMasterViewModel.Printed ?? false;
            journalMasterViewModel.Active = journalMasterViewModel.Active;

            if (journalMasterViewModel.IsSystem)
            {
                return BadRequest(JournalMasterConstants.JournalMasterControllerConstants.SystemJournalCantBeModified);
            }

            var mapper = new JournalMasterToJournalMasterViewModelMapper();
            var newJournalMaster = mapper.Map(journalMasterViewModel);
            try
            {
                var updatedJournalMaster = _journalMasterQueryProcessor.Update(newJournalMaster);
                journalMasterViewModel = mapper.Map(updatedJournalMaster);
                return Ok(journalMasterViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateJournalMaster, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getjournaltypes")]
        public ObjectResult GetJournalMasterTypes()
        {
            return Ok(_journalTypesQueryProcessor.GetActiveJournalTypes());
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.JournalMaster);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchJournalMasters")]
        public ObjectResult SearchJournalMasters()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewJournalMasters))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.JournalMaster);
                var result = _journalMasterQueryProcessor.SearchJournalMasters(requestInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewJournalMasters, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> journalMasterId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteJournalMaster))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (journalMasterId == null || journalMasterId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _journalMasterQueryProcessor.DeleteRange(journalMasterId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteJournalMaster, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

    }
}
