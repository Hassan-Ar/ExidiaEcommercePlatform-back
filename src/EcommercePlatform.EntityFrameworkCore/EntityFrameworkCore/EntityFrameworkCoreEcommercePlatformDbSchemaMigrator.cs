using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EcommercePlatform.Data;
using Volo.Abp.DependencyInjection;

namespace EcommercePlatform.EntityFrameworkCore;

public class EntityFrameworkCoreEcommercePlatformDbSchemaMigrator
    : IEcommercePlatformDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEcommercePlatformDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the EcommercePlatformDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EcommercePlatformDbContext>()
            .Database
            .MigrateAsync();
    }
}
