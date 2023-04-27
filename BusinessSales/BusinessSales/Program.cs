using Microsoft.EntityFrameworkCore;
using BusinessSales;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("connectionConfig.json");

var app = builder.Build();

DbConnectionConfig connectionConfig =
    app.Configuration.GetSection("connectionConfig").Get<DbConnectionConfig>();

Database database = new Database(
    (connectionConfig.ConnectionString, connectionConfig.MySqlServerVersion));

app.UseDefaultFiles();
app.UseStaticFiles();

app.Map("/signIn", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;

        DatabaseJson dbJson = await request.ReadFromJsonAsync<DatabaseJson>();
        database.setDbName(dbJson.name);
    });
});

app.Run();
