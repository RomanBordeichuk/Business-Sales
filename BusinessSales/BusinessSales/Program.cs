using Microsoft.EntityFrameworkCore;
using BusinessSales;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("connectionConfig.json");

var app = builder.Build();

DbConnectionConfig connectionConfig =
    app.Configuration.GetSection("connectionConfig").Get<DbConnectionConfig>();

WebAppDatabase webAppDatabase = new WebAppDatabase(
    (connectionConfig.ConnectionString, connectionConfig.MySqlServerVersion));

AccountDatabase accountDatabase = new AccountDatabase(
    (connectionConfig.ConnectionString, connectionConfig.MySqlServerVersion));

app.UseDefaultFiles();
app.UseStaticFiles();

app.Map("/signUp", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        DatabaseJson dbJson = await request.ReadFromJsonAsync<DatabaseJson>();

        if (webAppDatabase.pushNewAccount(dbJson.name, dbJson.password))
        {
            accountDatabase.setDbName(dbJson.name);
            await response.WriteAsJsonAsync<string>("success");
        }
        else await response.WriteAsJsonAsync<string>("account already exists");
    });
});

app.Map("/signIn", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        DatabaseJson dbJson = await request.ReadFromJsonAsync<DatabaseJson>();

        if (webAppDatabase.accountExists(dbJson.name, dbJson.password))
        {
            accountDatabase.setDbName(dbJson.name);

            await response.WriteAsJsonAsync("success");
        }
        else await response.WriteAsJsonAsync("account doesn't exists");
    });
});

app.Run();
