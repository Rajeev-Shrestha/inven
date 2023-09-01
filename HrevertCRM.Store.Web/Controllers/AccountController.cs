using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HrevertCRM.Store.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HrevertCRM.Store.Web.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ApiConfigurationService _service;

        public AccountController(ApiConfigurationService service)
        {
            _service = service;
        }

        public async void SignOut()
        {
            await HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");
        }
        [HttpPost("~/login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                const string Issuer = "https://hrevertCRM.com";
                var controller = new ApiController(_service);
                var data = await controller.PostData(String.Format("/api/customer/login/{0}/{1}",model.Email,model.Password));
             var loginResult=    JsonConvert.DeserializeObject<CustomerLoginResultViewModel>(data.Value.ToString());
                if (loginResult.LoggedIn)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginResult.FirstName, ClaimValueTypes.String, Issuer),
                        new Claim(ClaimTypes.Surname,  loginResult.LastName, ClaimValueTypes.String, Issuer),
                        new Claim(ClaimTypes.Email,  loginResult.Email, ClaimValueTypes.String, Issuer),
                        new Claim(ClaimTypes.Sid,  loginResult.CustomerId.ToString(), ClaimValueTypes.String, Issuer),

                    };


                    var userIdentity = new ClaimsIdentity(claims, "Passport");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);
                   

                   await HttpContext.Authentication.SignInAsync("CookiesHCRMStore", userPrincipal);
                  
                    return RedirectToLocal(returnUrl);
                   
                }
                else
                {
                    ModelState.AddModelError(string.Empty, loginResult.LoginMessage);
                  
                }
              
            }

            return View(model);

        }
   
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [AllowAnonymous]
        [HttpGet("~/login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        // [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Route("~/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CookiesHCRMStore");
            //var signInManager = _app.ApplicationServices.GetService<SignInManager<ApplicationUser>>();
            //await signInManager.SignOutAsync();

            //// delete authentication cookie
            //await _signInManager.SignOutAsync();

            //// set this so UI rendering sees an anonymous user
            //HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());


            return RedirectToAction("Login");
        }
    }


    //public class ClaimsPrincipal : IPrincipal
    //{
    //    public IIdentity Identity { get; }
    //    public IEnumerable<ClaimsIdentity> Identities { get; }
    //    public IEnumerable<Claim> Claims { get; }

    //    public bool IsInRole(string role)
    //    {
    //        return true;
    //    }
    //    public Claim FindFirst(string type) { return new Claim("Customer","true");}
    //    public Claim HasClaim(string type, string value) { return new Claim("Customer","true"); }
    //}

    //public class ClaimsIdentity : IIdentity
    //{
    //    public string AuthenticationType { get; }
    //    bool IIdentity.IsAuthenticated
    //    {
    //        get { return IsAuthenticated; }
    //    }

    //    public string Name { get; }

    //    string IIdentity.AuthenticationType
    //    {
    //        get { return AuthenticationType; }
    //    }

    //    public bool IsAuthenticated { get; }
    //    public IEnumerable<Claim> Claims { get; }

    
    //}

    public static class LastChangedValidator
    {
        //public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        //{
        //    // Pull database from registered DI services.
        //    var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        //    var userPrincipal = context.Principal;

        //    // Look for the last changed claim.
        //    string lastChanged;
        //    lastChanged = (from c in userPrincipal.Claims
        //                   where c.Type == "LastUpdated"
        //                   select c.Value).FirstOrDefault();

        //    if (string.IsNullOrEmpty(lastChanged) ||
        //        !userRepository.ValidateLastChanged(userPrincipal, lastChanged))
        //    {
        //        context.RejectPrincipal();
        //        await context.HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");
        //    }
        //}
    }
}
