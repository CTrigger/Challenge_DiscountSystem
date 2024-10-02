using DiscountCore;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRListener
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7258/DiscountHub")
                .Build();

            connection.On<string>("ReceiveMessage", message =>
            {
                Console.WriteLine($"Message from server: {message}");
            });
            connection.On<IEnumerable<DiscountData>>("BroadcastCodes", discounts =>
            {
                Console.WriteLine($"Codes were updated");
                foreach (var discount in discounts)
                {
                    Console.WriteLine($"{discount.Code}");
                }

            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connected to the server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to server: {ex.Message}");
                return;
            }

            // Keep the console application running
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }
    }
}
