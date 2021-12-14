using Rookie.Ecom.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitListDto>> GetAllAsync();
        Task<UnitDto> PagedQueryAsync(string name, int page, int limit);
        Task<UnitDto> GetByIdAsync(Guid id);
        Task AddAsync(UnitDto categoryDto);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(UnitDto categoryDto);
    }
}
