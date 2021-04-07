using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter country")]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter sity")]
        [Display(Name = "Город")]
        public string Sity { get; set; }

        [Required(ErrorMessage = "Please enter departure")]
        [Display(Name = "Конкретное место")]
        public string Departure { get; set; }

        public ICollection<LocationVoucher> LocationVouchers { get; set; }
    }
}
