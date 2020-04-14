using EFTimestamps.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EFTimestamps.Tests
{
    internal class TestContext : DbContext
    {
        public DbSet<TestEntityWithTwoTimestamps> TestsWithTwoTimestamps { get; set; }
        public DbSet<TestEntityWithOneTimestamp> TestsWithOneTimestamp { get; set; }
        public DbSet<TestEntityWithNoTimestamps> TestsWithNoTimestamps { get; set; }

        public TestContext(DbContextOptions<TestContext> options) : base(options) { }

        public override int SaveChanges()
        {
            this.UpdateTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.UpdateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.UpdateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
