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
    public class TestingJournalMasterAndJournalMasterController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void GetAllJournalMasters(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/getalljournalmasters");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveJournalMasters(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/getactivejournalmasters");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDeletedJournalMasters(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/getdeletedjournalmasters");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchActiveJournalMasters(int test)
        {
            string p = "2";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/searchactivejournalmasters/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllJournalMasters(int test)
        {
            string p = "1";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/searchalljournalmasters/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]

        public async void ActivateJournalMaster(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/activatejournalmaster/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetJournalMasterById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/getjournalmasterbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]

        public async void GetJournalMasterByFiscalYearId(int test)
        {
            int fiscalyearid = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/getjournalmasterbyid/" + fiscalyearid);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateJournalMasterShouldReturnId(int test)
        {
            var journalMaster = new JournalMasterViewModel
            {
                JournalType = Hrevert.Common.Enums.JournalType.General,
                Description = "journal description create",
                Debit = 100,
                Credit = 40,
                Note = "A sample note",
                Posted = false,
                Printed = false,
                Cancelled = false,
                IsSystem = false,
                WebActive = false,
                Active = true
            };

            var json = JsonConvert.SerializeObject(journalMaster);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/journalmaster/createjournalmaster", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedJournalMaster = JsonConvert.DeserializeObject<JournalMasterViewModel>(content);
            Assert.True(returnedJournalMaster.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateJournalMasterShouldReturnId(int test)
        {
            int id = 11;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/getjournalmasterbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var journalMaster = JsonConvert.DeserializeObject<JournalMasterViewModel>(content);
            journalMaster.Description = "Journal Description Update";
            journalMaster.Note = "Note changed";

            var json = JsonConvert.SerializeObject(journalMaster);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/journalmaster/updatejournalmaster", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedJournalMaster = JsonConvert.DeserializeObject<JournalMasterViewModel>(result);
            Assert.True(returnedJournalMaster.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteJournalMasterReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/journalmaster/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]
        public async void GetAllJournalMasterTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/journalmaster/getjournaltypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count>0);

        }

    }
}
