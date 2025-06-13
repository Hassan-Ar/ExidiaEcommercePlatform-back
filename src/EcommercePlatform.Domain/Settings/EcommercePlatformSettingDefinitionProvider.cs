using Volo.Abp.Settings;

namespace EcommercePlatform.Settings;

public class EcommercePlatformSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EcommercePlatformSettings.MySetting1));
    }
}
