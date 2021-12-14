using Rookie.Ecom.DataAccessor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class ProductCommentsDto : BaseEntity
    {
        [Display(Name = "Product")]
        public Guid ProductId { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
        public string Comment { get; set; }
    }

    public class ProductCommentsListDto : BaseEntity
    {
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public string UserReaction { get; set; }

    }

    public class ProductCommentsListByUser
    {

    }
}
