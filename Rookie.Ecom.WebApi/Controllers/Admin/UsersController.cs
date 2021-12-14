using IdentityModel.Client;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Refit;
using Rookie.Ecom.Business.Interfaces.Id4;
using Rookie.Ecom.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rookie.Ecom.WebApi.Controllers.Admin
{
    [Microsoft.AspNetCore.Authorization.Authorize("ADMIN_ROLE_POLICY")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {

        }
        // public async Task<IEnumerable<UserListDto>> GetUsersComment
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IEnumerable<UserListDto>> GetAllUsers()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api

            var indentityApi = RestService.For<IUserProvider>("https://localhost:5000");
            var res = await indentityApi.GetAllUserAsync(tokenResponse.AccessToken);
            return res;
        }
    }
}
