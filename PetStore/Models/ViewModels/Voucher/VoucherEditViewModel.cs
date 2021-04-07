using Microsoft.AspNetCore.Http;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels.Voucher
{
    public class VoucherEditViewModel
    {
        [Required]
        public Models.Voucher Voucher { get; set; }

        public IEnumerable<Transport> Transports { get; set; }

        public Product Product { get; set; }

        public IEnumerable<Models.Location> LocationsTO { get; set; }

        public IEnumerable<Models.Location> LocationsFROM { get; set; }

        public IEnumerable<Models.Categories> Categories { get; set; }

        [Required(ErrorMessage = "Выберите транспорт")]
        public string TransportInfo { get; set; }

        public string SaleInfo { get; set; }

        [Required(ErrorMessage = "Выберите локацию")]
        public string[] LocationTOInfo { get; set; }

        [Required(ErrorMessage = "Выберите локацию")]
        public string LocationFROMInfo { get; set; }

        [Required(ErrorMessage = "Выберите категорию")]
        public string CategoriesInfo { get; set; }

        public string Status { get; set; }

        public IFormFile Photo { get; set; }
    }
}
