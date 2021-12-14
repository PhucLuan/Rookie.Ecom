using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refit;
using Rookie.Ecom.Business.Extensions;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Business.Interfaces.Id4;
using Rookie.Ecom.Contracts.Dtos.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rookie.Ecom.CustomerSite.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;

        private readonly string CARTKEY = "CartItem";
        private readonly string ItemCountKEY = "itemCount";
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        public async Task<IActionResult> Index(bool isupdated = false)
        {
            if (isupdated)
            {
                ViewBag.isupdated = true;
            }

            var addToCartList = HttpContext.Session.Get<List<CartItem>>(CARTKEY);
            var itemcount = HttpContext.Session.GetInt32(ItemCountKEY);

            var res = await _orderService.GetOderPage(addToCartList,itemcount);
            return View(res);
        }

        [HttpPost]
        public IActionResult Index(IEnumerable<OrderDetailsDto> product,string submitButton)
        {
            if (submitButton == "updatecart")
            {
                var cart = product.Where(x => x.Quantity > 0).Select(x => new CartItem
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    FinalPrice = x.FinalPrice,
                }).ToList();

                ClearCart();
                SaveCartSession(cart);

                return RedirectToAction(nameof(Index),new { isupdated = true});
            }
            var modelCart = _orderService.GetCreateOrder(product);

            HttpContext.Session.Set<List<OrderDetailsDto>>(CARTKEY, modelCart);
            var itemInCart = HttpContext.Session.Get<List<OrderDetailsDto>>(CARTKEY).Count;
            HttpContext.Session.SetInt32(ItemCountKEY, itemInCart);

            var totalPrice = product.Aggregate(0d,(result, element) => result + element.Quantity * element.FinalPrice);

            HttpContext.Session.SetInt32("TotalPrice", Convert.ToInt32(totalPrice));


            return RedirectToAction(nameof(NewOrder));
        }

        #region AddNewOrders
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> NewOrder()
        {
            var addToCartList = HttpContext.Session.Get<List<CartItem>>(CARTKEY);
            var model = _orderService.CreateOrder(addToCartList);

            var indentityApi = RestService.For<IUserProvider>("https://localhost:5000");
            var useraddress = await indentityApi.GetUsersAddressAsync(GetUserId());

            ViewBag.UserAddress = useraddress.Select(x =>
                 new SelectListItem { Text = x.CustomerAddress, Value = x.AddressID });

            return View(model);
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        public IActionResult NewOrder(NewOrderDto model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something wrong");
                return View(model);
            }
            List<CartItem> addToCartList = HttpContext.Session.Get<List<CartItem>>(CARTKEY);

            if (addToCartList == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var num = HttpContext.Session.GetInt32("TotalPrice");

            double totalPrice = 0;
            if (num != null)
            {
                totalPrice = Convert.ToDouble(num);
            }

            _orderService.AddNewOrder(totalPrice, GetUserId(), addToCartList,model);

            //reset cart
            ResetCart(addToCartList);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region RemoveCartItem

        [HttpGet]
        public IActionResult RemoveItemFromCart(Guid product)
        {
            CartItem model = new CartItem();

            List<CartItem> addToCartList = HttpContext.Session.Get<List<CartItem>>(CARTKEY);
            //string name = "";
            model = addToCartList.FirstOrDefault(x => x.ProductId == product);
            

            return PartialView("_RemoveItemFromCart", model);
        }


        [HttpPost]
        public IActionResult RemoveItemFromCart(Guid ProductId, CartItem model)
        {
            var addToCartList = HttpContext.Session.Get<List<CartItem>>(CARTKEY);
            var cart =_orderService.RemoveItemFromCart(addToCartList,ProductId);

            HttpContext.Session.Set<List<CartItem>>(CARTKEY, cart);

            var itemInCart = HttpContext.Session.Get<List<CartItem>>(CARTKEY).Count;
            HttpContext.Session.SetInt32(ItemCountKEY, itemInCart);

            return RedirectToAction("Index");
        }
        #endregion

        //Remove cart from session -- clear all
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Save Cart (List of CartItem) in session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            session.Set<List<CartItem>>(CARTKEY, ls);
            HttpContext.Session.SetInt32(ItemCountKEY, ls.Count);
        }

        string GetUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userid = claims.FirstOrDefault(x => x.Type == "sub").Value;
            return userid;
        }

        void ResetCart(List<CartItem> addToCartList)
        {
            addToCartList = new List<CartItem>();
            HttpContext.Session.Set<List<CartItem>>(CARTKEY, addToCartList);

            int itemInCart = HttpContext.Session.Get<List<CartItem>>(CARTKEY).Count;

            HttpContext.Session.SetInt32(ItemCountKEY, itemInCart);
            HttpContext.Session.SetInt32("TotalPrice", 0);
        }
    }
}
