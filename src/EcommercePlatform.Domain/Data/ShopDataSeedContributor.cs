using System;
using System.Threading.Tasks;
using EcommercePlatform.Shops;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace EcommercePlatform.Data;

/// <summary>
/// Seeds a single default shop so that other entities that depend on a <c>ShopId</c> can be created safely.
/// </summary>
public class ShopDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Shop, Guid> _shopRepository;

    public ShopDataSeedContributor(IRepository<Shop, Guid> shopRepository)
    {
        _shopRepository = shopRepository;
    }

    /// <summary>
    /// Creates the default shop if it does not already exist for the current tenant.
    /// </summary>
    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        // Ensure that we have exactly one shop per tenant/host.
        if (await _shopRepository.GetCountAsync() > 0)
        {
            return; // A shop already exists – nothing to seed.
        }

        var shop = new Shop(
            Guid.NewGuid(),
            name: "Default Shop",
            description: "Default shop created by data seeder.",
            logoUrl: string.Empty,
            address: string.Empty,
            phoneNumber: string.Empty,
            email: string.Empty,
            businessHours: string.Empty,

            taxId: string.Empty,
            paymentMethods: string.Empty,
            shippingMethods: string.Empty,
            tenantId: context.TenantId
        );

        await _shopRepository.InsertAsync(shop, autoSave: true);
    }
} 