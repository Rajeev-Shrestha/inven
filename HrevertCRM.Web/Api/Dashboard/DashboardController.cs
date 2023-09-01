using HrevertCRM.Data.QueryProcessors;
using Microsoft.AspNetCore.Mvc;


namespace HrevertCRM.Web.Api
{
    [Route("api/[Controller]")]
    public class DashboardController : Controller
    {
        private readonly IDashboardQueryProcessor _dashboardQueryProcessor;

        public DashboardController(IDashboardQueryProcessor dashboardQueryProcessor)
        {
            _dashboardQueryProcessor = dashboardQueryProcessor;
        }

        [HttpGet]
        [Route("GetDashboardValues/{fiscalYearId}")]
        public ObjectResult GetDashboardValues(int fiscalYearId)
        {
            var result = _dashboardQueryProcessor.GetDashboardValues(fiscalYearId);
            return Ok(result);
        }
    }
}
