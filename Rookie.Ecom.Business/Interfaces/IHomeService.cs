using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Public;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface IHomeService
    {
        List<CartItem> AddToCart(List<CartItem> carts, ProductDto pro, CartItem model);        
        Task<ProductComments> AddProductCommentAsync(ProductComments productComment);
        Task<HomePage> GetHomePage();
        Task<ProductListDto> GetQuickViewProduct(Guid productid);
        Task<ProductListDto> GetProductDetails(Guid productId);
        //Task<IEnumerable<ProductListDto>> GetProductByCategoryAsync(Guid Brand = default(Guid), Guid Category = default(Guid), string search = "");
        List<CommentsListDto> GetAllCommentsByProduct(Guid productId);
        Task<IEnumerable<ProductRelatDto>> GetProductRelated(Guid productid);
        Task<PagedResponseModel<ProductListDto>> PagedQueryAsync(FilterProductsModel filter);
    }
}
