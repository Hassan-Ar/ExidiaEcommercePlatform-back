using Volo.Abp.Modularity;

namespace EcommercePlatform;

/* Inherit from this class for your domain layer tests. */
public abstract class EcommercePlatformDomainTestBase<TStartupModule> : EcommercePlatformTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
