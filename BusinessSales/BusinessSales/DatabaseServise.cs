using Microsoft.EntityFrameworkCore;

namespace BusinessSales
{
    class Database
    {
        private (string, int[]) connectionConfig;

        public Database((string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;
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
        public DbSet<Purchase> Purchases { set; get; } = null!;
        public DbSet<Sale> Sales { set; get; } = null!;

        public ApplicationContext((string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionConfig.Item1, 
                new MySqlServerVersion(new Version(
                    connectionConfig.Item2[0], 
                    connectionConfig.Item2[1], 
                    connectionConfig.Item2[2])));
        }
    }
}
