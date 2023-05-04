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

// ********************* SIGN IN / SIGN UP REQUESTS ***********************

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

            Console.WriteLine("New database successfully created");

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

            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Database doesn't exists");

            await response.WriteAsJsonAsync("account doesn't exists");
        }
    });
});

app.Map("/loadMainPage", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        LoadMainPageJson loadSettingsJson = new LoadMainPageJson(
            accountDatabase.Name, 
            accountDatabase.getTotalIncome(),
            accountDatabase.getTotalCount());

        Console.WriteLine("Main page load settings successfully setted");

        await response.WriteAsJsonAsync<LoadMainPageJson>(
            loadSettingsJson);
    });
});

// ********************* EXECUTING TRANSACTIONS REQUESTS ***********************

app.Map("/pushPurchase", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        PurchaseJson purchaseJson = await request.ReadFromJsonAsync<PurchaseJson>();

        Purchase purchase = new Purchase(Convert.ToDateTime(purchaseJson.date),
            purchaseJson.nameOfProducts, purchaseJson.priceOfProduct,
            purchaseJson.countOfProducts, purchaseJson.comment);

        accountDatabase.pushPurchase(purchase);

        Console.WriteLine("Purchase successfully saves");

        response.WriteAsJsonAsync<string>("success");
    });
});

app.Map("/pushSale", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        SaleJson saleJson = await request.ReadFromJsonAsync<SaleJson>();

        Sale sale = new Sale(Convert.ToDateTime(saleJson.date),
            saleJson.nameOfProducts, saleJson.priceOfProduct,
            saleJson.countOfProducts, saleJson.comment);

        accountDatabase.pushSale(sale);

        Console.WriteLine("Sale successfully saves");

        response.WriteAsJsonAsync<string>("success");
    });
});

// ********************* SETTINGS REQUESTS ***********************

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
