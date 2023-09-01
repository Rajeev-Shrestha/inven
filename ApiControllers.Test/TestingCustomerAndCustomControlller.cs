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
    public class TestingCustomerAndCustomControlller
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllCustomer(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getallcustomers?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllActiveCustomer(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getactivecustomers?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedCustomer(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getdeletedcustomers?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void ActivateCustomerById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/activatecustomer/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetCustomerById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getcustomerbyid/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllCustomer(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/searchallcustomers?pageNumber=1&pageSize=10&text=S&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllActiveCustomer(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/searchactivecustomers?pageNumber=1&pageSize=10&text=S&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateCustomerShouldReturnId(int test)
        {

            List<AddressViewModel> addresses = new List<AddressViewModel>();
            var address = new AddressViewModel
            {
                
                Title = Hrevert.Common.Enums.TitleType.Mr,
                FirstName = "Ramesh",
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
                Email = "customercreatew@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Contact,
                VendorId = 1,
                IsDefault = true,
                CompanyId = 1,
                Active = true
            };

            var addressViewModel = new AddressViewModel
            {
                Title = Hrevert.Common.Enums.TitleType.Mr,
                FirstName = "Ramesh",
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
                Email = "customercreatew@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Contact,
                VendorId = 1,
                IsDefault = true,
                CompanyId = 1,
                Active = true
            };

            addresses.Add(address);
            var customer = new CustomerViewModel
            {
                CustomerCode = "C-000099",
                Password = "p@77w0rd",
                ConfirmPassword = "p@77w0rd",
                OpeningBalance = 0,
                Note = "Test Description",
                TaxRegNo = "443",
                DisplayName = "Ramesh",
                BillingAddressId = 1,
                PaymentTermId = 1,
                CustomerLevelId = 1,
                Active = true,
                CompanyId = 1,
                WebActive = false,
                BillingAddress = addressViewModel,
                Addresses = addresses
            };
            var json = JsonConvert.SerializeObject(customer);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/customer/createcustomer", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedCustomer = JsonConvert.DeserializeObject<CustomerViewModel>(content);
            Assert.True(returnedCustomer.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateCustomerShouldReturnId(int test)
        {

            int id = 13;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getcustomerbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<EditCustomerViewModel>(content);
            customer.DisplayName="Ramesh Kushwha";
            var json = JsonConvert.SerializeObject(customer);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/customer/updatecustomer", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = response.Content.ReadAsStringAsync().Result;
            var returnedCustomer = JsonConvert.DeserializeObject<EditCustomerViewModel>(contents);
            Assert.True(returnedCustomer.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteCustomerShouldReturnOk(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/customer/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

      
        [Theory]
        [InlineData(1)]

        public async void GetAllTitles(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/gettitles");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
        [Theory]
        [InlineData(1)]

        public async void GetAllSuffix(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getsuffixes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetCustomerCodeCheck(int test)
        {
            string code = "C-000001";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/checkcode/"+code);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void SaveBillingAddressReturnsId(int test)
        {
            var address = new AddressViewModel
            {
                Title = Hrevert.Common.Enums.TitleType.Mr,
                FirstName = "Ramesh Manoj",
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
                Email = "billingaddresscustomer@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Billing,
                CustomerId = 13,
                IsDefault = true,
                Active = true,
                CompanyId = 1,
                VendorId = null,
                DeliveryZoneId = 1

            };
            var json = JsonConvert.SerializeObject(address);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/customer/savecustomeraddress", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedAddress = JsonConvert.DeserializeObject<AddressViewModel>(content);
            Assert.True(returnedAddress.Id > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetAllCustomerAllAddresses(int test)
        {
            int id = 4;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getCustomerAllAddresses/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetCode(int test)
        {
            
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getcode");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetAllCountries(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getcountries");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetCustomerOrderSummary(int test)
        {
            int customerId = 4;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/getorderssummary/"+customerId);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetAllActiveCustomerWithoutPaging(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/activecustomerwithoutpagaing");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetAllDeletedCustomerWithoutPaging(int test)
        {
            
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customer/deletedcustomerswithoutpagaing");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
    }
}
