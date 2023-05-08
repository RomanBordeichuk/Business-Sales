using Microsoft.EntityFrameworkCore;

namespace BusinessSales
{
    record AccountNameJson(string name);
    record PasswordJson(string password);
    record DatabaseJson(string name, string password);

    record LoadMainPageJson(string name, double totalIncome, int totalCount);

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
                new ApplicationContext(connectionConfig, this.name));
        }
        public void rewriteDatabase(string newDbName)
        {
            using (ApplicationContext newDb =
                new ApplicationContext(connectionConfig, newDbName))
            {
                using(ApplicationContext db =
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
            using(ApplicationContext db = 
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
            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                db.Sales.Add(sale);
                db.SaveChanges();
            }
        }

        public bool hasProducts(string nameOfProducts)
        {
            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach(ProductsBatch productsBatch in db.Store.ToList())
                {
                    if (productsBatch.NameOfProducts == nameOfProducts)
                        return true;
                }
            }

            return false;
        }

        public bool hasAnoughCount(string nameOfProducts, int countOfProducts)
        {
            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach(ProductsBatch productsBatch in db.Store.ToList())
                {
                    if (productsBatch.NameOfProducts == nameOfProducts &&
                        productsBatch.CountOfProducts >= countOfProducts) 
                        return true;
                }

                return false;
            }
        }

        public void appendProductsToBatch(ProductsBatch productsBatch)
        {
            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach(ProductsBatch batch in db.Store.ToList())
                {
                    if(batch.NameOfProducts == productsBatch.NameOfProducts)
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
            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach(ProductsBatch productsBatch in db.Store.ToList())
                {
                    if(productsBatch.NameOfProducts == nameOfProducts)
                    {
                        if(productsBatch.CountOfProducts > countOfProducts)
                        {
                            productsBatch.CountOfProducts -= countOfProducts;
                            
                            if(productsBatch.PurchasePrice <= priceOfSale)
                                productsBatch.PurchasePrice = 0;
                            else
                                productsBatch.PurchasePrice -= priceOfSale;

                            db.Store.Update(productsBatch);
                            db.SaveChanges();

                            return;
                        }
                        else if(productsBatch.CountOfProducts == countOfProducts)
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

            using(ApplicationContext db =
                new ApplicationContext(connectionConfig, name))
            {
                foreach(Purchase purchase in db.Purchases.ToList())
                {
                    PurchaseResponseJson purchaseResponseJson =
                        new PurchaseResponseJson(
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
        public List<SaleResponseJson> getSales()
        {
            List<SaleResponseJson> salesHistory = new List<SaleResponseJson>();

            using(ApplicationContext db = 
                new ApplicationContext(connectionConfig, name))
            {
                foreach (Sale sale in db.Sales.ToList())
                {
                    SaleResponseJson saleResponseJson = new SaleResponseJson(
                        Convert.ToString(sale.Date), sale.NameOfProducts,
                        Convert.ToString(sale.PriceOfProduct),
                        Convert.ToString(sale.CountOfProducts), sale.Comment,
                        Convert.ToString(sale.PriceOfSale));

                    salesHistory.Add(saleResponseJson);
                }
            }

            return salesHistory;
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
                        productsBatch.NameOfProducts,
                        Convert.ToString(productsBatch.CountOfProducts),
                        Convert.ToString(productsBatch.PurchasePrice));

                    store.Add(productsBatchJson);
                }
            }

            return store;
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
    }
}
