using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents a shop in the e-commerce platform.
    /// </summary>
    public class Shop : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this shop belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The name of the shop.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the shop.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The URL to the shop's logo.
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Indicates whether the shop is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The shop's contact email.
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// The shop's contact phone number.
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// The shop's default currency.
        /// </summary>
        public string DefaultCurrency { get; set; }

        /// <summary>
        /// The shop's default language.
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Creates a new Shop instance.
        /// </summary>
        protected Shop()
        {
        }

        /// <summary>
        /// Creates a new Shop instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the shop.</param>
        /// <param name="tenantId">The tenant ID that this shop belongs to.</param>
        /// <param name="name">The name of the shop.</param>
        /// <param name="description">The description of the shop.</param>
        /// <param name="logoUrl">The URL to the shop's logo.</param>
        public Shop(
            Guid id,
            Guid? tenantId,
            string name,
            string description = null,
            string logoUrl = null)
            : base(id)
        {
            TenantId = tenantId;
            Name = name;
            Description = description;
            LogoUrl = logoUrl;
            IsActive = true;
            DefaultCurrency = "USD";
            DefaultLanguage = "en";
        }
    }
}
