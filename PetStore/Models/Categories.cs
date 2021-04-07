using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models
{
    public class Categories
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
