using EFTimestamps.Annotations;
using System;

namespace EFTimestamps.Tests
{
    internal class TestEntityWithOneTimestamp
    {
        private static int LastCreatedId = 0;

        public int Id { get; set; }
        public string DisplayName { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }

        public TestEntityWithOneTimestamp()
        {
            Id = ++LastCreatedId;
        }
    }
}
