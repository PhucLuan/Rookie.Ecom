using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.DataAccessor.Data;
using Rookie.Ecom.DataAccessor.Models.Admin;

namespace FashionShopMVC.ViewComponents
{
    public class GetHeaderMainMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public GetHeaderMainMenuViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid categoryId = default(Guid))
        {
            List<CategoryDto> categories = new List<CategoryDto>();
            if (categoryId != default(Guid))
            {
                categories = await GetLists(categoryId);
                return View("SubMenu", categories.OrderBy(x=>x.Name).ToList());
            }
            else
            {
                categories = await GetLists();
                return View("MainMenu", categories.OrderBy(x => x.Name).ToList());
            }
            
        }

        private Task<List<CategoryDto>> GetLists(Guid categoryId = default(Guid))
        {
            List<CategoryDto> categories = new List<CategoryDto>();
            if (categoryId != default(Guid))
            {
                _categoryService.GetAll().Where(x => x.CategoryId == categoryId && x.Ispublish == true).ToList().ForEach(c =>
                {
                    CategoryDto category = new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    };
                   
                        categories.Add(category);
                   
                });
                return Task.Run(() => categories);
            }
            else
            {
                _categoryService.GetAll().Where(x => x.CategoryId == null && x.Ispublish == true).ToList().ForEach(c =>
                {
                    CategoryDto category = new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    };
                    
                        categories.Add(category);
                    
                });
                return Task.Run(() => categories);
            }
        }
    }
}