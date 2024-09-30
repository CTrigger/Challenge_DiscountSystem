using DiscountCore;
namespace SignalRDiscountCodeApp.Interfaces
{
    public interface IDiscountHub
    {
        Task<IEnumerable<DiscountData>> GenerateDiscountCodes(ushort batch);
    }
}
