using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels.TransportOwner
{
    public class TransportOwnerEditViewModel
    {
        public Models.TransportOwner Owner { get; set; }

        public IFormFile Photo { get; set; }
    }
}
