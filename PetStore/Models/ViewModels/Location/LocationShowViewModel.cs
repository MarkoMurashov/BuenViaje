using PetStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels.Location
{
    public class LocationShowViewModel
    {
        public IEnumerable<Models.Location> Locations { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string SearchName { get; set; }
    }
}
