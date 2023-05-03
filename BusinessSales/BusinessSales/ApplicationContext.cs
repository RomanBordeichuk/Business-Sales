using Microsoft.EntityFrameworkCore;

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

    class WebAppDbApplicationContext : DbContext
    {
        private (string, int[]) connectionConfig;
        private string dbName;

        public DbSet<Account> Accounts { set; get; } = null!;

        public WebAppDbApplicationContext(
            (string, int[]) connectionConfig, string dbName)
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
