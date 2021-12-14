using Rookie.Ecom.DataAccessor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class BrandDto : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class BrandListDto : BaseEntity
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Total Product")]
        public int TotalProduct { get; set; }
    }
}
