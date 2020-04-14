using EFTimestamps.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;

namespace EFTimestamps.Configuration
{
    public static class EFTimestampsExtensions
    {
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

                if (prop == null)
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

                if (prop == null)
                {
                    continue;
                }

                prop.SetValue(entry.Entity, DateTime.UtcNow);
            }
        }

        private static PropertyInfo GetMarkedProperty(this DbContext ctx, EntityEntry entry, Type attributeType)
        {
            return entry
                .Entity
                .GetType()
                .GetProperties()
                .Where(prop => prop.GetCustomAttribute(attributeType) != null)
                .FirstOrDefault();
        }
    }
}
