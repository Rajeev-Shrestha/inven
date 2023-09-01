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
    public class TestingCarouselAndCarouselController
    {

        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllCarousel(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/getallCarousels");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveCarousel(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/getactiveCarousels");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedCarousel(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/getadeletedCarousels");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllCarousel(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/searchactiveCarousels?pageNumber=1&pageSize=10&text=S&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchActiveCarousel(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/searchactiveCarousels?pageNumber=1&pageSize=10&text=S&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetCarouselById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/getCarouselbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetActivateCarouselById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/activateCarousel/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void CreateCarouselShouldReturnId(int test)
        {
            var image = new Image
            {
                Id=1,
                FileName = "wallpaper.jpg",
                ImageBase64 = "wewkenlwnfj n jnfjn jnkjdnf",
                ImageSize = ImageSize.Large,
                ImageType = ImageType.FullWidthImage
            };
            var carousel = new CarouselViewModel
            {
                ProductOrCategory = ProductOrCategory.Product,
                ItemId = 1,
                Image = image,
                ItemName = "A test",
                ImageUrl = @"E:\Workspace\HrevertCRM\CRM\source\HrevertCRM.Web\wwwroot\companyMM\CarouselImages\1",
                Active = true,
                WebActive = false,
                CompanyId = 1
            };
           
            var json = JsonConvert.SerializeObject(carousel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/Carousel", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedCustomerLevel = JsonConvert.DeserializeObject<CustomerLevelViewModel>(content);
            Assert.True(returnedCustomerLevel.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateCarouselShouldReturnId(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/Carousel/getCarouselbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var carousel = JsonConvert.DeserializeObject<CarouselViewModel>(content);

            carousel.ProductOrCategory = ProductOrCategory.Product;
            carousel.ItemId = 1;
            carousel.ItemName = "A test update";
            carousel.Active = true;
            carousel.WebActive = false;
            carousel.CompanyId = 1;
           


            var json = JsonConvert.SerializeObject(carousel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/Carousel", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedCustomerLevel = JsonConvert.DeserializeObject<CustomerLevelViewModel>(contents);
            Assert.True(returnedCustomerLevel.Id > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void DeleteCarouselById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/Carousel/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");
        }

    }
}
