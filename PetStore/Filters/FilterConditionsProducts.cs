using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using PetStore.Filters.FilterParameters;
using PetStore.Models;
using BuenViaje.Enum;

namespace PetStore.Filters
{
    public class FilterConditionsProducts : IFilterConditionsProducts
    {

        #region Fields

        private IEFContext<Location> _dbLocation { get; set; }

        private IEFContext<LocationVoucher> _dbLocationVoucher { get; set; }

        private IVoucher _dbVoucher { get; set; }


        #endregion

        #region Ctors

        public FilterConditionsProducts(IEFContext<Location> location,
            IEFContext<LocationVoucher> dbLocationVoucher, IVoucher dbVoucher)
        {
            _dbLocation = location;
            _dbLocationVoucher = dbLocationVoucher;
            _dbVoucher = dbVoucher;
        }

        #endregion

        public IQueryable<Product> GetProducts(IQueryable<Product> products, FilterParametersProducts filter)
        {
           
            if (filter.Categories != null)
            {
                foreach (var c in filter.Categories)
                {
                    products = products.Where(p => c.Contains(p.Categories.Name));
                }
            }

            if (!String.IsNullOrEmpty(filter.Name))
            {
                products = products.Where(c => c.Name.ToUpper().Contains(filter.Name.ToUpper()));
            }

            if (filter.MinPrice > 0)
            {
                products = products.Where(c => (decimal)c.PriceSell >= filter.MinPrice);
            }

            if (filter.MaxPrice > 0)
            {
                products = products.Where(c => (decimal)c.PriceSell <= filter.MaxPrice);
            }

            var vouchers = _dbVoucher.Entity;

            if (filter.IsHot)
            {
                vouchers = vouchers.Where(o => o.Status == VoucherStatus.Hot);
            }

            if (filter.Countries != null)
            {
                var strCountries = String.Join("", filter.Countries);

                var countries = _dbLocation.Entity
                    .Where(tmp => tmp.Country.Contains(strCountries)).ToArray();

                if (filter.Countries.Count > 1)
                {
                    countries = _dbLocation.Entity.Where(tmp => IsContains(filter.Countries, tmp.Country)).ToArray();
                }
                

                var vouchersFrom = vouchers;

                foreach (var from in countries)
                {
                    var conreteVouchersFrom = vouchers.Where(tour => tour.LocationFromId == from.Id);
                    vouchersFrom = vouchersFrom.Except(conreteVouchersFrom);
                }

                var vouchersTo = vouchers;
                foreach (var to in countries)
                {
                    var locVoucher = _dbLocationVoucher.Entity.Where(loc => loc.LocationId == to.Id);
                    foreach (var lv in locVoucher)
                    {
                        var conreteVouchersTo = vouchers.Where(tour => tour.ID == lv.VoucherId);
                        vouchersTo = vouchersTo.Except(conreteVouchersTo);
                    }
                }

                vouchers = vouchers.Except(vouchersFrom.Intersect(vouchersTo));
            }


            if (filter.MinDate > DateTime.MinValue)
            {
                vouchers = vouchers.Where(tour => tour.DateStart > filter.MinDate);
            }
            if (filter.MaxDate > DateTime.MinValue)
            {
                vouchers = vouchers.Where(tour => tour.DateEnd < filter.MaxDate);
            }

            IQueryable<Product> prList = products;
            foreach(var a in vouchers)
            {
                var conreteProduct = products.Where(d => d.ID == a.ProductId);
                prList = prList.Except(conreteProduct);
            }

            products = products.Except(prList);

            return products;
        }

        private bool IsContains(List<string> countries, string country)
        {
            foreach(var c in countries)
            {
                if (c.Contains(country))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
