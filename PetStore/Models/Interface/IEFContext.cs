using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.Interfaces
{
    public interface IEFContext<T>
    {
        IQueryable<T> Entity { get; }

        void SaveEntity(T product);

        T DeleteEntity(int ID);
    }
}
