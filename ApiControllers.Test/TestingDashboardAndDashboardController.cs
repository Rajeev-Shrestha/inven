using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingDashboardAndDashboardController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]

        public async void GetDashboardValues(int test)
        {
            int fiscalYearId = 7;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/dashboard/GetDashboardValues/"+fiscalYearId);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

      
    }
}
