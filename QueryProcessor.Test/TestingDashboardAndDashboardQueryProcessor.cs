using HrevertCRM.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QueryProcessor.Test
{
    public class TestingDashboardAndDashboardQueryProcessor
    {
        private readonly IDashboardQueryProcessor _dashboardQueryProcessor;

        public TestingDashboardAndDashboardQueryProcessor(IDashboardQueryProcessor dashboardQueryProcessor)
        {
            _dashboardQueryProcessor = dashboardQueryProcessor;
        }

        [Theory]
        [InlineData(1)]
        public void TestingGetingDashboardValues(int id)
        {

            int fiscalYearId = 1;

            var dashboard = _dashboardQueryProcessor.GetDashboardValues(fiscalYearId);

            Assert.True(dashboard.Id > 0);


        }
    }
}
