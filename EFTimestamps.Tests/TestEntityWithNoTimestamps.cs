namespace EFTimestamps.Tests
{
    internal class TestEntityWithNoTimestamps
    {
        private static int LastCreatedId = 0;

        public int Id { get; set; }
        public string DisplayName { get; set; }

        public TestEntityWithNoTimestamps()
        {
            Id = ++LastCreatedId;
        }
    }
}
