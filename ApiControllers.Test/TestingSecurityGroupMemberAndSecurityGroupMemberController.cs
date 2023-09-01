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
    public class TestingSecurityGroupMemberAndSecurityGroupMemberController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveSecurityGroupMember(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securitygroupmember/getallsecuritygroupmembers");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetSecurityGroupMemberById(int test)
        {
            int securityGroupId = 2;
            string memberId= "b0a18508-391c-4bc1-8320-71c9f4176e1c";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securitygroupmember/GetSecurityGroupMember/"+ securityGroupId + "/"+ memberId);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetMemberById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/securitygroupmember/getsecuritymember/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateSecurityGroupMemberShouldReturnId(int test)
        {
            List<string> membersOfGroupList = new List<string>();
            membersOfGroupList.Clear();

            var securityGroupMember = new SecurityGroupMemberViewModel
            {
                MemberId = "b0a18508-391c-4bc1-8320-71c9f4176e1c",
                SecurityGroupId = 3,
                WebActive = false,
                CompanyId = 1,
                Active = true,
                MembersOfGroupList = membersOfGroupList
            };

            var json = JsonConvert.SerializeObject(securityGroupMember);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/securitygroupmember/createsecuritygroup", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedProduct = JsonConvert.DeserializeObject<SecurityGroupMemberViewModel>(content);
            Assert.True(returnedProduct.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void DeleteSecurityGroupMemberShouldReturnOk(int test)
        {
            int id = 9;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/securitygroupmember/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void SaveMembersInGroupShouldReturnId(int test)
        {
            int id = 1;
            var membersList= new List<string>();
            membersList.Add("b0a18508-391c-4bc1-8320-71c9f4176e1c");
            var json = JsonConvert.SerializeObject(membersList);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response =  await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/securitygroupmember/savemembersingroup/" + id +','+ membersList, stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

    }
}
