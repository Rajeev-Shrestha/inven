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
    public class TestingPurchaseOrderLineAndPurchaseOrderLineController
    {

        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllPurchaseOrderLine(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/getallPurchaseOrderLines?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActivePurchaseOrderLine(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/getactivePurchaseOrderLines?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedPurchaseOrderLine(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/getdeletedPurchaseOrderLines?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }



        [Theory]
        [InlineData(1)]
        public async void ActivatePurchaseOrderLineById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/activatePurchaseOrderLine/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetPurchaseOrderLineById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/GetPurchaseOrderLine/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreatePurchaseOrderLinehouldReturnId(int test)
        {
            var puchaseOrderLine = new PurchaseOrderLineViewModel
            {
                Description = "A test",
                DescriptionType = DescriptionType.ProductDefault,
                ItemPrice = 1000,
                Shipped = false,
                ShippedQuantity = 10,
                ItemQuantity = 1000,
                LineOrder = 1,
                Discount = 1000,
                DiscountType = DiscountType.Fixed,
                TaxId = 1,
                TaxAmount = 1000,
                PurchaseOrderId = 3,
                ProductId = 1,
                Active = true,
                WebActive = true,
                CompanyId = 1

            };

            var json = JsonConvert.SerializeObject(puchaseOrderLine);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/purchaseorderline/createpurchaseorderline", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<PurchaseOrderLineViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdatePurchaseOrderLineShouldReturnId(int test)
        {

            int id = 7;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/GetPurchaseOrderLine/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var purchaseOrderLine = JsonConvert.DeserializeObject<PurchaseOrderLineViewModel>(content);

            purchaseOrderLine.Description = "Nokia will bounce back once again.";

            var json = JsonConvert.SerializeObject(purchaseOrderLine);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/purchaseorderline/updatepurchaseorderline", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<PurchaseOrderLineViewModel>(contents);
            Assert.True(returnedProduct.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeletePurchaseOrderLineShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/purchaseorderline/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllPurchaseOrderLineStatus(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/getdescriptiontypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetAllDiscountTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorderline/getdiscounttypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    }
}
