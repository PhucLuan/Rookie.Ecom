using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Business.Extensions;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IBaseRepository<Unit> _unitRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IProductImageService _productImageService;

        private readonly IMapper _mapper;

        public ProductService(
            IBaseRepository<Product> productRepository,
            IBaseRepository<Brand> brandRepository,
            IBaseRepository<Unit> unitRepository,
            IBaseRepository<Category> categoryRepository,
            IProductImageService productImageService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _unitRepository = unitRepository;
            _categoryRepository = categoryRepository;
            _productImageService = productImageService;
            _mapper = mapper;
        }

        public async Task<Product> AddAsync(ProductDto productDto)
        {
            var product = new Product();

            product = _mapper.Map<ProductDto, Product>(productDto);
            product.AddedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;

            return await _productRepository.InsertAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            Product product = await _productRepository.GetByIdAsync(id);
            Ensure.Any.IsNotNull(product, nameof(product));

            var productImages = await _productImageService.GetByIdProductAsync(product.Id);

            var public_ids = productImages.Select(x => x.PublicId);

            //Delete product image
            await _productImageService.DeleteListImageAsync(public_ids.ToArray());
            
            await _productRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _productRepository.ExistAsync(predicate);
        }

        public async Task<ProductDto> GetAddEditAsync(Guid id)
        {
            ProductDto model = new ProductDto();
            model.BrandList = _brandRepository.GetAll().Select(x => new MySelectListItem
            {
                Name = x.Name,
                Id = x.Id.ToString()
            }).ToList();

            model.CategoryList = _categoryRepository.GetAll().Select(x => new MySelectListItem
            {
                Name = x.Name,
                Id = x.Id.ToString()
            }).ToList();

            model.UnitList = _unitRepository.GetAll().Select(x => new MySelectListItem
            {
                Name = x.Name,
                Id = x.Id.ToString()
            }).ToList();

            if (id != default(Guid))
            {
                Product product = await _productRepository.GetByIdAsync(id);
                model.Name = product.Name;
                model.Code = product.Code;
                model.Tag = product.Tag;
                model.CategoryId = product.CategoryId;
                model.BrandId = product.BrandId;
                model.UnitId = product.UnitId;
                model.Description = product.Description;
                model.Price = product.Price;
                model.Discount = product.Discount;
                model.ProductStock = product.ProductStock;
                model.Ispublish = product.Ispublish;
            }
            return model;
        }

        public async Task<IEnumerable<ProductListDto>> GetAllAsync()
        {
            List<ProductListDto> model = new List<ProductListDto>();
            var dbData = await _productRepository.GetAllIncludeAsync(
                pi => pi.ProductImages);

            foreach (var b in dbData)
            {
                ProductListDto product = new ProductListDto();
                product = _mapper.Map<Product, ProductListDto>(b);
                product.FinalPrice = (b.Price - ((b.Price * b.Discount) / 100));
                if (b.ProductImages != null && b.ProductImages.Count > 0)
                {
                    product.ImagePath = b.ProductImages.FirstOrDefault().ImagePath;
                }
                //product.ProductStock = 0;
                model.Add(product);
            }

            return model;
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var res = _mapper.Map<ProductDto>(product);
            return res;
        }

        public async Task<ProductDto> GetByNameAsync(string name)
        {
            var product = await _productRepository.GetByAsync(x => x.Name == name);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ListFilterProduct> GetFilterProductAsync()
        {
            ListFilterProduct model = new ListFilterProduct();

            var brands = await _brandRepository.GetAllAsync();
            var brandSelectList = brands.Select(x => new MySelectListItem
            {
                Name = x.Name,
                Id = x.Id.ToString()
            }).ToList();

            brandSelectList.Add(
                new MySelectListItem
                {
                    Name = "Get All",
                    Id = default(Guid).ToString()
                }
            );
            model.BrandList = brandSelectList.ToList();

            //Get list category
            var categories = await _categoryRepository.GetAllAsync();

            var categorySelectList = categories.Select(x => new MySelectListItem
            {
                Name = x.Name,
                Id = x.Id.ToString()
            }).ToList();

            categorySelectList.Add(
                new MySelectListItem
                {
                    Name = "Get All",
                    Id = default(Guid).ToString()
                }
            );

            model.CategoryList = categorySelectList.ToList();

            return model;
        }

        public async Task<PagedResponseModel<ProductListDto>> PagedQueryAsync(FilterProductsModel filter)
        {
            //CategoryIds,StateId, KeySearch, Page, Limit
            var query = _productRepository.Entities;
            //List<string> CategoryIds = new List<string>();

            //if (CategoryIds.Count > 0)
            //{
            //    query = query.Where(x => CategoryIds.Contains(x.CategoryId.ToString()));
            //}

            if (filter.CategoryId != "" && filter.CategoryId != default(Guid).ToString())
            {
                query = query.Where(x => x.CategoryId.ToString() == filter.CategoryId);
            }

            if (filter.BrandId != "" && filter.BrandId != default(Guid).ToString())
            {
                query = query.Where(x => x.BrandId.ToString() == filter.BrandId);
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
                case "modifieddate":
                    if (filter.Desc)
                        query = query.OrderByDescending(x => x.ModifiedDate.Date).ThenByDescending(x => x.ModifiedDate.TimeOfDay);
                    else
                        query = query.OrderBy(x => x.ModifiedDate.Date).ThenBy(x => x.ModifiedDate.TimeOfDay);
                    break;
                default:
                    //query = query.OrderByPropertyName(filter.OrderProperty, filter.Desc);
                    break;
            }


            var products = await query.PaginateAsync(filter.Page, filter.Limit);

            var images = products.Items.Select(x => x.ProductImages).ToArray();

            var items = _mapper.Map<IEnumerable<ProductListDto>>(products.Items);

            int index = 0;

            foreach (var item in items)
            {
                var img = images[index++].OrderBy(x => x.Order).FirstOrDefault();

                item.ImagePath = img == null ? "" : img.ImagePath;
            }

            return new PagedResponseModel<ProductListDto>
            {
                CurrentPage = products.CurrentPage,
                TotalPages = products.TotalPages,
                TotalItems = products.TotalItems,
                Items = _mapper.Map<IEnumerable<ProductListDto>>(items)
            };
        }

        public async Task UpdateAsync(ProductDto model)
        {
            var product = _mapper.Map<Product>(model);
            product.ModifiedDate = DateTime.Now;
            await _productRepository.UpdateAsync(product);
        }
    }
}
