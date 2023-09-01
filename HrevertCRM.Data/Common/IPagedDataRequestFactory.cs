using System;

namespace HrevertCRM.Data.Common
{
    public interface IPagedDataRequestFactory
    {
        PagedDataRequest Create(Uri requestUri);
        PagedDataRequest CreateWithSearchString(Uri requestUri);

        PagedDataRequest CreateWithSearchStringCompanyActive(Uri requestUri);
    }
}
