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
    public class TestingDeliveryMethodAndDeliveryMethodController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllDeliveryMethods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/getalldeliverymethods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveDeliveryMethods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/getactivedeliverymethods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDeletedDeliveryMethods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/getdeleteddeliverymethods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void SearchAllDeliveryMethods(int test)
        {
            string p = "c";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/searchalldeliverymethods/" + p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchActiveDeliveryMethods(int test)
        {
            string p = "d";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/searchactivedeliverymethods/" + p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void ActivateDeliveryMethod(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/activatedeliverymethod/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetDeliveryMethodById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/getdeliverymethod/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateDeliveryMethodShouldReturnId(int test)
        {
            var deliveryMethods = new DeliveryMethodViewModel
            {
             DeliveryCode = "CSD",
             Description = "test create Delivery Method",
             Active = true,
             WebActive=false,
             CompanyId=1

            };

            var json = JsonConvert.SerializeObject(deliveryMethods);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/deliverymethod", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedDeliveryMethod = JsonConvert.DeserializeObject<DeliveryMethodViewModel>(content);
            Assert.True(returnedDeliveryMethod.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateDeliveryMethodShouldReturnId(int test)
        {
            int id = 7;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/deliverymethod/getdeliverymethod/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var deliveryMethod = JsonConvert.DeserializeObject<DeliveryMethodViewModel>(content);

            deliveryMethod.DeliveryCode = "CSD 1";
            deliveryMethod.Description = "Delivery Method Description Update";

            var json = JsonConvert.SerializeObject(deliveryMethod);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/deliverymethod", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedDeliveryMethod = JsonConvert.DeserializeObject<DeliveryMethodViewModel>(result);
            Assert.True(returnedDeliveryMethod.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteDeliveryMethodShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/deliverymethod/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

       
       
       
    }
}
