using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingDeliveryRateAndDeliveryRateController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void CreateDeliveryRateShouldReturnId(int test)
        {
            var deliveryRate = new DeliveryRateViewModel
            {
                WebActive = true,
                CompanyId = 1,
                Active = true
            };

            var json = JsonConvert.SerializeObject(deliveryRate);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/deliveryrate/createdeliveryrate", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedDeliveryRate = JsonConvert.DeserializeObject<DeliveryRateViewModel>(content);
            Assert.True(returnedDeliveryRate.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void UpdateDeliveryRateShouldReturnId(int test)
        {

            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/getDeliveryRatebyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var deliveryRate = JsonConvert.DeserializeObject<DeliveryRateViewModel>(content);

            deliveryRate.Active = false;
            var json = JsonConvert.SerializeObject(deliveryRate);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/deliveryrate/updatedeliveryrate", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedDeliveryRate = JsonConvert.DeserializeObject<DeliveryRateViewModel>(result);
            Assert.True(returnedDeliveryRate.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteDeliveryRateShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/deliveryrate/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllDeliveryRate(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/getallDeliveryRates");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveDeliveryRate(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/getactiveDeliveryRates");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedDeliveryRate(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/getadeletedDeliveryRates");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDeliveryRateById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/getDeliveryRatebyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetActivatedById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/activateDeliveryRate/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchActiveDeliveryRate(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/searchactiveDeliveryRates?pageNumber=1&pageSize=10&text=s&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllDeliveryRate(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliveryrate/searchallDeliveryRates?pageNumber=1&pageSize=10&text=s&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


    }
}
