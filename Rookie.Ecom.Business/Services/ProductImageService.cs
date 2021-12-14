using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IBaseRepository<ProductImage> _productimageRepository;
        private readonly IBaseRepository<Product> _productRepository;

        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductImageService(
            IBaseRepository<ProductImage> productimageRepository,
            IBaseRepository<Product> productRepository,
            IMapper mapper,
            ICloudinaryService cloudinaryService)
        {
            _productimageRepository = productimageRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task AddAsync(Account account,ProductImageDto productImageDto)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                productImageDto.ImageFile.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            Stream stream = new MemoryStream(fileBytes);

            string publicId = Guid.NewGuid().ToString();

            var resUrl = await _cloudinaryService.UploadImageByStream(
                    account, Guid.NewGuid().ToString(), stream, publicId);

            ProductImage pImage = new ProductImage
            {
                ProductId = productImageDto.ProductId,
                Title = productImageDto.Title,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Ispublish = productImageDto.Ispublish,
                ImagePath = resUrl,
                PublicId = publicId,
                Order = productImageDto.Order
            };

            await _productimageRepository.InsertAsync(pImage);
        }

        public async Task DeleteAsync(Account account,Guid id, string publicId)
        {
            

            var deleteResult = await _cloudinaryService.RemoveImage(account, publicId);
            if (deleteResult.StatusCode == HttpStatusCode.OK)
            {
                ProductImage productImage = await _productimageRepository.GetByIdAsync(id);
                if (productImage != null)
                {
                    await _productimageRepository.DeleteAsync(productImage.Id);
                }
            }
            

        }

        public async Task<IEnumerable<ProductImageListDto>> GetAllAsync(string search)
        {
            IEnumerable<ProductImageListDto> model = new List<ProductImageListDto>();
            if (String.IsNullOrEmpty(search))
            {
                model = await GetAllProductImageList();
            }
            else
            {
                var res = await GetAllProductImageList();

                model = res.Where(x => x.ProductName.ToLower()
                    .Contains(search.ToLower()) || x.Title.ToLower().Contains(search.ToLower()));
            }
            return model;
        }

        public async Task UpdateAsync(ProductImageDto model)
        {
            ProductImage pImage = await _productimageRepository.GetByIdAsync(model.Id);
            if (pImage != null)
            {
                pImage.Title = model.Title;
                pImage.ModifiedDate = DateTime.Now;
                pImage.Ispublish = model.Ispublish;
                pImage.Order = model.Order;
                await _productimageRepository.UpdateAsync(pImage);
            }
        }


        //Help method
        public async Task<List<ProductImageListDto>> GetAllProductImageList()
        {
            List<ProductImageListDto> productImageList = new List<ProductImageListDto>();
            var res = await _productimageRepository.GetAllIncludeAsync(p => p.Product);

            res.OrderByDescending(x => x.AddedDate).ToList().ForEach(p =>
            {
                ProductImageListDto productImage = new ProductImageListDto
                {
                    Id = p.Id,
                    ProductId = p.Id,
                    Title = p.Title,
                    ImagePath = p.ImagePath,
                    ProductName = p.Product.Name,
                    Ispublish = p.Ispublish,
                    AddedDate = p.AddedDate,
                    Order = p.Order
                };
                productImageList.Add(productImage);
            });
            return productImageList;
        }

        public async Task<IEnumerable<ProductImageListDto>> GetByIdProductAsync(Guid id)
        {
            List<ProductImageListDto> productImageList = new List<ProductImageListDto>();

            var productImages = await _productimageRepository.GetAllAsync();

            productImages.Where(x => x.ProductId == id).ToList().ForEach(x =>
            {
                ProductImageListDto pImage = new ProductImageListDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ImagePath = x.ImagePath,
                    Title = x.Title,
                    ProductName = _productRepository.GetById(id).Name,
                    Ispublish = x.Ispublish,
                    PublicId = x.PublicId,
                    AddedDate = x.AddedDate,
                    Order = x.Order
                };
                productImageList.Add(pImage);
            });

            return productImageList;
        }

        public async Task DeleteListImageAsync(string[] public_ids)
        {
            Account account = new Account(
                       "do4nzmm1t",
                       "281334857314385",
                       "P5SqKpG3AAQu97rM1uZFRD22yYI");

            await _cloudinaryService.RemoveListImage(account,public_ids);
        }
    }
}
