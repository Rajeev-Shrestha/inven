using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace ApiControllers.Test
{
    public class TestingCompanyAndComapnyController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void CreateCompanyShouldReturnId(int test)
        {
            var company = new CompanyViewModel()
            {
                Name = "Ram Janaki Technology",
                Active = true,
                Email = "ramjanaki@gmail",
                PhoneNumber = "051523248",
                WebsiteUrl = "www.ramjanaki.com",
                Address = "Shantinagar-34,Kathmandu",
                IsCompanyInitialized = true,
                FiscalYearFormat = FiscalYearFormat.WithPrefix,
                WebActive = true,
            };
            var json = JsonConvert.SerializeObject(company);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/company/createcompany", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedCompanyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(content);
            Assert.True(returnedCompanyViewModel.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateCompanyShouldReturnId(int test)
        {
            int id = 3;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/company/getcompanybyid/" + id);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var company = JsonConvert.DeserializeObject<CompanyViewModel>(content);
            company.Name = "Ramjanaki Technology Update";
            company.Active = false;
            var json = JsonConvert.SerializeObject(company);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/company/updatecompany", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedCompany = JsonConvert.DeserializeObject<CompanyViewModel>(result);
            Assert.True(returnedCompany.Id > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetCompanyByLoggedInUserId(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/company/getcompany");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedCompany = JsonConvert.DeserializeObject<CompanyViewModel>(content);
            Assert.True(returnedCompany.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void CheckCompanyIsInitializedByUserId(int test)
        {
            string userId = "de7f2a66-106c-4d6d-8f39-cb5277574487";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/company/CheckIfCompanyInitialized/" + userId);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.Equals("False")||content.Equals("True"));
        }

        [Theory]
        [InlineData(1)]
        public async void CheckEStoreIsInitializedByUserId(int test)
        {
            string userId = "de7f2a66-106c-4d6d-8f39-cb5277574487";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/company/CheckIfEstoreInitialized/" + userId);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.Equals("False") || content.Equals("True"));
        }

        [Theory]
        [InlineData(1)]
        public async void CreateNewCompanyAndSendItsUserEmailAndPasswordToCustomer(int test)
        {
            string email = "ramchandra@nepal";
            string company = "Ramesh Textile Company";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/company/GetUserEmailAndCompanyNameAfterPurchase/" + email+","+company);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllCompanies(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/company/getallcompanies?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }
        [Theory]
        [InlineData(1)]
        public async void DeleteCompanyById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/company/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");
        }

        [Theory]
        [InlineData(1)]
        public async void GetCompanyById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/company/getcompanybyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


      
    }
}
