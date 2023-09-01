using System.Collections.Generic;

namespace HrevertCRM.Data.Common
{
    public class PagedDataInquiryResponse<T>
    {
        private List<T> _items;
       public List<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
}
