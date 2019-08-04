using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Models
{
    public class Room
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Descrtiption { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Seats")]
        public int Seats { get; set; }

        [Display(Name = "Projector")]
        [UIHint("Boolean")]
        public bool Projector { get; set; }

        [Display(Name = "Board")]
        [UIHint("Boolean")]
        public bool Board { get; set; }

        [Display(Name = "Scheduled time")]
        public List<Reservation> Reservations { get; set; }

        public Room()
        {
            Reservations = new List<Reservation>();
        }
    }
}
