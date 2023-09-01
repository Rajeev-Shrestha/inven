using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingCustomerAndCustomerLevelController
    {

        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllCustomerLevel(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customerlevel/getallcustomerlevels");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
        [Theory]
        [InlineData(1)]
        public async void DeleteCustomerLevelById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/customerlevel/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content=="");
        }

        [Theory]
        [InlineData(1)]
        public async void GetCustomerLevelById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customerlevel/getcustomerlevelbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void CreateCustomerLevelShouldReturnId(int test)
        {
            var level = new CustomerLevelViewModel
            {
                Id = null,
                Name = "test 123",
                Active = true,
                CompanyId = 1
            };

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/customerlevel/createcustomerlevel", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedCustomerLevel = JsonConvert.DeserializeObject<CustomerLevelViewModel>(content);
            Assert.True(returnedCustomerLevel.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateCustomerLevelShouldReturnId(int test)
        {
            int id = 11;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customerlevel/getcustomerlevelbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var level = JsonConvert.DeserializeObject<CustomerLevelViewModel>(content);

            level.Active = false;
            level.Name = "test update";

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/customerlevel/updatecustomerlevel", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedCustomerLevel = JsonConvert.DeserializeObject<CustomerLevelViewModel>(contents);
            Assert.True(returnedCustomerLevel.Id > 0);
        }
    }
}
