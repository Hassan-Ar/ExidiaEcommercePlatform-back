using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace EcommercePlatform.Data;

public class RoleDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IIdentityRoleRepository _roleRepo;
    private readonly IdentityRoleManager _roleManager;

    public const string ShopAdminRole = "ShopAdmin";
    public const string CustomerRole = "Customer";

    public RoleDataSeedContributor(IIdentityRoleRepository roleRepo, IdentityRoleManager roleManager)
    {
        _roleRepo = roleRepo;
        _roleManager = roleManager;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateRoleIfNotExistsAsync(ShopAdminRole, isPublic:false);
        await CreateRoleIfNotExistsAsync(CustomerRole, isPublic:true);
    }

    private async Task CreateRoleIfNotExistsAsync(string roleName, bool isPublic)
    {
        if (await _roleRepo.FindByNormalizedNameAsync(roleName.ToUpper()) != null) return;
        var role = new IdentityRole(Guid.NewGuid(), roleName, null) { IsPublic = isPublic };
        await _roleManager.CreateAsync(role);
    }
} 