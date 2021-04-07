using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models
{
    public class LocationVoucher
    {
        public int ID { get; set; }

        public int LocationId { get; set; }

        [Required(ErrorMessage = "Please choose voucher")]
        public int VoucherId { get; set; }

        public Location Location { get; set; }

        public Voucher Voucher { get; set; }
    }
}
