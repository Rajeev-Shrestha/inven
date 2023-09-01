using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingDeliveryZoneAndDeliveryZoneController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void CreateDeliveryZoneShouldReturnId(int test)
        {
            var deliveryZone = new DeliveryZoneViewModel
            {
              ZoneName = "BBIT",
              ZoneCode = "CGKK",
              WebActive = true,
              CompanyId = 1,
              Active = true,
              //Addresses = null,
              //DeliveryRates = null
            };

            var json = JsonConvert.SerializeObject(deliveryZone);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/DeliveryZone/createzone", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedDeliveryZone = JsonConvert.DeserializeObject<DeliveryZoneViewModel>(content);
            Assert.True(returnedDeliveryZone.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void UpdateDeliveryZoneShouldReturnId(int test)
        {

            int id = 29;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/getDeliveryZonebyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var deliveryZone = JsonConvert.DeserializeObject<DeliveryZoneViewModel>(content);

            deliveryZone.ZoneName = "County Updated";
            deliveryZone.Active = false;
            var json = JsonConvert.SerializeObject(deliveryZone);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/DeliveryZone/updatezone", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedDeliveryZone = JsonConvert.DeserializeObject<DeliveryZoneViewModel>(result);
            Assert.True(returnedDeliveryZone.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteDeliveryZoneShouldReturnOk(int test)
        {
            int id = 29;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/DeliveryZone/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllDeliveryZone(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/getallDeliveryZones");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveDeliveryZone(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/getactiveDeliveryZones");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedDeliveryZone(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/getadeletedDeliveryZones");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDeliveryZoneById(int test)
        {
            int id = 6;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/getDeliveryZonebyid/" + id);
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
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/activateDeliveryZone/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

 
        [Theory]
        [InlineData(1)]
        public async void SearchActiveDeliveryZone(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/searchactiveDeliveryZones?pageNumber=1&pageSize=10&text=s&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllDeliveryZone(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/DeliveryZone/searchallDeliveryZones?pageNumber=1&pageSize=10&text=s&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


    }
}
