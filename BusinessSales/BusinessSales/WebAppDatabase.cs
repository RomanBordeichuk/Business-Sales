namespace BusinessSales
{
    public class Account
    {
        private int id;
        private string dbName;
        private string password;

        public int Id
        {
            set { }
            get { return id; }
        }
        public string DbName
        {
            set { }
            get { return dbName; }
        }
        public string Password
        {
            set { }
            get { return password; }
        }

        public Account(string dbName, string password)
        {
            this.dbName = dbName;
            this.password = password;
        }
    }

    public class WebAppDatabase
    {
        static private string name = "business_sales_db";

        private (string, int[]) connectionConfig;

        public WebAppDatabase((string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;
        }

        public bool pushNewAccount(string dbName, string password)
        {
            Account account = new Account(dbName, password);

            using (WebAppDbApplicationContext db =
                new WebAppDbApplicationContext(connectionConfig, name))
            {
                bool alreadyExists = false;

                foreach (Account acc in db.Accounts.ToList())
                {
                    if (acc.DbName == account.DbName)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    return true;
                }
                else return false;
            }
        }
        public bool accountExists(string dbName, string password)
        {
            Account account = new Account(dbName, password);

            using (WebAppDbApplicationContext db = 
                new WebAppDbApplicationContext(connectionConfig, name))
            {
                foreach (Account acc in db.Accounts.ToList())
                {
                    if (acc.DbName == account.DbName &&
                        acc.Password == account.Password)
                        return true;
                }

                return false;
            }
        }
    }
}
