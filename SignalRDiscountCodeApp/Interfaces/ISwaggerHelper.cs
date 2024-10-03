using DiscountCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRDiscountCodeApp.Interfaces
{
    public interface ISwaggerHelper
    {
        Task<IEnumerable<DiscountData>> SwaggerDiscount(ushort batch, byte length);
        Task<byte> SwaggerUseCode(string code);
    }
}
