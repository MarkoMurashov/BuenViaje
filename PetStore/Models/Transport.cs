using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuenViaje.Models
{
    public class Transport
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter owner")]
        [ForeignKey("TransportOwner")]
        public int TransportOwnerId { get; set; }

        public TransportOwner TransportOwner { get; set; }

        public string ImageId { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
