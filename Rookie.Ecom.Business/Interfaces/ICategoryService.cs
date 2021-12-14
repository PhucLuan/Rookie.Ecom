using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListDto>> GetAllAsync();
        Task<CategoryDto> PagedQueryAsync(string name, int page, int limit);
        Task<CategoryDto> GetByIdAsync(Guid id);
        Task<CategoryDto> GetByNameAsync(string name);
        Task<CategoryDto> AddAsync(CategoryDto categoryDto);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(CategoryDto categoryDto);
        ICollection<Category> GetAll();
        Task<bool> ExistAsync(Expression<Func<Category, bool>> predicate);
    }
}
