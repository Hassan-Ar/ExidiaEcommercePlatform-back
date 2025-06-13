using EcommercePlatform.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace EcommercePlatform.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EcommercePlatformEntityFrameworkCoreModule),
    typeof(EcommercePlatformApplicationContractsModule)
    )]
public class EcommercePlatformDbMigratorModule : AbpModule
{
}
