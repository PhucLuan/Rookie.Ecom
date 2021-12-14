using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rookie.Ecom.Business;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Business.Services;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using Rookie.Ecom.IntegrationTests.Common;
using Rookie.Ecom.WebApi.Controllers.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Rookie.Ecom.Test;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Rookie.Ecom.IntegrationTests
{
    public class CategoryControllerShould : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly SqliteInMemoryFixture _fixture;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IProductImageService _productImageService;

        private readonly IBaseRepository<ProductImage> _productimageRepository;
        private readonly IBaseRepository<Product> _productRepository;

        private readonly ICloudinaryService _cloudinaryService;


        private readonly IMapper _mapper;
        public CategoryControllerShould(SqliteInMemoryFixture fixture)
        {
            _fixture = fixture;
            _fixture.CreateDatabase();
            _categoryRepository = new BaseRepository<Category>(_fixture.Context);

            _productimageRepository = new BaseRepository<ProductImage>(_fixture.Context);//
            _productRepository = new BaseRepository<Product>(_fixture.Context);//
            _cloudinaryService = new CloudinaryService();//
            _productImageService = new ProductImageService(
                _productimageRepository, _productRepository,_mapper,_cloudinaryService);//

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Add_New_Category_Success()
        {
            // Arrange
            var categoryService = new CategoryService(_categoryRepository, _productImageService, _mapper);
            var categoryController = new CategoriesController(categoryService);

            var newCategory = new CategoryDto { Name = "Test Category", Description = "TC" };

            // Act
            var result = await categoryController.PostCategory(newCategory);
            
            // Assert
            result.Result.Should().HaveStatusCode(StatusCodes.Status201Created);
            result.Should().NotBeNull();

            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var returnValue = Assert.IsType<CategoryDto>(createdResult.Value);

            var categoriesExited = await categoryService.GetAllAsync();

            Assert.Equal(newCategory.Name, returnValue.Name);
            Assert.Equal(newCategory.Description, returnValue.Description);

            categoriesExited.FirstOrDefault().Id.Should().NotBe(default(Guid));
            //returnValue.Id.Should().NotBe(default(int));
        }

        [Fact]
        public async Task Add_Exist_Category_Existed()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var existCategory = new Category { Id = id, Name = "Laptop", Description = "LT" };
            await _categoryRepository.InsertAsync(existCategory);

            var categoryService = new CategoryService(_categoryRepository, _productImageService, _mapper);
            var categoryController = new CategoriesController(categoryService);

            var newCategory = new CategoryDto { Id = id, Name = "Laptop 2", Description = "ABC" };

            // Act
            var result = await categoryController.PostCategory(newCategory);

            // Assert
            result.Result.Should().HaveStatusCode(StatusCodes.Status400BadRequest);
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task Get_All_Categories()
        {
            //Arrange
            var category1 = new Category { Name = "Cate 1", Description = "Code1" };
            var category2 = new Category { Name = "Cate 2", Description = "Code2" };
            var category3 = new Category { Name = "Cate 3", Description = "Code3" };
            var category4 = new Category { Name = "Cate 4", Description = "Code4" };
            await _categoryRepository.InsertAsync(category1);
            await _categoryRepository.InsertAsync(category2);
            await _categoryRepository.InsertAsync(category3);
            await _categoryRepository.InsertAsync(category4);

            var categoryService = new CategoryService(_categoryRepository, _productImageService, _mapper);
            var categoryController = new CategoriesController(categoryService);

            // Act
            var result = await categoryController.GetAllCategories();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(4);
        }

        [Fact]
        public async Task Put_Category_Success()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var existCategory = new Category { Id = id, Name = "Laptop", Description = "LT" };
            var modifiedCategory = new CategoryDto { Id = id, Name = "LaptopAhihi", Description = "LT", CategoryId = null };

            await _categoryRepository.InsertAsync(existCategory);
            _fixture.Context.Entry(existCategory).State = EntityState.Detached;

            var categoryService = new CategoryService(_categoryRepository, _productImageService, _mapper);
            var categoryController = new CategoriesController(categoryService);

            // Act
            var result = await categoryController.PutCategory(modifiedCategory);
            var okResult = result as OkObjectResult;

            var categoriesExited = await categoryService.GetByIdAsync(modifiedCategory.Id);

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Update success", okResult.Value);
            Assert.Equal(modifiedCategory.Name, categoriesExited.Name);
        }

        [Fact]
        public async Task Put_Category_Failed_CategoryNotExist()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var existCategory = new Category { Id = id, Name = "Laptop", Description = "LT", CategoryId = null };
            var modifiedCategory = new CategoryDto { Id = Guid.NewGuid(), Name = "LaptopAhihi", Description = "LT", CategoryId = null };

            await _categoryRepository.InsertAsync(existCategory);

            var categoryService = new CategoryService(_categoryRepository, _productImageService, _mapper);
            var categoryController = new CategoriesController(categoryService);

            // Act
            var result = await categoryController.PutCategory(modifiedCategory);
            var badResult = result as BadRequestObjectResult;

            // assert
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public async Task Delete_Category_Success()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var newCategory = new Category { Id = id, Name = "Laptop", Description = "LT" };

            await _categoryRepository.InsertAsync(newCategory);

            var categoryService = new CategoryService(_categoryRepository, _productImageService, _mapper);
            var categoryController = new CategoriesController(categoryService);

            // Act
            var result = await categoryController.DeleteCategory(newCategory.Id);
            var okResult = result as OkObjectResult;

            var categoryExited = await categoryService.GetByIdAsync(newCategory.Id);

            // assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Delete Success", okResult.Value);
            categoryExited.Should().Be(null);
        }

        [Fact]
        public async Task Delete_Category_Failed()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var categoryService = new CategoryService(_categoryRepository, _productImageService, _mapper);
            var categoryController = new CategoriesController(categoryService);
            // Act
            var result = await categoryController.DeleteCategory(id);
            var failedResult = result as BadRequestResult;

            // assert
            Assert.Equal(400, failedResult.StatusCode);
        }
    }
}
