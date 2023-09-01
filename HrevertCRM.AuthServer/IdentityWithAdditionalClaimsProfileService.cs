﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HrevertCRM.AuthServer.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Extensions;
using System.Linq;

namespace HrevertCRM.AuthServer
{
    public class IdentityWithAdditionalClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityWithAdditionalClaimsProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            if (!context.AllClaimsRequested)
            {
                claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            }

            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));
            //new Claim(JwtClaimTypes.Role, "admin"),
            //new Claim(JwtClaimTypes.Role, "dataEventRecords.admin"),
            //new Claim(JwtClaimTypes.Role, "dataEventRecords.user"),
            //new Claim(JwtClaimTypes.Role, "dataEventRecords"),
            //new Claim(JwtClaimTypes.Role, "securedFiles.user"),
            //new Claim(JwtClaimTypes.Role, "securedFiles.admin"),
            //new Claim(JwtClaimTypes.Role, "securedFiles")

            //if (user.IsAdmin)
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
            //}
            //else
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, "user"));
            //}

            //if (user.DataEventRecordsRole == "dataEventRecords.admin")
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords.admin"));
            //    claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords.user"));
            //    claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords"));
            //}
            //else
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords.user"));
            //    claims.Add(new Claim(JwtClaimTypes.Role, "dataEventRecords"));
            //}

            //if (user.SecuredFilesRole == "securedFiles.admin")
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, "securedFiles.admin"));
            //    claims.Add(new Claim(JwtClaimTypes.Role, "securedFiles.user"));
            //    claims.Add(new Claim(JwtClaimTypes.Role, "securedFiles"));
            //}
            //else
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, "securedFiles.user"));
            //    claims.Add(new Claim(JwtClaimTypes.Role, "securedFiles"));
            //}

            claims.Add(new System.Security.Claims.Claim(StandardScopes.Email.Name, user.Email));


            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null && user.AccountExpires > DateTime.UtcNow;
        }
    }
}