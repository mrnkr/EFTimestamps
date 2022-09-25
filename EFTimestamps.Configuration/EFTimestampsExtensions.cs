using System.Reflection;

using EFTimestamps.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFTimestamps.Configuration;

public static class EFTimestampsExtensions
{
    public static void IndexTimestamps<T>(this EntityTypeBuilder<T> entityTypeBuilder) where T : class
    {
        entityTypeBuilder.IndexCreatedAtProperty();
        entityTypeBuilder.IndexUpdatedAtProperty();
    }

    private static void IndexCreatedAtProperty<T>(this EntityTypeBuilder<T> entityTypeBuilder) where T : class
    {
        var createdAtProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttribute<CreatedAtAttribute>() is not null);
        if (createdAtProperty is not null)
        {
            entityTypeBuilder.HasIndex(createdAtProperty.Name);
        }
    }

    private static void IndexUpdatedAtProperty<T>(this EntityTypeBuilder<T> entityTypeBuilder) where T : class
    {
        var updatedAtProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttribute<UpdatedAtAttribute>() is not null);
        if (updatedAtProperty is not null)
        {
            entityTypeBuilder.HasIndex(updatedAtProperty.Name);
        }
    }

    public static void UpdateTimestamps(this DbContext ctx)
    {
        ctx.PutCreatedAt();
        ctx.PutUpdatedAt();
    }

    private static void PutCreatedAt(this DbContext ctx)
    {
        var newEntries = ctx.ChangeTracker.Entries()
            .Where(entry => entry.State == EntityState.Added);

        foreach (var entry in newEntries)
        {
            var prop = ctx.GetMarkedProperty(entry, typeof(CreatedAtAttribute));

            if (prop is null)
            {
                continue;
            }

            prop.SetValue(entry.Entity, DateTime.UtcNow);
        }
    }

    private static void PutUpdatedAt(this DbContext ctx)
    {
        var updatedEntries = ctx.ChangeTracker.Entries()
            .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);

        foreach (var entry in updatedEntries)
        {
            var prop = ctx.GetMarkedProperty(entry, typeof(UpdatedAtAttribute));

            if (prop is null)
            {
                continue;
            }

            prop.SetValue(entry.Entity, DateTime.UtcNow);
        }
    }

    private static PropertyInfo? GetMarkedProperty(this DbContext ctx, EntityEntry entry, Type attributeType)
    {
        return entry
            .Entity
            .GetType()
            .GetProperties()
            .Where(prop => prop.GetCustomAttribute(attributeType) is not null)
            .Where(prop => prop.CanWrite)
            .FirstOrDefault();
    }
}