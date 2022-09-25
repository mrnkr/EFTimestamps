using EFTimestamps.Configuration;

using Microsoft.EntityFrameworkCore;

namespace EFTimestamps.Tests;

internal class TestContext : DbContext
{
    public DbSet<TestEntityWithTwoTimestamps> TestsWithTwoTimestamps { get; set; } = null!;
    public DbSet<TestEntityWithOneTimestamp> TestsWithOneTimestamp { get; set; } = null!;
    public DbSet<TestEntityWithNoTimestamps> TestsWithNoTimestamps { get; set; } = null!;

    public TestContext(DbContextOptions<TestContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestEntityWithTwoTimestamps>().IndexTimestamps();
        modelBuilder.Entity<TestEntityWithOneTimestamp>().IndexTimestamps();
        modelBuilder.Entity<TestEntityWithNoTimestamps>().IndexTimestamps();
    }

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

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
}