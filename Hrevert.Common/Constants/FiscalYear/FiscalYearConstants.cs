namespace Hrevert.Common.Constants.FiscalYear
{
    public class FiscalYearConstants
    {
        public static class FiscalYearControllerConstants
        {
            public static string FiscalYearNameExists = "Fiscal year with the same name already exists";
            public static string FiscalYearDatesExists = "Fiscal year with the same dates already exists";
            public static string StartDateMustBeLessThanEndDate = "Start date of the fiscal year must be less than the End Date";
        }

        public static class FiscalYearQueryProcessorConstants
        {
            public static string FiscalYearNotFound = "Fiscal Year Not Found";
        }
        public static class FiscalPeriodControllerConstants
        {
            public static string FiscalPeriodNameExists = "Fiscal Period with the same name already exists";
            public static string FiscalPeriodDatesExists = "Fiscal Period with the same dates already exists";
            public static string StartDateMustBeLessThanEndDate = "Start date of the fiscal Period must be less than the End Date";

            public static string FiscalPeriodDoesNotFallInFiscalYearRegion =
                "The Fiscal Period does not fall in the region of Fiscal Year! Check the Fiscal Period again";
        }

        public static class FiscalPeriodQueryProcessorConstants
        {
            public static string FiscalPeriodNotFound = "Fiscal Period Not Found";
        }
    }
}
