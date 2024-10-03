using DiscountCore;
using DiscountServices.Interfaces;
using SignalRDiscountCodeApp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDiscountCodeApp
{
    public class SwaggerHelper : ISwaggerHelper
    {
        private readonly IDiscountManager _discountManager;

        public SwaggerHelper(IDiscountManager discountManager)
        {
            _discountManager = discountManager;
        }
        public async Task<IEnumerable<DiscountData>> SwaggerDiscount(ushort batch, byte length)
        {
            IEnumerable<DiscountData> codes;
            if (batch == 0)
                codes = Enumerable.Empty<DiscountData>();

            codes = await _discountManager.GenerateDiscountCode(batch, length);
            return codes;
        }
        public async Task<byte> SwaggerUseCode(string code)
        {
            byte result = await _discountManager.UseCode(code);
            return result;
        }
    }
}
