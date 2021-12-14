using AutoMapper;
using FluentAssertions;
using Moq;
using Rookie.Ecom.Business;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Business.Services;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rookie.Ecom.UnitTest
{
    public class ProductServiceShould
    {
        private readonly ProductService _productService;
        private readonly Mock<IProductImageService> _productImageService;
        private readonly Mock<IBaseRepository<Product>> _productRepository;
        private readonly Mock<IBaseRepository<Brand>> _brandRepository;
        private readonly Mock<IBaseRepository<Unit>> _unitRepository;
        private readonly Mock<IBaseRepository<Category>> _categoryRepository;

        private readonly IMapper _mapper;

        public ProductServiceShould()
        {
            _productRepository = new Mock<IBaseRepository<Product>>();
            _brandRepository = new Mock<IBaseRepository<Brand>>();
            _unitRepository = new Mock<IBaseRepository<Unit>>();
            _categoryRepository = new Mock<IBaseRepository<Category>>();
            _productImageService = new Mock<IProductImageService>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = config.CreateMapper();

            _productService = new ProductService(
                    _productRepository.Object,
                    _brandRepository.Object,
                    _unitRepository.Object,
                    _categoryRepository.Object,
                    _productImageService.Object,
                    _mapper
                );
        }

        [Fact]
        public async Task AddCategoryShouldBeSuccessfullyAsync()
        {
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

            var newProductDto = new ProductDto
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

            _productRepository.Setup(x => x.InsertAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(newProduct));

            var result = await _productService.AddAsync(newProductDto);

            result.Should().NotBeNull();

            _productRepository.Verify(mock => mock.InsertAsync(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnNullAsync()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var entity = new Product()
            {
                Description = "code",
                Id = id,
                Name = "Name"
            };

            IList<Product> products = new List<Product>()
                 { new Product()
            {
                Description = "code",
                Id = id,
                Name = "Name",
            }};

            _productRepository
                 .Setup(x => x.GetAllIncludeAsync(
                     It.IsAny<Expression<Func<Product, object>>>()
                     ))
                 .ReturnsAsync(products);
            //Act
            var res = await _productService.GetAllAsync();

            //Assert
            res.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteProductShouldThrowArgumentNullExceptionAsync()
        {
            Guid id = Guid.NewGuid();

            _productRepository.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult<Product>(null));

            Func<Task> act = async () => await _productService.DeleteAsync(id);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteProductShouldSuccess()
        {
            Guid id = Guid.NewGuid();
            var entity = new Product()
            {
                Description = "code",
                Id = id,
                Name = "Name"
            };
            _productRepository.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult<Product>(entity));

            Func<Task> act = async () => await _productService.DeleteAsync(id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateProductShouldSuccess()
        {
            //Arrange
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

            var ProductDto = new ProductDto
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

            //Act
            Func<Task> act = async () => await _productService.UpdateAsync(ProductDto);

            //Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAsyncShouldReturnObjectAsync()
        {
            //Arrange
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

            var Product = new Product
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

            _productRepository.Setup(x => x.GetByIdAsync(Product.Id))
                .Returns(Task.FromResult(Product));

            //Act
            var result = await _productService.GetByIdAsync(Product.Id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(Product.Id);

            _productRepository.Verify(mock => mock.GetByIdAsync(Product.Id), Times.Once);
        }
    }
}
