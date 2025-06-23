using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Microsoft.Extensions.DependencyInjection;

namespace EcommercePlatform;

[DependsOn(
    typeof(EcommercePlatformDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(EcommercePlatformApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpBlobStoringModule)
    )]
public class EcommercePlatformApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EcommercePlatformApplicationModule>();
        });

        // Register AI Shopping Assistant dependencies
        context.Services.AddTransient<EcommercePlatform.ChatAssistant.IProductSearchService, EcommercePlatform.ChatAssistant.ProductSearchService>();
        context.Services.AddTransient<EcommercePlatform.ChatAssistant.IChatAssistantAppService, EcommercePlatform.ChatAssistant.ChatAssistantAppService>();
        context.Services.AddSingleton(provider => new EcommercePlatform.ChatAssistant.OllamaLLMClient("http://localhost:11434", "llama3.2:latest"));// "deepseek-r1:7b"));
    }
}
