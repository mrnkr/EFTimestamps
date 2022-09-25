using FluentAssertions;

using Microsoft.EntityFrameworkCore;

namespace EFTimestamps.Tests
{
    public class EFTimestampsExtensionsTest
    {
        private readonly string _dateFormat = "yyyy-MM-ddTH:mm";
        private readonly DbContextOptions<TestContext> _options = new DbContextOptionsBuilder<TestContext>()
            .UseInMemoryDatabase("test_db")
            .Options;

        [Fact]
        public async Task PutsTimestamps()
        {
            using (var ctx = new TestContext(_options))
            {
                var test = new TestEntityWithTwoTimestamps
                {
                    DisplayName = "Test display name"
                };

                ctx.TestsWithTwoTimestamps.Add(test);
                await ctx.SaveChangesAsync();
            }

            using (var ctx = new TestContext(_options))
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
            using (var ctx = new TestContext(_options))
            {
                var test = new TestEntityWithOneTimestamp
                {
                    DisplayName = "Test display name"
                };

                ctx.TestsWithOneTimestamp.Add(test);
                await ctx.SaveChangesAsync();
            }

            using (var ctx = new TestContext(_options))
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
            using (var ctx = new TestContext(_options))
            {
                var test = new TestEntityWithNoTimestamps
                {
                    DisplayName = "Test display name"
                };

                ctx.TestsWithNoTimestamps.Add(test);
                var changes = await ctx.SaveChangesAsync();

                // Just make sure it does not break
                changes.Should().Be(1);
            }
        }
    }
}