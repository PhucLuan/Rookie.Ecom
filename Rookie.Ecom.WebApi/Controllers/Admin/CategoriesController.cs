using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Constants;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;

namespace Rookie.Ecom.WebApi.Controllers.Admin
{
    [Authorize("ADMIN_ROLE_POLICY")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(
            ICategoryService categoryService
        )
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IEnumerable<CategoryListDto>> GetAllCategories()
        {
            
            return await _categoryService.GetAllAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutCategory(CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            bool categoryparenrexit = true;

            if (model.CategoryId != null)
            {
                categoryparenrexit = await _categoryService.ExistAsync(x => x.Id == model.CategoryId);
            }

            var categoryexit = await _categoryService.ExistAsync(x => x.Id == model.Id);

            if (categoryexit == false || categoryparenrexit == false)
            {
                return BadRequest("not exit category");
            }
            try
            {
                await _categoryService.UpdateAsync(model);
                return Ok("Update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            bool categoryparenrexit = true;
           
            if (model.CategoryId != null)
            {
                categoryparenrexit = await _categoryService.ExistAsync(x => x.Id == model.CategoryId);
            }
            var categoryexit = await _categoryService.ExistAsync(x => x.Id == model.Id);
            try
            {
                if (categoryexit == false && categoryparenrexit == true)
                {
                    var res = await _categoryService.AddAsync(model);
                    return Created(Endpoints.Category, res);
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var categoryexit = await _categoryService.ExistAsync(x => x.Id == id);
            if (categoryexit == false)
            {
                return BadRequest();
            }
            try
            {
                await _categoryService.DeleteAsync(id);
                return Ok("Delete Success");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
