using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingSecurityRightAndSecurityRightController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllSecurityRight(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securityright/getallsecurityright");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetSecurityRightById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securityright/GetSecurityRight/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var contents = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SecurityRight>(contents);
            Assert.True(returnedProduct.Id > 0);
        }

    
        [Theory]
        [InlineData(1)]
        public async void CreateSecurityRightShouldReturnId(int test)
        {
            var securityRight = new SecurityRightViewModel
            {
                Allowed = true,
                UserId = "b0a18508-391c-4bc1-8320-71c9f4176e1c",
                SecurityId = 160,
                SecurityGroupId = 1,
                CompanyId = 1,
                Active = true,
                WebActive = true
            };
            var json = JsonConvert.SerializeObject(securityRight);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/securityright/createsecurityright", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SecurityRightViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateSecurityRightShouldReturnId(int test)
        {

            int id = 235;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securityright/GetSecurityRight/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var securityright = JsonConvert.DeserializeObject<SecurityRightViewModel>(content);

            securityright.Active = false;
            securityright.SecurityGroupId = 2;

            var json = JsonConvert.SerializeObject(securityright);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/securityright/updatesecurityright", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SecurityRightViewModel>(contents);
            Assert.True(returnedProduct.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteSecurityRightShouldReturnOk(int test)
        {
            int id = 346;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/securityright/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

    }
}
