using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// Domain service for managing shops.
    /// </summary>
    public class ShopManager : DomainService, IShopManager
    {
        private readonly IRepository<Shop, Guid> _shopRepository;

        /// <summary>
        /// Creates a new instance of ShopManager.
        /// </summary>
        /// <param name="shopRepository">The shop repository.</param>
        public ShopManager(IRepository<Shop, Guid> shopRepository)
        {
            _shopRepository = shopRepository;
        }

        /// <summary>
        /// Creates a new shop.
        /// </summary>
        /// <param name="name">The name of the shop.</param>
        /// <param name="description">The description of the shop.</param>
        /// <param name="logoUrl">The URL to the shop's logo.</param>
        /// <returns>The newly created shop.</returns>
        public async Task<Shop> CreateAsync(
            string name,
            string description = null,
            string logoUrl = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Shop name cannot be empty.", nameof(name));
            }

            // Check if shop with same name already exists for this tenant
            if (await _shopRepository.AnyAsync(s => s.Name == name && s.TenantId == CurrentTenant.Id))
            {
                throw new ArgumentException($"A shop with name '{name}' already exists.", nameof(name));
            }

            // Create shop
            var shop = new Shop(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                name,
                description,
                logoUrl);

            return await _shopRepository.InsertAsync(shop);
        }

        /// <summary>
        /// Updates an existing shop.
        /// </summary>
        /// <param name="id">The ID of the shop to update.</param>
        /// <param name="name">The new name of the shop.</param>
        /// <param name="description">The new description of the shop.</param>
        /// <param name="logoUrl">The new logo URL of the shop.</param>
        /// <returns>The updated shop.</returns>
        public async Task<Shop> UpdateAsync(
            Guid id,
            string name,
            string description = null,
            string logoUrl = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Shop name cannot be empty.", nameof(name));
            }

            // Get shop
            var shop = await _shopRepository.GetAsync(id);

            // Check if another shop with same name already exists for this tenant
            if (name != shop.Name && await _shopRepository.AnyAsync(s => s.Name == name && s.TenantId == CurrentTenant.Id && s.Id != id))
            {
                throw new ArgumentException($"A shop with name '{name}' already exists.", nameof(name));
            }

            // Update shop properties
            shop.Name = name;
            shop.Description = description;
            shop.LogoUrl = logoUrl;

            return await _shopRepository.UpdateAsync(shop);
        }

        /// <summary>
        /// Activates a shop.
        /// </summary>
        /// <param name="id">The ID of the shop to activate.</param>
        /// <returns>The activated shop.</returns>
        public async Task<Shop> ActivateAsync(Guid id)
        {
            var shop = await _shopRepository.GetAsync(id);
            shop.IsActive = true;
            return await _shopRepository.UpdateAsync(shop);
        }

        /// <summary>
        /// Deactivates a shop.
        /// </summary>
        /// <param name="id">The ID of the shop to deactivate.</param>
        /// <returns>The deactivated shop.</returns>
        public async Task<Shop> DeactivateAsync(Guid id)
        {
            var shop = await _shopRepository.GetAsync(id);
            shop.IsActive = false;
            return await _shopRepository.UpdateAsync(shop);
        }

        /// <summary>
        /// Updates the shop settings.
        /// </summary>
        /// <param name="id">The ID of the shop to update.</param>
        /// <param name="defaultCurrency">The default currency for the shop.</param>
        /// <param name="defaultLanguage">The default language for the shop.</param>
        /// <param name="contactEmail">The contact email for the shop.</param>
        /// <param name="contactPhone">The contact phone for the shop.</param>
        /// <returns>The updated shop.</returns>
        public async Task<Shop> UpdateSettingsAsync(
            Guid id,
            string defaultCurrency = null,
            string defaultLanguage = null,
            string contactEmail = null,
            string contactPhone = null)
        {
            var shop = await _shopRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(defaultCurrency))
            {
                shop.DefaultCurrency = defaultCurrency;
            }

            if (!string.IsNullOrWhiteSpace(defaultLanguage))
            {
                shop.DefaultLanguage = defaultLanguage;
            }

            if (!string.IsNullOrWhiteSpace(contactEmail))
            {
                shop.ContactEmail = contactEmail;
            }

            if (!string.IsNullOrWhiteSpace(contactPhone))
            {
                shop.ContactPhone = contactPhone;
            }

            return await _shopRepository.UpdateAsync(shop);
        }
    }

    /// <summary>
    /// Interface for the shop manager domain service.
    /// </summary>
    public interface IShopManager : IDomainService
    {
        /// <summary>
        /// Creates a new shop.
        /// </summary>
        Task<Shop> CreateAsync(
            string name,
            string description = null,
            string logoUrl = null);

        /// <summary>
        /// Updates an existing shop.
        /// </summary>
        Task<Shop> UpdateAsync(
            Guid id,
            string name,
            string description = null,
            string logoUrl = null);

        /// <summary>
        /// Activates a shop.
        /// </summary>
        Task<Shop> ActivateAsync(Guid id);

        /// <summary>
        /// Deactivates a shop.
        /// </summary>
        Task<Shop> DeactivateAsync(Guid id);

        /// <summary>
        /// Updates the shop settings.
        /// </summary>
        Task<Shop> UpdateSettingsAsync(
            Guid id,
            string defaultCurrency = null,
            string defaultLanguage = null,
            string contactEmail = null,
            string contactPhone = null);
    }
}
