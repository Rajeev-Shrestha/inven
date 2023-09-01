using System;
using System.Linq;
using Hrevert.Common.Constants.FiscalYear;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using NUglify.Helpers;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class FiscalYearController : Controller
    {
        private readonly IFiscalYearQueryProcessor _fiscalYearQueryProcessor;
        private readonly IFiscalPeriodQueryProcessor _fiscalPeriodQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDbContext _dbContext;
        private readonly ILogger<FiscalYearController> _logger;

        public FiscalYearController(IFiscalYearQueryProcessor fiscalYearQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor,
            IFiscalPeriodQueryProcessor fiscalPeriodQueryProcessor,
            UserManager<ApplicationUser> userManager,
            IDbContext dbContext)
        {
            _fiscalYearQueryProcessor = fiscalYearQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _fiscalPeriodQueryProcessor = fiscalPeriodQueryProcessor;
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = factory.CreateLogger<FiscalYearController>();
        }
        
        [HttpGet]
        [Route("activatefiscalyear/{id}")]
        public ObjectResult ActivateFiscalYear(int id)
        {
            var mapper = new FiscalYearToFiscalYearViewModelMapper();
            return Ok(mapper.Map(_fiscalYearQueryProcessor.ActivateFiscalYear(id)));
        }

        [HttpGet]
        [Route("getfiscalyearbyid/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFiscalYears))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_fiscalYearQueryProcessor.GetFiscalYear(id));
        }

        [HttpPost]
        [Route("createfiscalyear")]
        public ObjectResult Create([FromBody] FiscalYearViewModel fiscalYearViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddFiscalYear))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    fiscalYearViewModel.CompanyId = currentUserCompanyId;
                    fiscalYearViewModel.Name = fiscalYearViewModel.Name?.Trim();
                    fiscalYearViewModel.StartDate = fiscalYearViewModel.StartDate.Date;
                    fiscalYearViewModel.EndDate = fiscalYearViewModel.EndDate.Date;

                    var model = fiscalYearViewModel;
                    if (_fiscalYearQueryProcessor.Exists(p => p.Name == model.Name && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(FiscalYearConstants.FiscalYearControllerConstants.FiscalYearNameExists);
                    }
                    if (!(fiscalYearViewModel.StartDate.Date.Ticks <= fiscalYearViewModel.EndDate.Date.Ticks))
                    {
                        return BadRequest(FiscalYearConstants.FiscalYearControllerConstants.StartDateMustBeLessThanEndDate);
                    }

                    ObjectResult badRequest;
                    if (CheckFiscalYearExist(fiscalYearViewModel, out badRequest)) return badRequest;

                    var mapper = new FiscalYearToFiscalYearViewModelMapper();
                    var periodMapper = new FiscalPeriodToFiscalPeriodViewModelMapper();
                    var newFiscalYear = mapper.Map(fiscalYearViewModel);

                    var savedFiscalYear = _fiscalYearQueryProcessor.Save(newFiscalYear);
                    if (fiscalYearViewModel.FiscalPeriodViewModels != null && fiscalYearViewModel.FiscalPeriodViewModels.Count > 0)
                    {
                        var fiscalPeriods = fiscalYearViewModel.FiscalPeriodViewModels;
                        fiscalPeriods.ForEach(x => x.FiscalYearId = savedFiscalYear.Id);
                        fiscalPeriods.ForEach(x => x.CompanyId = savedFiscalYear.CompanyId);

                        foreach (var item in fiscalPeriods)
                        {
                            if (!CheckFiscalPeriodFallsInFiscalYearRange(item, out badRequest)) return badRequest;
                            if (CheckFiscalPeriodExist(item, out badRequest)) return badRequest;
                            _fiscalPeriodQueryProcessor.Save(periodMapper.Map(item));
                        }
                    }
                    fiscalYearViewModel = mapper.Map(savedFiscalYear);
                    trans.Commit();
                    return Ok(fiscalYearViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddFiscalYear, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteFiscalYear))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _fiscalYearQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteFiscalYear, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatefiscalyear")]
        public ObjectResult Put([FromBody] FiscalYearViewModel fiscalYearViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateFiscalYear))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    fiscalYearViewModel.Name = fiscalYearViewModel.Name?.Trim();
                    fiscalYearViewModel.StartDate = fiscalYearViewModel.StartDate.Date;
                    fiscalYearViewModel.EndDate = fiscalYearViewModel.EndDate.Date;

                    var model = fiscalYearViewModel;
                    if (_fiscalYearQueryProcessor.Exists(p => p.Name == model.Name && p.CompanyId == model.CompanyId && p.Id != model.Id))
                    {
                        return BadRequest(FiscalYearConstants.FiscalYearControllerConstants.FiscalYearNameExists);
                    }
                    if (!(fiscalYearViewModel.StartDate.Date.Ticks <= fiscalYearViewModel.EndDate.Date.Ticks))
                    {
                        return BadRequest(FiscalYearConstants.FiscalYearControllerConstants.StartDateMustBeLessThanEndDate);
                    }

                    ObjectResult badRequest;
                    if (CheckFiscalYearExist(fiscalYearViewModel, out badRequest)) return badRequest;

                    var mapper = new FiscalYearToFiscalYearViewModelMapper();
                    var periodMapper = new FiscalPeriodToFiscalPeriodViewModelMapper();
                    var newFiscalYear = mapper.Map(fiscalYearViewModel);
                    var updatedFiscalYear = _fiscalYearQueryProcessor.Update(newFiscalYear);
                    fiscalYearViewModel.Id = updatedFiscalYear.Id;

                    if (fiscalYearViewModel.FiscalPeriodViewModels != null &&
                        fiscalYearViewModel.FiscalPeriodViewModels.Count > 0)
                    {
                        var fiscalPeriods = fiscalYearViewModel.FiscalPeriodViewModels.Where(x => x.Active).ToList();
                        fiscalPeriods.ForEach(x => x.FiscalYearId = updatedFiscalYear.Id);
                        fiscalPeriods.ForEach(x => x.CompanyId = updatedFiscalYear.CompanyId);

                        foreach (var item in fiscalPeriods)
                        {
                            if (!CheckFiscalPeriodFallsInFiscalYearRange(item, out badRequest)) return badRequest;

                            //if (CheckFiscalPeriodExist(item, out badRequest)) return badRequest;

                            if (item.Id == null)
                            {
                                _fiscalPeriodQueryProcessor.Save(periodMapper.Map(item));
                            }
                            else
                            {
                                _fiscalPeriodQueryProcessor.Update(periodMapper.Map(item));
                            }
                        }
                    }
                    trans.Commit();
                    return Ok(fiscalYearViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateFiscalYear, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        private bool CheckFiscalYearExist(FiscalYearViewModel fiscalYear, out ObjectResult badRequest)
        {
            var isConflict = false;
            badRequest = null;

            var existingFiscalYearDates = _fiscalYearQueryProcessor.GetFiscalYearDates();

            foreach (var fiscalYearDate in existingFiscalYearDates)
            {
                if (!(fiscalYearDate.StartDate.Date.Ticks >= fiscalYear.EndDate.Date.Ticks ||
                      fiscalYearDate.EndDate.Date.Ticks <= fiscalYear.StartDate.Date.Ticks) && fiscalYear.Id!=fiscalYearDate.Id)
                    isConflict = true;
            }

            if (isConflict)
                badRequest = BadRequest(FiscalYearConstants.FiscalYearControllerConstants.FiscalYearDatesExists);
            return isConflict;
        }


        private bool CheckFiscalPeriodFallsInFiscalYearRange(FiscalPeriodViewModel fiscalPeriodViewModel, out ObjectResult badRequest)
        {
            var isFalls = true;
            badRequest = null;

            var fiscalYear = _fiscalPeriodQueryProcessor.GetFiscalYearDates(fiscalPeriodViewModel.FiscalYearId);

            if (!(fiscalPeriodViewModel.StartDate.Date.Ticks >= fiscalYear.StartDate.Date.Ticks &&
                fiscalPeriodViewModel.EndDate.Date.Ticks <= fiscalYear.EndDate.Date.Ticks))
                isFalls = false;

            if (isFalls == false)
                badRequest = BadRequest("The Fiscal Period does not fall in the region of Fiscal Year! Check the Fiscal Period again!");
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
                      fiscalPeriodDate.EndDate.Date.Ticks <= fiscalPeriodViewModel.StartDate.Date.Ticks) && fiscalPeriodDate.Id != fiscalPeriodViewModel.Id)
                    isOverlap = true;
            }

            if (isOverlap)
                badRequest = BadRequest("Fiscal Period with same dates already exists");
            return isOverlap;
        }

        [HttpGet]
        [Route("searchFiscalYears/{active}/{searchText}")]
        public ObjectResult SearchFiscalYears(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFiscalYears))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new FiscalYearToFiscalYearViewModelMapper();
                return Ok(_fiscalYearQueryProcessor.SearchFiscalYears(active, searchText).Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewFiscalYears, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> fiscalYearId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteFiscalYear))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (fiscalYearId == null || fiscalYearId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _fiscalYearQueryProcessor.DeleteRange(fiscalYearId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteFiscalYear, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpGet]
        [Route("getCurrentFiscalYear")]
        public ObjectResult GetFiscalYearByCurrentDate()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFiscalYears))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var fiscalYear = _fiscalYearQueryProcessor.GetFiscalYearByCurrentDate();
                if(fiscalYear == null) return BadRequest(
                    "Fiscal Year does not exist for current date, Please add a new Fiscal Year for Current Date");
                return Ok(fiscalYear);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewFiscalYears, ex, ex.Message);
                return BadRequest(
                    "Fiscal Year does not exist for current date, Please add a new Fiscal Year for Current Date");
            }
        }
    }
}
