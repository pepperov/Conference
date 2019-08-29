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

        [Required(ErrorMessage = "Required")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Starts")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Ends")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Owner")]
        public int? UserId { get; set; }

        public User User { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Room")]
        public int? RoomId { get; set; }

        public Room Room { get; set; }
    }
}
