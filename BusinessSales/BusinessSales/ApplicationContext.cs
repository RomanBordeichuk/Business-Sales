using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BusinessSales
{
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
        public DbSet<ProductsBatch> Store { set; get; } = null!;

        public ApplicationContext((string, int[]) connectionConfig, string dbName)
        {
            this.connectionConfig = connectionConfig;
            this.dbName =
                "business_sales_" + String.Join("_", dbName.Split(" ")) + "_db";

            Database.EnsureCreated();
        }
        
        public void deleteDatabase()
        {
            Database.EnsureDeleted();
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

    class WebAppDbApplicationContext : DbContext
    {
        static private string dbName = "business_sales_db";

        private (string, int[]) connectionConfig;

        public DbSet<Account> Accounts { set; get; } = null!;

        public WebAppDbApplicationContext(
            (string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;

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
