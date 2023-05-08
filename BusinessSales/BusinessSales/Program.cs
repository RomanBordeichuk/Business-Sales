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

        DateOnly date;
        string nameOfProducts = purchaseJson.nameOfProducts;
        double priceOfProduct;
        int countOfProducts;
        string comment = purchaseJson.comment;

        string responseMessage = "";

        bool purchaseJsonCorrect = true;

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
                    Sale sale = new Sale(date, nameOfProducts,
                        priceOfProduct, countOfProducts, comment);

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
