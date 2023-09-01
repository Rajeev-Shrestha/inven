using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HrevertCRM.Store.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApiConfigurationService _service;

        public HomeController(ApiConfigurationService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()

        {
            //    var token = _service.BusinessServiceAccessToken;
            //   var disco = await DiscoveryClient.GetAsync(_service.ServiceEndPoint);
            ////var email=  User.Claims.Where(w => w.Type == ClaimTypes.Email).FirstOrDefault().Value;
            //   // var id = User.Claims.Where(w => w.Type == ClaimTypes.Sid).FirstOrDefault().Value;
            //    // request token
            //    var tokenClient = new TokenClient(disco.TokenEndpoint, "HrevertCRMStore", "secret");
            //    var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("admin@hrevert", "p@77w0rd", "apis offline_access profile");

            //    //var tokenClient = new TokenClient(disco.TokenEndpoint, "HrevertCRMStoreClient", "secret");

            //    //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("apis");

            //    if (tokenResponse.IsError)
            //    {
            //        Console.WriteLine(tokenResponse.Error);
            //        ViewData["Message"] = tokenResponse.Error;
            //    }


            //    ViewData["Message"] = tokenResponse.Json;

            //    var client = new HttpClient();
            //    client.SetBearerToken(tokenResponse.AccessToken);

            //    var response = await client.GetAsync(Constants.TokenAuthorityUrl + "/api/EmailSetting/getallemailsettings");
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        Console.WriteLine(response.StatusCode);
            //    }

            //    var content = response.Content.ReadAsStringAsync().Result;
            //  //  Console.WriteLine(JArray.Parse(content));


            ViewData["Title"] = "Home Page";
            return View();
            
        }

        [Route("items/{param?}")]
        public IActionResult Items(string param="")
        {
            ViewData["Title"] = "Product Page";
            return View();
        }

        [Authorize]
        [Route("checkout")]
        public IActionResult CheckOut()
        {
            ViewData["Title"] = "Checkout Page";
            return View();
        }
        

        public IActionResult AllItemList()
        {
            ViewData["Title"] = "All Items Page";
            return View();
        }

        public IActionResult RegisterUser()
        {
            ViewData["Title"] = "Register User Page";
            return View();
        }

        public IActionResult PlaceOrder()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Category()
        {
            return View();
        }

        public IActionResult CartDetails()
        {
            return View();
        }

        public IActionResult Compare()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

    }
}
