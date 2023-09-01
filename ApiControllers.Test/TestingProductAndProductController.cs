using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
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
    public class TestingProductAndProductController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllProducts(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/getallproducts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveProducts(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/getallactiveproducts");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedProducts(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/getdeletedproducts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetActiveProducts(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/getactiveproducts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void ActivateProductById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/activateproduct/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetProductById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/GetProduct/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetProductByCategoryId(int test)
        {
            int id = 4;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/getProductsByCategoryId/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllProducts(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/searchallproducts?pageNumber=1&pageSize=10&text=S&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllActiveProducts(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/searchactiveproducts?pageNumber=1&pageSize=10&text=S&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateProductShouldReturnId(int test)
        {
            var categories = new List<int>();
            var images = new List<Image>();
            var imagesUrl = new List<string>();
            categories.Add(1);
            images.Clear();
            imagesUrl.Clear();

            var product = new ProductViewModel {
                Code = "N-ELite",
                Name = "Nokia",
                ShortDescription = "It's amazing",
                LongDescription = "It's amazing",
                UnitPrice = 10000,
                QuantityOnHand = 10,
                QuantityOnOrder =1,
                Active = true,
                CompanyId = 1,
                WebActive = false,
                Commissionable =false,
                CommissionRate =0,
                Categories = categories,
                Images = images,
                DiscountType = Hrevert.Common.Enums.DiscountType.None,
                DiscountPercentage = 0,
                DiscountPrice = 0,
                ProductType = Hrevert.Common.Enums.ProductType.Kit,
                Taxes = null,
                TaxesInProducts = null,
                ProductsReferencedByKitAndAssembledType = null,
                ProductsRefByAssembledAndKit = null,
             // ImageUrls = imagesUrl

            };
            var json = JsonConvert.SerializeObject(product);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/product/createproduct", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<ProductViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateProductShouldReturnId(int test)
        {

            int id = 25;
            var response = await Setup.GetClient(_service)
                .GetAsync(_service.ServiceEndPoint + "/api/product/GetProduct/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
           var taxes=new List<int>();
            var categories= new List<int>();
            var images = new List<Image>();
            var proRef=new List<string>();
            taxes.Add(1);
            categories.Add(1);
            categories.Add(3);
            images.Clear();
            proRef.Add("Ref");
            proRef.Add("Data");
            var product = JsonConvert.DeserializeObject<ProductViewModel>(content);
            product.ShortDescription = "Nokia will bounce back once again.";
            product.Name = "Nokia Android";
            product.Taxes = taxes;
            product.Categories = categories;
            product.Images = images;
            product.ListOfRefProductNames = proRef;
            product.ProductsRefByAssembledAndKit =null;
            var json = JsonConvert.SerializeObject(product);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/product", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<ProductViewModel>(contents);
            Assert.True(returnedProduct.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteProductShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/product/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }


        [Theory]
        [InlineData(1)]
        public async void GetProductsForStore(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/getstorefrontproducts/");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetProductsWithDiscountForStore(int test)
        {
            int customerId = 1;
            int categoryId = 1;
            int  level= 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/GetProductsForStore/" + customerId + "/" + categoryId+"/"+level);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchProductsByCategoryIdAndText(int test)
        {
            int id = 1;
            string text = "Hp";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/SearchProductByCategoryIdAndText/"+id+"/"+text);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetRegularProducts(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/GetRegularProductsOnly");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetProductsForStoreFront(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/product/getstorefrontproducts");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

      
    }
}
