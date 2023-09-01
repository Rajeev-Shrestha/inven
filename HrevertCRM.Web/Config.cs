using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace HrevertCRM.Web
{
    public class Config
    {
        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
          



        }

       

      
        private static IConfiguration _configuration;
        // scopes define the resources in your system
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                 StandardScopes.OpenId,
                  StandardScopes.Email,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = "apis",
                    Description = "HreverCRM API",
                     ScopeSecrets = new List<Secret>
                    {
                        new Secret("secretaAPIs".Sha256())
                    },
                     Claims = new List<ScopeClaim>
        {
              new ScopeClaim("name"),

        },

    IncludeAllClaimsForUser = true
                }

            };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "HrevertCRMStore",
                   // ClientName = "HrevertCRM Store",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                   // RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //RedirectUris = {Constants.HrevertCRMUrl + "/signin-oidc"},
                    //PostLogoutRedirectUris = {Constants.HrevertCRMUrl},
                    AllowedScopes =
                    {
                       "apis",
                        StandardScopes.Profile.Name,
                       StandardScopes.OfflineAccess.Name
                    }
                },
                new Client
                {
                    ClientId = "HrevertCRMStoreClient",
                   // ClientName = "HrevertCRM Store",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                   // RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //RedirectUris = {Constants.HrevertCRMUrl + "/signin-oidc"},
                    //PostLogoutRedirectUris = {Constants.HrevertCRMUrl},
                    AllowedScopes =
                    {
                       "apis"
                    }
                }
            };
        }
    }
}
