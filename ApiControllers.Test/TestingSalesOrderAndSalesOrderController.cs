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
    public class TestingSalesOrderAndSalesOrderController
    {

        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllSalesOrder(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesOrder/getallsalesorders?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveSalesOrder(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorder/getactivesalesorders?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedSalesOrder(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesOrder/getdeletedsalesorders?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


    
        [Theory]
        [InlineData(1)]
        public async void ActivateSalesOrderById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesOrder/activatesalesorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetSalesOrderById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesOrder/getsalesorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    
        [Theory]
        [InlineData(1)]
        public async void SearchAllSalesOrder(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesOrder/searchallsalesorders?pageNumber=1&pageSize=10&text=S&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllActiveSalesOrder(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorder/searchactivesalesorders?pageNumber=1&pageSize=10&text=1&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetDefaultValues(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesOrder/getdefaults/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateSalesOrderShouldReturnId(int test)
        {
            List<SalesOrderLineViewModel> salesOrderLines = new List<SalesOrderLineViewModel>();
           
            salesOrderLines.Clear();

            var salesOrder = new SalesOrderViewModel
            {
                PurchaseOrderNumber = "120",
                Status = SalesOrderStatus.SalesOrder,
                DueDate = DateTime.Now,
                SalesPolicy = "A Test",
                IsWebOrder = false,
                FullyPaid = false,
                TotalAmount = 0,
                PaidAmount =  0,
                BillingAddressId = 1,
                PaymentTermId = 1,
                FiscalPeriodId = 1,
                PaymentMethodId = 1,
                OrderType = SalesOrderType.Order,
                DeliveryMethodId = 1,
                ShippingAddressId = 1,
                SalesRepId = "228786fb-0bd0-45a3-8b2a-edd321f38894",
                CustomerId = 1,
                InvoicedDate = DateTime.Now,
                PaymentDueOn = DateTime.Now,
                Active = true,
                WebActive = false,
                CompanyId = 1,
                ShippingStatus = ShippingStatus.Pending,
                SalesOrderLines = salesOrderLines
                
            };

            var json = JsonConvert.SerializeObject(salesOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/salesorder/createneworder", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedSalesOrder = JsonConvert.DeserializeObject<SalesOrderViewModel>(content);
            Assert.True(returnedSalesOrder.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void CreateSalesOrderInvoicedShouldReturnId(int test)
        {
            List<SalesOrderLineViewModel> salesOrderLines = new List<SalesOrderLineViewModel>();

            salesOrderLines.Clear();

            var salesOrder = new SalesOrderViewModel
            {
                PurchaseOrderNumber = "121",
                Status = SalesOrderStatus.SalesInvoice,
                DueDate = DateTime.Now,
                SalesPolicy = "A Test",
                IsWebOrder = false,
                FullyPaid = false,
                TotalAmount = 0,
                PaidAmount = 0,
                BillingAddressId = 1,
                PaymentTermId = 1,
                FiscalPeriodId = 1,
                PaymentMethodId = 1,
                OrderType = SalesOrderType.Invoice,
                DeliveryMethodId = 1,
                ShippingAddressId = 1,
                SalesRepId = "228786fb-0bd0-45a3-8b2a-edd321f38894",
                CustomerId = 1,
                InvoicedDate = DateTime.Now,
                PaymentDueOn = DateTime.Now,
                Active = true,
                WebActive = false,
                CompanyId = 1,
                ShippingStatus = ShippingStatus.Pending,
                SalesOrderLines = salesOrderLines
            };

            var json = JsonConvert.SerializeObject(salesOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/salesorder/createnewinvoice", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SalesOrderViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void CreateSalesOrderQuoteShouldReturnId(int test)
        {
            List<SalesOrderLineViewModel> salesOrderLines = new List<SalesOrderLineViewModel>();

            salesOrderLines.Clear();

            var salesOrder = new SalesOrderViewModel
            {
                PurchaseOrderNumber = "1321",
                SalesOrderCode = "Ramm32323b2b3b2hb3",
                DueDate = DateTime.Now,
                PaymentMethodId =1,
                Status = SalesOrderStatus.SalesQuote,
                SalesPolicy = "A Test",
                IsWebOrder = false,
                FullyPaid = false,
                TotalAmount = 1000,
                PaidAmount = 100000,
                BillingAddressId = 1,
                ShippingAddressId = 1,
                PaymentTermId = 1,
                FiscalPeriodId = 47,
                OrderType = SalesOrderType.Quote,
                DeliveryMethodId = 1,
                SalesRepId = "b0a18508-391c-4bc1-8320-71c9f4176e1c",
                CustomerId = 1,
                ShippingCost = 3330,
                ShippingStatus = ShippingStatus.PartiallyShipped,
                InvoicedDate = DateTime.Now,
                PaymentDueOn = DateTime.Now,
                Active = true,
                WebActive = false,
                CompanyId = 1,
                SalesOrderLines = salesOrderLines
            };

            var json = JsonConvert.SerializeObject(salesOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/salesorder/createnewquote", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedSalesOrder = JsonConvert.DeserializeObject<SalesOrderViewModel>(content);
            Assert.True(returnedSalesOrder.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateSalesOrderShouldReturnId(int test)
        {
            List<SalesOrderLineViewModel> salesOrderLines = new List<SalesOrderLineViewModel>();

            salesOrderLines.Clear();

            int id = 9;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorder/getsalesorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var salesorder = JsonConvert.DeserializeObject<SalesOrderViewModel>(content);
            salesorder.CustomerId = 2;
            salesorder.FullyPaid = true;
            salesorder.PaidAmount = 1000;
            salesorder.SalesOrderLines = salesOrderLines;
            var json = JsonConvert.SerializeObject(salesorder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/salesorder/updateorder", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SalesOrderViewModel>(contents);
            Assert.True(returnedProduct.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteSalesOrderShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/salesorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllSalesOrderStatus(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorder/getsalesorderstatuses");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetAllSalesOrderTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/salesorder/getsalesordertypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


    }
}
