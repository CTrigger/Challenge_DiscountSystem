using Microsoft.AspNetCore.SignalR;
using DiscountServices.Interfaces;
using DiscountCore;
namespace SignalRDiscountCodeApp
{
    public sealed class DiscountHub : Hub
    {
        private readonly IDiscountManager _discountManager;

        public DiscountHub(IDiscountManager discountManager)
        {
            _discountManager = discountManager;
        }

        public override async Task OnConnectedAsync()
        {

            await Clients.All.SendAsync("DiscountCodes", $"{Context.ConnectionId} has joined");
        }


        public async Task<IEnumerable<DiscountData>> GenerateDiscountCodes(ushort batch)
        {
            if (batch == 0)
            {
                return Enumerable.Empty<DiscountData>();
            }

            IEnumerable<DiscountData> codes = await _discountManager.GenerateDiscountCode(batch);
            return codes;  // Return the result to the client
            //return await Task.FromResult(codes);  // Return the result to the client
        }
    }
}
