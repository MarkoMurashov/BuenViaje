using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models
{
    public class TransportOwner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter surname")]
        public string Surname { get; set; }

        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Please enter phone")]
        public string Phone { get; set; }

        public string ImageId { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
