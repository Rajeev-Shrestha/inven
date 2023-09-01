namespace HrevertCRM.Data.Common
{
    public class PagedDataRequest
    {

        public PagedDataRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PagedDataRequest(int pageNumber, int pageSize, string searchText)
        {
            SearchText = searchText;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PagedDataRequest(int pageNumber, int pageSize, string searchText, bool active, string sortColumn, string sortOrder)
        {
            SearchText = searchText;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Active = active;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
        }
        public int PageNumber { get; private set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public string SortColumn { get; set; }

        public string SortOrder { get; set; }
        public bool IsInit { get; set; }
        public bool Active { get; set; }

    }
}
