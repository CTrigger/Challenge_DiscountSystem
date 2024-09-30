using Microsoft.AspNetCore.SignalR.Client;
using DiscountCore;
using Microsoft.Extensions.Logging;

public class Program
{
    public static async Task Main(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7258/DiscountHub")
            .Build();
        do
        {
            // Start the connection
            await connection.StartAsync();
            Console.WriteLine("Connection started.");

            Console.Write("Number of requests:");
            if (UInt16.TryParse(Console.ReadLine(), out UInt16 input))
            {
                Console.Write("Requesting codes");
                var discountCodes = await connection.InvokeAsync<IEnumerable<DiscountData>>("GenerateDiscountCodes", input);
                string list = "Generated codes: \r\n" + string.Join(Environment.NewLine, (from c in discountCodes select c.Code));
                Console.WriteLine(list);
            }
            else
            {
                Console.WriteLine("Not valid number");
            }

            // Close the connection
            await connection.StopAsync();
            Console.WriteLine("Connection ended.");
        } while (true);
    }
}
