using Refit;
using Rookie.Ecom.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces.Id4
{
    public interface IUserProvider
    {
        [Get("/api/AdminUsers")]
        Task<IEnumerable<UserListDto>> GetAllUserAsync([Authorize("Bearer")] string token);

        [Get("/api/AdminUsers/UsersComment")]
        Task<IEnumerable<UserListDto>> GetUsersCommentAsync([Body]List<string> userids);

        [Get("/api/AdminUsers/UsersAddress/{userid}")]
        Task<IEnumerable<UserAddressDto>> GetUsersAddressAsync(string userid);

        [Get("/api/AdminUsers/UsersInfo")]
        Task<IEnumerable<UserListDto>> GetUsersInfoAsync([Body]List<string> userids);

        [Get("/api/AdminUsers/GetOrderUserInfo/{userid}/{addressid}")]
        Task<IEnumerable<UserListDto>> GetOrderUserInfoAsync(string userid, Guid addressid);
    }
}
