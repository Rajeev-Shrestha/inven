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
    public class TestingSalesOrderLineAndSalesOrderLineController
    {

        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllSalesOrderLine(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/getallSalesOrderLines?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveSalesOrderLine(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/getactiveSalesOrderLines?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedSalesOrderLine(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/getdeletedSalesOrderLines?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


    
        [Theory]
        [InlineData(1)]
        public async void ActivateSalesOrderLineById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/activateSalesOrderLine/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetSalesOrderLineById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/GetSalesOrderLine/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    
      
        [Theory]
        [InlineData(1)]
        public async void CreateSalesOrderLineShouldReturnId(int test)
        {
            
                var salesOrderLine = new SalesOrderLineViewModel
                {
                  Description  = "A test",
                  DescriptionType = DescriptionType.ProductDefault,
                  ItemPrice = 10,
                  Shipped = false,
                  DiscountType = DiscountType.None,
                  ItemQuantity = 100,
                  ShippedQuantity = 100,
                  LineOrder = 1,
                  Discount = 10,
                  TaxAmount = 100,
                  SalesOrderId = 1,
                  ProductId = 1,
                  CompanyId = 1,
                  Active = true,
                  WebActive = true
                };
            

            var json = JsonConvert.SerializeObject(salesOrderLine);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/salesorderline", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedSalesOrderLine = JsonConvert.DeserializeObject<SalesOrderLineViewModel>(content);
            Assert.True(returnedSalesOrderLine.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateSalesOrderLineShouldReturnId(int test)
        {

            int id = 4;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/GetSalesOrderLine/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<SalesOrderLineViewModel>(content);
            customer.Description = "Nokia will bounce back once again.";
            var json = JsonConvert.SerializeObject(customer);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/salesorderline", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = response.Content.ReadAsStringAsync().Result;
            var returnedSalesOrderLine = JsonConvert.DeserializeObject<SalesOrderLineViewModel>(contents);
            Assert.True(returnedSalesOrderLine.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteSalesOrderLineShouldReturnOk(int test)
        {
            int id = 4;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/salesorderline/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllDescriptionTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/getdescriptiontypes");
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
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorderline/getdiscounttypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


    }
}
