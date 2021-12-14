using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.DataAccessor.Models.Admin;
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
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(
            IUnitService unitService
        )
        {
            _unitService = unitService;
        }

        // GET: api/Units
        [HttpGet]
        public async Task<IEnumerable<UnitListDto>> GetUnit(string search = "")
        {
            return await _unitService.GetAllAsync();
        }

        //GET: api/Units/5
        [HttpGet("{id}")]
        public async Task<UnitDto> GetUnit(Guid id)
        {
            return await _unitService.GetByIdAsync(id);
        }

        // PUT: api/Units/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUnit(UnitDto model)
        {
            if (!ModelState.IsValid)
            {
                BadRequest("Input error");
            }

            try
            {
                await _unitService.UpdateAsync(model);
                return Ok("Update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // POST: api/Units
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Unit>> PostUnit(UnitDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Lỗi input");
            }

            try
            {
                await _unitService.AddAsync(model);
                return Ok("Thêm thành công");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // DELETE: api/Units/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(Guid id)
        {
            try
            {
                await _unitService.DeleteAsync(id);
                return Ok("Delete Success");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //private bool UnitExists(int id)
        //{
        //    return _context.Unit.Any(e => e.Id == id);
        //}
    }
}
