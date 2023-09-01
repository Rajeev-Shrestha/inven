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
    public class TestingPaymentMethodAndPaymentMethodController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllPaymentMethods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/getallpaymentmethods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActivePaymentMethods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/getactivepaymentmethods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedPaymentMethods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/getdeletedpaymentmethods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchActivePaymentMethods(int test)
        {
            string p = "c";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/searchactivepaymentmethods/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllPaymentMethods(int test)
        {
            string p = "d";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/searchallpaymentmethods/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void ActivatePaymentMethod(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/activatepaymentmethod/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void DeletePaymentMethodShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/paymentmethod/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content=="");
        }


        [Theory]
        [InlineData(1)]
        public async void CreatePaymentMethodShouldReturnId(int test)
        {
            var paymentMethod = new PaymentMethodViewModel
            {
                MethodCode = "KODE",
                MethodName = "Method Name",
                ReceipentMemo = "test",
                WebActive = false,
                CompanyId = 1,
                Active = true

            };
           

            var json = JsonConvert.SerializeObject(paymentMethod);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/paymentmethod", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedPaymentMethod = JsonConvert.DeserializeObject<PaymentMethodViewModel>(content);
            Assert.True(returnedPaymentMethod.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdatePaymentMethodShouldReturnId(int test)
        {
           
            int id = 15;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/getpaymentmethodbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var paymentMethod = JsonConvert.DeserializeObject<PaymentMethodViewModel>(content);

            paymentMethod.MethodName = "Payment Method Test Update";

            var json = JsonConvert.SerializeObject(paymentMethod);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/paymentmethod", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedPaymentMethod = JsonConvert.DeserializeObject<PaymentMethodViewModel>(contents);
            Assert.True(returnedPaymentMethod.Id > 0);


        }

       
        [Theory]
        [InlineData(1)]

        public async void GetPaymentMethodById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentmethod/getpaymentmethodbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        

    }
}
