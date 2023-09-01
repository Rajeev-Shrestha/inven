using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingMeasureUnitAndMeasureUnitController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void CreateMeasureUnitShouldReturnId(int test)
        {
            var measureUnit = new MeasureUnitViewModel
            {
                Measure = "A Measure Unit",
                MeasureCode = "M0003",
                EntryMethod = Hrevert.Common.Enums.EntryMethod.Decimal,
                DeliveryRates = null,
                ItemMeasures = null,
                WebActive = true,
                CompanyId = 1,
                Active = true
            };

            var json = JsonConvert.SerializeObject(measureUnit);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/measureunit/createmeasureunit", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedMeasureUnit = JsonConvert.DeserializeObject<MeasureUnitViewModel>(content);
            Assert.True(returnedMeasureUnit.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void UpdateMeasureUnitShouldReturnId(int test)
        {

            int id = 3;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/getMeasureUnitbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var measureUnit = JsonConvert.DeserializeObject<MeasureUnitViewModel>(content);

            measureUnit.Measure = "A Measure Unit Updated";
            measureUnit.Active = false;
            var json = JsonConvert.SerializeObject(measureUnit);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/measureunit/updatemeasureunit", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedMeasureUnit = JsonConvert.DeserializeObject<MeasureUnitViewModel>(result);
            Assert.True(returnedMeasureUnit.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteMeasureUnitShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/measureunit/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllMeasureUnit(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/getallMeasureUnits");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveMeasureUnit(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/getactiveMeasureUnits");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedMeasureUnit(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/getadeletedMeasureUnits");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetMeasureUnitById(int test)
        {
            int id = 3;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/getMeasureUnitbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetActivatedById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/activateMeasureUnit/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchActiveMeasureUnit(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/searchactiveMeasureUnits?pageNumber=1&pageSize=10&text=s&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void SearchAllMeasureUnit(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/measureunit/searchallMeasureUnits?pageNumber=1&pageSize=10&text=s&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

    }
}
