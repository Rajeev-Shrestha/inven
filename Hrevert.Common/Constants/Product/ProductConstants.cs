namespace Hrevert.Common.Constants.Product
{
    public class ProductConstants
    {
        public static class ProductControllerConstants
        {
            public static string ProductNameExists = "Product with the same name already exists";
            public static string ProductCodeExists = "Product with the same code already exists";
            public static string DeActivatedProductWithSameCodeExists = "Product with the same code exists but is deactivated";
            public static string DeActivatedProductWithSameNameExists = "Product with the same name exists but is deactivated";
        }
        public static class ProductCategoryControllerConstants
        {
            public static string ProductCategoryExists = "Product category with the same name already exists";
        }
        public static class ProductMetadataControllerConstants
        {
            public static string ProductMetadataExists = "Product metadata with the same URL already exists";
        }

        public static class ProductQueryProcessorConstants
        {
            public static string ProductNotFound = "Product not found";
        }
        public static class ProductCategoryQueryProcessorConstants
        {
            public static string ProductCategoryNotFound = "Product category not found";
        }
        public static class ProductMetadataQueryProcessorConstants
        {
            public static string ProductMetadataNotFound = "Product metadata not found";
        }
        public static class ProductPriceRuleQueryProcessorConstants
        {
            public static string ProductPriceRuleNotFound = "Product Price Rule not found";
        }
        public static class ProductPriceRuleControllerConstants
        {
            public static string ProductPriceRuleExists = "Product Price Rule already exists";
        }
    }
}
