using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrevertCRM.AuthServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    [Authorize]
    public class ValuesController : Controller
    {
        [Authorize]
        [Route("secret")]
        public string GetValue()
        {
            return "secret";
        }

        [Authorize]
        [Route("open")]
        public string GetOpenValue()
        {
            return "open";
        }
    }
}