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

        public void showPurchases()
        {

        }
        public void showSales()
        {

        }
        public double getTotalIncome()
        {
            double totalIncome = 0;

            using(ApplicationContext db = 
                new ApplicationContext(connectionConfig, name))
            {
                foreach(Sale sale in db.Sales.ToList())
                    totalIncome += sale.PriceOfProduct;
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
