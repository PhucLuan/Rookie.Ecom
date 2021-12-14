using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace Rookie.Ecom.WebApi.Controllers.Admin
{
    [Authorize("ADMIN_ROLE_POLICY")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        //private readonly IMapper _mapper;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<IEnumerable<BrandListDto>> GetBrands(string search = "")
        {
            return await _brandService.GetAllAsync();
        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrand(Guid id)
        {
            return await _brandService.GetByIdAsync(id);
        }

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBrand(BrandDto model)
        {
            if (!ModelState.IsValid)
            {
                BadRequest("Input error");
            }

            try
            {
                await _brandService.UpdateAsync(model);
                return Ok("Update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostBrand([FromBody] BrandDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Input Error");
            }
            try
            {
                await _brandService.AddAsync(model);
                return Ok("Add Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            try
            {
                await _brandService.DeleteAsync(id);
                return Ok("Xóa thành công");
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}
