using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IVendorQueryProcessor
    {
        Vendor Update(Vendor vendor);
        EditVendorViewModel GetVendorViewModel(int vendorId);
        int SaveAll(List<Vendor> vendors);
        bool Delete(int vendorId);
        bool Exists(Expression<Func<Vendor, bool>> @where);
        Vendor[] GetVendors(Expression<Func<Vendor, bool>> @where = null);
        Vendor Save(Vendor vendor);
        //List<Vendor> SearchForVendors(string searchString);
        //PagedDataInquiryResponse<VendorViewModel> SearchForVendors(PagedDataRequest requestInfo, Expression<Func<Vendor, bool>> @where = null);
        Vendor ActivateVendor(int id);
        Address SaveBillingAddress(Address address);
        string GenerateVendorCode();
        bool CheckIfVendorCodeExistsOrNot(string vendorCode);
        IQueryable<Vendor> GetActiveVendorsWithoutPaging();
        IQueryable<Address> GetVendorAllAddresses(int vendorId);
        Vendor CheckIfDeletedVendorWithSameCodeExists(string code);

        List<TaskDocIdViewModel> GetVendorNames();
        PagedDataInquiryResponse<EditVendorViewModel> GetVendors(PagedDataRequest requestInfo, Expression<Func<Vendor, bool>> @where = null);
        bool DeleteRange(List<int?> vendorsId);
    }
}