using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels
{
    public class TransportEditViewModel
    {
        public Transport Transports { get; set; }

        public IEnumerable<Models.TransportOwner> TransportOwners { get; set; }

        [Required(ErrorMessage = "Please enter owner")]
        public string FIOOwner { get; set; }

        public IFormFile Photo { get; set; }
    }
}
