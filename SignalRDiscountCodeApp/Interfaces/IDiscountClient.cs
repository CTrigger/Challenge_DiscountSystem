using DiscountCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SignalRDiscountCodeApp.Interfaces
{
    public interface IDiscountClient
    {
        Task ReceiveMessage(string message);
        Task BroadcastCodes(IEnumerable<DiscountData> codes);
        Task CodeResponse(byte result);
        Task CodeUsed(string code);
        Task AllCodes(IEnumerable<DiscountData> codes);
    }
}
