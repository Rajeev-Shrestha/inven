namespace HrevertCRM.Common
{
    public enum UserType
    {
        SuperAdmin=1, //SAS Admin, only this user can create Companies and other SuperAdmin
        CompanyAdmin=2, //Company admin, this user manages everything for a company
        CompanyUsers = 3, //these are company users with specific rights
        WebUser = 4, // these are customers of a company
    }
}
