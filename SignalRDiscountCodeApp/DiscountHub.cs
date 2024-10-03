using DiscountCore;
using DiscountServices;
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
            await Clients.All.AllCodes(await _discountManager.ListAllCodes());
        }

        public async Task GenerateCodes(ushort batch, byte length)
        {
            IEnumerable<DiscountData> codes;
            if (batch == 0)
                codes = Enumerable.Empty<DiscountData>();
            else
                codes = await _discountManager.GenerateDiscountCode(batch, length);
            await Clients.All.BroadcastCodes(codes);
        }

        public async Task<byte> CodeUsage(string code)
        {
            byte status = await _discountManager.UseCode(code);
            switch ((CodeUseEnum)status)
            {
                case CodeUseEnum.Success:
                    await Clients.All.CodeUsed(code);
                    break;
            }
            return status;
        }

    }
}

