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
    public class TestingProductMetadataAndProductMetadataMetadataController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllProductMetadatas(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/getallProductMetadatas?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveProductMetadatas(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/getactiveProductMetadatas?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedProductMetadatas(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/getdeletedProductMetadatas?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void ActivateProductMetadataById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/activateProductMetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetProductMetadataById(int test)
        {
            int id = 33;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/GetProductMetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

      
        [Theory]
        [InlineData(1)]
        public async void SearchAllProductMetadatas(int test)
        {
            string searchText = "F";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/searchallproductmetadatas/"+ searchText);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllActiveProductMetadatas(int test)
        {
            string searchText = "m";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/searchactiveproductmetadatas/"+searchText);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateProductMetadataShouldReturnId(int test)
        {
            List<int> categories = new List<int>();
            categories.Add(1);
            var productMetadata = new ProductMetadataViewModel
            {
             ProductId = 1,
             MediaType = Hrevert.Common.Enums.MediaType.Photo,
             MediaUrl = @"E:\Users\rkushwaha\Downloads\ajaysharma.jpeg",
             CompanyId = 1,
             Active = true,
             WebActive = false

        };
            var json = JsonConvert.SerializeObject(productMetadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/productmetadata/createproductmetadata", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProductMetadata = JsonConvert.DeserializeObject<ProductMetadataViewModel>(content);
            Assert.True(returnedProductMetadata.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateProductMetadataShouldReturnId(int test)
        {

            int id = 33;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/GetProductMetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var productMetadata = JsonConvert.DeserializeObject<ProductMetadataViewModel>(content);

            productMetadata.MediaUrl = "www.facebook.com/photos/ram.jpeg";
           
            var json = JsonConvert.SerializeObject(productMetadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/productmetadata/updateproductmetadata", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedProductMetadata = JsonConvert.DeserializeObject<ProductMetadataViewModel>(contents);
            Assert.True(returnedProductMetadata.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteProductMetadataShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/productmetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]
        public async void GetProductMetadataMediaTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productmetadata/getmediatypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
    }
}
