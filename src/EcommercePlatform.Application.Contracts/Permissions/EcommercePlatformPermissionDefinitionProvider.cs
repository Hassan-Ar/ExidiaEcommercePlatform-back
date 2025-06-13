using EcommercePlatform.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EcommercePlatform.Permissions;

public class EcommercePlatformPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EcommercePlatformPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(EcommercePlatformPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EcommercePlatformResource>(name);
    }
}
