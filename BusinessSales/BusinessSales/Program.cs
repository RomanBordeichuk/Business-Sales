using Microsoft.EntityFrameworkCore;
using BusinessSales;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("connectionConfig.json");
builder.Services.AddSingleton<Database>();

var app = builder.Build();

DbConnectionConfig connectionConfig = 
    app.Configuration.GetSection("connectionConfig").Get<DbConnectionConfig>();

Database databae = new Database(
    (connectionConfig.ConnectionString, connectionConfig.MySqlServerVersion));

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
