using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Business.Extensions;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Filter;
using Rookie.Ecom.Contracts.Dtos.Public;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<Orders> _ordersRepository;
        private readonly IBaseRepository<OrderDetails> _orderDetailsRepository;

        private readonly IMapper _mapper;

        public OrderService(
            IBaseRepository<Product> productRepository,
            IBaseRepository<Orders> ordersRepository,
            IBaseRepository<OrderDetails> orderDetailsRepository,

            IMapper mapper,

            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _ordersRepository = ordersRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _mapper = mapper;
        }

        public List<OrderDetailsDto> GetCreateOrder(IEnumerable<OrderDetailsDto> product)
        {

            var modelCart = new List<OrderDetailsDto>();
 
            foreach (var item in product)
            {
                OrderDetailsDto cart = new OrderDetailsDto
                {
                    ProductId = item.ProductId,
                    FinalPrice = item.FinalPrice,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                };

                modelCart.Add(cart);

            }
            
            return modelCart;
        }

        public async Task<List<OrderDetailsDto>> GetOderPage(List<CartItem> addToCartList, int? itemcount)
        {
            List<OrderDetailsDto> model = new List<OrderDetailsDto>();

            int itemInCart = 0;
            if (itemcount != null)
            {
                itemInCart = itemcount.Value;
            }

            var dbProducts = await _productRepository.GetAllIncludeAsync(b => b.Brand, p => p.ProductImages);
            if (addToCartList != null)
            {
                addToCartList.ForEach(c =>
                {
                    OrderDetailsDto orderDetails = new OrderDetailsDto();
                    orderDetails.ProductId = c.ProductId;
                    orderDetails.ProductName = c.ProductName;
                    orderDetails.FinalPrice = c.FinalPrice;
                    orderDetails.Quantity = c.Quantity;
                    var product = dbProducts.FirstOrDefault(x => x.Id == c.ProductId);

                    if (product != null)
                    {
                        orderDetails.BrandId = product.BrandId;
                        orderDetails.BrandName = product.Brand.Name;
                        orderDetails.Price = product.Price;
                        orderDetails.ImagePath = product.ProductImages.OrderBy(x => x.Order).FirstOrDefault(x => x.ProductId == c.ProductId)
                            ?.ImagePath;

                        if (product.ProductStock > 0)
                        {
                            orderDetails.Stock = product.ProductStock - c.Quantity;
                        }

                        model.Add(orderDetails);
                    }
                });
            }
            return model;
        }

        public List<OrderDetailsDto> GetItemOrderDetails(List<CartItem> addToCartList)
        {
            var model = new List<OrderDetailsDto>();

            if (addToCartList != null)
            {
                var dbProducts = _productRepository.Query()
                    .Include(b => b.Brand)
                    .Include(p => p.ProductImages);

                addToCartList.ForEach(c =>
                {
                    OrderDetailsDto orderDetails = new OrderDetailsDto();

                    orderDetails.ProductId = c.ProductId;
                    orderDetails.ProductName = c.ProductName;
                    orderDetails.FinalPrice = c.FinalPrice;
                    orderDetails.Quantity = c.Quantity;

                    var product = dbProducts.FirstOrDefault(x => x.Id == c.ProductId);

                    if (product != null)
                    {
                        orderDetails.BrandId = product.BrandId;
                        orderDetails.BrandName = product.Brand.Name;
                        orderDetails.Price = product.Price;
                        orderDetails.ImagePath = product.ProductImages.OrderBy(x => x.Order).FirstOrDefault(x => x.ProductId == c.ProductId)
                            ?.ImagePath;
                        
                        orderDetails.Stock = product.ProductStock - c.Quantity;

                        model.Add(orderDetails);
                    }
                });
            }
            return model;
        }

        public List<CartItem> RemoveItemFromCart(List<CartItem> addToCartList, Guid productid)
        {
           
            
            if (addToCartList.Where(x => x.ProductId == productid).ToList().Count > 0)
            {
                var cart = addToCartList.FirstOrDefault(x => x.ProductId == productid);

                if (cart != null)
                {
                    addToCartList.Remove(cart);
                }

            }
            
            return addToCartList;
        }

        public NewOrderDto CreateOrder(List<CartItem> addToCartList)
        {
            NewOrderDto newOrderDto = new NewOrderDto();

            newOrderDto.OrderDetailsList = GetItemOrderDetails(addToCartList);
            newOrderDto.PaymentMethod = "Thanh toán khi nhận hàng";

            return newOrderDto;
        }

        public Task AddNewOrder(
            double totalPrice,
            string userid,
            List<CartItem> addToCartList,
            NewOrderDto model)
        {
            //Inserting Order
            Orders orders = new Orders
            {
                AddedDate = DateTime.Now,
                UserAdressId = model.UserAdressId,
                ModifiedDate = DateTime.Now,
                Number = GenerateRandomNo().ToString(),
                PaymentMethod = "Tien mat",
                Total = totalPrice,
                OrderStatus = OrderStatus.Pending
            };

            orders.UserId = userid;
            _ordersRepository.Insert(orders);

            //Inserting Order Details

            Guid oId = orders.Id;


            for (int i = 0; i < addToCartList.Count; i++)
            {

                OrderDetails orderDetails = new OrderDetails();

                orderDetails.AddedDate = DateTime.Now;
                orderDetails.ModifiedDate = DateTime.Now;
                orderDetails.OrderId = oId;
                orderDetails.ProductId = addToCartList[i].ProductId;
                orderDetails.Quantity = addToCartList[i].Quantity;
                orderDetails.Rate = addToCartList[i].FinalPrice;
                orderDetails.Remarks = "";

                var pro = _productRepository.GetById(orderDetails.ProductId);
                pro.ProductStock -= addToCartList[i].Quantity;

                _orderDetailsRepository.Insert(orderDetails);
            }

            //_orderDetailsRepository.InsertAddrange(orderDetails1);




            return Task.CompletedTask;
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public async Task<PagedResponseModel<OrdersDto>> PagedQueryAsync(FilterOrdersModel filter)
        {
            var query = _ordersRepository.Entities;

            query = query.OrderByDescending(x => x.AddedDate.Date).ThenBy(x => x.AddedDate.TimeOfDay);

            var orders = await query.PaginateAsync(filter.Page, filter.Limit);



            return new PagedResponseModel<OrdersDto>
            {
                CurrentPage = orders.CurrentPage,
                TotalPages = orders.TotalPages,
                TotalItems = orders.TotalItems,
                Items = _mapper.Map<IEnumerable<OrdersDto>>(orders.Items)
            };
        }

        public async Task<OrderDetailsAdminDto> Details(Guid id)
        {
            var model = new OrderDetailsAdminDto();
            var query = _ordersRepository.Entities;
            var order = await query
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order != null)
            {
                //find product from product details
                var productIds = order.OrderDetails.Select(x => x.ProductId);
                var productList = await _productRepository.Entities
                    .Include(p => p.ProductImages)
                    .Include(x => x.Brand)
                    .Where(x => productIds.Any(p => p == x.Id)).ToListAsync();

                model = _mapper.Map<OrderDetailsAdminDto>(order);

                model.UserAddressId = order.UserAdressId;
  
                foreach (var item in order.OrderDetails)
                {
                    var product = productList.FirstOrDefault(f => f.Id == item.ProductId);
                    var orderDetails = new OrderDetailsDto();
                    orderDetails.ProductId = item.ProductId;
                    orderDetails.Quantity = item.Quantity;

                    if (product != null)
                    {
                        orderDetails.ProductName = product.Name;
                        orderDetails.FinalPrice = item.Rate * item.Quantity;
                        orderDetails.BrandId = product.BrandId;
                        orderDetails.BrandName = product.Brand.Name;
                        orderDetails.Price = item.Rate;
                        orderDetails.ImagePath = product.ProductImages.OrderBy(x => x.Order).FirstOrDefault(x => x.ProductId == item.ProductId)
                            ?.ImagePath;

                        model.OrderProductLists.Add(orderDetails);
                    }
                }

                
                return model;
            }
            return null;
        }
    }
}
