using Microsoft.EntityFrameworkCore;

namespace BusinessSales
{
    record DatabaseJson(string name);

    class Database
    {
        private (string, int[]) connectionConfig;
        private string name;
        
        public string Name
        {
            set { }
            get { return name; }
        }

        public Database((string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;
        }

        public void setDbName(string name)
        {
            this.name = 
                "business_sales_" + String.Join("_", name.Split(" ")) + "_db";

            using (ApplicationContext db =
                new ApplicationContext(connectionConfig, this.name));

            Console.WriteLine($"Database {this.name} created");
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

    class DbConnectionConfig
    {
        public string ConnectionString { set; get; }
        public int[] MySqlServerVersion { set; get; }
    }

    class ApplicationContext : DbContext
    {
        private (string, int[]) connectionConfig;
        private string dbName;

        public DbSet<Purchase> Purchases { set; get; } = null!;
        public DbSet<Sale> Sales { set; get; } = null!;

        public ApplicationContext((string, int[]) connectionConfig, string dbName)
        {
            this.connectionConfig = connectionConfig;
            this.dbName = dbName;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionConfig.Item1 + dbName + ";",
                new MySqlServerVersion(new Version(
                    connectionConfig.Item2[0], 
                    connectionConfig.Item2[1], 
                    connectionConfig.Item2[2])));
        }
    }
}
