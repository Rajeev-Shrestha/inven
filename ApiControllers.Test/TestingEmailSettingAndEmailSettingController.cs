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
    public class TestingEmailSettingAndEmailSettingController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();



        [Theory]
        [InlineData(1)]
        public async void GetAllEmailSettings(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/getallemailsettings");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveEmailSettings(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/getactiveemailsettings");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetDeletedEmailSettings(int test)
        {

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/getdeletedemailsettings");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void SearchAllEmailSettings(int test)
        {
            string p = "c";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/searchallemailsettings/" + p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SearchActiveEmailSettings(int test)
        {
            string p = "d";
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/searchactiveemailsettings/" + p);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]

        public async void ActivateEmailSetting(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/activateemailsetting/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]

        public async void GetEmailSettingById(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/getemailbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }


        [Theory]
        [InlineData(1)]
        public async void CreateEmailSettingShouldReturnId(int test)
        {
            var emailSetting = new EmailSettingViewModel
            {
                Host = "192.168.2.175",
                Port = 8080,
                Sender = "Hrevert Tech",
                UserName = "ram@gmail.com",
                Password = "p@77w0rd",
                Name = "Ramesh",
                RequireAuthentication = true,
                EncryptionType = Hrevert.Common.Enums.EncryptionType.SSL,
                WebActive = false,
                Active = true,
                CompanyId =1,
                ConfirmPassword = "p@77w0rd"

            };

            var json = JsonConvert.SerializeObject(emailSetting);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/emailsetting/createemailsetting", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedEmailSetting = JsonConvert.DeserializeObject<EmailSettingViewModel>(content);
            Assert.True(returnedEmailSetting.Id > 0);


        }


        [Theory]
        [InlineData(1)]
        public async void UpdateEmailSettingShouldReturnId(int test)
        {

            int id = 3;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/getemailbyid/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var emailSetting = JsonConvert.DeserializeObject<EmailSettingViewModel>(content);

            emailSetting.Name = "Rambabu Prasad Kushwaha";
            emailSetting.Password = "Qwerty@7";
            emailSetting.ConfirmPassword = "Qwerty@7";
            var json = JsonConvert.SerializeObject(emailSetting);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/emailsetting/updateemailsetting", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedEmailSetting = JsonConvert.DeserializeObject<EmailSettingViewModel>(result);
            Assert.True(returnedEmailSetting.Id > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void DeleteEmailSettingReturnOk(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/emailsetting/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");

        }

        [Theory]
        [InlineData(1)]
        public async void GetAllEncryptionTypes(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/emailsetting/getencryptiontypes");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count>0);

        }


    }
}
