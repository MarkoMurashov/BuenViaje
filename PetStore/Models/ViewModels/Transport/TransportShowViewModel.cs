using PetStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels
{
    public class TransportShowViewModel
    {
        public IEnumerable<Transport> Transports { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string SearchName { get; set; }

    }
}
