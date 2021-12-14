// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rookie.Ecom.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };
        /// <summary>
        /// /////////////
        /// </summary>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("identity.api", "Identity API"),
                new ApiResource("test.api","Test API"),
                new ApiResource("api1", "API #1") {Scopes = {"api1"}},
        };
        }
        /// <summary>
        /// /////////////
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("api1", "Full access to API #1"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1", "test.api", "api1" }
                },
                new Client
                {
                    ClientId = "m2m.password",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("Ahihi".Sha256()) },

                    AllowedScopes = { "scope2" , "test.api", "api1" }
                },
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope2" }
                },
                // Đăng nhập thông qua WebApi
                new Client
                    {
                        ClientId = "demo_api_swagger",
                        ClientName = "Swagger UI for demo_api",
                        ClientSecrets = {new Secret("secret".Sha256())}, // change me!

                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        RequireClientSecret = false,

                        RedirectUris = {"https://localhost:5001/swagger/oauth2-redirect.html"},
                        AllowedCorsOrigins = {"https://localhost:5001"},
                        AllowedScopes = {"api1"}
                    },
                 // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AlwaysIncludeUserClaimsInIdToken = true,

                },

                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:3000/callback" },
                    PostLogoutRedirectUris = { "http://localhost:3000/" },
                    AllowedCorsOrigins =     { "http://localhost:3000" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },

                    AllowAccessTokensViaBrowser = true,
                    //AccessTokenLifetime = 60 * 1,
                },
                // m2m
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                }
            };

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "user",
                    Password = "user",
                    Claims = new[]
                    {
                        new Claim("roleType", "CanReaddata")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "admin",
                    Password = "admin",
                    Claims = new[]
                    {
                        new Claim("roleType", "CanUpdatedata")
                    }
                }
            };
        }
    }
}