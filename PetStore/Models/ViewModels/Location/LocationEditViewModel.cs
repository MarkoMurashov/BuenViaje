using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.ViewModels.Location
{
    public class LocationEditViewModel
    {
        public Models.Location Location { get; set; }

        public IEnumerable<Models.Voucher> Vouchers { get; set; }

        [Required(ErrorMessage = "Please choose voucher")]
        public string VoucherInfo { get; set; }
    }
}
