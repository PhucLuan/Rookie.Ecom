using AutoMapper;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Contracts.Dtos.Public;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IProductImageService _productImageService;
        private readonly IMapper _mapper;

        public CategoryService() { }
        public CategoryService(
            IBaseRepository<Category> categoryRepository,
            IProductImageService productImageService,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _productImageService = productImageService;
            _mapper = mapper;
        }

        public async Task<CategoryDto> AddAsync(CategoryDto categoryDto)
        {
            Ensure.Any.IsNotNull(categoryDto, nameof(categoryDto));

            var exitcategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);

            if (exitcategory != null)
            {
                throw new ArgumentException();
            }

            var category = _mapper.Map<CategoryDto, Category>(categoryDto);

            category.AddedDate = DateTime.Now;
            category.ModifiedDate = DateTime.Now;

            var item = await _categoryRepository.InsertAsync(category);
            return _mapper.Map<CategoryDto>(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetSingleIncludeAsync(x => x.Id == id,x => x.CategoriesList);
            Ensure.Any.IsNotNull(category, nameof(category));
            var CategoriesList = category.CategoriesList;
            if (category.CategoriesList.Count > 0)
            {
                var subcategory = await _categoryRepository.Query().Where(x => x.CategoryId == category.Id).ToListAsync();
                foreach (var item in subcategory)
                {
                    item.CategoryId = null;
                    await _categoryRepository.UpdateAsync(item);
                }
            }
            
            //Get list image of category
            var images = _categoryRepository.Query().Where(x => x.Id == category.Id)
                .Include(x => x.Products)
                .ThenInclude(p => p.ProductImages)
                .SelectMany(i => i.Products.SelectMany(l => l.ProductImages.Select(m => m.PublicId)));

            await _productImageService.DeleteListImageAsync(images.ToArray());


            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _categoryRepository.ExistAsync(predicate);
        }

        public ICollection<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public async Task<IEnumerable<CategoryListDto>> GetAllAsync()
        {
            List<CategoryListDto> model = new List<CategoryListDto>();

            var categories = await _categoryRepository
                .GetAllIncludeAsync(x => x.Categoris, p => p.Products);

            categories = categories.OrderByDescending(x => x.ModifiedDate.Date)
                .ThenBy(x => x.ModifiedDate.TimeOfDay).ToList();

            foreach (var c in categories)
            {
                CategoryListDto categoryList = new CategoryListDto();
                categoryList = _mapper.Map<Category, CategoryListDto>(c);
                categoryList.CategoryParentName = c.Categoris?.Name;
                categoryList.TotalProduct = c.Products?.Count ?? 0;
                model.Add(categoryList);
            }
            return model;
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            
            CategoryDto model = new CategoryDto();
            
            if (id != default(Guid))
            {
                Category category = await _categoryRepository.GetByIdAsync(id);

                if (category != null)
                {
                    model = _mapper.Map<CategoryDto>(category);

                    model.Categories = _categoryRepository
                        .GetAll()
                        .Select(c => new MySelectListItem
                        {
                            Name = c.Name,
                            Id = c.Id.ToString()
                        }).OrderBy(x => x.Name).ToList();

                    var totalCategory = await _categoryRepository.CountAsync();
                    model.Order = totalCategory + 1;
                    return model;
                }
                else return null;

            }
            else throw new ArgumentException();
        }

        public async Task<CategoryDto> GetByNameAsync(string name)
        {
            var category = await _categoryRepository.GetByAsync(x => x.Name == name);
            return _mapper.Map<CategoryDto>(category);
        }

        public Task<CategoryDto> PagedQueryAsync(string name, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(CategoryDto categoryDto)
        {

            var category = _mapper.Map<Category>(categoryDto);

            category.ModifiedDate = DateTime.Now;
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
