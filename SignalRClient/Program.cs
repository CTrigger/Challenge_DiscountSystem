using DiscountCore;
using Microsoft.AspNetCore.SignalR.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Build the connection to the SignalR Hub
        var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7258/DiscountHub")
            .Build();

        // Event handler for receiving messages
        connection.On<string>("ReceiveMessage", message =>
        {
            Console.WriteLine($"Message from server: {message}");
        });

        // Event handler for receiving broadcasted discount codes
        connection.On<IEnumerable<DiscountData>>("BroadcastCodes", discounts =>
        {
            Console.WriteLine($"Codes were updated:");
            foreach (var discount in discounts)
            {
                Console.WriteLine($"{discount.Code}");
            }
        });

        // Start the connection to the SignalR server
        Console.WriteLine("Press any key to start");
        Console.ReadKey();
        await connection.StartAsync();
        Console.WriteLine("Connection started.");

        // Infinite loop to keep requesting new codes
        do
        {
            Console.Write("Number of requests: ");
            if (UInt16.TryParse(Console.ReadLine(), out UInt16 input))
            {
                // Invoke the server method to generate codes without expecting a return value
                Console.WriteLine("Requesting codes...");
                await connection.InvokeAsync("GenerateCodes", input);
            }
            else
            {
                Console.WriteLine("Not a valid number");
            }
        } while (true);

        // Stop the connection when done (this won't be reached in the current structure)
        await connection.StopAsync();
        Console.WriteLine("Connection ended.");
    }
}
