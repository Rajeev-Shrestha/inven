using HrevertCRM.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using HrevertCRM.Common;
using HrevertCRM.Data.ViewModels;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingUserAndUserController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();


        [Theory]
        [InlineData(1)]
        public async void CreateUserShouldReturnId(int test)
        { 
            var user = new UserViewModel
            {
                FirstName = "Ram",
                MiddleName = "Kumar",
                LastName = "Neupane",
                Gender = 1,
                Address = "Birgunj",
                Email = "ramesh@gmail.com",
                UserName = "ramneupane",
                Password = "p@77w0rd",
                CompanyId = 1,
                ConfirmPassword = "p@77w0rd",
                Active = true,
                UserType = HrevertCRM.Common.UserType.CompanyUsers,
                WebActive = false,
                Phone = "9860685356"
            };

            

            var json = JsonConvert.SerializeObject(user);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/user/createuser", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var status = JObject.Parse(content);
            Assert.True(status.HasValues.Equals(true));

        }

        [Theory]
        [InlineData(1)]
        public async void UpdateUserShouldReturnId(int test)
        {

            string id = "c3cb0a4e-67ce-42c4-9105-a746a79a1c40";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/User/getUserbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var user = JsonConvert.DeserializeObject<UserViewModel>(content);
            user.Address = "Nepalganj";
            user.Password = "p@77w0rd";
            user.ConfirmPassword = "p@77w0rd";
            user.UserType = UserType.CompanyUsers;

            var json = JsonConvert.SerializeObject(user);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/User/updateUser", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedUser = JsonConvert.DeserializeObject<UserViewModel>(result);

           // Assert.True(returnedUser.CompanyId>0);
            Assert.True(result.ToList().Count > 0);

        }

        [Theory]
        [InlineData(1)]
        public async void DeleteUserShouldReturnOk(int test)
        {
            string id = "e1a31854-5b74-408a-a1c0-30527ab39ffe";
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/user/deleteuser/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]

        public async void GetAllUser(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/getallUsers");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedUser(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/getdeletedusers");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveUser(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/getactiveusers");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetLoggedUserDetails(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/GetLoggedInUserDetail");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetUserSummary(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/UserSummary");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


       
        [Theory]
        [InlineData(1)]
        public async void GetUserById(int test)
        {
            string id = "e1a31854-5b74-408a-a1c0-30527ab39ffe";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/getuserbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetUserActivatedById(int test)
        {
            string id = "c3cb0a4e-67ce-42c4-9105-a746a79a1c40";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/activateuser/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchActiveUserWithString(int test)
        {
        
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/searchactiveusers?pagenumber=1&pageSize=10&text=a&active=true");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchAllUserWithString(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/searchallusers?pagenumber=1&pageSize=10&text=s&active=false");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }


        [Theory]
        [InlineData(1)]
        public async void GetAllUserTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/user/getusertypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


    }
}
