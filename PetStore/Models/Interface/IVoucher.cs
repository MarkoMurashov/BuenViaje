using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.Interfaces
{
    public interface IVoucher: IEFContext<Voucher>
    {
        void AddVoucherLocation(string[] locID, int voucherID);

        IQueryable<LocationVoucher> VoucherLocation { get; }

        List<LocationVoucher> GetVoucherLocation(int voucherID);
    }
}
