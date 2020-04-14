using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace EFTimestamps.Tests
{
    [TestClass]
    public class EFTimestampsExtensionsTest
    {
        private readonly DbContextOptions<TestContext> options = new DbContextOptionsBuilder<TestContext>()
            .UseInMemoryDatabase("test_db")
            .Options;

        [TestMethod]
        public async Task PutsTimestamps()
        {
            using (var ctx = new TestContext(options))
            {
                var test = new TestEntityWithTwoTimestamps();
                test.DisplayName = "Test display name";

                ctx.TestsWithTwoTimestamps.Add(test);
                await ctx.SaveChangesAsync();
            }

            using (var ctx = new TestContext(options))
            {
                var tests = await ctx.TestsWithTwoTimestamps.ToListAsync();
                foreach (var test in tests)
                {
                    var format = "yyyy-MM-ddTH:mm";
                    var createdAt = test.CreatedAt.ToString(format);
                    var updatedAt = test.UpdatedAt.ToString(format);
                    Assert.AreEqual(expected: DateTime.UtcNow.ToString(format), createdAt);
                    Assert.AreEqual(expected: DateTime.UtcNow.ToString(format), updatedAt);
                }
            }
        }

        [TestMethod]
        public async Task PutsCreationTimestampAlone()
        {
            using (var ctx = new TestContext(options))
            {
                var test = new TestEntityWithOneTimestamp();
                test.DisplayName = "Test display name";

                ctx.TestsWithOneTimestamp.Add(test);
                await ctx.SaveChangesAsync();
            }

            using (var ctx = new TestContext(options))
            {
                var tests = await ctx.TestsWithOneTimestamp.ToListAsync();
                foreach (var test in tests)
                {
                    var format = "yyyy-MM-ddTH:mm";
                    var createdAt = test.CreatedAt.ToString(format);
                    Assert.AreEqual(expected: DateTime.UtcNow.ToString(format), createdAt);
                }
            }
        }

        [TestMethod]
        public async Task ActsAsNoOp()
        {
            using (var ctx = new TestContext(options))
            {
                var test = new TestEntityWithNoTimestamps();
                test.DisplayName = "Test display name";

                ctx.TestsWithNoTimestamps.Add(test);
                var changes = await ctx.SaveChangesAsync();

                // Just make sure it does not break
                Assert.AreEqual(expected: 1, actual: changes);
            }
        }
    }
}
