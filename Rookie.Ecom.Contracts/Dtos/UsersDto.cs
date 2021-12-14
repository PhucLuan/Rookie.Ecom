using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class UsersDto
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password), MinLength(8, ErrorMessage = "Password must be 8 character")]
        public string Password { get; set; }

        [DataType(DataType.Password), MinLength(8, ErrorMessage = "Password must be 8 character")]
        [Compare("Password", ErrorMessage = "Password is not same")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }

        [Required]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Day")]
        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }

    public class UserListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Gender { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }

    }
}
