using System.Threading.Tasks;

namespace EcommercePlatform.Data;

public interface IEcommercePlatformDbSchemaMigrator
{
    Task MigrateAsync();
}
