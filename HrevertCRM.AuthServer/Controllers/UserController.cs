using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HrevertCRM.AuthServer.Models;
using HrevertCRM.AuthServer.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace HrevertCRM.Web.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
       

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        
        }

        [HttpGet("{id}")]
        public async Task<ObjectResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound("Application User Not Found");

        }

        [Route("login"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginInfo)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result =
                await
                    _signInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, isPersistent: false,
                        lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Json("OK");
            }

            return BadRequest();
        }

  
       
     

      

       
    }
}
