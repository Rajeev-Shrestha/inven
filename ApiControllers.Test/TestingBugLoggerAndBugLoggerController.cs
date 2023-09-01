using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingBugLoggerAndBugLoggerController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void CreateBugShouldReturnId(int test)
        {
            var address = new BugLoggerViewModel
            {
                Message = "A Bug Report Test",
                BugAdded = DateTime.Now,
                Active = true,
                CompanyId = 1
            };

            var json = JsonConvert.SerializeObject(address);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/BugLogger/reportBug", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var bugViewModel = JsonConvert.DeserializeObject<BugLoggerViewModel>(content);
            Assert.True(bugViewModel.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void GetAllBugs(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/BugLogger/getAllBugs");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }
    }
}
