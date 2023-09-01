using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingSecurityGroupAndSecurityGroupController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveSecurityGroup(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securitygroup/getallsecuritygroup");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetSecurityGroupById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securitygroup/GetSecurityGroup/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        

        [Theory]
        [InlineData(1)]
        public async void CreateSecurityGroupShouldReturnId(int test)
        {
           
            var securityGroupMember = new SecurityGroupViewModel
            {
              GroupName = "Super Admin",
              GroupDescription = "A test Description",
              CompanyId = 1,
              Active =  true,
              WebActive = true
            };

            var json = JsonConvert.SerializeObject(securityGroupMember);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/securitygroup/createsecuritygroup", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SecurityGroupViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void AssignSecurityToGroupShouldReturnId(int test)
        {
           
            var assignsecurityToGroup = new AssignSecurityToGroupViewModel
            {
                GroupId = 1,
                IsAssigned = true,
                SecurityId = 1
            };

            var json = JsonConvert.SerializeObject(assignsecurityToGroup);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/securitygroup/assignsecuritytogroup", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<AssignSecurityToGroupViewModel>(content);
            Assert.True(returnedProduct.SecurityId > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateSecurityGroupShouldReturnId(int test)
        {

            int id = 9;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securitygroup/GetSecurityGroup/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var securityGroup = JsonConvert.DeserializeObject<SecurityGroupViewModel>(content);

            securityGroup.GroupName = "A GroupName Update";
            securityGroup.GroupDescription = "A GroupDescription Update";

            var json = JsonConvert.SerializeObject(securityGroup);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/securitygroup/updatesecuritygroup", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SecurityGroupViewModel>(contents);
            Assert.True(returnedProduct.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteSecurityGroupShouldReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/securitygroup/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetSecurityAssignedToGroup(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securitygroup/getsecurityassignedtogroup/"+id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    }
}
