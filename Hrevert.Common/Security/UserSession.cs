using System;
using System.Linq;
using System.Security.Claims;
using Hrevert.Common.Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hrevert.Common.Security
{
    public class UserSession : IWebUserSession
    {
        public string Firstname
        {
            get
            {
                return ((ClaimsPrincipal) HttpHelper.HttpContext.User).FindFirst(ClaimTypes.
                    GivenName).Value;
            }
        }

        public string Lastname
        {
            get
            {
                return ((ClaimsPrincipal) HttpHelper.HttpContext.User).FindFirst(ClaimTypes.Surname).
                    Value;
            }
        }

        public string Username
        {
            get
            {
                try
                {
                    // return ((ClaimsPrincipal)HttpHelper.HttpContext.User).FindFirst(ClaimTypes.Name).Value;
                    var result =new JsonResult(from c in HttpHelper.HttpContext.User.Claims select new {c.Type, c.Value});
                    return (((ClaimsPrincipal) HttpHelper.HttpContext.User)).Identity.Name;

                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
               
            }
            
            // TODO Fix this function return ((ClaimsPrincipal)HttpHelper.HttpContext.User).FindFirst(ClaimTypes.Name).Value
        }
        public bool IsInRole(string roleName)
        {
            return HttpHelper.HttpContext.User.IsInRole(roleName);
        }
        public Uri RequestUri
        {


            get
            {
                return new Uri(HttpHelper.HttpContext.Request.GetDisplayUrl());

               
            }
        }
        public string HttpRequestMethod
        {

            get
            {
              return  HttpHelper.HttpContext.Request.Method;
                
            }
        }
        public string ApiVersionInUse
        {
            get
            {
                const int versionIndex = 2;
                if (RequestUri.Segments.Count() < versionIndex + 1)
                {
                    return string.Empty;
                }
                var apiVersionInUse = RequestUri.Segments[versionIndex].Replace(
                "/", string.Empty);
                return apiVersionInUse;
            }
        }
    }
}
