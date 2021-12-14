using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class CommentDto
    {
        public string UserId { get; set; }
        [Required]
        public string Comment { get; set; }
        public Guid ProductId { get; set; }

    }

    public class CommentsListDto
    {
        public string UserId { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public string AddedDate { get; set; }
    }
}
