using Volo.Abp.Modularity;

namespace EcommercePlatform;

[DependsOn(
    typeof(EcommercePlatformDomainModule),
    typeof(EcommercePlatformTestBaseModule)
)]
public class EcommercePlatformDomainTestModule : AbpModule
{

}
