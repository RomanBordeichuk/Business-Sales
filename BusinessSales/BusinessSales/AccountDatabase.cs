using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BusinessSales
{
    record AccountNameJson(string name);
    record PasswordJson(string password);
    record DatabaseJson(string name, string password);

    record LoadMainPageJson(string name, double totalIncome, int totalCount, 
        int avPercent, int countStore, double costStore, string currentDate);

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

        public void setDbName(string name)
        {
            this.name = name;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, this.name)) ;
        }
        public void rewriteDatabase(string newDbName)
        {
            using (ApplicationContext newDb =
                new ApplicationContext(connectionConfig, newDbName))
            {
                using (ApplicationContext db =
                    new ApplicationContext(connectionConfig, name))
                {
                    newDb.Purchases = db.Purchases;
                    newDb.Sales = db.Sales;
                    newDb.Store = db.Store;

                    name = newDbName;

                    db.deleteDatabase();
                }
            }
        }
        public void deleteDatabase()
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                db.deleteDatabase();
            }
        }

        public void pushPurchase(Purchase purchase)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
            }
        }
        public void pushSale(Sale sale)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                db.Sales.Add(sale);
                db.SaveChanges();
            }
        }

        public bool hasProducts(string nameOfProducts)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                return db.Store.ToList().Any(x => x.NameOfProducts == nameOfProducts);
            }

            return false;
        }

        public bool hasAnoughCount(string nameOfProducts, int countOfProducts)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch productsBatch in db.Store.ToList())
                {
                    if (productsBatch.NameOfProducts == nameOfProducts &&
                        productsBatch.CountOfProducts >= countOfProducts)
                        return true;
                }

                return false;
            }
        }

        public double getCostOfSale(string nameOfProducts, int countOfProducts)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch productsBatch in db.Store.ToList())
                {
                    if (productsBatch.NameOfProducts == nameOfProducts)
                    {
                        return Math.Round((double)countOfProducts /
                            (double)productsBatch.CountOfProducts *
                            (double)productsBatch.PurchasePrice, 2);
                    }
                }

                return 0;
            }
        }

        public void appendProductsToBatch(ProductsBatch productsBatch)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch batch in db.Store.ToList())
                {
                    if (batch.NameOfProducts == productsBatch.NameOfProducts)
                    {
                        batch.CountOfProducts += productsBatch.CountOfProducts;
                        batch.PurchasePrice += productsBatch.PurchasePrice;

                        db.Store.Update(batch);
                        db.SaveChanges();

                        return;
                    }
                }

                db.Store.Add(productsBatch);
                db.SaveChanges();
            }
        }
        public void popProductsFromBatch(string nameOfProducts,
            int countOfProducts, double priceOfSale)
        {
            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch productsBatch in db.Store.ToList())
                {
                    if (productsBatch.NameOfProducts == nameOfProducts)
                    {
                        if (productsBatch.CountOfProducts > countOfProducts)
                        {
                            productsBatch.CountOfProducts -= countOfProducts;

                            if (productsBatch.PurchasePrice <= priceOfSale)
                                productsBatch.PurchasePrice = 0;
                            else
                                productsBatch.PurchasePrice -= priceOfSale;

                            db.Store.Update(productsBatch);
                            db.SaveChanges();

                            return;
                        }
                        else if (productsBatch.CountOfProducts == countOfProducts)
                        {
                            db.Store.Remove(productsBatch);
                            db.SaveChanges();

                            return;
                        }
                    }
                }
            }
        }

        public List<PurchaseResponseJson> getPurchases()
        {
            List<PurchaseResponseJson> purchasesHistory =
                new List<PurchaseResponseJson>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Purchase purchase in db.Purchases.ToList())
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

            return purchasesHistory;
        }

        public bool deletePurchasesHistoryField(int id)
        {
            using (ApplicationContext db = new
                ApplicationContext(connectionConfig, name))
            {
                foreach (Purchase purchase in db.Purchases.ToList())
                {
                    if (purchase.Id == id)
                    {
                        db.Purchases.Remove(purchase);
                        db.SaveChanges();

                        return true;
                    }
                }

                return false;
            }
        }

        public List<string> getPurchasesInfo()
        {
            List<string> info = new List<string>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Purchase purchase in db.Purchases.ToList())
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
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Purchase purchase in db.Purchases.ToList())
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
                new ApplicationContext(connectionConfig, name))
                numColors = db.Purchases.ToList().Count();

            Random rand = new Random();

            for (int i = 0; i < numColors; i++)
                colors.Add(colorsList[rand.Next(0, 5)]);

            return colors;
        }

        public List<SaleResponseJson> getSales()
        {
            List<SaleResponseJson> salesHistory = new List<SaleResponseJson>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
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

            return salesHistory;
        }

        public bool deleteSalesHistoryField(int id)
        {
            using (ApplicationContext db = new
                ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
                {
                    if (sale.Id == id)
                    {
                        db.Sales.Remove(sale);
                        db.SaveChanges();

                        return true;
                    }
                }

                return false;
            }
        }

        public List<string> getSalesInfo()
        {
            List<string> info = new List<string>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
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
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
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
                new ApplicationContext(connectionConfig, name))
                numColors = db.Sales.ToList().Count();

            Random rand = new Random();

            for (int i = 0; i < numColors; i++)
                colors.Add(colorsList[rand.Next(0, 5)]);

            return colors;
        }

        public List<string> getNetIncomeInfo()
        {
            List<string> netIncomeInfo = new List<string>();

            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
                {
                    netIncomeInfo.Add(sale.NameOfProducts);
                }
            }

            return netIncomeInfo;
        }

        public List<double> getNetIncomePrices()
        {
            List<double> netIncomePrices = new List<double>();

            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
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
                new ApplicationContext(connectionConfig, name))
                numColors = db.Sales.ToList().Count();

            Random rand = new Random();

            for (int i = 0; i < numColors; i++)
                colors.Add(colorsList[rand.Next(0, 5)]);

            return colors;
        }

        public List<ProductsBatchJson> getStore()
        {
            List<ProductsBatchJson> store = new List<ProductsBatchJson>();

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch productsBatch in db.Store.ToList())
                {
                    ProductsBatchJson productsBatchJson = new ProductsBatchJson(
                        Convert.ToString(productsBatch.Id),
                        productsBatch.NameOfProducts,
                        Convert.ToString(productsBatch.CountOfProducts),
                        Convert.ToString(productsBatch.PurchasePrice));

                    store.Add(productsBatchJson);
                }
            }

            return store;
        }

        public bool deleteStoreField(int id)
        {
            using (ApplicationContext db = new
                ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch productsBatch in db.Store.ToList())
                {
                    if (productsBatch.Id == id)
                    {
                        db.Store.Remove(productsBatch);
                        db.SaveChanges();

                        return true;
                    }
                }

                return false;
            }
        }

        public double getTotalIncome()
        {
            double totalIncome = 0;

            using(ApplicationContext db = 
                new ApplicationContext(connectionConfig, name))
            {
                foreach(Sale sale in db.Sales.ToList())
                    totalIncome += sale.PriceOfSale;
            }

            return totalIncome;
        }
        public int getTotalCount()
        {
            int totalCount = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
                    totalCount += sale.CountOfProducts;
            }

            return totalCount;
        }
        public int getAvPercent()
        {
            double sumPercent = 0;
            int countOfSales = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
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
        public int getCountStore()
        {
            int countStore = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch productsBatch in db.Store.ToList())
                {
                    countStore += productsBatch.CountOfProducts;
                }
            }

            return countStore;
        }
        public double getCostStore()
        {
            double costStore = 0;

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach (ProductsBatch productsBatch in db.Store.ToList())
                {
                    costStore += productsBatch.PurchasePrice;
                }
            }

            return costStore;
        }
    }
}
