using DiscountServices;
using DiscountServices.Interfaces;
using FileManager;
using FileManager.Interfaces;
using DataFileIO;
using DataFileIO.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFileIO, FileIO>();
builder.Services.AddTransient<IDataHandler, DataHandler>();
builder.Services.AddTransient<IDiscountManager, DiscountManager>();
builder.Services.AddSignalR();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json","Discount wss");
    //});
}


app.MapHub<SignalRDiscountCodeApp.DiscountHub>("/DiscountHub");
//app.MapGet("/", () => "Hello World!");

app.Run();
