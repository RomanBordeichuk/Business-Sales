using Microsoft.EntityFrameworkCore;

namespace BusinessSales
{
    class ApplicationContext : DbContext
    {
        private (string, int[]) connectionConfig;

        public DbSet<Account> Accounts { set; get; } = null!;
        public DbSet<Purchase> Purchases { set; get; } = null!;
        public DbSet<Sale> Sales { set; get; } = null!;
        public DbSet<ProductsBatch> Store { set; get; } = null!;

        public ApplicationContext((string, int[]) connectionConfig)
        {
            this.connectionConfig = connectionConfig;

            //Database.EnsureDeleted();
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany<Purchase>(a => a.Purchases)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>()
                .HasMany<Sale>(a => a.Sales)
                .WithOne(s => s.Account)
                .HasForeignKey(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>()
                .HasMany<ProductsBatch>(a => a.Store)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
