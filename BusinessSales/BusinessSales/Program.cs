using Microsoft.EntityFrameworkCore;
using BusinessSales;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Database>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Map("/signIn", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;


    });
});

app.Run();
