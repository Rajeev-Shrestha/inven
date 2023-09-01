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
    public class TestingItemMeasureAndItemMeasureController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void CreateItemMeasureShouldReturnId(int test)
        {
            var itemMeasure = new ItemMeasureViewModel
            {
                ProductId = 8,
                MeasureUnitId = 1,
                Price = 100,
                WebActive = true,
                CompanyId = 1,
                Active = true
            };

            var json = JsonConvert.SerializeObject(itemMeasure);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/itemmeasure/createitemmeasure", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedItemMeasure = JsonConvert.DeserializeObject<ItemMeasureViewModel>(content);
            Assert.True(returnedItemMeasure.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void UpdateItemMeasureShouldReturnId(int test)
        {

            int id = 9;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/itemmeasure/getItemMeasurebyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var itemMeasure = JsonConvert.DeserializeObject<ItemMeasureViewModel>(content);

            itemMeasure.Price=10000;
            itemMeasure.Active = false;
            var json = JsonConvert.SerializeObject(itemMeasure);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/itemmeasure/updateitemmeasure", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedItemMeasure = JsonConvert.DeserializeObject<ItemMeasureViewModel>(result);
            Assert.True(returnedItemMeasure.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteItemMeasureShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/itemmeasure/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllItemMeasure(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/itemmeasure/getallItemMeasures");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveItemMeasure(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/itemmeasure/getactiveItemMeasures");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedItemMeasure(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/itemmeasure/getadeletedItemMeasures");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetItemMeasureById(int test)
        {
            int id = 6;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/itemmeasure/getItemMeasurebyid/" + id);
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
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/itemmeasure/activateItemMeasure/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchActiveItemMeasure(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/ItemMeasure/searchactiveItemMeasures?pageNumber=1&pageSize=10&text=s&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllItemMeasure(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/ItemMeasure/searchallItemMeasures?pageNumber=1&pageSize=10&text=s&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

    }
}
