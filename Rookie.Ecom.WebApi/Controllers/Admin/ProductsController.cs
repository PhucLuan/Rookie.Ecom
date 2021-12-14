using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Constants;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;

namespace Rookie.Ecom.WebApi.Controllers.Admin
{
    [Authorize("ADMIN_ROLE_POLICY")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IHomeService _homeService;
        public ProductsController(
            IProductService productService,
            IHomeService homeService
        )
        {
            _productService = productService;
            _homeService = homeService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IEnumerable<ProductListDto>> GetProduct(string search)
        {

            return await _productService.GetAllAsync();
        }


        [HttpPost("find")]
        public async Task<PagedResponseModel<ProductListDto>>
            FindAsync(FilterProductsModel filter)
            => await _productService.PagedQueryAsync(filter);


        [HttpGet("AddEditProduct/{id}")]
        public async Task<ProductDto> AddEditProduct(Guid id)
        {
            return await _productService.GetAddEditAsync(id);
        }

        [HttpGet("GetFilterProductAsync")]
        public async Task<ListFilterProduct> GetFilterProductAsync()
        {
            return await _productService.GetFilterProductAsync();
        }

        [HttpGet("GetAllCommentsByProduct/{id}")]
        public List<CommentsListDto> GetAllCommentsByProduct(Guid id)
        {
            return _homeService.GetAllCommentsByProduct(id);
        }
        // GET: api/Products/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //    var product = await _context.Product.FindAsync(id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return product;
        //}

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProduct(ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var productexit = await _productService.ExistAsync(x => x.Id == product.Id);
            if (productexit == false)
            {
                return BadRequest("Not Exit product");
            }

            try
            {
                await _productService.UpdateAsync(product);

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

            return Ok("Update success");
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto product)
        {
            var productexit = await _productService.ExistAsync(x => x.Id == product.Id);
            if (productexit == true)
            {
                return BadRequest("Exit product");
            }
            var res = await _productService.AddAsync(product);
            return Created(Endpoints.Product, res);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var productexit = await _productService.ExistAsync(x => x.Id == id);
            if (productexit == false)
            {
                return BadRequest();
            }
            await _productService.DeleteAsync(id);

            return Ok("Delete success");
        }
    }
}
