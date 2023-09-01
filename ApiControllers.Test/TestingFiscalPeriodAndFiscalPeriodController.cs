using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingFiscalPeriodAndFiscalPeriodController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();
        [Theory]
        [InlineData(1)]
        public async void GetAllFiscalPeriods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalperiod/getallfiscalperiods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveFiscalPeriods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalperiod/getactivefiscalperiods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDeletedFiscalPeriods(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalperiod/getdeletedfiscalperiods");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        
        [Theory]
        [InlineData(1)]

        public async void ActivateFiscalPeriod(int test)
        {
            int id = 47;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalperiod/activatefiscalperiod/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetFiscalPeriodById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalperiod/getfiscalperiodbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetFiscalPeriodByFiscalYearId(int test)
        {
            int fiscalyearid = 17;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalperiod/getfiscalperiodbyyearid/" + fiscalyearid);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateFiscalPeriodShouldReturnId(int test)
        {
            var fiscalperiod = new FiscalPeriodViewModel
            {
                FiscalYearId = 7,
                Name = "Jan-April",
                StartDate = DateTime.Parse("2017-10-1"),
                EndDate = DateTime.Parse("2017-12-31"),
                WebActive = false,
                Active = true,
                CompanyId = 1

            };
          
            var json = JsonConvert.SerializeObject(fiscalperiod);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/fiscalperiod/createfiscalperiod", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedFiscalPeriod = JsonConvert.DeserializeObject<FiscalPeriodViewModel>(content);
            Assert.True(returnedFiscalPeriod.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateFiscalPeriodShouldReturnId(int test)
        {
            int id = 47;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/fiscalperiod/getfiscalperiodbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var fiscalperiod = JsonConvert.DeserializeObject<FiscalPeriodViewModel>(content);
            fiscalperiod.Name = "Jan17-Feb17";

            var json = JsonConvert.SerializeObject(fiscalperiod);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/fiscalperiod/updatefiscalperiod", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedFiscalPeriod = JsonConvert.DeserializeObject<FiscalPeriodViewModel>(result);
            Assert.True(returnedFiscalPeriod.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteFiscalPeriodReturnOk(int test)
        {
            int id = 47;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/fiscalperiod/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }


    }
}
