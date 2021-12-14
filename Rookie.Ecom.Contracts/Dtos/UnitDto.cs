using Rookie.Ecom.DataAccessor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class UnitDto : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UnitListDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalProducts { get; set; }
    }
}
