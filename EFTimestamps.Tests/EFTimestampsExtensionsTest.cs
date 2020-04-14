using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace EFTimestamps.Tests
{
    [TestClass]
    public class EFTimestampsExtensionsTest
    {
        [TestMethod]
        public async Task PutsTimestamps()
        {
            var options = new DbContextOptionsBuilder<TestContext>()
                .UseInMemoryDatabase("test_db")
                .Options;

            using (var ctx = new TestContext(options))
            {
                var test = new TestEntity();
                test.DisplayName = "Test display name";

                ctx.tests.Add(test);
                await ctx.SaveChangesAsync();
            }

            using (var ctx = new TestContext(options))
            {
                var tests = await ctx.tests.ToListAsync();
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
    }
}
