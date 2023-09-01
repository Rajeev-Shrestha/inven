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
    public class TestingAddressAndAddressController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();
         [Theory]
         [InlineData(1)]
        public async void GetAllAddresses(int test)
        {
         
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/address/getalladdresses");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateAddressShouldReturnId(int test)
        {
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
                Telephone = "05125230",
                MobilePhone = "9845886831",
                Fax = "test121",
                Email = "address@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Contact,
                VendorId = 1,
                IsDefault = true,
                Active = true,
                CompanyId = 1
            };

            var json = JsonConvert.SerializeObject(address);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/address/createaddress", stringContent);
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
        public async void UpdateAddressShouldReturnId(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/address/getaddress/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var address = JsonConvert.DeserializeObject<AddressViewModel>(content);

            address.Active = true;
            address.Fax = "231";
            address.ZipCode = "5541";
            address.Email="davidhudsonn@gmail.com";
            var json = JsonConvert.SerializeObject(address);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/address/updateaddress", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedAddress = JsonConvert.DeserializeObject<AddressViewModel>(result);
            Assert.True(returnedAddress.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteAddressShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/address/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllAddressById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/address/getaddress/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetAllTitlesTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/address/gettitletypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
        [Theory]
        [InlineData(1)]

        public async void GetAllSuffixTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/address/getsuffixtypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetAllAddressTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/address/getaddresstypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    }
}
