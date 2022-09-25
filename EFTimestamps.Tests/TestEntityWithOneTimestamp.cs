using System;

using EFTimestamps.Annotations;

namespace EFTimestamps.Tests;

internal class TestEntityWithOneTimestamp
{
    private static int s_lastCreatedId = 0;

    public int Id { get; set; }
    public string? DisplayName { get; set; }

    [CreatedAt]
    public DateTime CreatedAt { get; private set; }

    public TestEntityWithOneTimestamp()
    {
        Id = ++s_lastCreatedId;
    }
}