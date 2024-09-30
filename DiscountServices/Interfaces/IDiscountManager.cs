using DiscountCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountServices.Interfaces
{
    public interface IDiscountManager
    {
        Task<DiscountData> GenerateDiscountCode();
        Task<IEnumerable<DiscountData>> GenerateDiscountCode(ushort batch);
    }
}
