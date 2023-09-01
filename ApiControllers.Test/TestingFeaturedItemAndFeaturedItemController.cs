using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingFeaturedItemAndFeaturedItemController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllFeaturedItem(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getallfeatureitems?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetActiveFeaturedItem(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getactivefeatureditems?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveFeaturedItem(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getallactivefeatureditems");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedFeaturedItem(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getdeletedfeatureditems?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void CreateFeaturedItemShouldReturnId(int test)
        {
            var image = new Image
            {
                Id = 1,
                FileName = "ramesh.jpg",
                ImageType = ImageType.HalfWidthImage,
                ImageSize = ImageSize.Large,
                ImageBase64 = "erereterterter"
            };

            var bannerImages = new List<Image>();
            
                bannerImages.Add(image);
            
            var address = new FeaturedItemViewModel
            {
              ImageType = ImageType.HalfWidthImage,
              SortOrder = false,
              WebActive = true,
              ItemId = 7,
              ItemName = "Featured Item Test",
              Active = true,
              BannerImage = bannerImages,
              CompanyId = 1
              
            };

            var json = JsonConvert.SerializeObject(address);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/FeaturedItem", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedAddress = JsonConvert.DeserializeObject<FeaturedItemViewModel>(content);
            Assert.True(returnedAddress.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateFeaturedItemShouldReturnId(int test)
        {
            var image = new List<Image>();
            image.Clear();
            int id = 4;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getfeatureditem/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var address = JsonConvert.DeserializeObject<FeaturedItemViewModel>(content);
            address.BannerImage = image;
            address.Active = true;
            address.ItemName = "BannerImage Update";
            var json = JsonConvert.SerializeObject(address);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/FeaturedItem", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedAddress = JsonConvert.DeserializeObject<FeaturedItemViewModel>(result);
            Assert.True(returnedAddress.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteFeaturedItemShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/FeaturedItem/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetFeaturedItemById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getfeatureditem/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void ActivateFeaturedItem(int test)
        {
            int id = 2;

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/activatefeatureditem/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetAllFeaturedItemsForBannerImagesForStoreFrontByImageTypeIdAndCategoryId(int test)
        {
            int imageTypeId = 1;
            int categoryId = 4;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getBannerImagesByCategoryIdAndImageTypeId/"+imageTypeId+"/"+categoryId);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetAllBannerImagesByCategoryId(int test)
        {
            int id = 4;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getAllBannerImagesByCategoryId/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
        [Theory]
        [InlineData(1)]

        public async void GetAllBannerImages(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getAllBannerImages");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetAllImageTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/FeaturedItem/getimagetypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
    }
}
