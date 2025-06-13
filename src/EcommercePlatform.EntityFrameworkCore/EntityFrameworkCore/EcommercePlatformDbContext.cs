using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using EcommercePlatform.Products;
using EcommercePlatform.Categories;
using EcommercePlatform.Orders;
using EcommercePlatform.Shops;
using EcommercePlatform.DynamicPricing;
using EcommercePlatform.AiChat;
using EcommercePlatform.Carts;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EcommercePlatform.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class EcommercePlatformDbContext :
    AbpDbContext<EcommercePlatformDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<DynamicPriceRule> DynamicPriceRules { get; set; }
    public DbSet<AiChatSession> AiChatSessions { get; set; }
    public DbSet<AiChatMessage> AiChatMessages { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public EcommercePlatformDbContext(DbContextOptions<EcommercePlatformDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Product>(b =>
        {
            b.ToTable("Products");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.Property(x => x.SKU).IsRequired().HasMaxLength(64);
            b.Property(x => x.ImageUrl).HasMaxLength(512);
            b.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            b.HasOne(x => x.Shop).WithMany(x => x.Products).HasForeignKey(x => x.ShopId);
        });

        builder.Entity<Category>(b =>
        {
            b.ToTable("Categories");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.Property(x => x.ImageUrl).HasMaxLength(512);
            b.HasOne(x => x.ParentCategory).WithMany(x => x.SubCategories).HasForeignKey(x => x.ParentCategoryId);
        });

        builder.Entity<Order>(b =>
        {
            b.ToTable("Orders");
            b.ConfigureByConvention();
            b.Property(x => x.OrderNumber).IsRequired().HasMaxLength(64);
            b.Property(x => x.ShippingAddress).IsRequired().HasMaxLength(512);
            b.Property(x => x.BillingAddress).IsRequired().HasMaxLength(512);
            b.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(64);
            b.Property(x => x.PaymentStatus).IsRequired().HasMaxLength(32);
            b.Property(x => x.TrackingNumber).HasMaxLength(128);
            b.Property(x => x.Notes).HasMaxLength(2000);
        });

        builder.Entity<OrderItem>(b =>
        {
            b.ToTable( "OrderItems");
            b.ConfigureByConvention();
            b.Property(x => x.ProductName).IsRequired().HasMaxLength(128);
            b.Property(x => x.ProductSku).IsRequired().HasMaxLength(64);
            b.HasOne(x => x.Order).WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId);
            b.HasOne(x => x.Product).WithMany(x => x.OrderItems).HasForeignKey(x => x.ProductId);
        });

        builder.Entity<Shop>(b =>
        {
            b.ToTable("Shops");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.Property(x => x.LogoUrl).HasMaxLength(512);
            b.Property(x => x.Address).IsRequired().HasMaxLength(512);
            b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(32);
            b.Property(x => x.Email).IsRequired().HasMaxLength(128);
            b.Property(x => x.BusinessHours).HasMaxLength(256);
            b.Property(x => x.TaxId).HasMaxLength(64);
            b.Property(x => x.PaymentMethods).HasMaxLength(512);
            b.Property(x => x.ShippingMethods).HasMaxLength(512);
        });

        builder.Entity<DynamicPriceRule>(b =>
        {
            b.ToTable("DynamicPriceRules");
            b.ConfigureByConvention();
            b.Property(x => x.RuleType).IsRequired().HasMaxLength(64);
            b.Property(x => x.Condition).IsRequired().HasMaxLength(512);
            b.Property(x => x.AdjustmentType).IsRequired().HasMaxLength(32);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.HasOne(x => x.Product).WithMany(x => x.DynamicPriceRules).HasForeignKey(x => x.ProductId);
        });

        builder.Entity<AiChatSession>(b =>
        {
            b.ToTable("AiChatSessions");
            b.ConfigureByConvention();
            b.Property(x => x.SessionId).IsRequired().HasMaxLength(64);
            b.Property(x => x.Status).IsRequired().HasMaxLength(32);
            b.Property(x => x.Context).HasMaxLength(2000);
        });

        builder.Entity<AiChatMessage>(b =>
        {
            b.ToTable("AiChatMessages");
            b.ConfigureByConvention();
            b.Property(x => x.Role).IsRequired().HasMaxLength(32);
            b.Property(x => x.Content).IsRequired().HasMaxLength(4000);
            b.Property(x => x.MessageType).IsRequired().HasMaxLength(32);
            b.Property(x => x.Metadata).HasMaxLength(2000);
            b.HasOne(x => x.Session).WithMany(x => x.Messages).HasForeignKey(x => x.SessionId);
        });

        builder.Entity<Cart>(b =>
        {
            b.ToTable("Carts");
            b.ConfigureByConvention();
            b.Property(x => x.UserId).IsRequired();
        });

        builder.Entity<CartItem>(b =>
        {
            b.ToTable("CartItems");
            b.ConfigureByConvention();
            b.Property(x => x.ProductName).IsRequired().HasMaxLength(128);
            b.Property(x => x.ProductSku).IsRequired().HasMaxLength(64);
            b.HasOne(x => x.Cart).WithMany(x => x.Items).HasForeignKey(x => x.CartId);
            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
        });
    }
}
