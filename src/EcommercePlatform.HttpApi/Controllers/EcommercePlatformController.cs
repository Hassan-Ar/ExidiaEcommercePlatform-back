using EcommercePlatform.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EcommercePlatform.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EcommercePlatformController : AbpControllerBase
{
    protected EcommercePlatformController()
    {
        LocalizationResource = typeof(EcommercePlatformResource);
    }
}
