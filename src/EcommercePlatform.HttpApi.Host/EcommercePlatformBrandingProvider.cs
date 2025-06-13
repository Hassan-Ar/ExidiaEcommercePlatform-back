using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EcommercePlatform;

[Dependency(ReplaceServices = true)]
public class EcommercePlatformBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "EcommercePlatform";
}
