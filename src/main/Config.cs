using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static works.ei8.Identity.Constants;

namespace works.ei8.Identity
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("cortex-graph-out", "neurUL Cortex Graph (Out)") { ApiSecrets = { new Secret("secret".Sha256()) } },
                new ApiResource("cortex-in", "neurUL Cortex (In)") { ApiSecrets = { new Secret("secret".Sha256()) } }
            };
        }

        public static IEnumerable<Client> GetClients(IConfigurationSection configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "xamarin",
                    ClientName = "eShop Xamarin OpenId Client",
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials, // TODO: GrantTypes.Hybrid,                    
                    //Used to retrieve the access token on the back channel.
                    // should not require client secret since client is a native app (needed for web server apps only)
                    RequireClientSecret = false,
                    //ClientSecrets =
                    //{
                    //    new Secret("secret".Sha256())
                    //},                    
                    RedirectUris = { $"{Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ClientsXamarin)}/Account/LoginCallback" },
                    RequireConsent = false,
                    // require code challenge?
                    RequirePkce = false,
                    PostLogoutRedirectUris = { Config.GetLogoutRedirectUri(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ClientsXamarin)) },
                    AllowedCorsOrigins = { $"{Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ClientsXamarin)}" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "cortex-graph-out",
                        "cortex-in"
                        //"orders",
                        //"basket",
                        //"locations",
                        //"marketing"
                    },
                    //Allow requesting refresh tokens for long lived API access
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    //Access token life time is 7200 seconds (2 hour)
                    AccessTokenLifetime = 172800,
                    //Identity token life time is 7200 seconds (2 hour)
                    IdentityTokenLifetime = 172800
                }
            };
        }

        internal static string GetLogoutRedirectUri(string client) // TODO: get configuration specific values - IConfigurationSection configuration)
        {
            return $"{client}{Constants.Paths.Logout}";
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",
                    Claims = new Claim[]
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "www.alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
