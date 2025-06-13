using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EcommercePlatform.Data;

/* This is used if database provider does't define
 * IEcommercePlatformDbSchemaMigrator implementation.
 */
public class NullEcommercePlatformDbSchemaMigrator : IEcommercePlatformDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
