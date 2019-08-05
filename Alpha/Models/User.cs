using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Models
{
    public class User
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Тип учетной записи")]
        public Role Role { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
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
