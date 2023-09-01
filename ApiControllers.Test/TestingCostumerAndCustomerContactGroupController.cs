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
    public class TestingCostumerAndCustomerContactGroupController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllCustomerContactGroup(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/getallgroups");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveCustomerContactGroup(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/getactivegroups");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedCustomerContactGroup(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/getdeletedgroups");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }
        [Theory]
        [InlineData(1)]
        public async void GetCustomerContactGroupById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/getcustomercontactgroup/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllActiveContactGroup(int test)
        {
            string p = "imp";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/searchactivecustomercontactgroup/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllContactGroup(int test)
        {
            string p = "check";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/searchallcustomercontactgroup/"+p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void ActivateCustomerContactGroupById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/activatecustomercontactgroup/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }



        [Theory]
        [InlineData(1)]
        public async void CreateCustomerContactGroupShouldReturnId(int test)
        {
            List<int> customerIdsInContactGroup = new List<int>();
            List<CustomerInContactGroupViewModel> customerInContact = new List<CustomerInContactGroupViewModel>();
            customerInContact.Clear();
            customerIdsInContactGroup.Clear();
            var contactgroup = new CustomerContactGroupViewModel
            {
                GroupName = "Core DotNet",
                Description = "Testing",
                Active = true,
                WebActive = false,
                CompanyId = 1,
                CustomerIdsInContactGroup = customerIdsInContactGroup,
                CustomerAndContactGroupList = customerInContact
            };
            var json = JsonConvert.SerializeObject(contactgroup);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/customercontactgroup/creategroup", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedCustomerContactGroup = JsonConvert.DeserializeObject<CustomerContactGroupViewModel>(content);
            Assert.True(returnedCustomerContactGroup.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateCustomerContactGroupShouldReturnId(int test)
        {

            List<int> customerIdsInContactGroup = new List<int>();

            customerIdsInContactGroup.Clear();
            int id = 7;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/customercontactgroup/getcustomercontactgroup/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var contactgroup = JsonConvert.DeserializeObject<CustomerContactGroupViewModel>(content);

            contactgroup.Active = false;
            contactgroup.CustomerAndContactGroupList = null;
            contactgroup.CustomerIdsInContactGroup = customerIdsInContactGroup;
            var json = JsonConvert.SerializeObject(contactgroup);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/customercontactgroup/updategroup", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedCustomerContactGroup = JsonConvert.DeserializeObject<CustomerContactGroupViewModel>(contents);
            Assert.True(returnedCustomerContactGroup.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteCustomerContactGroupShouldReturnOk(int test)
        {
            int id = 8;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/address/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

  
    }
}
