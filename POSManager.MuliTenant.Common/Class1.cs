﻿using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;

namespace POSManager.MultiTenant.Common;
public class MultiTenantStoreDbContext : EFCoreStoreDbContext<TenantInfo>
{
    public MultiTenantStoreDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("EfCoreStoreSampleConnectionString");
        base.OnConfiguring(optionsBuilder);
    }
}