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
    public class TestingFeaturedItemMetadataAndFeaturedItemMetadataController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllFeaturedItemMetadata(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/getallfeatureditemmetadatas?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveFeaturedItemMetadata(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/getactivefeatureditemmetadatas?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedFeaturedItemMetadata(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/getdeletedfeatureditemmetadatas?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void SearchActiveFeaturedItemMetadata(int test)
        {
            string name = "ram";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/searchactivefeatureditemmetadatas/" + name);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void SearchAllFeaturedItemMetadata(int test)
        {
            string name = "cc";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/searchallfeatureditemmetadatas/" + name);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void CreateFeaturedItemMetadataShouldReturnId(int test)
        {
            var metadata = new FeaturedItemMetadataViewModel
            {
                FeaturedItemId = 2,
                ItemId = 1,
                MediaType = MediaType.Photo,
                MediaUrl = "BannerImages/1/QuaterWidth",
                CompanyId = 1,
                Active = true,
                ImageType = ImageType.FullWidthImage, 
                WebActive = true
            };
            var json = JsonConvert.SerializeObject(metadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/createfeatureditemmetadata", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedAddress = JsonConvert.DeserializeObject<FeaturedItemMetadataViewModel>(content);
            Assert.True(returnedAddress.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateFeaturedItemMetadataShouldReturnId(int test)
        {

            int id = 17;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/getfeatureditemmetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var metadata = JsonConvert.DeserializeObject<FeaturedItemMetadataViewModel>(content);
            metadata.ItemId = 5;
            metadata.Active = false;
            metadata.WebActive = true;
            var json = JsonConvert.SerializeObject(metadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/updatefeatureditemmetadata", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedAddress = JsonConvert.DeserializeObject<FeaturedItemMetadataViewModel>(result);
            Assert.True(returnedAddress.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteFeaturedItemMetadataShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetFeaturedItemMetadataById(int test)
        {
            int id = 19;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/getfeatureditemmetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void ActivateFeaturedItemMetadata(int test)
        {
            int id = 18;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/activatefeatureditemmetadata/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
        [Theory]
        [InlineData(1)]

        public async void RemoveBannerImageFromFeaturedItemMetadata(int test)
        {
            var featuredItemMetadata = new FeaturedItemRemoveMetadata
            {
                ImageTypeId = 1,
                ImageUrl = "annerImages/1/FullWidth"
            };

            var json = JsonConvert.SerializeObject(featuredItemMetadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/removeBannerFromFeaturedItemMetadata",stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content=="");
        }

        [Theory]
        [InlineData(1)]

        public async void GetMediaTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItemMetadata/getmediatypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
    }
}
