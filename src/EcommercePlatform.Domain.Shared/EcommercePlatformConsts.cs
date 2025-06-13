namespace EcommercePlatform
{
    /// <summary>
    /// Contains constants used throughout the EcommercePlatform application.
    /// </summary>
    public static class EcommercePlatformConsts
    {
        /// <summary>
        /// The name of the application.
        /// </summary>
        public const string ApplicationName = "EcommercePlatform";

        /// <summary>
        /// The database table prefix for all tables in the application.
        /// </summary>
        public const string DbTablePrefix = "Ecp";

        /// <summary>
        /// The database schema for all tables in the application.
        /// Can be null for the default schema.
        /// </summary>
        public const string DbSchema = null;

        /// <summary>
        /// The maximum length for shop names.
        /// </summary>
        public const int MaxShopNameLength = 100;

        /// <summary>
        /// The maximum length for shop descriptions.
        /// </summary>
        public const int MaxShopDescriptionLength = 1000;

        /// <summary>
        /// The maximum length for product names.
        /// </summary>
        public const int MaxProductNameLength = 200;

        /// <summary>
        /// The maximum length for product descriptions.
        /// </summary>
        public const int MaxProductDescriptionLength = 2000;

        /// <summary>
        /// The maximum length for category names.
        /// </summary>
        public const int MaxCategoryNameLength = 100;

        /// <summary>
        /// The maximum length for category descriptions.
        /// </summary>
        public const int MaxCategoryDescriptionLength = 500;

        /// <summary>
        /// The maximum length for order numbers.
        /// </summary>
        public const int MaxOrderNumberLength = 50;

        /// <summary>
        /// The maximum length for AI chat messages.
        /// </summary>
        public const int MaxAiChatMessageLength = 2000;

        /// <summary>
        /// The maximum length for dynamic price rule names.
        /// </summary>
        public const int MaxDynamicPriceRuleNameLength = 100;

        /// <summary>
        /// The maximum length for dynamic price rule descriptions.
        /// </summary>
        public const int MaxDynamicPriceRuleDescriptionLength = 500;

        /// <summary>
        /// The maximum length for dynamic price rule parameters.
        /// </summary>
        public const int MaxDynamicPriceRuleParametersLength = 2000;
    }
}
