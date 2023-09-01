using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingVendorAndVendorController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]

        public async void GetAllVendor(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/vendor/getallvendors?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveVendor(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/vendor/getactivevendors?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedVendor(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/vendor/getdeletedvendors?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetVendorById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/vendor/getvendorbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetVendorActivatedById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/vendor/activatevendor/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteVendorShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/vendor/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }


        [Theory]
        [InlineData(1)]
        public async void CreateVendorShouldReturnId(int test)
        {
            List<AddressViewModel> addresses = new List<AddressViewModel>();
            List<PurchaseOrderViewModel>purchaseOrders=new List<PurchaseOrderViewModel>();
            purchaseOrders.Clear();
            var billingAddress = new AddressViewModel
            {
                Title = Hrevert.Common.Enums.TitleType.Mr,
                FirstName = "Ramesh Suresh",
                MiddleName = "Prasad",
                LastName = "Kushwaha",
                Suffix = Hrevert.Common.Enums.SuffixType.Businessman,
                AddressLine1 = "Birgunj-19",
                AddressLine2 = "Birgunj-3",
                City = "Birgunj",
                State = "Central",
                ZipCode = "44013",
                CountryId = 1,
                Telephone = "05152230",
                MobilePhone = "9845886831",
                Fax = "test121",
                Email = "vendor@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Billing,
                IsDefault = true,
                Active = true,
                CompanyId = 1
            };

            var shippingAddress = new AddressViewModel
            {
                Title = Hrevert.Common.Enums.TitleType.Mr,
                FirstName = "Ramesh Suresh",
                MiddleName = "Prasad",
                LastName = "Kushwaha",
                Suffix = Hrevert.Common.Enums.SuffixType.Businessman,
                AddressLine1 = "Birgunj-19",
                AddressLine2 = "Birgunj-3",
                City = "Birgunj",
                State = "Central",
                ZipCode = "44013",
                CountryId = 1,
                Telephone = "0515230",
                MobilePhone = "9845886831",
                Fax = "test121",
                Email = "vendor@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Contact,
                IsDefault = true,
                Active = true,
                CompanyId = 1
            };

            addresses.Add(billingAddress);
            addresses.Add(shippingAddress);

            var vendor = new VendorViewModel
            {
                Code = "V-000002",
                CreditLimit = 321,
                Debit = 21,
                Credit = 122,
                ContactName = "Ramesh Kumar Neupane",
                PaymentMethodId = 1,
                PaymentTermId = 1,
                CompanyId = 1,
                Active = true,
                WebActive = false,
                BillingAddress = billingAddress,
                PurchaseOrders = purchaseOrders,
                Addresses = addresses
            };

            var json = JsonConvert.SerializeObject(vendor);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/vendor/createvendor", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedVendor = JsonConvert.DeserializeObject<VendorViewModel>(content);
            Assert.True(returnedVendor.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void SearchVendorWithString(int test)
        {
            string str = "a";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/vendor/searchvendors/" + str);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateVendorShouldReturnId(int test)
        {
            List<AddressViewModel> addresses = new List<AddressViewModel>();

            var address = new AddressViewModel
            {
                Title = Hrevert.Common.Enums.TitleType.Mr,
                FirstName = "Ramesh Suresh",
                MiddleName = "Prasad",
                LastName = "Kushwaha",
                Suffix = Hrevert.Common.Enums.SuffixType.Businessman,
                AddressLine1 = "Birgunj-19",
                AddressLine2 = "Birgunj-3",
                City = "Birgunj",
                State = "Central",
                ZipCode = "44013",
                CountryId = 1,
                Telephone = "0515230",
                MobilePhone = "9845886831",
                Fax = "test121",
                Email = "abcdef@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Contact,
                VendorId = 1,
                IsDefault = true,
                Active = true,
                CompanyId = 1
            };

            addresses.Add(address);

            int id = 5;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/vendor/getvendorbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var vendor = JsonConvert.DeserializeObject<EditVendorViewModel>(content);

            vendor.ContactName = "Ram Kumar Neupane";

            var json = JsonConvert.SerializeObject(vendor);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/vendor/updatevendor", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedVendor = JsonConvert.DeserializeObject<EditVendorViewModel>(result);
            Assert.True(returnedVendor.Id > 0);


        }


    }
}
