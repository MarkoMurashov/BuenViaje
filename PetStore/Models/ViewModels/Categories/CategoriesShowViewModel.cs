using PetStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels.Categories
{
    public class CategoriesShowViewModel
    {
        public IEnumerable<Models.Categories> Categories { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string SearchName { get; set; }
    }
}
