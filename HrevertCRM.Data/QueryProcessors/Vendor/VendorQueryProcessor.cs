using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Vendor;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.EditVendorViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class VendorQueryProcessor : QueryBase<Vendor>, IVendorQueryProcessor
    {
        private readonly IAddressQueryProcessor _addressQueryProcessor;
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;

        public VendorQueryProcessor(IUserSession userSession, 
            IDbContext dbContext, 
            IAddressQueryProcessor addressQueryProcessor,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor) : base(userSession, dbContext)
        {
            _addressQueryProcessor = addressQueryProcessor;
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
        }
        public Vendor Update(Vendor vendor)
        {
            var original = GetValidVendor(vendor.Id);
            ValidateAuthorization(vendor);
            CheckVersionMismatch(vendor, original);
            original.Code = vendor.Code;
            original.CreditLimit = vendor.CreditLimit;
            original.Debit = vendor.Debit;
            original.Credit = vendor.Credit;
            original.ContactName = vendor.ContactName;
            original.PaymentTermId = vendor.PaymentTermId;
            original.PaymentMethodId = vendor.PaymentMethodId;
            original.Active = vendor.Active;
            original.WebActive = vendor.WebActive;
            original.CompanyId = vendor.CompanyId;
            _addressQueryProcessor.Update(vendor.BillingAddress);

            if (vendor.Addresses.Count>0)
            {
                foreach (var address in vendor.Addresses) {
                    if (address.Id == 0)
                    {
                        address.VendorId = vendor.Id;
                        address.CompanyId = LoggedInUser.CompanyId;
                        address.Active = true;
                        _addressQueryProcessor.Save(address);
                    }
                 
                     else
                     {
                        _addressQueryProcessor.Update(address);
                    }
                }
                
            }
           
            
            _dbContext.Set<Vendor>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual Vendor GetValidVendor(int vendorId)
        {
            var vendor = _dbContext.Set<Vendor>().FirstOrDefault(sc => sc.Id == vendorId);
            if (vendor == null)
            {
                throw new RootObjectNotFoundException(VendorConstants.VendorQueryProcessorConstants.VendorNotFound);
            }
            return vendor;
        }

        public EditVendorViewModel GetVendorViewModel(int vendorId)
        {
            var addresses =
                _dbContext.Set<Address>()
                    .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.VendorId == vendorId && x.Active && x.AddressType != AddressType.Billing).ToList();
            var vendor = _dbContext.Set<Vendor>().FirstOrDefault(d => d.CompanyId == LoggedInUser.CompanyId && d.Id == vendorId);
            var billingAddress = _dbContext.Set<Address>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.VendorId == vendorId && x.Active).FirstOrDefault(d => d.AddressType == AddressType.Billing);

            if (billingAddress != null && addresses != null)
            {
                vendor.BillingAddress = billingAddress;
                vendor.Addresses = addresses;
            }

            var mapper = new VendorToEditVendorViewModelMapper();
            return mapper.Map(vendor);
        }
        public Vendor GetVendor(int vendorId)
        {
            var vendor = _dbContext.Set<Vendor>().FirstOrDefault(d => d.Id == vendorId);
            return vendor;
        }
        public void SaveAllVendor(List<Vendor> vendors)
        {
            _dbContext.Set<Vendor>().AddRange(vendors);
            _dbContext.SaveChanges();
        }
        
        public Vendor Save(Vendor vendor)
        {
            vendor.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Vendor>().Add(vendor);
            _dbContext.SaveChanges();
            return vendor;
        }

        public int SaveAll(List<Vendor> vendors)
        {
            _dbContext.Set<Vendor>().AddRange(vendors);
            return _dbContext.SaveChanges();
        }

        public List<Vendor> SearchForVendors(string searchString)
        {
            var vendors = new List<Vendor>();
            if (!string.IsNullOrEmpty(searchString))
            {
                vendors =
                    _dbContext.Set<Vendor>().Where(s => s.Code.ToUpper().Contains(searchString.ToUpper())
                                                          || s.ContactName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            vendors = vendors.Where(c => c.CompanyId == LoggedInUser.CompanyId).ToList();
            return vendors;
        }

        public Vendor ActivateVendor(int id)
        {
            var original = GetValidVendor(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<Vendor>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public Address SaveBillingAddress(Address address)
        {
            address.CompanyId = LoggedInUser.CompanyId;
            return _addressQueryProcessor.Save(address);
        }

        public bool Delete(int vendorId)
        {
            var doc = GetVendor(vendorId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Vendor>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Vendor, bool>> @where)
        {
            return _dbContext.Set<Vendor>().Any(@where);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<Vendor> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new VendorToEditVendorViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<EditVendorViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public Vendor[] GetVendors(Expression<Func<Vendor, bool>> @where = null)
        {

            var query = _dbContext.Set<Vendor>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        private PagedTaskDataInquiryResponse FormatResultForSearchPaging(PagedDataRequest requestInfo, IQueryable<Vendor> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new VendorToEditVendorViewModelMapper();
            var docs =
                query.Where(s => s.CompanyId == LoggedInUser.CompanyId)
                    .OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(
                        s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<EditVendorViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public string GenerateVendorCode()
        {
            var getCompanyWebSetting = _companyWebSettingQueryProcessor.Get(LoggedInUser.CompanyId);
            var serialNo = getCompanyWebSetting.VendorSerialNo + 1;

            var vendorCode = serialNo.ToString("00000000");
            while (true)
            {
                if (CheckIfVendorCodeExistsOrNot(vendorCode))
                {
                    serialNo = serialNo + 1;
                    vendorCode = serialNo.ToString("00000000");
                }
                else
                {
                    break;
                }
            }
            UpdateCompanyWebSetting(getCompanyWebSetting, serialNo);
            return "V-" + vendorCode;
        }

        private void UpdateCompanyWebSetting(CompanyWebSetting getCompanyWebSetting, int serialNo)
        {
            var companyWebSetting = new CompanyWebSetting
            {
                CompanyId = LoggedInUser.CompanyId,
                Id = getCompanyWebSetting.Id,
                AllowGuest = true,
                CustomerSerialNo = getCompanyWebSetting.CustomerSerialNo,
                VendorSerialNo = serialNo,
                SalesRepId = LoggedInUser.Id,
                ShippingCalculationType = getCompanyWebSetting.ShippingCalculationType,
                DiscountCalculationType = getCompanyWebSetting.DiscountCalculationType,
                Active = getCompanyWebSetting.Active,
                WebActive = getCompanyWebSetting.WebActive,
                Version = getCompanyWebSetting.Version
            };
            _companyWebSettingQueryProcessor.Update(companyWebSetting);
        }

        public bool CheckIfVendorCodeExistsOrNot(string vendorCode)
        {
            var getVendorCodes = _dbContext.Set<Vendor>().Where(x => x.CompanyId == LoggedInUser.CompanyId).Select(x => x.Code).ToList();
            var value = getVendorCodes.Contains(vendorCode);
            return value;
        }

        //public IQueryable<Vendor> GetActiveVendors()
        //{
        //    return _dbContext.Set<Vendor>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        //}

        //public IQueryable<Vendor> GetDeletedVendors()
        //{
        //    return _dbContext.Set<Vendor>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        //}
        //public IQueryable<Vendor> GetAllVendors()
        //{
        //    return _dbContext.Set<Vendor>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
        //}


       public IQueryable<Vendor> GetActiveVendorsWithoutPaging()
        {
            var billingAddress = _dbContext.Set<Address>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.AddressType == AddressType.Billing).ToList();

            var vendors = _dbContext.Set<Vendor>().Where(FilterByActiveTrueAndCompany);

            foreach (var item in vendors)
            {
                item.BillingAddress = billingAddress.FirstOrDefault(x => x.VendorId == item.Id);
            }

            return vendors;
        }


        public IQueryable<Address> GetVendorAllAddresses(int vendorId)
        {
            var addresses = _dbContext.Set<Address>()
                .Where(x => x.VendorId == vendorId && x.CompanyId == LoggedInUser.CompanyId && x.AddressType != AddressType.Billing);
            return addresses;
        }

        public List<TaskDocIdViewModel> GetVendorNames()
        {
            var vendorNamesList = new List<TaskDocIdViewModel>();
            var vendorNames = _dbContext.Set<Address>()
                .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.AddressType != AddressType.Billing && x.VendorId != null && x.Active).Select(x => new { x.Id, x.FirstName, x.MiddleName, x.LastName});
            foreach (var vendorName in vendorNames)
            {
                var name = vendorName.MiddleName == null
                    ? vendorName.FirstName + " " + vendorName.LastName
                    : vendorName.FirstName + " " + vendorName.MiddleName + " " + vendorName.LastName;
                vendorNamesList.Add(new TaskDocIdViewModel
                {
                    Id = vendorName.Id,
                    Name = name
                });
            }
            return vendorNamesList;
        }

        public PagedTaskDataInquiryResponse GetVendors(PagedDataRequest requestInfo, Expression<Func<Vendor, bool>> @where = null)
        {
            var vendors = _dbContext.Set<Vendor>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            var billingAddresses = _dbContext.Set<Address>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.AddressType == AddressType.Billing).ToList();
            foreach (var item in vendors)
            {
                item.BillingAddress = billingAddresses.FirstOrDefault(x => x.VendorId == item.Id);
            }
            if (requestInfo.Active)
                vendors = vendors.Where(x => x.Active);

            var query = string.IsNullOrEmpty(requestInfo.SearchText) || requestInfo.SearchText.Contains("null") ? vendors : vendors.Where(s
                                                                 => s.Code.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                 || s.ContactName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                 || s.Addresses.Single(x => x.AddressType == AddressType.Billing).Email.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, query);

        }

        public Vendor CheckIfDeletedVendorWithSameCodeExists(string code)
        {
            var vendor =
                _dbContext.Set<Vendor>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.Code == code && (x.Active ||
                             x.Active == false));
            return vendor;
        }

        public bool DeleteRange(List<int?> vendorsId)
        {
            var vendorList = vendorsId.Select(vendorId => _dbContext.Set<Vendor>().FirstOrDefault(x => x.Id == vendorId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            vendorList.ForEach(x => x.Active = false);
            _dbContext.Set<Vendor>().UpdateRange(vendorList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
