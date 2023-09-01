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
    public class TestingPaymentTermAndPaymentTermController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllPaymentTerm(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getallpaymentterms");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActivePaymentTerm(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getactivepaymentterms");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedPaymentTerm(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getdeletedpaymentterms");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchActivePaymentTerms(int test)
        {
            string p = "c";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/searchactivepaymentterms/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllPaymentTerms(int test)
        {
            string p = "d";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/searchallpaymentterms/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void ActivatePaymentTermById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/activatepaymenterm/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void DeletePaymentTermShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/paymentterm/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllPaymentTermById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getpaymenttermbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreatePaymentTermShouldReturnId(int test)
        {
            var paymentterm = new PaymentTermViewModel
            {
                Description = "Payment Term Test",
                TermCode = "P0T",
                TermName = "Power Point",
                DueDateType = Hrevert.Common.Enums.DueDateType.None,
                TermType = Hrevert.Common.Enums.TermType.OnAccount,
                DiscountValue = 0,
                DiscountDays = 0,
                DiscountType = Hrevert.Common.Enums.PaymentDiscountType.None,
                CompanyId = 1,
                WebActive = false,
                Active = true
            };
           
            var json = JsonConvert.SerializeObject(paymentterm);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/paymentterm", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedPaymentTerm = JsonConvert.DeserializeObject<PaymentTermViewModel>(content);
            Assert.True(returnedPaymentTerm.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdatePaymentTermShouldReturnId(int test)
        {
            int id = 7;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getpaymenttermbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var paymentterm = JsonConvert.DeserializeObject<PaymentTermViewModel>(content);

            paymentterm.Description = "Payment Term Test Update";

            var json = JsonConvert.SerializeObject(paymentterm);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/paymentterm", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedPaymentTerm = JsonConvert.DeserializeObject<PaymentTermViewModel>(contents);
            Assert.True(returnedPaymentTerm.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void GetAllTermTypes(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/gettermtypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllPaymentDiscountTypes(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getpaymentdiscounttypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDueTypes(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getduetypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDueDateTypes(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getduedatetypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDueDateById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getduedate/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetPaymentTermsWithoutOnAccountType(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getTermsWithoutAccountType");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetPayMethodsInPayTerms(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/paymentterm/getPayMethodsInPayTerm/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [InlineData(1)]
        public async void SavePaymentInPayTerms(int test)
        {

            var ids =new  List<int>();
            ids.Add(1);
            ids.Add(2);
            var paymentterm = new PayMethodInPayTermViewModel
            {
               PayTermId = 1,
               PayMethodIds = ids,
                CompanyId = 1,
                WebActive = false,
                Active = true
            };

            var json = JsonConvert.SerializeObject(paymentterm);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/paymentterm/savePayMethodInPayTerms", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
    }
}
