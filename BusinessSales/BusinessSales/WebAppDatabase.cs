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
            set { dbName = value; }
            get { return dbName; }
        }
        public string Password
        {
            set { password = value; }
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
        private (string, int[]) connectionConfig;

        public WebAppDatabase((string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;
        }

        public bool pushNewAccount(string dbName, string password)
        {
            Account account = new Account(dbName, password);

            using (WebAppDbApplicationContext db =
                new WebAppDbApplicationContext(connectionConfig))
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
                new WebAppDbApplicationContext(connectionConfig))
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
        public bool changeAccountName(string dbName, string newDbName)
        {
            using (WebAppDbApplicationContext db =
                new WebAppDbApplicationContext(connectionConfig))
            {
                foreach (Account acc in db.Accounts.ToList())
                {
                    if (acc.DbName == newDbName) return false;
                }

                foreach(Account acc in db.Accounts.ToList())
                {
                    if(acc.DbName == dbName)
                    {
                        acc.DbName = newDbName;

                        db.Accounts.Update(acc);
                        db.SaveChanges();

                        return true;
                    }
                }

                return false;
            }
        }
        public bool changePassword(string dbName, string password)
        {
            using(WebAppDbApplicationContext db =
                new WebAppDbApplicationContext(connectionConfig))
            {
                foreach(Account acc in db.Accounts.ToList())
                {
                    if(acc.DbName == dbName)
                    {
                        acc.Password = password;

                        db.Accounts.Update(acc);
                        db.SaveChanges();

                        return true;
                    }
                }

                return false;
            }
        }
        public bool deleteAccount(string dbName)
        {
            using(WebAppDbApplicationContext db = 
                new WebAppDbApplicationContext(connectionConfig))
            {
                foreach(Account acc in db.Accounts.ToList())
                {
                    if(acc.DbName == dbName)
                    {
                        db.Accounts.Remove(acc);
                        db.SaveChanges();

                        return true;
                    }
                }

                return false;
            }
        }
    }
}
