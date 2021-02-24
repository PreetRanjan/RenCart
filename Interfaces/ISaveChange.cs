using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Interfaces
{
    public interface ISaveChange
    {
        Task<bool> SaveChangesAsync();
    }
}
