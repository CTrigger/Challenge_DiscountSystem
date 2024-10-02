using DiscountCore;
using DiscountServices.Interfaces;
using Microsoft.AspNetCore.SignalR;
using SignalRDiscountCodeApp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SignalRDiscountCodeApp
{
    public sealed class DiscountHub : Hub<IDiscountClient>
    {
        private readonly IDiscountManager _discountManager;

        public DiscountHub(IDiscountManager discountManager)
        {
            _discountManager = discountManager;
        }

        public override async Task OnConnectedAsync()
        {

            await Clients.All.ReceiveMessage($"{Context.ConnectionId} has joined");
        }

        public async Task GenerateCodes(ushort batch)
        {
            IEnumerable<DiscountData> codes;
            if (batch == 0)
                codes = Enumerable.Empty<DiscountData>();
            else
                codes = await _discountManager.GenerateDiscountCode(batch);
            await Clients.All.BroadcastCodes(codes);

        }

    }
}

