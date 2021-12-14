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
    public class CategoryServiceShould
    {
        private readonly CategoryService _categoryService;
        private readonly Mock<IBaseRepository<Category>> _categoryRepository;
        private readonly Mock<IProductImageService> _productImageService;//
        private readonly IMapper _mapper;

        public CategoryServiceShould()
        {
            _categoryRepository = new Mock<IBaseRepository<Category>>();

            _productImageService = new Mock<IProductImageService>();//

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();

            _categoryService = new CategoryService(
                    _categoryRepository.Object,
                    _productImageService.Object,
                    _mapper
                );
        }


        [Fact]
        public async Task GetAsyncShouldReturnNullAsync()
        {
            Random rnd = new Random();

            Guid id = Guid.NewGuid();

            Guid id2 = Guid.NewGuid();

            var entity = new Category()
            {
                Description = "code",
                Id = id2,
                Name = "Name"
            };

            List<Category> categories = new List<Category>();
            categories.Add(entity);
            _categoryRepository
                  .Setup(x => x.GetByIdAsync(id))
                  .Returns(Task.FromResult<Category>(null));

            _categoryRepository
                  .Setup(x => x.GetAll())
                  .Returns(categories);

            var result = await _categoryService.GetByIdAsync(id);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsyncShouldReturnObjectAsync()
        {
            //Arrange
            var entity = new Category()
            {
                Description = "code",
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            List<Category> categories = new List<Category>();
            categories.Add(entity);

            _categoryRepository
                 .Setup(x => x.GetAll())
                 .Returns(categories);

            _categoryRepository.Setup(x => x.GetByIdAsync(entity.Id)).Returns(Task.FromResult(entity));
            
            //Act
            var result = await _categoryService.GetByIdAsync(entity.Id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(entity.Id);

            _categoryRepository.Verify(mock => mock.GetByIdAsync(entity.Id), Times.Once);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnList()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var entity = new Category()
            {
                Description = "code",
                Id = id,
                Name = "Name"
            };

            IList<Category> categories = new List<Category>()
                 { new Category()
            {
                Description = "code",
                Id = id,
                Name = "Name"
            }};
            
            _categoryRepository
                 .Setup(x => x.GetAllIncludeAsync(
                     It.IsAny<Expression<Func<Category, object>>>(),
                     It.IsAny<Expression<Func<Category, object>>>())
                 )
                 .ReturnsAsync(categories);
            //Act
            var result = await _categoryService.GetAllAsync();
            //Assert

            result.Should().NotBeNull();
        }
        [Fact]
        public async Task AddCategoryShouldThrowExceptionAsync()
        {
            Func<Task> act = async () => await _categoryService.AddAsync(null);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddCategoryShouldBeSuccessfullyAsync()
        {
            var category = new Category()
            {
                Description = "code",
                Id = Guid.NewGuid(),
                Name = "name"
            };

            var categoryDto = new CategoryDto()
            {
                Description = "code",
                Id = Guid.NewGuid(),
                Name = "name"
            };
            _categoryRepository.Setup(x => x
                .GetByAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<string>()))
                .Returns(Task.FromResult<Category>(category));

            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<Category>(null));

            _categoryRepository.Setup(x => x.InsertAsync(It.IsAny<Category>()))
                .Returns(Task.FromResult(category));

            var result = await _categoryService.AddAsync(categoryDto);

            result.Should().NotBeNull();

            _categoryRepository.Verify(mock => mock.InsertAsync(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public async Task AddCategoryShouldThrowArgumentExceptionAsync()
        {
            Random rnd = new Random();

            Guid id = Guid.NewGuid();

            //int id2 = id;
            
            var category = new Category()
            {
                Description = "code",
                Id = id,
                Name = "name"
            };

            var categoryDto = new CategoryDto()
            {
                Description = "code",
                Id = id,
                Name = "name"
            };

            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(category));

            Func<Task> act = async () => await _categoryService.AddAsync(categoryDto);
            await act.Should().ThrowAsync<ArgumentException>();

            _categoryRepository.Verify(mock => mock.InsertAsync(It.IsAny<Category>()), Times.Never());
        }

        
        [Fact]
        public async Task DeleteCategoryShouldThrowArgumentNullExceptionAsync()
        {
            Guid id = Guid.NewGuid();

            _categoryRepository.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult<Category>(null));

            Func<Task> act = async () => await _categoryService.DeleteAsync(id);
            
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteCategoryShouldSuccess()
        {
            List<Category> categories = new List<Category>();

            Guid id = Guid.NewGuid();
            var entity = new Category()
            {
                Description = "code",
                Id = id,
                Name = "Name",
                CategoriesList = categories
            };

            _categoryRepository.Setup(x => x.GetSingleIncludeAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, object>>>()))
                .Returns(Task.FromResult<Category>(entity));

            Func<Task> act = async () => await _categoryService.DeleteAsync(id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateCategoryShouldSuccess()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var categoryDto = new CategoryDto()
            {
                Description = "code",
                Id = id,
                Name = "Name"
            };

            //Act
            Func<Task> act = async () => await _categoryService.UpdateAsync(categoryDto);

            //Assert
            await act.Should().NotThrowAsync();
        }
    }
    
}
