using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Incorrect e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Login (e-mail)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
