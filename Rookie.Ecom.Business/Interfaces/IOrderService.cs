using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Filter;
using Rookie.Ecom.Contracts.Dtos.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDetailsDto>> GetOderPage(List<CartItem> addToCartList, int? itemcount);
        List<CartItem> RemoveItemFromCart(List<CartItem> addToCartList, Guid productid);
        List<OrderDetailsDto> GetCreateOrder(IEnumerable<OrderDetailsDto> product);
        NewOrderDto CreateOrder(List<CartItem> addToCartList);
        Task AddNewOrder(double totalPrice, string userid, List<CartItem> addToCartList, NewOrderDto model);

        Task<PagedResponseModel<OrdersDto>> PagedQueryAsync(FilterOrdersModel filter);
        Task<OrderDetailsAdminDto> Details(Guid id);
    }
}
