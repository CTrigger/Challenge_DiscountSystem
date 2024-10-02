using DiscountCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SignalRDiscountCodeApp.Interfaces
{
    public interface IDiscountClient
    {
        Task ReceiveMessage(string message);
        Task BroadcastCodes(IEnumerable<DiscountData> codes);
    }
}
