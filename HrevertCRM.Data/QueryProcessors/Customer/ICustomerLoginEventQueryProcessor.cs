using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICustomerLoginEventQueryProcessor
    {
        CustomerLoginResultViewModel CheckLogin(string email, string password);
    }
}
