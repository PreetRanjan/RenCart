using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    
    public class UserCreateDto
    {
        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Enter a valid email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MinLength(3),MaxLength(30)]
        public string FullName { get; set; }
    }
}
