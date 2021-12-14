using Rookie.Ecom.DataAccessor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class CategoryDto : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool Ispublish { get; set; }

        [Display(Name = "Category")]
        public Guid? CategoryId { get; set; }
        public List<MySelectListItem> Categories { get; set; }
        public List<CategoryDto> CategoryMenus { get; set; }
    }

    public class CategoryListDto : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool Ispublish { get; set; }

        public Guid? CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryParentName { get; set; }
        public int TotalProduct { get; set; }
    }
}
