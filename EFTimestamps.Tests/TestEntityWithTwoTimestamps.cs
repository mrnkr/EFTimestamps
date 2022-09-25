using System;

using EFTimestamps.Annotations;

namespace EFTimestamps.Tests;

internal class TestEntityWithTwoTimestamps
{
    private static int s_lastCreatedId = 0;

    public int Id { get; set; }
    public string? DisplayName { get; set; }

    [CreatedAt]
    public DateTime CreatedAt { get; private set; }

    [UpdatedAt]
    public DateTime UpdatedAt { get; private set; }

    public TestEntityWithTwoTimestamps()
    {
        Id = ++s_lastCreatedId;
    }
}