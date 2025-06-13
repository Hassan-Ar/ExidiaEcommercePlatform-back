using System;
using System.Collections.Generic;
using System.Text;
using EcommercePlatform.Localization;
using Volo.Abp.Application.Services;

namespace EcommercePlatform;

/* Inherit your application services from this class.
 */
public abstract class EcommercePlatformAppService : ApplicationService
{
    protected EcommercePlatformAppService()
    {
        LocalizationResource = typeof(EcommercePlatformResource);
    }
}
