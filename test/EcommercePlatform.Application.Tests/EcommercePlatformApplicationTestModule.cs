using Volo.Abp.Modularity;

namespace EcommercePlatform;

[DependsOn(
    typeof(EcommercePlatformApplicationModule),
    typeof(EcommercePlatformDomainTestModule)
)]
public class EcommercePlatformApplicationTestModule : AbpModule
{

}
