using Microsoft.EntityFrameworkCore;

namespace BusinessSales
{
    class Database
    {
        private (string, MySqlServerVersion) connectionConfig;

        public Database((string, MySqlServerVersion) connectionConfig)
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

    class ApplicationContext : DbContext
    {
        private (string, MySqlServerVersion) connectionConfig;
        public DbSet<Purchase> Purchases { set; get; } = null!;
        public DbSet<Sale> Sales { set; get; } = null!;

        public ApplicationContext((string, MySqlServerVersion) connectionConfig)
        {
            this.connectionConfig = connectionConfig;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionConfig.Item1, connectionConfig.Item2);
        }
    }

}
