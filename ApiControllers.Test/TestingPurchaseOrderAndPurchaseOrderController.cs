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
    public class TestingPurchaseOrderAndPurchaseOrderController
    {

        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllPurchaseOrder(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getallpurchaseorders?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActivePurchaseOrder(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getactivepurchaseorders?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedPurchaseOrder(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getdeletedpurchaseorders?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }



        [Theory]
        [InlineData(1)]
        public async void ActivatePurchaseOrderById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/activatpurchaseorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetPurchaseOrderById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getpurchaseorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllPurchaseOrder(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/searchallpurchaseorders?pageNumber=1&pageSize=10&text=S&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllActivePurchaseOrder(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/searchactivepurchaseorders?pageNumber=1&pageSize=10&text=S&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]  
        [InlineData(1)]

        public async void GetDefault(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getdefaults/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreatePurchaseOrderShouldReturnId(int test)
        {
            List<PurchaseOrderLineViewModel> purchseOrderLines = new List<PurchaseOrderLineViewModel>();

            purchseOrderLines.Clear();

            var purchaseOrder = new PurchaseOrderViewModel
            {
                SalesOrderNumber = "1",
                OrderDate = DateTime.Now,
                OrderType = PurchaseOrderType.Order,
                DueDate = DateTime.Now,
                PurchaseOrderCode = "wkjfkljf",
                Status = PurchaseOrderStatus.CreditOrder,
                FullyPaid = true,
                TotalAmount = 1000,
                PaymentDueOn = DateTime.Now,
                InvoicedDate = DateTime.Now,
                PaidAmount = 100,
                DeliveryMethodId = 1,
                FiscalPeriodId = 1,
                PaymentTermId = 1,
                BillingAddressId = 1,
                ShippingAddressId = 1,
                PurchaseRepId = "7c267886-cecc-464e-abe4-0b06a32f0ae6",
                VendorId = 1,
                CompanyId = 1,
                WebActive = true,
                PurchaseOrderLines = purchseOrderLines
                
            };
            var json = JsonConvert.SerializeObject(purchaseOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/purchaseorder/createneworder", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<PurchaseOrderViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void CreatePurchaseOrderInvoiceShouldReturnId(int test)
        {
            List<PurchaseOrderLineViewModel> purchseOrderLines = new List<PurchaseOrderLineViewModel>();

            purchseOrderLines.Clear();

            var purchaseOrder = new PurchaseOrderViewModel
            {
                SalesOrderNumber = "1",
                OrderDate = DateTime.Now,
                OrderType = PurchaseOrderType.Invoice,
                DueDate = DateTime.Now,
                Status = PurchaseOrderStatus.CreditOrder,
                FullyPaid = true,
                TotalAmount = 1000,
                PaymentDueOn = DateTime.Now,
                InvoicedDate = DateTime.Now,
                PaidAmount = 100,
                DeliveryMethodId = 1,
                FiscalPeriodId = 1,
                PaymentTermId = 1,
                BillingAddressId = 1,
                ShippingAddressId = 1,
                PurchaseRepId = "7c267886-cecc-464e-abe4-0b06a32f0ae6",
                VendorId = 1,
                CompanyId = 1,
                WebActive = true,
                PurchaseOrderLines = purchseOrderLines

            };
            var json = JsonConvert.SerializeObject(purchaseOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/purchaseorder/createnewinvoice", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<PurchaseOrderViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void CreatePurchaseOrderQuoteShouldReturnId(int test)
        {
            List<PurchaseOrderLineViewModel> purchseOrderLines = new List<PurchaseOrderLineViewModel>();

            purchseOrderLines.Clear();

            var purchaseOrder = new PurchaseOrderViewModel
            {
                SalesOrderNumber = "1",
                OrderDate = DateTime.Now,
                OrderType = PurchaseOrderType.Quote,
                DueDate = DateTime.Now,
                Status = PurchaseOrderStatus.CreditQuote,
                FullyPaid = true,
                TotalAmount = 1000,
                PaymentDueOn = DateTime.Now,
                InvoicedDate = DateTime.Now,
                PaidAmount = 100,
                DeliveryMethodId = 1,
                FiscalPeriodId = 1,
                PaymentTermId = 1,
                BillingAddressId = 1,
                ShippingAddressId = 1,
                PurchaseRepId = "7c267886-cecc-464e-abe4-0b06a32f0ae6",
                VendorId = 1,
                CompanyId = 1,
                WebActive = true,
                PurchaseOrderLines = purchseOrderLines

            };
            var json = JsonConvert.SerializeObject(purchaseOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/purchaseorder/createnewqoute", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<PurchaseOrderViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdatePurchaseOrderShouldReturnId(int test)
        {
            List<PurchaseOrderLineViewModel> purchseOrderLines = new List<PurchaseOrderLineViewModel>();

            purchseOrderLines.Clear();


            int id = 9;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getpurchaseorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var purchaseOrder = JsonConvert.DeserializeObject<PurchaseOrderViewModel>(content);

            purchaseOrder.TotalAmount = 1000;
            purchaseOrder.FullyPaid = true;
            purchaseOrder.PaymentDueOn=DateTime.Now.AddDays(3);
            purchaseOrder.PurchaseOrderLines = purchseOrderLines;

            var json = JsonConvert.SerializeObject(purchaseOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/purchaseorder/updatepurchaseorder", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<PurchaseOrderViewModel>(contents);
            Assert.True(returnedProduct.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeletePurchaseOrderShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/purchaseorder/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllPurchaseOrderStatus(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getpurchaseorderstatuses");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetAllPurchaseOrderTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/purchaseorder/getpurchaseordertypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    }
}
