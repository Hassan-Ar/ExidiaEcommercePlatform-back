//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Volo.Abp.Application.Dtos;
//using Volo.Abp.Application.Services;

//namespace EcommercePlatform.Application.Contracts.Services
//{
//    /// <summary>
//    /// Interface for the shop application service.
//    /// </summary>
//    public interface IShopAppService : IApplicationService
//    {
//        /// <summary>
//        /// Gets a shop by ID.
//        /// </summary>
//        Task<ShopDto> GetAsync(Guid id);

//        /// <summary>
//        /// Gets a list of all shops.
//        /// </summary>
//        Task<List<ShopDto>> GetListAsync();

//        /// <summary>
//        /// Gets a paged list of shops.
//        /// </summary>
//        Task<PagedResultDto<ShopDto>> GetPagedListAsync(PagedAndSortedResultRequestDto input);

//        /// <summary>
//        /// Creates a new shop.
//        /// </summary>
//        Task<ShopDto> CreateAsync(CreateShopDto input);

//        /// <summary>
//        /// Updates an existing shop.
//        /// </summary>
//        Task<ShopDto> UpdateAsync(Guid id, UpdateShopDto input);

//        /// <summary>
//        /// Deletes a shop.
//        /// </summary>
//        Task DeleteAsync(Guid id);

//        /// <summary>
//        /// Activates a shop.
//        /// </summary>
//        Task<ShopDto> ActivateAsync(Guid id);

//        /// <summary>
//        /// Deactivates a shop.
//        /// </summary>
//        Task<ShopDto> DeactivateAsync(Guid id);

//        /// <summary>
//        /// Updates the shop settings.
//        /// </summary>
//        Task<ShopDto> UpdateSettingsAsync(Guid id, UpdateShopSettingsDto input);
//    }

//    /// <summary>
//    /// DTO for shop data.
//    /// </summary>
//    public class ShopDto : EntityDto<Guid>
//    {
//        /// <summary>
//        /// The name of the shop.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the shop.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The URL to the shop's logo.
//        /// </summary>
//        public string LogoUrl { get; set; }

//        /// <summary>
//        /// Indicates whether the shop is active.
//        /// </summary>
//        public bool IsActive { get; set; }

//        /// <summary>
//        /// The shop's contact email.
//        /// </summary>
//        public string ContactEmail { get; set; }

//        /// <summary>
//        /// The shop's contact phone number.
//        /// </summary>
//        public string ContactPhone { get; set; }

//        /// <summary>
//        /// The shop's default currency.
//        /// </summary>
//        public string DefaultCurrency { get; set; }

//        /// <summary>
//        /// The shop's default language.
//        /// </summary>
//        public string DefaultLanguage { get; set; }

//        /// <summary>
//        /// The creation time of the shop.
//        /// </summary>
//        public DateTime CreationTime { get; set; }
//    }

//    /// <summary>
//    /// DTO for creating a shop.
//    /// </summary>
//    public class CreateShopDto
//    {
//        /// <summary>
//        /// The name of the shop.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the shop.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The URL to the shop's logo.
//        /// </summary>
//        public string LogoUrl { get; set; }
//    }

//    /// <summary>
//    /// DTO for updating a shop.
//    /// </summary>
//    public class UpdateShopDto
//    {
//        /// <summary>
//        /// The name of the shop.
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The description of the shop.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// The URL to the shop's logo.
//        /// </summary>
//        public string LogoUrl { get; set; }
//    }

//    /// <summary>
//    /// DTO for updating shop settings.
//    /// </summary>
//    public class UpdateShopSettingsDto
//    {
//        /// <summary>
//        /// The default currency for the shop.
//        /// </summary>
//        public string DefaultCurrency { get; set; }

//        /// <summary>
//        /// The default language for the shop.
//        /// </summary>
//        public string DefaultLanguage { get; set; }

//        /// <summary>
//        /// The contact email for the shop.
//        /// </summary>
//        public string ContactEmail { get; set; }

//        /// <summary>
//        /// The contact phone number for the shop.
//        /// </summary>
//        public string ContactPhone { get; set; }
//    }
//} 