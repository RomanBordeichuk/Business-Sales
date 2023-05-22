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

        if(dbJson.name != "" && dbJson.password != "")
        {
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
        }
        else
        {
            Console.WriteLine("Incorrect input account name or password");

            await response.WriteAsJsonAsync<string>("Incorrect name of password");
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

            await response.WriteAsJsonAsync("Incorrect account name or password");
        }
    });
});

// ********************* MAIN PAGE REQUESTS ***********************

app.Map("/loadMainPage", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        DateTime today = DateTime.Today;
        string currentDate = today.Day + "." + today.Month + "." + today.Year;

        LoadMainPageJson loadSettingsJson = new LoadMainPageJson(
            accountDatabase.Name, 
            accountDatabase.getTotalIncome(),
            accountDatabase.getTotalCount(),
            accountDatabase.getAvPercent(),
            accountDatabase.getCountStore(),
            accountDatabase.getCostStore(),
            currentDate);

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

        DateOnly date;
        string nameOfProducts = purchaseJson.nameOfProducts;
        double priceOfProduct;
        int countOfProducts;
        string comment = purchaseJson.comment;

        string responseMessage = "";

        bool purchaseJsonCorrect = true;

        if(nameOfProducts == "")
        {
            Console.WriteLine("Incorrect input name of products");
            responseMessage += "Incorrect input name of products\n";

            purchaseJsonCorrect = false;
        }
        if (!DateOnly.TryParse(purchaseJson.date, out date))
        {
            Console.WriteLine("Incorrect input date");
            responseMessage += "Incorrect input date\n";

            purchaseJsonCorrect = false;
        }
        if (!double.TryParse(purchaseJson.priceOfProduct, out priceOfProduct))
        {
            Console.WriteLine("Incorrect input price of each product");
            responseMessage += "Incorrect input price of each product\n";

            purchaseJsonCorrect = false;
        }
        if (!int.TryParse(purchaseJson.countOfProducts, out countOfProducts))
        {
            Console.WriteLine("Incorrect input count of products");
            responseMessage += "Incorrect input count of products\n";

            purchaseJsonCorrect = false;
        }

        if (purchaseJsonCorrect)
        {
            Purchase purchase = new Purchase(date, nameOfProducts, 
                priceOfProduct, countOfProducts, comment);

            ProductsBatch productsBatch = new ProductsBatch(purchase.NameOfProducts, 
                purchase.CountOfProducts, purchase.PriceOfPurchase);

            accountDatabase.pushPurchase(purchase);
            accountDatabase.appendProductsToBatch(productsBatch);

            Console.WriteLine("Purchase successfully saves");
            responseMessage = "success";
        }

        await response.WriteAsJsonAsync<string>(responseMessage);
    });
});

app.Map("/pushSale", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var request = context.Request;
        var response = context.Response;

        SaleJson saleJson = await request.ReadFromJsonAsync<SaleJson>();

        DateOnly date;
        string nameOfProducts = saleJson.nameOfProducts;
        double priceOfProduct;
        int countOfProducts;
        string comment = saleJson.comment;

        string responseMessage = "";

        bool saleJsonCorrect = true;

        if(nameOfProducts == "")
        {
            Console.WriteLine("Incorrect input name of products");
            responseMessage += "Incorrect input name of products\n";

            saleJsonCorrect = false;
        }
        if (!DateOnly.TryParse(saleJson.date, out date))
        {
            Console.WriteLine("Incorrect input date");
            responseMessage += "Incorrect input date\n";

            saleJsonCorrect = false;
        }
        if (!double.TryParse(saleJson.priceOfProduct, out priceOfProduct))
        {
            Console.WriteLine("Incorrect input price of each product");
            responseMessage += "Incorrect input price of each product\n";

            saleJsonCorrect = false;
        }
        if (!int.TryParse(saleJson.countOfProducts, out countOfProducts))
        {
            Console.WriteLine("Incorrect input count of products");
            responseMessage += "Incorrect input count of products\n";

            saleJsonCorrect = false;
        }

        if (saleJsonCorrect)
        {
            if (accountDatabase.hasProducts(nameOfProducts))
            {
                if (accountDatabase.hasAnoughCount(
                    nameOfProducts, countOfProducts))
                {
                    double costOfSale = accountDatabase.getCostOfSale(
                        nameOfProducts, countOfProducts);

                    Sale sale = new Sale(date, nameOfProducts,
                        priceOfProduct, countOfProducts, comment, costOfSale);

                    accountDatabase.pushSale(sale);
                    accountDatabase.popProductsFromBatch(
                        sale.NameOfProducts, sale.CountOfProducts, sale.PriceOfSale);

                    Console.WriteLine("Sale successfully saves");
                    responseMessage = "success";
                }
                else
                {
                    Console.WriteLine("Not anough count of products in store database");
                    responseMessage = "Not anough count of products in store database";
                }
            }
            else
            {
                Console.WriteLine("Products had't found in store database");
                responseMessage = "Products had't found in store database";
            }
        }

        await response.WriteAsJsonAsync<string>(responseMessage);
    });
});

// **************** SALES / PURCHASES HISTORY & STORE REQUESTS *******************

app.Map("/purchasesHistory", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<PurchaseResponseJson> purchasesHistory = accountDatabase.getPurchases();

        Console.WriteLine("History of purchases successfully responded");

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

        if (accountDatabase.deletePurchasesHistoryField(purchaseId.id))
        {
            Console.WriteLine("Purchases history field successfully deleted");

            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Purchases history field hadn't found");

            await response.WriteAsJsonAsync<string>(
                "Purchases history field hadn't found");
        }
    });
});

app.Map("/salesHistory", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<SaleResponseJson> salesHistory = accountDatabase.getSales();

        Console.WriteLine("History of sales successfully responded");

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

        if (accountDatabase.deleteSalesHistoryField(saleId.id))
        {
            Console.WriteLine("Sales history field successfully deleted");

            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Sales history field hadn't found");

            await response.WriteAsJsonAsync<string>("Sales history field hadn't found");
        }
    });
});

app.Map("/store", defaultMiddleware =>
{
    defaultMiddleware.Run(async (context) =>
    {
        var response = context.Response;

        List<ProductsBatchJson> store = accountDatabase.getStore();

        Console.WriteLine("Store of unsold products successfully responded");

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

        if (accountDatabase.deleteStoreField(productsBatchId.id))
        {
            Console.WriteLine("Store field successfully deleted");

            await response.WriteAsJsonAsync<string>("success");
        }
        else
        {
            Console.WriteLine("Store field hadn't found");

            await response.WriteAsJsonAsync<string>("Store field hadn't found");
        }
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

        if(nameJson.name != "")
        {
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
        }
        else
        {
            Console.WriteLine("Name field cannot be empty");

            await response.WriteAsJsonAsync<string>("Name field cannot be empty");
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

        if(passwordJson.password != "")
        {
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
        }
        else
        {
            Console.WriteLine("Password field cannot be empty");

            await response.WriteAsJsonAsync<string>("Password field cannot be empty");
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
