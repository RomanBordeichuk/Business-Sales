using Microsoft.EntityFrameworkCore;

namespace BusinessSales
{
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
            this.name = 
                "business_sales_" + String.Join("_", name.Split(" ")) + "_db";

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, this.name));
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
