namespace Hrevert.Common.Constants
{
    public class DiscountConstants
    {
        public static class DiscountControllerConstants
        {
            public const string DiscountAlreadyExists = "Discount with the same code already exists";
            public const string AllowGuestDisabled = "Discount allow guest is disabled";
            public const string DiscountAlreadyExistsWithThisEmail = "Discount with the same email already exists";
        }

        public static class DiscountQueryProcessorConstants
        {
            public const string DiscountNotFound = "Discount not found";
        }
    }
}
