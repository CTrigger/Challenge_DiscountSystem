using DiscountCore;
using Microsoft.AspNetCore.SignalR.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Build the connection to the SignalR Hub
        HubConnection connection = new HubConnectionBuilder()
                                        .WithUrl("https://localhost:7258/DiscountHub")
                                        .Build();

        // Event handler for receiving messages
        connection.On<string>("ReceiveMessage", message =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Message from server: {message}");
            Console.ForegroundColor = ConsoleColor.White;
        });

        // Event handler for receiving broadcasted discount codes
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
        // Start the connection to the SignalR server
        //Console.WriteLine("Press any key to start");
        //Console.ReadKey();
        Thread.Sleep(300);
        await connection.StartAsync();
        Console.WriteLine("Connection started.");

        // Infinite loop to keep requesting new codes
        do
        {
            Console.WriteLine("============================================");
            Console.WriteLine("Pick an Option: ");
            Console.WriteLine($"{(int)Menu.Request}:{Menu.Request}");
            Console.WriteLine($"{(int)Menu.Code}:{Menu.Code}");
            if (int.TryParse(Console.ReadLine(), out int option))
                switch ((Menu)option)
                {
                    case Menu.Request:
                        await Request_Code(connection);
                        break;
                    case Menu.Code:
                        await Use_Code(connection);
                        break;
                    default:
                        break;
                }
            Console.WriteLine("============================================");
        } while (true);

        // Stop the connection when done (this won't be reached in the current structure)
        await connection.StopAsync();
        Console.WriteLine("Connection ended.");
    }
    public static async Task Request_Code(HubConnection connection)
    {
        Console.Write("Number of requests: ");
        if (UInt16.TryParse(Console.ReadLine(), out UInt16 input))
        {

            Console.Write("Length of Code: ");
            if (UInt16.TryParse(Console.ReadLine(), out UInt16 length))
            {
                // Invoke the server method to generate codes without expecting a return value
                Console.WriteLine("Requesting codes...");
                await connection.InvokeAsync("GenerateCodes", input, length);

            }
        }
        else
        {
            Console.WriteLine("Not a valid number");
        }

    }
    public static async Task Use_Code(HubConnection connection)
    {
        Console.Write("Code: ");
        string code = Console.ReadLine() ?? string.Empty;
        if (code.Length == 7 || code.Length == 8)
            Console.WriteLine(await connection.InvokeAsync<byte>("CodeUsage", code));
        else
            Console.WriteLine("Invalid Code");


    }
    enum Menu
    {
        Request,
        Code,

    }
}
