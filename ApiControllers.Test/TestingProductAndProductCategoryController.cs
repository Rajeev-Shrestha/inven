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
    public class TestingProductAndProductCategoryController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllProductCategories(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getallcategories");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveProductCategories(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getactivecategories");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedProductCategories(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getdeletedcategories");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;


            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetCategoryTree(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/categorytree");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SaveCategoryTreeShouldReturnId(int test)
        {
            List<ProductCategoryTreeViewModel> productCategoriesTree = new List<ProductCategoryTreeViewModel>();

            var productCategoryTree = new ProductCategoryTreeViewModel
            {
                Name= "Novel Collection",
                Description = "Novel latest edition",
                CategoryRank = 1,
                WebActive = false,
                Children = null
                
            };

            productCategoriesTree.Add(productCategoryTree);

            var json = JsonConvert.SerializeObject(productCategoriesTree);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/productcategory/categorytree", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            //Assert.True(content> 0);
        }

       
        [Theory]
        [InlineData(1)]
        public async void GetProductCategoryById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getproductcategory/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetFullProductCategories(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getcategories");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }



        [Theory]
        [InlineData(1)]
        public async void CreateProductCategoryShouldReturnId(int test)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            List<ProductInCategoryViewModel> productsInCategory = new List<ProductInCategoryViewModel>();

            List<int> categories = new List<int>();
            categories.Add(1);
            var product = new ProductViewModel
            {
                Code = "Nokia Lumia",
                Name = "Nokia",
                ShortDescription = "It's amazing",
                UnitPrice = 10000,
                QuantityOnHand = 10,
                QuantityOnOrder = 1,
                Active = true,
                CompanyId = 1,
                WebActive = false,
                Commissionable = false,
                CommissionRate = 0,
                Categories = categories
            };

            products.Add(product);

            var productInCatehory= new ProductInCategoryViewModel{
                ProductId =1,
                CategoryId =1
            };
            productsInCategory.Add(productInCatehory);
            var productCategory = new ProductCategoryViewModel
            {
              Name = "Xiaomi Mobiles",
              Description = "A latest branded mobile",
              CategoryRank = 1,
              WebActive = false,
              Active = true,
              ProductInCategories=productsInCategory,
              Products = products
      
            };
            var json = JsonConvert.SerializeObject(productCategory);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/productcategory/createcategory", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProductCategory = JsonConvert.DeserializeObject<ProductCategoryViewModel>(content);
            Assert.True(returnedProductCategory.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateProductCategoryShouldReturnId(int test)
        {

            int id = 13;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getproductcategory/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var productCategory = JsonConvert.DeserializeObject<ProductCategoryViewModel>(content);

            productCategory.Description = "Nokia will bounce back once again.";

            var json = JsonConvert.SerializeObject(productCategory);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/productcategory/updatecategory", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedProductCategory = JsonConvert.DeserializeObject<ProductCategoryViewModel>(contents);
            Assert.True(returnedProductCategory.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteProductCategoryShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/productcategory/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }



        [Theory]
        [InlineData(1)]

        public async void GetCategoryWithProducts(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getcategorywithproducts/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetCategoryWithChildren(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/productcategory/getcategorywithchildren/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
    }
}
