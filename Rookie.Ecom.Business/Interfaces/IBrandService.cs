using Rookie.Ecom.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandListDto>> GetAllAsync();
        Task<BrandDto> PagedQueryAsync(string name, int page, int limit);
        Task<BrandDto> GetByIdAsync(Guid id);
        Task AddAsync(BrandDto categoryDto);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(BrandDto categoryDto);
    }
}
