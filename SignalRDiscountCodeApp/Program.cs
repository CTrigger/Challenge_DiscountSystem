using DataFileIO;
using DataFileIO.Interfaces;
using DiscountServices;
using DiscountServices.Interfaces;
using FileManager;
using FileManager.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRDiscountCodeApp;
using SignalRDiscountCodeApp.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IFileIO, FileIO>();
        builder.Services.AddSingleton<IDataHandler, DataHandler>();
        builder.Services.AddTransient<IDiscountManager, DiscountManager>();
        builder.Services.AddTransient<ISwaggerHelper, SwaggerHelper>(); //this is custom for swagger
        builder.Services.AddSignalR();


        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Add MapPost for SignalR method invocation
        app.MapPost("/generate-codes", async (ushort batch, byte length,
            IHubContext<DiscountHub, IDiscountClient> hubContext,
            ISwaggerHelper swaggerHelper) =>
        {
            if (batch == 0)
                return Results.BadRequest("Batch size cannot be zero.");

            await hubContext.Clients.All.ReceiveMessage($"Batch request from Swagger: gen {batch} codes.");
            var discountList = await swaggerHelper.SwaggerDiscount(batch, length);

            await hubContext.Clients.All.BroadcastCodes(discountList);

            return Results.Ok("Codes have been broadcasted.");
        });

        app.MapPost("/use-code", async (string code,
            IHubContext<DiscountHub, IDiscountClient> hubContext,
            ISwaggerHelper swaggerHelper) =>
        {
            if (code.Length < 7 || code.Length > 8)
                return Results.BadRequest("bad code length");

            await hubContext.Clients.All.ReceiveMessage($"Batch request from Swagger: use {code}.");
            byte result = await swaggerHelper.SwaggerUseCode(code);

            await hubContext.Clients.All.CodeResponse(result);

            return Results.Ok("Codes have been broadcasted.");
        });


        app.MapHub<DiscountHub>("/DiscountHub");

        app.Run();
    }
}