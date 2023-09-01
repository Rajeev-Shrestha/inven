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
    public class TestingTaxAndTaxController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllTaxes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/tax/getallTaxes?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveTaxes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/tax/getactiveTaxes?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedTaxes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/tax/getdeletedTaxes?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteTaxById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/tax/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");
        }

        [Theory]
        [InlineData(1)]
        public async void GetTaxById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/tax/getTax/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void ActivateTaxById(int test)
        {
            int id = 5;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/tax/activateTax/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateTaxShouldReturnId(int test)
        {
            var level = new TaxViewModel
            {
               TaxCode = "Va-Ra",
               Description = "Tax Description",
               IsRecoverable = false,
               TaxType = Hrevert.Common.Enums.TaxCaculationType.Fixed,
               TaxRate = 10,
               WebActive = false,
               Active = false,
               CompanyId = 1,
               RecoverableCalculationType = 1
               
            };
            
            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/Tax/createTax", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedTax = JsonConvert.DeserializeObject<TaxViewModel>(content);
            Assert.True(returnedTax.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateTaxShouldReturnId(int test)
        {
            int id = 9;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/tax/getTax/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var level = JsonConvert.DeserializeObject<TaxViewModel>(content);

            level.Active = false;
            level.Description = "Tax Description test update";

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/tax/updateTax", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedTax = JsonConvert.DeserializeObject<TaxViewModel>(contents);
            Assert.True(returnedTax.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllTaxCalculationTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/tax/gettaxcalculationtypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    }
}
