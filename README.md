# EFTimestamps

[![NuGet version][nuget-image]][nuget-url]
[![Downloads][downloads-image]][nuget-url]
[![Build Status](https://travis-ci.com/mrnkr/EFTimestamps.svg?branch=master)](https://travis-ci.com/mrnkr/EFTimestamps)
[![codecov](https://codecov.io/gh/mrnkr/EFTimestamps/branch/master/graph/badge.svg)](https://codecov.io/gh/mrnkr/EFTimestamps)
[![License][license]][nuget-url]

[nuget-image]:https://img.shields.io/nuget/v/EFTimestamps.Annotations
[nuget-url]:https://www.nuget.org/packages/EFTimestamps.Annotations
[downloads-image]:https://img.shields.io/nuget/dt/EFTimestamps.Annotations
[license]:https://img.shields.io/github/license/mrnkr/EFTimestamps

Put timestamps in your entities the easy way

## Motivation

Using timestamps is something I do in almost every app I make, if you're here probably you do too. Since I always follow the same approach to implement them I just ended up making this library in order not to have to rewrite this code ever again. Hopefully you'll be able to find some use for it too!

## Quick start

Install both [`EFTimestamps.Annotations`](https://www.nuget.org/packages/EFTimestamps.Annotations) and [`EFTimestamps.Configuration`](https://www.nuget.org/packages/EFTimestamps.Configuration). It is divided in two libs so that you can have your entities in a separate `dll` that does not know about EFCore.

After that, mark your timestamp properties with the data annotations.

```cs
using EFTimestamps.Annotations;

public class TestEntity
{
    [CreatedAt]
    public DateTime CreatedAt { get; set; }

    [UpdatedAt]
    public DateTime UpdatedAt { get; set; }
}
```

Then, go to your DbContext and override the variant of `SaveChanges` or `SaveChangesAsync` that you use in your persistence layer.

Additionally, you can tell EFCore to create indexes for your timestamps by calling `modelBuilder.IndexTimestamps()` in `OnModelCreating`.

Here is an example of a `DbContext` that does both these things.

```cs
using EFTimestamps.Configuration;
using Microsoft.EntityFrameworkCore;

public class TestContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.IndexTimestamps();
    }

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

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
}
```

## Changelog

* 1.0.0 - First release
* 1.0.1 - Bug fix
* 1.1.0 - Add indexing for timestamp properties
