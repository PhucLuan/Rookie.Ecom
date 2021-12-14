using Microsoft.AspNetCore.Http;
using Rookie.Ecom.DataAccessor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class ProductDto
    {
        public ProductDto()
        {
            //Images = new IFormFile();
            CategoryList = new List<MySelectListItem>();
            BrandList = new List<MySelectListItem>();
            UnitList = new List<MySelectListItem>();
        }
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Tag { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid BrandId { get; set; }
        [Required]
        public Guid UnitId { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public int Discount { get; set; }
        public int ProductStock { get; set; }
        public bool Ispublish { get; set; }
        public List<MySelectListItem> CategoryList { get; set; }
        public List<MySelectListItem> BrandList { get; set; }
        public List<MySelectListItem> UnitList { get; set; }
    }

    public class ProductListDto : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Tag { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public string BrandName { get; set; }
        public Guid BrandId { get; set; }
        public string UnitName { get; set; }
        public Guid UnitId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public double FinalPrice { get; set; }
        public int ProductComments { get; set; }
        public int ProductStock { get; set; }
        public int TotalImage { get; set; }
        public double AverageRating { get; set; }
        public bool Ispublish { get; set; }

        //For HomePage
        public string ImageTitle { get; set; }
        public string ImagePath { get; set; }
        public List<ProductImageListDto> ImageList { get; set; }
        public string SecondImagePath { get; set; }
        public List<CommentsListDto> ProductCommentsList { get; set; }
    }

    public class ProductImageListByProduct
    {
        public string Path { get; set; }
        public List<ProductImageListDto> ProuctImages { get; set; }
    }
}
