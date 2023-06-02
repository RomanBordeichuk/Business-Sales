using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BusinessSales
{
    class AccountDatabase
    {
        private (string, int[]) connectionConfig;
        private string name;

        public string Name
        {
            set { }
            get { return name; }
        }

        public AccountDatabase((string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;
        }

        public string pushNewAccount(string dbName, string password)
        {
            if (dbName != "" && password != "")
            {
                Account account = new Account(dbName, password);

                using (ApplicationContext db =
                    new ApplicationContext(connectionConfig))
                {
                    foreach (Account acc in db.Accounts.ToList())
                    {
                        if (acc.Name == account.Name)
                        {
                            Console.WriteLine("Account already exists");
                            return "Account already exists";
                        }
                    }

                    db.Accounts.Add(account);
                    db.SaveChanges();

                    name = account.Name;

                    Console.WriteLine("New account successfully created");
                    return "success";
                }
            }
            else
            {
                Console.WriteLine("Incorrect input account name or password");
                return "Incorrect name of password";
            }
        }

        public string accountExists(string dbName, string password)
        {
            AccountJson account = new AccountJson(dbName, password);

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                foreach (Account acc in db.Accounts.ToList())
                {
                    if (acc.Name == account.name &&
                        acc.Password == account.password)
                    {
                        name = account.name;

                        Console.WriteLine("Account successfully found");
                        return "success";
                    }
                }

                Console.WriteLine("Account doesn't exists");
                return "Incorrect account name or password";
            }
        }

        public LoadMainPageJson loadMainPage()
        {
            DateTime today = DateTime.Today;
            string currentDate = today.Day + "." + today.Month + "." + today.Year;

            LoadMainPageJson mainPageSettings = new LoadMainPageJson(
                Name,
                getTotalIncome(),
                getTotalCount(),
                getAvPercent(),
                getCountStore(),
                getCostStore(),
                currentDate);

            Console.WriteLine("Main page load settings successfully setted");
            return mainPageSettings;
        }

        public string changeAccountName(string newName)
        {
            if(newName != "")
            {
                using(ApplicationContext db = 
                    new ApplicationContext(connectionConfig))
                {
                    Account currentAccount =
                        db.Accounts.FirstOrDefault(a => a.Name == Name);

                    bool nameAlreadyExists =
                        db.Accounts.FirstOrDefault(a => a.Name == newName) != null;

                    if (!nameAlreadyExists)
                    {
                        currentAccount.Name = newName;

                        db.SaveChanges();

                        Console.WriteLine("Account name successfully changed");
                        return "success";
                    }
                    else
                    {
                        Console.WriteLine("Account already exists");
                        return "Account already exists";
                    }
                }

                return "";
            }
            else
            {
                Console.WriteLine("Name field cannot be empty");
                return "Name field cannot be empty";
            }
        }

        public string changeAccountPass(string newPass)
        {
            if (newPass != "")
            {
                using(ApplicationContext db =
                    new ApplicationContext(connectionConfig))
                {
                    Account currentAccount =
                        db.Accounts.FirstOrDefault(a => a.Name == Name);

                    currentAccount.Password = newPass;

                    db.SaveChanges();

                    Console.WriteLine("Account password successfully changed");
                    return "success";
                }
            }
            else
            {
                Console.WriteLine("Password field cannot be empty");
                return "Password field cannot be empty";
            }
        }

        public void deleteAccount()
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                Account currentAccount = 
                    db.Accounts.FirstOrDefault(a => a.Name == Name);

                db.Accounts.Remove(currentAccount);
                db.SaveChanges();
            }
        }

        public string pushPurchase(PurchaseJson purchaseJson)
        {

            string responseMessage = "";

            DateOnly date;
            string nameOfProducts = purchaseJson.nameOfProducts;
            double priceOfProduct;
            int countOfProducts;
            string comment = purchaseJson.comment;

            bool purchaseJsonCorrect = true;

            if (nameOfProducts == "")
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
                using (ApplicationContext db =
                    new ApplicationContext(connectionConfig))
                {
                    int currentAccountId = 
                        db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                    ProductsBatch productsBatch = new ProductsBatch(nameOfProducts,
                        countOfProducts, priceOfProduct * countOfProducts, 
                        currentAccountId);

                    appendProductsToBatch(productsBatch);

                    Purchase purchase = new Purchase(date, nameOfProducts,
                        priceOfProduct, countOfProducts, comment, currentAccountId);

                    db.Purchases.Add(purchase);
                    db.SaveChanges();
                }

                Console.WriteLine("Purchase successfully saves");
                responseMessage = "success";
            }

            return responseMessage;
        }

        public string pushSale(SaleJson saleJson)
        {
            string responseMessage = "";

            DateOnly date;
            string nameOfProducts = saleJson.nameOfProducts;
            double priceOfProduct;
            int countOfProducts;
            string comment = saleJson.comment;

            bool saleJsonCorrect = true;

            if (nameOfProducts == "")
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
                if (hasProducts(nameOfProducts))
                {
                    if (hasAnoughCount(nameOfProducts, countOfProducts))
                    {
                        double costOfSale = getCostOfSale(
                            nameOfProducts, countOfProducts);

                        using(ApplicationContext db =
                            new ApplicationContext(connectionConfig))
                        {
                            int currentAccountId =
                                db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                            Sale sale = new Sale(date, nameOfProducts,
                                priceOfProduct, countOfProducts, comment,
                                costOfSale, currentAccountId);

                            popProductsFromBatch(sale.NameOfProducts,
                                sale.CountOfProducts, sale.PriceOfSale);

                            db.Sales.Add(sale);
                            db.SaveChanges();

                            Console.WriteLine("Sale successfully saves");
                            responseMessage = "success";
                        }
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

            return responseMessage;
        }

        public List<PurchaseResponseJson> getPurchases()
        {
            List<PurchaseResponseJson> purchasesHistory =
                new List<PurchaseResponseJson>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var purchases = db.Purchases.Where(p => p.AccountId == currentAccountId);

                foreach (Purchase purchase in purchases.ToList())
                {
                    PurchaseResponseJson purchaseResponseJson =
                        new PurchaseResponseJson(
                            Convert.ToString(purchase.Id),
                            Convert.ToString(purchase.Date),
                            purchase.NameOfProducts,
                            Convert.ToString(purchase.PriceOfProduct),
                            Convert.ToString(purchase.CountOfProducts),
                            purchase.Comment,
                            Convert.ToString(purchase.PriceOfPurchase));

                    purchasesHistory.Add(purchaseResponseJson);
                }
            }

            Console.WriteLine("History of purchases successfully responded");
            return purchasesHistory;
        }

        public string deletePurchasesHistoryField(int id)
        {
            using (ApplicationContext db = new
                ApplicationContext(connectionConfig))
            {
                Purchase purchase =
                    db.Purchases.FirstOrDefault(p => p.Id == id);

                db.Purchases.Remove(purchase);
                db.SaveChanges();
            }

            Console.WriteLine("Purchases history field successfully deleted");
            return "success";
        }

        public List<SaleResponseJson> getSales()
        {
            List<SaleResponseJson> salesHistory = new List<SaleResponseJson>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var sales =
                    db.Sales.Where(s => s.AccountId == currentAccountId);

                foreach (Sale sale in sales.ToList())
                {
                    SaleResponseJson saleResponseJson = new SaleResponseJson(
                        Convert.ToString(sale.Id),
                        Convert.ToString(sale.Date), sale.NameOfProducts,
                        Convert.ToString(sale.PriceOfProduct),
                        Convert.ToString(sale.CountOfProducts), sale.Comment,
                        Convert.ToString(sale.PriceOfSale),
                        Convert.ToString(sale.CostOfSale));

                    salesHistory.Add(saleResponseJson);
                }
            }

            Console.WriteLine("History of sales successfully responded");
            return salesHistory;
        }

        public string deleteSalesHistoryField(int id)
        {
            using (ApplicationContext db = new
                ApplicationContext(connectionConfig))
            {
                Sale sale =
                    db.Sales.FirstOrDefault(s => s.Id == id);

                db.Sales.Remove(sale);
                db.SaveChanges();
            }

            Console.WriteLine("Sales history field successfully deleted");
            return "success";
        }

        public List<ProductsBatchJson> getStore()
        {
            List<ProductsBatchJson> store = new List<ProductsBatchJson>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var storeList = db.Store.Where(s => s.AccountId == currentAccountId);

                foreach (ProductsBatch productsBatch in storeList.ToList())
                {
                    ProductsBatchJson productsBatchJson = new ProductsBatchJson(
                        Convert.ToString(productsBatch.Id),
                        productsBatch.NameOfProducts,
                        Convert.ToString(productsBatch.CountOfProducts),
                        Convert.ToString(productsBatch.PurchasePrice));

                    store.Add(productsBatchJson);
                }
            }

            Console.WriteLine("Store of unsold products successfully responded");
            return store;
        }

        public string deleteStoreField(int id)
        {
            using (ApplicationContext db = new
                ApplicationContext(connectionConfig))
            {
                ProductsBatch productsBatch =
                    db.Store.FirstOrDefault(s => s.Id == id);

                db.Store.Remove(productsBatch);
                db.SaveChanges();
            }

            Console.WriteLine("Store field successfully deleted");
            return "success";
        }

        public List<string> getPurchasesInfo()
        {
            List<string> info = new List<string>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var purchases = db.Purchases.Where(p => p.AccountId == currentAccountId);

                foreach (Purchase purchase in purchases.ToList())
                {
                    info.Add(Convert.ToString(purchase.Date) + ", " +
                        Convert.ToString(purchase.NameOfProducts));
                }
            }

            return info;
        }

        public List<double> getPurchasesPrices()
        {
            List<double> prices = new List<double>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var purchases = db.Purchases.Where(p => p.AccountId == currentAccountId);

                foreach (Purchase purchase in purchases.ToList())
                {
                    prices.Add(purchase.PriceOfPurchase);
                }
            }

            return prices;
        }

        public List<string> getPurchasesColors()
        {
            List<string> colors = new List<string>();

            string[] colorsList = new string[] {
                "rgba(151, 185, 129, 1)",
                "rgba(90, 149, 117, 1)",
                "rgba(42, 112, 104, 1)",
                "rgba(6, 74, 85, 1)",
                "rgba(0, 39, 57, 1)",
            };

            int numColors;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                numColors = db.Purchases
                    .Where(p => p.AccountId == currentAccountId)
                    .ToList().Count();
            }

            Random rand = new Random();

            for (int i = 0; i < numColors; i++)
                colors.Add(colorsList[rand.Next(0, 5)]);

            return colors;
        }

        public List<string> getSalesInfo()
        {
            List<string> info = new List<string>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var sales = db.Sales.Where(p => p.AccountId == currentAccountId);

                foreach (Sale sale in sales.ToList())
                {
                    info.Add(Convert.ToString(sale.Date) + ", " +
                        Convert.ToString(sale.NameOfProducts));
                }
            }

            return info;
        }

        public List<double> getSalesPrices()
        {
            List<double> prices = new List<double>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var sales = db.Sales.Where(p => p.AccountId == currentAccountId);

                foreach (Sale sale in sales.ToList())
                {
                    prices.Add(sale.PriceOfSale);
                }
            }

            return prices;
        }

        public List<string> getSalesColors()
        {
            List<string> colors = new List<string>();

            string[] colorsList = new string[] {
                "rgba(151, 185, 129, 1)",
                "rgba(90, 149, 117, 1)",
                "rgba(42, 112, 104, 1)",
                "rgba(6, 74, 85, 1)",
                "rgba(0, 39, 57, 1)",
            };

            int numColors;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                numColors = db.Sales
                    .Where(s => s.AccountId == currentAccountId)
                    .ToList().Count();
            }

            Random rand = new Random();

            for (int i = 0; i < numColors; i++)
                colors.Add(colorsList[rand.Next(0, 5)]);

            return colors;
        }

        public List<string> getNetIncomeInfo()
        {
            List<string> netIncomeInfo = new List<string>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var sales = db.Sales.Where(p => p.AccountId == currentAccountId);

                foreach (Sale sale in sales.ToList())
                {
                    netIncomeInfo.Add(sale.NameOfProducts);
                }
            }

            return netIncomeInfo;
        }

        public List<double> getNetIncomePrices()
        {
            List<double> netIncomePrices = new List<double>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                var sales = db.Sales.Where(p => p.AccountId == currentAccountId);

                foreach (Sale sale in sales.ToList())
                {
                    netIncomePrices.Add(sale.PriceOfSale - sale.CostOfSale);
                }
            }

            return netIncomePrices;
        }

        public List<string> getNetIncomeColors()
        {
            List<string> colors = new List<string>();

            string[] colorsList = new string[] {
                "rgba(151, 185, 129, 1)",
                "rgba(90, 149, 117, 1)",
                "rgba(42, 112, 104, 1)",
                "rgba(6, 74, 85, 1)",
                "rgba(0, 39, 57, 1)",
            };

            int numColors;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId =
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                numColors = db.Sales
                    .Where(p => p.AccountId == currentAccountId)
                    .ToList().Count();
            }

            Random rand = new Random();

            for (int i = 0; i < numColors; i++)
                colors.Add(colorsList[rand.Next(0, 5)]);

            return colors;
        }

        // ********************* PRIVATE METHODS ***********************

        private void appendProductsToBatch(ProductsBatch productsBatch)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                ProductsBatch? currentBatch = db.Store.FirstOrDefault(
                    p => p.NameOfProducts == productsBatch.NameOfProducts);

                if(currentBatch != null)
                {
                    currentBatch.CountOfProducts += productsBatch.CountOfProducts;
                    currentBatch.PurchasePrice += productsBatch.PurchasePrice;

                    db.Store.Update(currentBatch);
                }
                else db.Store.Add(productsBatch);

                db.SaveChanges();
            }
        }

        private bool hasProducts(string nameOfProducts)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId = 
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                return db.Store.FirstOrDefault(s =>
                    s.AccountId == currentAccountId && 
                    s.NameOfProducts == nameOfProducts) != null;
            }
        }

        public bool hasAnoughCount(string nameOfProducts, int countOfProducts)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                int currentAccountId = 
                    db.Accounts.FirstOrDefault(a => a.Name == Name).Id;

                return db.Store.FirstOrDefault(s =>
                    s.AccountId == currentAccountId &&
                    s.NameOfProducts == nameOfProducts &&
                    s.CountOfProducts >= countOfProducts) != null;
            }
        }

        private double getCostOfSale(string nameOfProducts, int countOfProducts)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                ProductsBatch? productsBatch 
                    = db.Store.FirstOrDefault(p => p.NameOfProducts == nameOfProducts);

                if (productsBatch != null)
                {
                    return Math.Round((double)countOfProducts /
                        (double)productsBatch.CountOfProducts *
                        (double)productsBatch.PurchasePrice, 2);
                }
                else return 0;
            }
        }

        public void popProductsFromBatch(string nameOfProducts,
            int countOfProducts, double priceOfSale)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                ProductsBatch productsBatch = db.Store.FirstOrDefault(p => 
                    p.NameOfProducts == nameOfProducts);

                if (productsBatch.CountOfProducts > countOfProducts)
                {
                    productsBatch.CountOfProducts -= countOfProducts;

                    if (productsBatch.PurchasePrice < priceOfSale)
                        productsBatch.PurchasePrice = 0;
                    else
                        productsBatch.PurchasePrice -= priceOfSale;

                    db.Store.Update(productsBatch);
                }
                else db.Store.Remove(productsBatch);

                db.SaveChanges();
            }
        }

        private double getTotalIncome()
        {
            double totalIncome = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                var sales = db.Sales.Where(s => s.Account.Name == name);

                foreach (Sale sale in sales)
                    totalIncome += sale.PriceOfSale;
            }

            return totalIncome;
        }

        private int getTotalCount()
        {
            int totalCount = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                var sales = db.Sales.Where(s => s.Account.Name == name);

                foreach (Sale sale in sales)
                    totalCount += sale.CountOfProducts;
            }

            return totalCount;
        }

        private int getAvPercent()
        {
            double sumPercent = 0;
            int countOfSales = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                var sales = db.Sales.Where(s => s.Account.Name == name);

                foreach (Sale sale in sales)
                {
                    if (sale.CostOfSale != 0)
                    {
                        sumPercent += (sale.PriceOfSale - sale.CostOfSale) /
                            sale.CostOfSale;
                    }

                    countOfSales++;
                }
            }

            if (countOfSales == 0) return 0;
            return (int)Math.Round(sumPercent / countOfSales * 100);
        }

        private int getCountStore()
        {
            int countStore = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                var store = db.Store.Where(s => s.Account.Name == name);

                foreach (ProductsBatch productsBatch in store)
                {
                    countStore += productsBatch.CountOfProducts;
                }
            }

            return countStore;
        }

        private double getCostStore()
        {
            double costStore = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig))
            {
                var store = db.Store.Where(s => s.Account.Name == name);

                foreach (ProductsBatch productsBatch in store)
                {
                    costStore += productsBatch.PurchasePrice;
                }
            }

            return costStore;
        }
    }
}
