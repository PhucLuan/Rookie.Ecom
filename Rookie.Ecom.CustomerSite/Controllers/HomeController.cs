using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;
using Rookie.Ecom.Business.Extensions;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Business.Interfaces.Id4;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Public;
using Rookie.Ecom.Customersite.Models;
using Rookie.Ecom.Customersite.ViewModel.Public;
using Rookie.Ecom.DataAccessor.Models.Admin;
using Rookie.Ecom.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rookie.Ecom.Customersite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IHomeService _homeService;
        
        private readonly string CARTKEY = "CartItem";
        private readonly string ItemCountKEY = "itemCount";
        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            IHomeService homeService)
        {
            _logger = logger;
            _productService = productService;
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var homePage = await _homeService.GetHomePage();
            return View(homePage);
        }

        [HttpGet]
        public async Task<IActionResult> QuickViewProduct(Guid productId)
        {
            var productListDto = await _homeService.GetQuickViewProduct(productId);
            return PartialView("_QuickView", productListDto);
        }


        [HttpGet]
        public async Task<IActionResult> ProductDetails(Guid product)
        {
            var productListDto = await _homeService.GetProductDetails(product);

            return View(productListDto);
        }

        [HttpGet]
        public async Task<IActionResult> Product(
            int? pageNumber,
            string sortOrder,
            double? minprice,
            double? maxprice,
            Guid Brand = default(Guid), 
            Guid Category = default(Guid), 
            string search = "")
        {

            ViewData["CurrentSort"] = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "default" : sortOrder;
            ViewData["priceSortParm"] = sortOrder;
            ViewData["Category"] = Category == default(Guid) ? default(Guid).ToString(): Category.ToString();
            ViewBag.minprice = minprice == null ? 0 : minprice;
            ViewBag.maxprice = maxprice == null ? 800000 : maxprice;

            var filter = new FilterProductsModel {
                BrandId = Brand.ToString(),
                CategoryId = Category.ToString(),
                KeySearch = search,
                Page = pageNumber ?? 0,
                Limit = 2,
                MinPrice = minprice,
                MaxPrice = maxprice
            };

            switch (sortOrder)
            {
                case "name_az":
                    filter.OrderProperty = "name";
                    filter.Desc = false;
                    break;
                case "name_za":
                    filter.OrderProperty = "name";
                    break;
                case "price_desc":
                    filter.OrderProperty = "price";
                    break;
                case "price_asc":
                    filter.OrderProperty = "price";
                    filter.Desc = false;
                    break;
                default:
                    break;
            }

            var res = await _homeService.PagedQueryAsync(filter);
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(Guid product)
        {

            if (product != default(Guid))
            {
                var pro = await _productService.GetByIdAsync(product);

                if (pro != null)
                {
                    CartItem addTo = new CartItem
                    {
                        ProductId = pro.Id,
                        ProductName = pro.Name,
                        FinalPrice = (pro.Price - ((pro.Price * pro.Discount) / 100)),
                        Quantity = 1
                    };
                    ViewBag.productId = product;
                    return PartialView("_AddtoCart", addTo);
                }
            }

            return PartialView("_AddtoCart");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItem model)
        {
            var pro = await _productService.GetByIdAsync(model.ProductId);
            
            if (pro == null)
                return NotFound("Không có sản phẩm");

            // Handling Cart ...
            var cart = GetCartItems();
            cart = _homeService.AddToCart(cart,pro,model);

            // Save cart to Session
            SaveCartSession(cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string commentMessage, Guid pro, double rating)
        {
            if (commentMessage != null)
            {
                //Get userID
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var userid = claims.FirstOrDefault(x => x.Type == "sub").Value;

                ProductComments comments = new ProductComments();
                comments.Comment = commentMessage;
                comments.ProductId = pro;
                comments.AddedDate = DateTime.Now;
                comments.ModifiedDate = DateTime.Now;
                comments.UserId = userid;
                comments.Rating = rating;

                await _homeService.AddProductCommentAsync(comments);

            }
            return RedirectToAction("ProductDetails", new { product = pro });
        }

        [HttpGet]
        public async Task<IActionResult> GetComment(string productID)
        {

            var comments = _homeService.GetAllCommentsByProduct(new Guid(productID));

            if (comments == null)
            {
                return PartialView("_ProductComment", null);
            }

            var indentityApi = RestService.For<IUserProvider>("https://localhost:5000");
            var usercomments = await indentityApi.GetUsersCommentAsync(comments.Select(x => x.UserId.ToString()).ToList());

            var commentView = comments.Join(usercomments, x => x.UserId, y => y.Id,
                (x, y) => new CommentsListDto
                {
                    UserId = x.UserId,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Comment = x.Comment,
                    UserName = y.Name,
                    AddedDate = x.AddedDate,
                    Rating = x.Rating
                });

            return PartialView("_ProductComment", commentView);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductRelated(string productID)
        {
            var products = await _homeService.GetProductRelated(new Guid(productID));
            return PartialView("_ProductRelated", products);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Get cart from Session (list of CartItem)
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            var cart = session.Get<List<CartItem>>(CARTKEY);
            
            if (cart != null)
            {
                return cart;
            }
            return new List<CartItem>();
        }

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
    }
}
