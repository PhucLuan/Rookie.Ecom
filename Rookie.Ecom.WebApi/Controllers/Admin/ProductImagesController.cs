using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Data;
using Rookie.Ecom.DataAccessor.Models.Admin;

namespace Rookie.Ecom.WebApi.Controllers.Admin
{
    [Authorize("ADMIN_ROLE_POLICY")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        // GET: api/ProductImages
        [HttpGet]
        public async Task<IEnumerable<ProductImageListDto>> GetProductImage(string search)
        {
            return await _productImageService.GetAllAsync(search);
        }


        // GET: api/ProductImages/5
        [HttpGet("GetImageOfProduct/{id}")]
        public async Task<IEnumerable<ProductImageListDto>> GetImageOfProduct(Guid id)
        {
            return await _productImageService.GetByIdProductAsync(id);
        }


        // PUT: api/ProductImages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProductImage(ProductImageDto model)
        {
            await _productImageService.UpdateAsync(model);
            return Ok("Update success");
        }

        // POST: api/ProductImages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostProductImage([FromForm]ProductImageDto productImageDto)
        {
            Account account = new Account(
                       Environment.GetEnvironmentVariable("cloudinarycloudName"),
                       Environment.GetEnvironmentVariable("cloudinaryApiKey"),
                       Environment.GetEnvironmentVariable("cloudinarySecret"));

            await _productImageService.AddAsync(account,productImageDto);

            return Ok("Add success");
        }

        // DELETE: api/ProductImages/5
        [HttpDelete("{id}/{publicId}")]
        public async Task<IActionResult> DeleteProductImage(Guid id, string publicId)
        {
            Account account = new Account(
                       Environment.GetEnvironmentVariable("cloudinarycloudName"),
                       Environment.GetEnvironmentVariable("281334857314385"),
                       Environment.GetEnvironmentVariable("cloudinarySecret"));

            await _productImageService.DeleteAsync(account, id, publicId);

            return Ok("Delete success");
        }

        //private bool ProductImageExists(int id)
        //{
        //    return _context.ProductImage.Any(e => e.Id == id);
        //}
    }
}
