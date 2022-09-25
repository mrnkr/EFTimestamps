using System.Reflection;

using EFTimestamps.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFTimestamps.Configuration;

public static class EFTimestampsExtensions
{
    public static void IndexTimestamps(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            IndexAnnotatedProperty<CreatedAtAttribute>(entityType);
            IndexAnnotatedProperty<UpdatedAtAttribute>(entityType);
        }
    }

    private static void IndexAnnotatedProperty<TAttribute>(IMutableEntityType entityType) where TAttribute : Attribute
    {
        var properties = entityType.GetProperties();
        var property = properties.FirstOrDefault(prop => prop.PropertyInfo?.GetCustomAttribute<TAttribute>() is not null);
        if (property is not null)
        {
            entityType.AddIndex(property);
        }
    }

    public static void UpdateTimestamps(this DbContext ctx)
    {
        PutTimestamp<CreatedAtAttribute>(ctx);
        PutTimestamp<UpdatedAtAttribute>(ctx);
    }

    private static void PutTimestamp<TAttribute>(DbContext ctx) where TAttribute : Attribute
    {
        var newEntries = ctx.ChangeTracker.Entries()
            .Where(entry => entry.State == EntityState.Added);

        foreach (var entry in newEntries)
        {
            var prop = GetMarkedProperty(entry, typeof(TAttribute));

            if (prop is null)
            {
                continue;
            }

            prop.SetValue(entry.Entity, DateTime.UtcNow);
        }
    }

    private static PropertyInfo? GetMarkedProperty(EntityEntry entry, Type attributeType)
    {
        return entry
            .Entity
            .GetType()
            .GetProperties()
            .FirstOrDefault(prop => prop.GetCustomAttribute(attributeType) is not null && prop.CanWrite);
    }
}