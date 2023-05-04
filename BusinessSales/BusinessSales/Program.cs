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
            Console.WriteLine("New database successfully created");

            accountDatabase.setDbName(dbJson.name);
            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Database already exists");

            await response.WriteAsJsonAsync<string>("account already exists");
        }
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
            Console.WriteLine("Database successfully found");

            accountDatabase.setDbName(dbJson.name);
            await response.WriteAsJsonAsync("success");
        }
        else
        {
            Console.WriteLine("Database doesn't exists");

            await response.WriteAsJsonAsync("account doesn't exists");
        }
    });
});

app.Map("/changePassword", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        PasswordJson passwordJson = await request.ReadFromJsonAsync<PasswordJson>();

        if (webAppDatabase.changePassword(
            accountDatabase.Name, passwordJson.password))
        {
            Console.WriteLine("Account password successfully changed");

            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Account doen't exists");

            await response.WriteAsJsonAsync<string>("Account doen't exists");
        }
    });
});

app.Map("/changeName", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        AccountNameJson nameJson = 
            await request.ReadFromJsonAsync<AccountNameJson>();

        if (webAppDatabase.changeAccountName(
            accountDatabase.Name, nameJson.name))
        {
            accountDatabase.rewriteDatabase(nameJson.name);

            Console.WriteLine("Account name successfully changed");

            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Account already exists");

            await response.WriteAsJsonAsync<string>("Account already exists");
        }
    });
});

app.Map("/deleteAccount", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        if (webAppDatabase.deleteAccount(accountDatabase.Name))
        {
            accountDatabase.deleteDatabase();

            Console.WriteLine("Database successfully deleted");

            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Database doesn't exists");

            await response.WriteAsJsonAsync<string>("Database dosn't exists");
        }
    });
});

app.Run();
