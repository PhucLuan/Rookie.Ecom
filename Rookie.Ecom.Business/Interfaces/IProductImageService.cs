
using CloudinaryDotNet;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageListDto>> GetAllAsync(string search);
        Task<IEnumerable<ProductImageListDto>> GetByIdProductAsync(Guid id);
        Task AddAsync(Account account, ProductImageDto productImageDto);
        Task DeleteAsync(Account account, Guid id, string publicId);
        Task DeleteListImageAsync(string[] public_ids);
        Task UpdateAsync(ProductImageDto categoryDto);
    }
}
