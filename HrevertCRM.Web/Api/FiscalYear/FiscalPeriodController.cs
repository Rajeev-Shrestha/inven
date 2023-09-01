using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class FiscalPeriodController : Controller
    {
        private readonly IFiscalPeriodQueryProcessor _fiscalPeriodQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<FiscalPeriodController> _logger;

        public FiscalPeriodController(IFiscalPeriodQueryProcessor fiscalPeriodQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _fiscalPeriodQueryProcessor = fiscalPeriodQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<FiscalPeriodController>();
        }        

        [HttpGet]
        [Route("getfiscalperiodbyyearid/{fiscalYearId}")]
        public ObjectResult GetFiscalPeriodsByFiscalYear(int fiscalYearId)
        {
            try
            {
                var mapper = new FiscalPeriodToFiscalPeriodViewModelMapper();
                return
                    Ok(
                        _fiscalPeriodQueryProcessor.GetActiveFiscalPeriodsByFiscalYear(fiscalYearId)
                            .Select(f => mapper.Map(f))
                            .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.ViewFiscalPeriods, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getfiscalperiodbyid/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFiscalPeriods))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_fiscalPeriodQueryProcessor.GetFiscalPeriod(id));
        }

        [HttpPost]
        [Route("createfiscalperiod")]
        public ObjectResult Create([FromBody] FiscalPeriodViewModel fiscalPeriodViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddFiscalPeriod))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            fiscalPeriodViewModel.CompanyId = currentUserCompanyId;
            fiscalPeriodViewModel.Name = fiscalPeriodViewModel.Name?.Trim();

            fiscalPeriodViewModel.FiscalYearId = GetFiscalYearIdForFiscalPeriod(fiscalPeriodViewModel.StartDate, fiscalPeriodViewModel.EndDate);
            //if (fiscalPeriodViewModel.FiscalYearId == 0)
            //{
            //    fiscalPeriodViewModel.FiscalYearId = GetFiscalYearIdForFiscalPeriod(fiscalPeriodViewModel.StartDate, fiscalPeriodViewModel.EndDate);
            //}
            var model = fiscalPeriodViewModel;
            if (
                _fiscalPeriodQueryProcessor.Exists(
                    p => p.Name == model.Name && p.FiscalYearId == model.FiscalYearId && p.CompanyId == model.CompanyId))
            {
                return BadRequest("FiscalPeriod with the same name already exists!");
            }
            if (!(fiscalPeriodViewModel.StartDate.Date.Ticks <= fiscalPeriodViewModel.EndDate.Date.Ticks))
            {
                return BadRequest("Start date of the fiscal period must be less than the End Date!");
            }

            ObjectResult badRequest;
            if (!CheckFiscalPeriodFallsInFiscalYearRange(fiscalPeriodViewModel, out badRequest)) return badRequest;

            if (CheckFiscalPeriodExist(fiscalPeriodViewModel, out badRequest)) return badRequest;

            var mapper = new FiscalPeriodToFiscalPeriodViewModelMapper();
            var newFiscalPeriod = mapper.Map(fiscalPeriodViewModel);

            try
            {
                var savedFiscalPeriod = _fiscalPeriodQueryProcessor.Save(newFiscalPeriod);
                fiscalPeriodViewModel = mapper.Map(savedFiscalPeriod);
                return Ok(fiscalPeriodViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddFiscalPeriod, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteFiscalPeriod))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _fiscalPeriodQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddFiscalPeriod, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatefiscalperiod")]
        public ObjectResult Put([FromBody] FiscalPeriodViewModel fiscalPeriodViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateFiscalPeriod))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            fiscalPeriodViewModel.Name = fiscalPeriodViewModel.Name?.Trim();
            var model = fiscalPeriodViewModel;
            if (_fiscalPeriodQueryProcessor.Exists(p => p.Name == model.Name && p.FiscalYearId == model.FiscalYearId && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest("Another FiscalPeriod with the same name already exists!");
            }
            if (!(fiscalPeriodViewModel.StartDate.Date.Ticks <= fiscalPeriodViewModel.EndDate.Date.Ticks))
            {
                return BadRequest("Start date of the fiscal period must be less than the End Date!");
            }
            var mapper = new FiscalPeriodToFiscalPeriodViewModelMapper();
            var newFiscalPeriod = mapper.Map(fiscalPeriodViewModel);
            try
            {
                var updatedFiscalPeriod = _fiscalPeriodQueryProcessor.Update(newFiscalPeriod);
                fiscalPeriodViewModel = mapper.Map(updatedFiscalPeriod);
                return Ok(fiscalPeriodViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateFiscalPeriod, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("activatefiscalperiod/{id}")]
        public ObjectResult ActivateFiscalPeriod(int id)
        {
            return Ok(_fiscalPeriodQueryProcessor.ActivateFiscalPeriod(id));
        }

        private bool CheckFiscalPeriodFallsInFiscalYearRange(FiscalPeriodViewModel fiscalPeriodViewModel, out ObjectResult badRequest)
        {
            var isFalls = true;
            badRequest = null;

            var fiscalYear = _fiscalPeriodQueryProcessor.GetFiscalYearDates(fiscalPeriodViewModel.FiscalYearId);

            if (!(fiscalPeriodViewModel.StartDate.Date.Ticks >= fiscalYear.StartDate.Date.Ticks &&
                fiscalPeriodViewModel.EndDate.Date.Ticks <= fiscalYear.EndDate.Date.Ticks))
            {
                isFalls = false;
            }

            if (isFalls == false)
            {
                badRequest =
                    BadRequest("The Fiscal Period does not fall in the region of Fiscal Year! Check the Fiscal Period again!");
            }
            return isFalls;
        }

        private bool CheckFiscalPeriodExist(FiscalPeriodViewModel fiscalPeriodViewModel, out ObjectResult badRequest)
        {
            var isOverlap = false;
            badRequest = null;

            var existingFiscalPeriodDates = _fiscalPeriodQueryProcessor.GetFiscalPeriodDates(fiscalPeriodViewModel.FiscalYearId);

            foreach (var fiscalPeriodDate in existingFiscalPeriodDates)
            {
                if (!(fiscalPeriodDate.StartDate.Date.Ticks >= fiscalPeriodViewModel.EndDate.Date.Ticks ||
                      fiscalPeriodDate.EndDate.Date.Ticks <= fiscalPeriodViewModel.StartDate.Date.Ticks))
                {
                    isOverlap = true;
                }
            }

            if (isOverlap)
            {
                badRequest = BadRequest("Fiscal Period with same dates already exists");
            }
            return isOverlap;
        }

        private int GetFiscalYearIdForFiscalPeriod(DateTime startDate,DateTime endDate)
        {
           
           var fiscalYear = _fiscalPeriodQueryProcessor.GetFiscalYearIdByFiscalPeriods(startDate,endDate);
            return fiscalYear.Id;
        }

        [HttpGet]
        [Route("GetFiscalPeriods/{active}")]
        public ObjectResult GetFiscalPeriods(bool active)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFiscalPeriods))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new FiscalPeriodToFiscalPeriodViewModelMapper();
                return Ok(_fiscalPeriodQueryProcessor.GetFiscalPeriods(active).Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewFiscalPeriods, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
