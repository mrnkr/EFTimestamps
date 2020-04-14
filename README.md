# EFTimestamps

[![NuGet version][nuget-image]][nuget-url]
[![Downloads][downloads-image]][nuget-url]

[nuget-image]:https://img.shields.io/nuget/v/EFTimestamps.Annotations
[nuget-url]:https://www.nuget.org/packages/EFTimestamps.Annotations
[downloads-image]:https://img.shields.io/nuget/dt/EFTimestamps.Annotations

Put timestamps in your entities the easy way

### Motivation

Using timestamps is something I do in almost every app I make, if you're here probably you do too. Since I always follow the same approach to implement them I just ended up making this library in order not to have to rewrite this code ever again. Hopefully you'll be able to find some use for it too!

### Quick start

Install both [`EFTimestamps.Annotations`](https://www.nuget.org/packages/EFTimestamps.Annotations) and [`EFTimestamps.Configuration`](https://www.nuget.org/packages/EFTimestamps.Configuration). It is divided in two libs so that you can have your entities in a separate dll that does not know about EFCore.

After that, just mark your timestamp props with the data annotations, you don't need to use both timestamps for it to work!

```cs
using EFTimestamps.Annotations;
using System;

public class TestEntity
{
    [CreatedAt]
    public DateTime CreatedAt { get; set; }

    [UpdatedAt]
    public DateTime UpdatedAt { get; set; }
}
```

Then, go to your DbContext and override the variant of `SaveChanges` or `SaveChangesAsync` that you use in your persistence layer. If you just use your context directly with no intermediary feel free to override all of them. In case it helps, here are all the methods you may need to override and how to override them 🙃

```cs
using EFTimestamps.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class TestContext : DbContext
{
    public override int SaveChanges()
    {
        this.UpdateTimestamps();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.UpdateTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        this.UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
}
```

### Changelog

* 1.0.0 - First release
