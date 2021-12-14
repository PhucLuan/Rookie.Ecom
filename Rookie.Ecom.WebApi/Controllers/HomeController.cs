using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        //private readonly IMapper _mapper;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        // GET: api/Home
        [HttpGet]
        public async Task<HomePage> GetHomePage(string search = "")
        {
            return await _homeService.GetHomePage();
        }

        [HttpGet("QuickViewProduct/{productId}")]
        public async Task<ProductListDto> QuickViewProduct(Guid productId)
        {
            return await _homeService.GetQuickViewProduct(productId);
        }
        [HttpGet("ProductDetails/{productId}")]
        public async Task<ProductListDto> ProductDetails(Guid productId)
        {
            return await _homeService.GetProductDetails(productId);
        }
        //[HttpGet("ProductByCategory")]
        //public async Task<IEnumerable<ProductListDto>> ProductByCategory(Guid Brand = default(Guid), Guid Category = default(Guid), string search = "")
        //{
        //    return await _homeService.GetProductByCategoryAsync(Brand, Category, search);
        //}
    }
}
