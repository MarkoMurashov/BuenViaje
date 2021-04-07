using PetStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels
{
    public class TransportOwnerShowViewModel
    {
        public IEnumerable<Models.TransportOwner> Owners { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public string SearchName { get; set; }
    }
}
