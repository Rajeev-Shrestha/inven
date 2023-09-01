using System;
using System.Linq;
using Hrevert.Common;
using Hrevert.Common.Extensions;

namespace HrevertCRM.Data.Common
{
    public class PagedDataRequestFactory : IPagedDataRequestFactory
    {
        public const int DefaultPageSize = Constants.Paging.DefaultPageSize;
        public const int MaxPageSize = 1000;

        public PagedDataRequest Create(Uri requestUri)
        {
            int? pageNumber;
            int? pageSize;
            try
            {
                var valueCollection = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(requestUri.Query);

                pageNumber = PrimitiveTypeParser.Parse<int?>(valueCollection[Constants.CommonParameterNames.PageNumber]);
                pageSize = PrimitiveTypeParser.Parse<int?>(valueCollection[Constants.CommonParameterNames.PageSize]);
            }
            catch (Exception e)
            {
                //TODO:log error, Error parsing input with e
                throw new Exception(e.Message); //ParseHTTPException?
            }
            pageNumber = pageNumber.GetBoundedValue(Constants.Paging.DefaultPageNumber,Constants.Paging.MinPageNumber);
            pageSize = pageSize.GetBoundedValue(DefaultPageSize,
            Constants.Paging.MinPageSize, MaxPageSize);
            return new PagedDataRequest(pageNumber.Value, pageSize.Value);
        }
        public PagedDataRequest CreateWithSearchStringCompanyActive(Uri requestUri)
        {
            var pdr = Create(requestUri);
            string searchText;
            string sortColumn = null;
            string sortOrder=null;
            bool active;
            try
            {
                var valueCollection = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(requestUri.Query);

                searchText = valueCollection[Constants.CommonParameterNames.SearchText].ToString();
                active =bool.Parse(valueCollection[Constants.CommonParameterNames.CompanyActive].ToString());
                if (valueCollection.Keys.Any(k => k == Constants.CommonParameterNames.SortColumn))
                {
                    sortColumn = valueCollection[Constants.CommonParameterNames.SortColumn].ToString();
                    if (valueCollection.Keys.Any(k => k == Constants.CommonParameterNames.SortOrder))
                        sortOrder = valueCollection[Constants.CommonParameterNames.SortOrder].ToString();
                    else
                    {
                        sortOrder = "ASC";
                    }
                }
               
                

            }
            catch (Exception e)
            {
                //TODO:log error,Error parsing input with e
                throw new Exception(e.Message); //ParseHTTPExceptin?
            }
            return new PagedDataRequest(pdr.PageNumber, pdr.PageSize, searchText,active,sortColumn,sortOrder);
        }

        public PagedDataRequest CreateWithSearchString(Uri requestUri)
       {
            var pdr = Create(requestUri);
            string searchText;
            try
            {
                var valueCollection = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(requestUri.Query);

                searchText = valueCollection[Constants.CommonParameterNames.SearchText].ToString();
                
            }
            catch (Exception e)
            {
                //TODO:log error,Error parsing input with e
                throw new Exception(e.Message); //ParseHTTPExceptin?
            }
            return new PagedDataRequest(pdr.PageNumber, pdr.PageSize, searchText);
        }
    }

}
