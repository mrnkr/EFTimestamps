using EFTimestamps.Annotations;
using System;

namespace EFTimestamps.Tests
{
    internal class TestEntityWithTwoTimestamps
    {
        private static int LastCreatedId = 0;

        public int Id { get; set; }
        public string DisplayName { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }

        [UpdatedAt]
        public DateTime UpdatedAt { get; set; }

        public TestEntityWithTwoTimestamps()
        {
            Id = ++LastCreatedId;
        }
    }
}
