namespace HrevertCRM.Data.Common
{
    public class Constants
    {
        public static class Paging
        {
            public const int MinPageSize = 0;
            public const int MinPageNumber = 1;
            public const int DefaultPageNumber = 1;
            public const int DefaultPageSize = 10;
        }
        public static class CommonParameterNames
        {
            public const string PageNumber = "pageNumber";
            public const string PageSize = "pageSize";
            public const string SearchText = "text";
            public const string CompanyActive = "active";
            public const string SortColumn = "sortColumn";
            public const string SortOrder = "sortOrder";
        }
    }
}
