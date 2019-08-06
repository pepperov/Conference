using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Models
{
    public class User
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Role")]
        public Role Role { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Incorrect e-mail")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Обязательное поле")]
        //[Display(Name = "Пароль")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        public List<Reservation> Reservations { get; set; }

        public User()
        {
            Reservations = new List<Reservation>();
        }
    }
}
