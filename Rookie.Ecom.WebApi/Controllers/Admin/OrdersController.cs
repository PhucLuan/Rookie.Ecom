using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Business.Interfaces.Id4;
using Rookie.Ecom.Business.Services;
using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rookie.Ecom.WebApi.Controllers.Admin
{
    [Microsoft.AspNetCore.Authorization.Authorize("ADMIN_ROLE_POLICY")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHelper _helper;
        public OrdersController(
            IOrderService orderService,
            IHelper helper
        )
        {
            _orderService = orderService;
            _helper = helper;
        }


        [HttpPost("find")]
        public async Task<PagedResponseModel<OrdersDto>> FindAsync(FilterOrdersModel filter)
        {
            var orders = await _orderService.PagedQueryAsync(filter);

            var userids = orders.Items.Select(x => x.UserId).ToList();

            //GetAccessToken from Id4
            //var token = await _helper.GetAccessTokenAsync();

            // call api

            var indentityApi = RestService.For<IUserProvider>("https://localhost:5000");
            var usersinfo = await indentityApi.GetUsersInfoAsync(userids);

            orders.Items = orders.Items.Join(usersinfo, x => x.UserId, y => y.Id,
                (x, y) => new OrdersDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Email = y.Email,
                    PaymentMethod = x.PaymentMethod,
                    DeliveryAddress = y.Address,
                    OrderStatus = x.OrderStatus,
                    AddedDate = x.AddedDate,
                    Total = x.Total
                }
            );

            return orders;
        }

        [HttpGet("Orderdetail/{id}")]
        public async Task<OrderDetailsAdminDto> GetOrderdetailAsync(Guid id)
        {
            var res = await _orderService.Details(id);
            // call api
            var indentityApi = RestService.For<IUserProvider>("https://localhost:5000");
            var usersinfo = await indentityApi.GetOrderUserInfoAsync(res.UserId, new Guid(res.UserAddressId));
            var user = usersinfo.FirstOrDefault();

            res.CustomerEmail = user.Email;
            res.CustomerName = user.Name;
            res.DeliveryAddress = user.Address;

            return res;
        }
        // GET: api/<OdersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OdersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OdersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
