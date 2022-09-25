using FluentAssertions;

using Microsoft.EntityFrameworkCore;

namespace EFTimestamps.Tests;

public class EFTimestampsExtensionsTest
{
    private readonly string _dateFormat = "yyyy-MM-ddTH:mm";
    private readonly DbContextOptions<TestContext> _options = new DbContextOptionsBuilder<TestContext>()
        .UseInMemoryDatabase("test_db")
        .Options;

    [Fact]
    public async Task PutsTimestamps()
    {
        using (TestContext ctx = new(_options))
        {
            TestEntityWithTwoTimestamps test = new()
            {
                DisplayName = "Test display name"
            };

            ctx.TestsWithTwoTimestamps.Add(test);
            await ctx.SaveChangesAsync();
        }

        using (TestContext ctx = new(_options))
        {
            var tests = await ctx.TestsWithTwoTimestamps.ToListAsync();
            foreach (var test in tests)
            {
                var createdAt = test.CreatedAt.ToString(_dateFormat);
                var updatedAt = test.UpdatedAt.ToString(_dateFormat);

                createdAt.Should().Be(DateTime.UtcNow.ToString(_dateFormat));
                updatedAt.Should().Be(DateTime.UtcNow.ToString(_dateFormat));
            }
        }
    }

    [Fact]
    public async Task PutsCreationTimestampAlone()
    {
        using (TestContext ctx = new(_options))
        {
            TestEntityWithOneTimestamp test = new()
            {
                DisplayName = "Test display name"
            };

            ctx.TestsWithOneTimestamp.Add(test);
            await ctx.SaveChangesAsync();
        }

        using (TestContext ctx = new(_options))
        {
            var tests = await ctx.TestsWithOneTimestamp.ToListAsync();
            foreach (var test in tests)
            {
                var createdAt = test.CreatedAt.ToString(_dateFormat);
                createdAt.Should().Be(DateTime.UtcNow.ToString(_dateFormat));
            }
        }
    }

    [Fact]
    public async Task ActsAsNoOp()
    {
        using TestContext ctx = new(_options);
        TestEntityWithNoTimestamps test = new()
        {
            DisplayName = "Test display name"
        };

        ctx.TestsWithNoTimestamps.Add(test);
        var changes = await ctx.SaveChangesAsync();

        // Just make sure it does not break
        changes.Should().Be(1);
    }

    [Fact]
    public void IndexesTimestamps()
    {
        using TestContext ctx = new(_options);
        var entityType = ctx.Model.FindEntityTypes(typeof(TestEntityWithTwoTimestamps)).First();

        var properties = entityType.GetProperties();

        properties.Should().Contain(prop => prop.Name == nameof(TestEntityWithTwoTimestamps.CreatedAt) && prop.IsIndex());
        properties.Should().Contain(prop => prop.Name == nameof(TestEntityWithTwoTimestamps.UpdatedAt) && prop.IsIndex());
    }
}