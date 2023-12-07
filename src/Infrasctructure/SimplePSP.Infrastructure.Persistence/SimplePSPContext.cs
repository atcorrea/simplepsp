using Microsoft.EntityFrameworkCore;
using SimplePSP.Infrastructure.Persistence.DTOs;

namespace SimplePSP.Infrastructure.Persistence
{
    public class SimplePSPContext(DbContextOptions<SimplePSPContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<TransactionDTO> Transactions { get; set; }
        public DbSet<PayableDTO> Payables { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }
    }
}