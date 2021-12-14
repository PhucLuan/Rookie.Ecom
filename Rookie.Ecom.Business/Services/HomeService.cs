using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Business.Extensions;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Public;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class HomeService : IHomeService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IBaseRepository<ProductComments> _productcommentRepository;

        private readonly IMapper _mapper;

        public HomeService() { }
        public HomeService(
            IBaseRepository<Product> productRepository,
            IBaseRepository<Category> categoryRepository,
            IBaseRepository<Brand> brandRepository,
            IBaseRepository<ProductComments> productcommentRepository,

            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productcommentRepository = productcommentRepository;

            _mapper = mapper;
        }
        public async Task<HomePage> GetHomePage()
        {
            HomePage homePage = new HomePage();
            homePage.Categories = GetMainCategory();

            homePage.ProductList = new List<ProductListDto>();

            var ctgList = _categoryRepository.Query().Where(x => x.Ispublish == true).OrderBy(x => x.Order).Take(4).ToList();
            homePage.CategoryNameList = ctgList != null ? ctgList.Select(x => x.Name).ToList() : new List<string>();
            homePage.SecondTab = new List<ProductListDto>();
            homePage.FirstTab = new List<ProductListDto>();
            homePage.ThirdTab = new List<ProductListDto>();
            homePage.FourthTab = new List<ProductListDto>();
            var counter = 0;
            foreach (var item in ctgList)
            {
                if (counter == 0)
                {
                    homePage.FirstTab = GetAllProductList(ctgid: item.Id, take: 8);
                }
                if (counter == 1)
                {
                    homePage.SecondTab = GetAllProductList(ctgid: item.Id, take: 8);
                }
                if (counter == 2)
                {
                    homePage.ThirdTab = GetAllProductList(ctgid: item.Id, take: 8);
                }
                if (counter == 3)
                {
                    homePage.FourthTab = GetAllProductList(ctgid: item.Id, take: 8);
                }
                counter++;
            }

            homePage.BrandList = await GetAllBrand();
            return homePage;
        }

        #region Brand
        public async Task<List<BrandListDto>> GetAllBrand()
        {
            List<BrandListDto> brandList = new List<BrandListDto>();
            var brands = await _brandRepository.GetAllAsync();
            brands.ToList().ForEach(x =>
           {
               BrandListDto brand = new BrandListDto
               {
                   Name = x.Name,
                   Id = x.Id,
                   Description = x.Description
               };
               brandList.Add(brand);
           });
            return brandList;
        }
        #endregion

        #region CategoryMenu
        public List<CategoryDto> GetMainCategory()
        {
            List<CategoryDto> categoryList = new List<CategoryDto>();
            var categories = _categoryRepository.GetAll().ToList();
            categories.Where(x => x.CategoryId == null)
             .OrderBy(x => x.Order).ToList().ForEach(c =>
        {
            CategoryDto ctg = new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                CategoryId = c.Id,

            };
            ctg.CategoryMenus = GetSubCategory(c.Id);
            categoryList.Add(ctg);
        });


            return categoryList;
        }

        public List<CategoryDto> GetSubCategory(Guid id)
        {
            List<CategoryDto> categoryList = new List<CategoryDto>();
            var categories = _categoryRepository.GetAll();
            categories.Where(x => x.CategoryId == id).ToList().ForEach(c =>
           {
               CategoryDto ctg = new CategoryDto
               {
                   Name = c.Name,
                   CategoryId = c.CategoryId,
                   Id = c.Id

               };
               ctg.CategoryMenus = GetSubCategory(c.Id);
               categoryList.Add(ctg);
               //test.Add(c.Id);
           });
            return categoryList;
        }
        #endregion

        public List<ProductListDto> GetAllProductList(int id = 0, int take = 1000, Guid ctgid = default(Guid))
        {
            List<ProductListDto> productList = new List<ProductListDto>();
            IQueryable<Product> dbProducts = _productRepository.Query()
                .Include(x => x.Brand)
                .Include(c => c.Category)
                .Include(u => u.Unit)
                .Include(pc => pc.ProductCommentses)
                .Include(pi => pi.ProductImages);

            if (ctgid != default(Guid))
            {
                dbProducts = dbProducts.Where(x => (x.CategoryId == ctgid || x.Category.Id == ctgid) && x.Ispublish == true);
            }

            dbProducts = dbProducts.OrderByDescending(x => x.AddedDate).Take(take);
            foreach (var b in dbProducts)
            {
                var product = _mapper.Map<ProductListDto>(b);

                product.BrandName = b.Brand.Name;
                product.CategoryName = b.Category.Name;
                product.UnitName = b.Unit.Name;
                product.FinalPrice = (b.Price - ((b.Price * b.Discount) / 100));
                product.ProductComments = b.ProductCommentses.Count;
                product.TotalImage = b.ProductImages.Count;

                var productImageList = b.ProductImages
                    .Where(x => x.ProductId == b.Id && x.Ispublish == true)
                    .OrderBy(x => x.Order).ToList();

                var productImage = productImageList.FirstOrDefault();
                if (productImage != null)
                {
                    product.ImageTitle = productImage.Title;
                    product.ImagePath = productImage.ImagePath;
                    if (productImageList.Where(x => x.Ispublish == true).Count() > 1)
                    {
                        product.SecondImagePath = productImageList.Skip(1).FirstOrDefault()?.ImagePath;
                    }
                    else
                    {
                        product.SecondImagePath = product.ImagePath;
                    }
                }
                else
                {
                    product.ImagePath = "noproductimage.png";
                    product.SecondImagePath = "noproductimage.png";
                }



                productList.Add(product);
            }


            return productList;

        }
        public List<CommentsListDto> GetAllCommentsByProduct(Guid productId)
        {
            List<CommentsListDto> commentsList = new List<CommentsListDto>();
            _productcommentRepository
                .GetIncludeList(x => x.ProductId == productId, p => p.Product)
                .OrderByDescending(x => x.AddedDate).ToList().ForEach(x =>
            {
                CommentsListDto comments = new CommentsListDto
                {
                    ProductId = x.ProductId,
                    Comment = x.Comment,
                    ProductName = x.Product.Name,
                    UserId = x.UserId,
                    Rating = x.Rating
                };
                DateTime date = x.AddedDate;
                comments.AddedDate = TimeAgoCustom.TimeAgo(date);
                comments.UserName = "Anonymous";
                commentsList.Add(comments);
            });

            return commentsList;
        }

        public async Task<ProductListDto> GetQuickViewProduct(Guid productid)
        {
            ProductListDto pList = new ProductListDto();
            Product pro = await _productRepository
                 .GetSingleIncludeAsync(x => x.Id == productid,
                                           b => b.Brand, c => c.Category,
                                           u => u.Unit, rc => rc.ProductCommentses,
                                           pi => pi.ProductImages);
            double averageRating = 0;
            if (pro?.ProductCommentses.Count > 0)
            {
                var ratings = pro.ProductCommentses.Average(x => x.Rating);
                averageRating = Math.Round(ratings, 1);
            }

            if (pro != null)
            {
                pList = _mapper.Map<ProductListDto>(pro);
                pList.BrandName = pro.Brand.Name;
                pList.CategoryName = pro.Category.Name;
                pList.UnitName = pro.Unit.Name;
                pList.FinalPrice = (pro.Price - ((pro.Price * pro.Discount) / 100));
                pList.ProductComments = pro.ProductCommentses.Count(x => x.ProductId == pro.Id);
                pList.TotalImage = pro.ProductImages.Count(x => x.ProductId == pro.Id && x.Ispublish == true);


                var productImageList = pro.ProductImages
                    .Where(x => x.ProductId == pro.Id && x.Ispublish == true)
                    .OrderBy(x => x.Order).ToList();

                var productImage = productImageList.FirstOrDefault();
                if (productImage != null)
                {
                    pList.ImageTitle = productImage.Title;
                    pList.ImagePath = productImage.ImagePath;

                }
                pList.AverageRating = averageRating;
            }
            return pList;
        }

        public async Task<ProductListDto> GetProductDetails(Guid productId)
        {
            ProductListDto pList = new ProductListDto();
            Product pro = await _productRepository
                .GetSingleIncludeAsync(x => x.Id == productId, b => b.Brand, c => c.Category, u => u.Unit, rc => rc.ProductCommentses, pi => pi.ProductImages); ;
            if (pro != null)
            {
                pList = _mapper.Map<ProductListDto>(pro);

                pList.BrandName = pro.Brand.Name;
                pList.CategoryName = pro.Category.Name;
                pList.UnitName = pro.Unit.Name;
                pList.FinalPrice = (pro.Price - ((pro.Price * pro.Discount) / 100));
                pList.ProductComments = pro.ProductCommentses.Count(x => x.ProductId == pro.Id);
                pList.TotalImage = pro.ProductImages
                    .Count(x => x.ProductId == pro.Id && x.Ispublish == true);
                
                double averageRating = 0;
                if (pro?.ProductCommentses.Count > 0)
                {
                    var ratings = pro.ProductCommentses.Average(x => x.Rating);
                    averageRating = Math.Round(ratings, 1);
                }

                pList.AverageRating = averageRating;
                pList.ImageList = new List<ProductImageListDto>();

                var productImageList = pro.ProductImages
                                           .Where(x => x.ProductId == pro.Id && x.Ispublish == true)
                                           .OrderBy(x => x.Order)
                                           .ToList();

                var productImage = productImageList.FirstOrDefault();
                if (productImage != null)
                {
                    pList.ImageTitle = productImage.Title;
                    pList.ImagePath = productImage.ImagePath;
                    productImageList.ForEach(x =>
                    {
                        ProductImageListDto image = new ProductImageListDto
                        {
                            Id = x.Id,
                            ImagePath = x.ImagePath,
                            Title = x.Title,
                        };
                        pList.ImageList.Add(image);
                    });
                }
            }
            return pList;
        }

        public async Task<IEnumerable<ProductRelatDto>> GetProductRelated(Guid productid)
        {
            var pro = await _productRepository.GetByIdAsync(productid);
            //get top 4 similar product
            var s = pro.Tag.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var products = await _productRepository
                                .GetAllIncludeAsync(x => x.ProductImages);
            var allProduct = products.Where(x => x.CategoryId == pro.CategoryId && x.Id != pro.Id).ToList();

            var res = allProduct.Select(x => new ProductRelatDto
            {
                product = x,
                count = x.Tag.Split(new char[] { ' ' }).Sum(p => s.Contains(p) ? 1 : 0),
                ImagePaths = x.ProductImages
                .Where(x => x.Ispublish == true)
                .OrderBy(x => x.Order)
                .Select(i => i.ImagePath).Take(2).ToList()
            }).OrderByDescending(p => p.count).Take(4).ToList();

            return res;
        }
        public List<CartItem> AddToCart(List<CartItem> carts, ProductDto pro, CartItem model)
        {
            var cartitem = carts.Find(p => p.ProductId == model.ProductId);
            if (cartitem != null)
            {
                // Already exist, increase by Quantity
                cartitem.Quantity += model.Quantity;
            }
            else
            {
                //  Add new
                carts.Add(new CartItem
                {
                    ProductId = pro.Id,
                    ProductName = pro.Name,
                    FinalPrice = (pro.Price - ((pro.Price * pro.Discount) / 100)),
                    Quantity = model.Quantity
                });
            }
            return carts;
        }

        public async Task<ProductComments> AddProductCommentAsync(ProductComments productComment)
        {
            var item = await _productcommentRepository.InsertAsync(productComment);
            return item;
        }

        public async Task<PagedResponseModel<ProductListDto>> PagedQueryAsync(FilterProductsModel filter)
        {
            var query = _productRepository.Entities;

            if (filter.CategoryId != "" && filter.CategoryId != default(Guid).ToString())
            {
                query = query.Where(x => x.CategoryId.ToString() == filter.CategoryId);
            }

            if (filter.BrandId != "" && filter.BrandId != default(Guid).ToString())
            {
                query = query.Where(x => x.BrandId.ToString() == filter.BrandId);
            }

            if (filter.MinPrice > 0)
            {
                query = query.Where(x => (x.Price - ((x.Price * x.Discount) / 100) >= filter.MinPrice));
            }

            if (filter.MaxPrice > 0)
            {
                query = query.Where(x => (x.Price - ((x.Price * x.Discount) / 100) <= filter.MaxPrice));
            }

            if (!String.IsNullOrEmpty(filter.KeySearch))
            {
                query = query.Where(x => string.IsNullOrEmpty(filter.KeySearch)
                                || x.Name.ToLower().Contains(filter.KeySearch.ToLower())
                                || x.Code.ToLower().Contains(filter.KeySearch.ToLower()));
            }
            

            query = query.Include(x => x.ProductImages);

            switch (filter.OrderProperty)
            {
                case "name":
                    if (filter.Desc)
                        query = query.OrderByDescending(a => a.Name);
                    else
                        query = query.OrderBy(a => a.Name);
                    break;
                case "price":
                    if (filter.Desc)
                        query = query.OrderByDescending(a => (a.Price - ((a.Price * a.Discount) / 100)));
                    else
                        query = query.OrderBy(a => (a.Price - ((a.Price * a.Discount) / 100)));
                    break;
                case "":
                    break;
                default:
                    query = query.OrderByPropertyName(filter.OrderProperty, filter.Desc);
                    break;
            }

            var products = await query.PaginateAsync(filter.Page, filter.Limit);

            var images = products.Items.Select(x => x.ProductImages.OrderBy(y => y.Order)).ToArray();

            var items = _mapper.Map<IEnumerable<ProductListDto>>(products.Items);

            //Update Final Price
            foreach (var i in items)
            {
                i.FinalPrice = (i.Price - ((i.Price * i.Discount) / 100));
            }

            int index = 0;

            foreach (var item in items)
            {
                var img = images[index].OrderBy(x => x.Order).FirstOrDefault();

                item.ImagePath = img == null ? "" : img.ImagePath;

                if (images[index].Count() > 1)
                {
                    item.SecondImagePath = images[index].OrderBy(x => x.Order).Skip(1).FirstOrDefault().ImagePath;
                }
                else
                    item.SecondImagePath = item.ImagePath;

                index++;
            }


            return new PagedResponseModel<ProductListDto>
            {
                CurrentPage = products.CurrentPage,
                TotalPages = products.TotalPages,
                TotalItems = products.TotalItems,
                Items = _mapper.Map<IEnumerable<ProductListDto>>(items)
            };
        }
    }
}
