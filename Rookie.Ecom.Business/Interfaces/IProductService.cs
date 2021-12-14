using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListDto>> GetAllAsync();
        Task<ListFilterProduct> GetFilterProductAsync();//Get list item for filter
        Task<PagedResponseModel<ProductListDto>> PagedQueryAsync(FilterProductsModel filter);
        Task<ProductDto> GetByIdAsync(Guid id);
        Task<ProductDto> GetByNameAsync(string name);
        Task<Product> AddAsync(ProductDto productDto);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(ProductDto categoryDto);
        Task<ProductDto> GetAddEditAsync(Guid id);
        Task<bool> ExistAsync(Expression<Func<Product, bool>> predicate);
    }
}
