using BuenViaje.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models
{
    public class Voucher
    {
        public int ID { get; set; }
           
        [ForeignKey("Transport")]
        public int TranspotId { get; set; }


        public Transport Transport { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }


        public Product Product { get; set; }

        
        public ICollection<LocationVoucher> LocationVouchers { get; set; }

        public VoucherStatus Status { get; set; } = VoucherStatus.Ordinary;

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        [ForeignKey("Location")]
        public int LocationFromId { get; set; }

        public Location LocationFrom { get; set; }

        [NotMapped]
        public IEnumerable<string> CountriesTo { get; set; }

        [NotMapped]
        public IEnumerable<string> SitiesTo { get; set; }

        public Voucher()
        {
            LocationVouchers = new List<LocationVoucher>();
        }

    }

}
