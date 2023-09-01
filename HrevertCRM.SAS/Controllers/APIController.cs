using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HrevertCRM.SAS.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        private readonly ApiConfigurationService _service;

        public ApiController(ApiConfigurationService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("getdata")]
        public async Task<ObjectResult> GetData(string apiUrl)

        {
            //TODO: Make sure only this is availble to client store, ie this app

            var token = _service.BusinessServiceAccessToken;

            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.GetAsync(_service.ServiceEndPoint + apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            return Ok(content);
        }
        [Route("postdata")]
        public async Task<ObjectResult> PostData(string apiUrl)

        {
            //TODO: Make sure only this is availble to client store, ie this app

            var token = _service.BusinessServiceAccessToken;

            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.PostAsync(_service.ServiceEndPoint + apiUrl, null);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            return Ok(content);
        }
        [HttpPost]
        [Route("postwithdata")]
        public async Task<ObjectResult> PostWithData([FromBody]PostDataViewModel model)

        {
            //TODO: Make sure only this is availble to client store, ie this app

            var token = _service.BusinessServiceAccessToken;

            var client = new HttpClient();
            client.SetBearerToken(token);
            var json = JsonConvert.SerializeObject(model.Data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_service.ServiceEndPoint + model.Url, stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            return Ok(content);
        }

        // for put

        [HttpPut]
        [Route("putwithdata")]
        public async Task<ObjectResult> PutWithData([FromBody]PostDataViewModel model)

        {
            //TODO: Make sure only this is availble to client store, ie this app

            var token = _service.BusinessServiceAccessToken;

            var client = new HttpClient();
            client.SetBearerToken(token);
            var json = JsonConvert.SerializeObject(model.Data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(_service.ServiceEndPoint + model.Url, stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            return Ok(content);
        }

        [HttpGet]
        [Route("getcrmurl")]
        public ObjectResult GetCrmUrl()
        {
            return Ok(_service.ServiceEndPoint);
        }
        [HttpGet]
        [Route("getloggedinuser")]
        public ObjectResult GetLoggedInUser()

        {
            if (User.Identity.IsAuthenticated)
            {
                //var user = User.Claims;
                //return Ok(user);
                return Ok(User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.Sid).Value);
            }
            else
            {
                return Ok("Login");
            }

        }

        [HttpGet]
        [Route("logoutUser")]
        public ObjectResult LogoutUser()
        {
            return Ok(User.Identity.IsAuthenticated == false);
        }
    }

    public class PostDataViewModel
    {
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
