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
    public class TestingFiscalYearAndFiscalYearController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllFiscalYears(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/getallfiscalyears");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveFiscalYears(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/getactivefiscalyears");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDeletedFiscalYears(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/getdeletedfiscalyears");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void searchAllFiscalYears(int test)
        {
            string p = "f";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/searchallfiscalyears/" + p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void searchActiveFiscalYears(int test)
        {
            string p = "1";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/searchactivefiscalyears/" + p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void ActivateFiscalYear(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/activatefiscalyear/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetFiscalYearById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/getfiscalyearbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateFiscalYearShouldReturnId(int test)
        {
            var periods = new  List<FiscalPeriodViewModel>();
            periods.Clear();
            var fiscalyear = new FiscalYearViewModel
            {
                Name = "FY2020/21",
                StartDate = DateTime.Parse("2020/1/1").Date,
                EndDate = DateTime.Parse("2020/12/31").Date,
                FiscalPeriodVieModels = periods,
                CompanyId = 1,
                Active = true,
                WebActive = true
            };

            var json = JsonConvert.SerializeObject(fiscalyear);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/fiscalyear/createfiscalyear", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedAccount = JsonConvert.DeserializeObject<FiscalYearViewModel>(content);
            Assert.True(returnedAccount.Id > 0);

        }


        [Theory]
        [InlineData(1)]
        public async void UpdateFiscalYearShouldReturnId(int test)
        {
            int id = 21;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalyear/getfiscalyearbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var fiscalyear = JsonConvert.DeserializeObject<FiscalYearViewModel>(content);
            var periods = new List<FiscalPeriodViewModel>();
            periods.Clear();
            fiscalyear.Name = "FY-2018/19";
            fiscalyear.FiscalPeriodVieModels = periods;
            var json = JsonConvert.SerializeObject(fiscalyear);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/fiscalyear/updatefiscalyear", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedFiscalYear = JsonConvert.DeserializeObject<FiscalYearViewModel>(result);
            Assert.True(returnedFiscalYear.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteFiscalYearReturnOk(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/fiscalyear/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

     

    }
}
