using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace HrevertCRM.AuthServer
{
    public class Config
    {
        public static string HOST_URL = "http://localhost:10576";
        // scopes define the resources in your system
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                 StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = "apis",
                    Description = "HreverCRM API",
                     ScopeSecrets = new List<Secret>
                    {
                        new Secret("secretaAPIs".Sha256())
                    }
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
                       "apis"
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
