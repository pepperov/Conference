using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Models
{
    public class Reservation
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Время начала")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Время окончания")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Пользователь")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Конференц-зал")]
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
