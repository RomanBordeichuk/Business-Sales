using Microsoft.EntityFrameworkCore;
using BusinessSales;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("connectionConfig.json");

var app = builder.Build();

DbConnectionConfig connectionConfig =
    app.Configuration.GetSection("connectionConfig").Get<DbConnectionConfig>();

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

        AccountJson accJson = await request.ReadFromJsonAsync<AccountJson>();

        string responseMessage =
            accountDatabase.pushNewAccount(accJson.name, accJson.password);

        await response.WriteAsJsonAsync<string>(responseMessage);
    });
});

app.Map("/signIn", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        AccountJson accJson = await request.ReadFromJsonAsync<AccountJson>();

        string responseMessage =
            accountDatabase.accountExists(accJson.name, accJson.password);

        await response.WriteAsJsonAsync<string>(responseMessage);
    });
});

// ********************* MAIN PAGE REQUESTS ***********************

app.Map("/loadMainPage", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        LoadMainPageJson loadSettingsJson = accountDatabase.loadMainPage();

        await response.WriteAsJsonAsync<LoadMainPageJson>(loadSettingsJson);
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

        string responseString = accountDatabase.pushPurchase(purchaseJson);

        await response.WriteAsJsonAsync<string>(responseString);
    });
});

app.Map("/pushSale", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        SaleJson saleJson = await request.ReadFromJsonAsync<SaleJson>();

        string responseString = accountDatabase.pushSale(saleJson);

        await response.WriteAsJsonAsync<string>(responseString);
    });
});

// **************** SALES / PURCHASES HISTORY & STORE REQUESTS *******************

app.Map("/purchasesHistory", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<PurchaseResponseJson> purchasesHistory = accountDatabase.getPurchases();

        await response.WriteAsJsonAsync<List<PurchaseResponseJson>>(purchasesHistory);
    });
});

app.Map("/deletePurchasesHistoryField", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        PurchaseId purchaseId = await request.ReadFromJsonAsync<PurchaseId>();

        string responseString =
            accountDatabase.deletePurchasesHistoryField(purchaseId.id);

        await response.WriteAsJsonAsync<string>(responseString);
    });
});

app.Map("/salesHistory", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<SaleResponseJson> salesHistory = accountDatabase.getSales();

        await response.WriteAsJsonAsync<List<SaleResponseJson>>(salesHistory);
    });
});

app.Map("/deleteSalesHistoryField", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        SaleId saleId = await request.ReadFromJsonAsync<SaleId>();

        string responseString = accountDatabase.deleteSalesHistoryField(saleId.id);

        await response.WriteAsJsonAsync<string>(responseString);
    });
});

app.Map("/store", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<ProductsBatchJson> store = accountDatabase.getStore();

        await response.WriteAsJsonAsync<List<ProductsBatchJson>>(store);
    });
});

app.Map("/deleteStoreField", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        ProductsBatchId productsBatchId =
            await request.ReadFromJsonAsync<ProductsBatchId>();

        string responseString = accountDatabase.deleteStoreField(productsBatchId.id);

        await response.WriteAsJsonAsync<string>(responseString);
    });
});

// ********************* GRAPHS REQUESTS ***********************

app.Map("/loadCostGraph", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<string> info = accountDatabase.getPurchasesInfo();
        List<double> values = accountDatabase.getPurchasesPrices();
        List<string> colors = accountDatabase.getPurchasesColors();

        CostGraphJson costGraph = new CostGraphJson(info, values, colors);

        await response.WriteAsJsonAsync<CostGraphJson>(costGraph);

        Console.WriteLine("Cost graph configuration successfully setted");
    });
});

app.Map("/loadIncomeGraph", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<string> info = accountDatabase.getSalesInfo();
        List<double> values = accountDatabase.getSalesPrices();
        List<string> colors = accountDatabase.getSalesColors();

        IncomeGraphJson incomeGraph = new IncomeGraphJson(info, values, colors);

        await response.WriteAsJsonAsync<IncomeGraphJson>(incomeGraph);

        Console.WriteLine("Income graph configuration successfully setted");
    });
});

app.Map("/loadNetIncomeGraph", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<string> info = accountDatabase.getNetIncomeInfo();
        List<double> values = accountDatabase.getNetIncomePrices();
        List<string> colors = accountDatabase.getNetIncomeColors();

        NetIncomeGraphJson netIncomeGraph = new NetIncomeGraphJson(info, values, colors);

        await response.WriteAsJsonAsync<NetIncomeGraphJson>(netIncomeGraph);

        Console.WriteLine("Net income graph configuration successfully setted");
    });
});

// ********************* SETTINGS REQUESTS ***********************

app.Map("/changeName", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        AccountNameJson nameJson =
            await request.ReadFromJsonAsync<AccountNameJson>();

        string responseString = accountDatabase.changeAccountName(nameJson.name);

        await response.WriteAsJsonAsync<string>(responseString);
    });
});

app.Map("/changePassword", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        PasswordJson passJson = await request.ReadFromJsonAsync<PasswordJson>();

        string responseString = accountDatabase.changeAccountPass(passJson.password);

        await response.WriteAsJsonAsync<string>(responseString);
    });
});

app.Map("/deleteAccount", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        accountDatabase.deleteAccount();

        await response.WriteAsJsonAsync<string>("success");
    });
});

app.Run();
