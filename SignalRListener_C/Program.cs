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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Message from server: {message}");
                Console.ForegroundColor = ConsoleColor.White;
            });

            connection.On<IEnumerable<DiscountData>>("BroadcastCodes", discounts =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Codes were updated:");
                foreach (var discount in discounts)
                {
                    Console.WriteLine($"{discount.Code}");
                }
                Console.ForegroundColor = ConsoleColor.White;
            });

            connection.On<string>("CodeUsed", code =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Code used: {code}");
                Console.ForegroundColor = ConsoleColor.White;

            });

            connection.On<IEnumerable<DiscountData>>("AllCodes", discounts =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Codes available");
                foreach (var discount in discounts)
                {
                    Console.WriteLine($"{discount.Code}");
                }
                Console.ForegroundColor = ConsoleColor.White;


            });
            //Thread.Sleep(1000);

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
