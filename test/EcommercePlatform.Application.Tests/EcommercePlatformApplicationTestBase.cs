using Volo.Abp.Modularity;

namespace EcommercePlatform;

public abstract class EcommercePlatformApplicationTestBase<TStartupModule> : EcommercePlatformTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
