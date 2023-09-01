using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingDiscountAndDiscountController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllDiscount(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/getallDiscounts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveDiscount(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/getactiveDiscounts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedDiscount(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/getdeletedDiscount?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteDiscountById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/Discount/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");
        }

        [Theory]
        [InlineData(1)]
        public async void GetDiscountById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/getDiscountbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void ActivateDiscountById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/activateDiscount/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateDiscountShouldReturnId(int test)
        {
            var level = new DiscountViewModel
            {
               DiscountType = DiscountType.Fixed,
               DiscountValue = 10000,
               MinimumQuantity = 4,
               DiscountStartDate = DateTime.Now,
               DiscountEndDate = DateTime.Now.AddDays(5),
               ItemId = 4,
               CustomerId = 0,
               CustomerLevelId = 0,
               CategoryId = 0,
               WebActive = false,
               Active = true,
               CompanyId = 1
            };

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/Discount/creatediscount", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedTax = JsonConvert.DeserializeObject<DiscountViewModel>(content);
            Assert.True(returnedTax.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateDiscountShouldReturnId(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/getDiscountbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var level = JsonConvert.DeserializeObject<DiscountViewModel>(content);

            level.Active = false;
            level.ItemId = 5;
            level.MinimumQuantity = 5;

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/Discount/updateDiscount", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedTax = JsonConvert.DeserializeObject<DiscountViewModel>(contents);
            Assert.True(returnedTax.Id > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedDiscountWithoutPaging(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/deleteddiscountswithoutpagaing");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveDiscountWithoutPaging(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/activediscountwithoutpagaing");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDiscountTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Discount/getdiscounttypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
    }
}
