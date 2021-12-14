using Microsoft.AspNetCore.Http;
using Rookie.Ecom.DataAccessor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class ProductImageDto : BaseEntity
    {
        [Display(Name = "Product")]
        public Guid ProductId { get; set; }
        public string ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Title { get; set; }
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }
        public bool Ispublish { get; set; }
        public int Order { get; set; }
        public List<MySelectListItem> Categories { get; set; }
    }

    public class ProductImageListDto : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string ProductName { get; set; }
        public bool Ispublish { get; set; }
        public string PublicId { get; set; }
        public int Order { get; set; }
    }
}
