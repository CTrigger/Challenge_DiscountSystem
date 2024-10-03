using DiscountCore;

namespace DiscountServices.Interfaces
{
    public interface IDiscountManager
    {
        Task<DiscountData> GenerateDiscountCode();
        Task<IEnumerable<DiscountData>> GenerateDiscountCode(ushort batch, byte length);
        Task<byte> UseCode(string code);
        Task<IEnumerable<DiscountData>> ListAllCodes();
    }
}
