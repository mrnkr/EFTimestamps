namespace EFTimestamps.Tests
{
    internal class TestEntityWithNoTimestamps
    {
        private static int s_lastCreatedId = 0;

        public int Id { get; set; }
        public string? DisplayName { get; set; }

        public TestEntityWithNoTimestamps()
        {
            Id = ++s_lastCreatedId;
        }
    }
}