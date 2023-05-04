using Microsoft.EntityFrameworkCore;

namespace BusinessSales
{
    record AccountNameJson(string name);
    record PasswordJson(string password);
    record DatabaseJson(string name, string password);

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

        public void pushPurchase()
        {

        }
        public void showPurchases()
        {

        }

        public void pushSale()
        {

        }
        public void showSales()
        {

        }
    }
}
