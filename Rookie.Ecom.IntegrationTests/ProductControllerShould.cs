using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Business;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Business.Services;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using Rookie.Ecom.IntegrationTests.Common;
using Rookie.Ecom.Test;
using Rookie.Ecom.WebApi.Controllers.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rookie.Ecom.IntegrationTests
{
    public class ProductControllerShould : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly SqliteInMemoryFixture _fixture;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IBaseRepository<Unit> _unitRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IProductImageService _productImageService;
        private readonly IBaseRepository<ProductImage> _productimageRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IBaseRepository<ProductComments> _productcommentRepository;

        private readonly IMapper _mapper;
        public ProductControllerShould(SqliteInMemoryFixture fixture)
        {
            _fixture = fixture;
            _fixture.CreateDatabase();

            _categoryRepository = new BaseRepository<Category>(_fixture.Context);
            _unitRepository = new BaseRepository<Unit>(_fixture.Context);
            _brandRepository = new BaseRepository<Brand>(_fixture.Context);

            _productRepository = new BaseRepository<Product>(_fixture.Context);
            _productcommentRepository = new BaseRepository<ProductComments>(_fixture.Context);
            _productimageRepository = new BaseRepository<ProductImage>(_fixture.Context);//
            _productRepository = new BaseRepository<Product>(_fixture.Context);//
            _cloudinaryService = new CloudinaryService();//
            _productImageService = new ProductImageService(
                _productimageRepository, _productRepository, _mapper, _cloudinaryService);//

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Add_New_Product_Success()
        {
            // Arrange
            var productService = new ProductService(_productRepository, _brandRepository,_unitRepository,_categoryRepository, _productImageService, _mapper);
            var homeService = new HomeService(_productRepository,_categoryRepository,_brandRepository,_productcommentRepository,_mapper);
            var productController = new ProductsController(productService, homeService);

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category",
                Description = "Adidas"
            };

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Brand"
            };

            var unit = new Unit
            {
                Id = Guid.NewGuid(),
                Name = "unit"
            };

            await _categoryRepository.InsertAsync(category);
            await _brandRepository.InsertAsync(brand);
            await _unitRepository.InsertAsync(unit);

            var newProduct = new ProductDto { 
                Name = "Test Category", 
                Description = "TC",
                Code = "code",
                Tag = "Tag",
                Price = 100000,
                Discount = 10,
                ProductStock = 10,
                CategoryId = category.Id,
                BrandId = brand.Id,
                UnitId = unit.Id,
            };

            // Act
            var result = await productController.PostProduct(newProduct);

            // Assert
            result.Result.Should().HaveStatusCode(StatusCodes.Status201Created);
            result.Should().NotBeNull();

            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var returnValue = Assert.IsType<Product>(createdResult.Value);

            var productsExited = await productService.GetAllAsync();

            Assert.Equal(newProduct.Name, returnValue.Name);
            Assert.Equal(newProduct.Description, returnValue.Description);

            productsExited.FirstOrDefault().Id.Should().NotBe(default(Guid));
        }

        [Fact]
        public async Task Add_New_Product_Fail_Exited()
        {
            // Arrange
            var productService = new ProductService(_productRepository, _brandRepository, _unitRepository, _categoryRepository, _productImageService, _mapper);
            var homeService = new HomeService(_productRepository, _categoryRepository, _brandRepository, _productcommentRepository, _mapper);
            var productController = new ProductsController(productService, homeService);

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category",
                Description = "Adidas"
            };

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Brand"
            };

            var unit = new Unit
            {
                Id = Guid.NewGuid(),
                Name = "unit"
            };

            await _categoryRepository.InsertAsync(category);
            await _brandRepository.InsertAsync(brand);
            await _unitRepository.InsertAsync(unit);

            var newProduct = new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Category",
                Description = "TC",
                Code = "code",
                Tag = "Tag",
                Price = 100000,
                Discount = 10,
                ProductStock = 10,
                CategoryId = category.Id,
                BrandId = brand.Id,
                UnitId = unit.Id,
            };
            var exitedProduct = _mapper.Map<Product>(newProduct);
            await _productRepository.InsertAsync(exitedProduct);

            // Act
            var result = await productController.PostProduct(newProduct);

            // Assert
            result.Result.Should().HaveStatusCode(StatusCodes.Status400BadRequest);
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task Delete_Product_Success()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category",
                Description = "Adidas"
            };

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Brand"
            };

            var unit = new Unit
            {
                Id = Guid.NewGuid(),
                Name = "unit"
            };

            await _categoryRepository.InsertAsync(category);
            await _brandRepository.InsertAsync(brand);
            await _unitRepository.InsertAsync(unit);

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Category",
                Description = "TC",
                Code = "code",
                Tag = "Tag",
                Price = 100000,
                Discount = 10,
                ProductStock = 10,
                CategoryId = category.Id,
                BrandId = brand.Id,
                UnitId = unit.Id,
            };

            await _productRepository.InsertAsync(newProduct);

            var productService = new ProductService(_productRepository, _brandRepository, _unitRepository, _categoryRepository, _productImageService, _mapper);
            var homeService = new HomeService(_productRepository, _categoryRepository, _brandRepository, _productcommentRepository, _mapper);
            var productController = new ProductsController(productService, homeService);

            // Act
            var result = await productController.DeleteProduct(newProduct.Id);
            var okResult = result as OkObjectResult;

            var productExited = await productService.GetByIdAsync(newProduct.Id);

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Delete success", okResult.Value);
            productExited.Should().Be(null);
        }

        [Fact]
        public async Task Delete_Product_Failed()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var productService = new ProductService(_productRepository, _brandRepository, _unitRepository, _categoryRepository, _productImageService, _mapper);
            var homeService = new HomeService(_productRepository, _categoryRepository, _brandRepository, _productcommentRepository, _mapper);
            var productController = new ProductsController(productService, homeService);
            // Act
            var result = await productController.DeleteProduct(id);
            var failedResult = result as BadRequestResult;

            // Assert
            Assert.Equal(400, failedResult.StatusCode);
        }

        [Fact]
        public async Task Put_Category_Success()
        {
            // Arrange
            Guid productid = Guid.NewGuid();
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category",
                Description = "Adidas"
            };

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Brand"
            };

            var unit = new Unit
            {
                Id = Guid.NewGuid(),
                Name = "unit"
            };

            await _categoryRepository.InsertAsync(category);
            await _brandRepository.InsertAsync(brand);
            await _unitRepository.InsertAsync(unit);

            var existProduct = new Product
            {
                Id = productid,
                Name = "Test Category",
                Description = "TC",
                Code = "code",
                Tag = "Tag",
                Price = 100000,
                Discount = 10,
                ProductStock = 10,
                CategoryId = category.Id,
                BrandId = brand.Id,
                UnitId = unit.Id,
            };

            var modifiedProduct = new ProductDto
            {
                Id = productid,
                Name = "Test Category Ahihi",
                Description = "TC",
                Code = "code",
                Tag = "Tag",
                Price = 100000,
                Discount = 10,
                ProductStock = 10,
                CategoryId = category.Id,
                BrandId = brand.Id,
                UnitId = unit.Id,
            };

            await _productRepository.InsertAsync(existProduct);
            _fixture.Context.Entry(existProduct).State = EntityState.Detached;

            var productService = new ProductService(_productRepository, _brandRepository, _unitRepository, _categoryRepository, _productImageService, _mapper);
            var homeService = new HomeService(_productRepository, _categoryRepository, _brandRepository, _productcommentRepository, _mapper);
            var productController = new ProductsController(productService, homeService);

            // Act
            var result = await productController.PutProduct(modifiedProduct);
            var okResult = result as OkObjectResult;

            var productExited = await productService.GetByIdAsync(modifiedProduct.Id);

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Update success", okResult.Value);
            Assert.Equal(modifiedProduct.Name, productExited.Name);
        }

        [Fact]
        public async Task Put_Product_Failed_ProductNotExist()
        {
            // Arrange
            Guid productid = Guid.NewGuid();
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category",
                Description = "Adidas"
            };

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Brand"
            };

            var unit = new Unit
            {
                Id = Guid.NewGuid(),
                Name = "unit"
            };

            await _categoryRepository.InsertAsync(category);
            await _brandRepository.InsertAsync(brand);
            await _unitRepository.InsertAsync(unit);

            var existProduct = new Product
            {
                Id = productid,
                Name = "Test Category",
                Description = "TC",
                Code = "code",
                Tag = "Tag",
                Price = 100000,
                Discount = 10,
                ProductStock = 10,
                CategoryId = category.Id,
                BrandId = brand.Id,
                UnitId = unit.Id,
            };

            var modifiedProduct = new ProductDto
            {
                Id = productid,
                Name = "Test Category Ahihi",
                Description = "TC",
                Code = "code",
                Tag = "Tag",
                Price = 100000,
                Discount = 10,
                ProductStock = 10,
                CategoryId = category.Id,
                BrandId = brand.Id,
                UnitId = unit.Id,
            };

            await _productRepository.InsertAsync(existProduct);

            var productService = new ProductService(_productRepository, _brandRepository, _unitRepository, _categoryRepository, _productImageService, _mapper);
            var homeService = new HomeService(_productRepository, _categoryRepository, _brandRepository, _productcommentRepository, _mapper);
            var productController = new ProductsController(productService, homeService);

            // Act
            var result = await productController.PutProduct(modifiedProduct);
            var badResult = result as BadRequestObjectResult;

            // assert
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }
    }
}
