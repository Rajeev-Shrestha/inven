using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Purchase;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse =
    HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.PurchaseOrderViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class PurchaseOrderQueryProcessor : QueryBase<PurchaseOrder>, IPurchaseOrderQueryProcessor
    {
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;
        private readonly IFiscalYearQueryProcessor _fiscalYearQueryProcessor;
        private readonly ICompanyQueryProcessor _companyQueryProcessor;

        public PurchaseOrderQueryProcessor(IUserSession userSession, IDbContext dbContext,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor,
            IFiscalYearQueryProcessor fiscalYearQueryProcessor,
            ICompanyQueryProcessor companyQueryProcessor
            ) : base(userSession, dbContext)
        {
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
            _fiscalYearQueryProcessor = fiscalYearQueryProcessor;
            _companyQueryProcessor = companyQueryProcessor;
        }

        public PurchaseOrder Update(PurchaseOrder purchaseOrder)
        {
            var original = GetValidPurchaseOrder(purchaseOrder.Id);
            ValidateAuthorization(purchaseOrder);
            CheckVersionMismatch(purchaseOrder, original);

            original.Id = purchaseOrder.Id;
            original.SalesOrderNumber = purchaseOrder.SalesOrderNumber;
            original.OrderDate = purchaseOrder.OrderDate;
            original.DueDate = purchaseOrder.DueDate;
            original.Status = purchaseOrder.Status;
            original.OrderType = purchaseOrder.OrderType;
            original.FullyPaid = purchaseOrder.FullyPaid;
            original.TotalAmount = purchaseOrder.TotalAmount;
            original.PaidAmount = purchaseOrder.PaidAmount;
            original.FiscalPeriodId = purchaseOrder.FiscalPeriodId;
            original.PaymentTermId = purchaseOrder.PaymentTermId;
            original.DeliveryMethodId = purchaseOrder.DeliveryMethodId;
            original.BillingAddressId = purchaseOrder.BillingAddressId;
            original.ShippingAddressId = purchaseOrder.ShippingAddressId;
            original.PurchaseRepId = purchaseOrder.PurchaseRepId;
            original.VendorId = purchaseOrder.VendorId;
            original.InvoicedDate = purchaseOrder.InvoicedDate;
            original.PaymentDueOn = purchaseOrder.PaymentDueOn;
            original.Active = purchaseOrder.Active;
            original.CompanyId = purchaseOrder.CompanyId;
            original.WebActive = purchaseOrder.WebActive;

            //if (purchaseOrder.PurchaseOrderLines.Count>0)
            //{
            //    foreach (var orderLine in purchaseOrder.PurchaseOrderLines)
            //    {
            //        orderLine.DescriptionType = DescriptionType.Modified;
            //        orderLine.CompanyId = original.CompanyId;
            //    }
                
            //    foreach (var orderLines in purchaseOrder.PurchaseOrderLines)
            //    {
            //        if (orderLines.Id == 0)
            //        {
            //            _dbContext.Set<PurchaseOrderLine>().Add(orderLines);
            //        }
            //        else
            //        {
            //            _dbContext.Set<PurchaseOrderLine>().Update(orderLines);
            //        }
                  
            //    }
            //}
          
            //original.PurchaseOrderLines = purchaseOrder.PurchaseOrderLines;
            _dbContext.Set<PurchaseOrder>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual PurchaseOrder GetValidPurchaseOrder(int purchaseOrderId)
        {
            var purchaseOrder = _dbContext.Set<PurchaseOrder>().FirstOrDefault(sc => sc.Id == purchaseOrderId);
            if (purchaseOrder == null)
                throw new RootObjectNotFoundException(PurchaseOrderConstants.PurchaseOrderQueryProcessorConstants.PurchaseOrderNotFound);
            return purchaseOrder;
        }

        public PurchaseOrder GetPurchaseOrder(int purchaseOrderId)
        {
            var purchaseOrder = _dbContext.Set<PurchaseOrder>().FirstOrDefault(d => d.Id == purchaseOrderId);
            return purchaseOrder;
        }

        public void SaveAllPurchaseOrder(List<PurchaseOrder> purchaseOrders)
        {
            _dbContext.Set<PurchaseOrder>().AddRange(purchaseOrders);
            _dbContext.SaveChanges();
        }

        public PurchaseOrder Save(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.PurchaseRepId = LoggedInUser.Id;
            purchaseOrder.CompanyId = LoggedInUser.CompanyId;

            //foreach (var orderLine in purchaseOrder.PurchaseOrderLines)
            //{
            //    orderLine.CompanyId = LoggedInUser.CompanyId;
            //    orderLine.DescriptionType = DescriptionType.Modified;

            //    _dbContext.Set<PurchaseOrderLine>().Add(orderLine);
            //}

            _dbContext.Set<PurchaseOrder>().Add(purchaseOrder);
            _dbContext.SaveChanges();
            return purchaseOrder;
        }

        public int SaveAll(List<PurchaseOrder> purchaseOrders)
        {
            _dbContext.Set<PurchaseOrder>().AddRange(purchaseOrders);
            return _dbContext.SaveChanges();
        }

        public PurchaseOrder ActivatePurchaseOrder(int id)
        {
            var original = GetValidPurchaseOrder(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<PurchaseOrder>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public PurchaseOrderViewModel GetPurchaseOrderViewModel(int id)
        {
            var purchaseOrder = _dbContext.Set<PurchaseOrder>().Include(x => x.PurchaseOrderLines).Single(s => s.Id == id);
            var mapper = new PurchaseOrderToPurchaseOrderViewModelMapper();
            return mapper.Map(purchaseOrder);
        }

        public PurchaseOrderDefaultValuesViewModel GetDefaultValues(int vendorId)
        {
            var mapper = new AddressToAddressViewModelMapper();
            var custMapper = new VendorToVendorViewModelMapper();

            var vendor= _dbContext.Set<Vendor>().Include(a => a.Addresses).FirstOrDefault(x => x.Id == vendorId);
            var billingAddress = _dbContext.Set<Address>()
                .FirstOrDefault(x => x.VendorId == vendorId && x.IsDefault && x.AddressType == AddressType.Billing);
            var shippingAddress = _dbContext.Set<Address>()
                .FirstOrDefault(x => x.VendorId == vendorId && x.IsDefault && x.AddressType == AddressType.Shipping);
            var paymentTerms = _dbContext.Set<PurchaseOrder>().FirstOrDefault(x => x.VendorId == vendorId);
            var billingAddresses = vendor.Addresses.Where(c => c.AddressType == AddressType.Billing).ToList();
            var shippingAddresses = vendor.Addresses.Where(c => c.AddressType == AddressType.Shipping).ToList();

            var res = new PurchaseOrderDefaultValuesViewModel()
            {
                LoggedInUserId = LoggedInUser.Id,
                BillingAddresses = billingAddress == null? null : mapper.Map(billingAddresses),
                ShippingAddresses = shippingAddresses == null? null : mapper.Map(shippingAddresses),
                BillingAddressId = billingAddress?.Id,
                ShippingAddressId = shippingAddress?.Id,
                PaymentTermsId = paymentTerms?.Id,
                Vendor = custMapper.Map(vendor)
            };
            return res;
        }

        public List<TaskDocIdViewModel> GetPurchaseOrderNumbers()
        {
            var purchaseOrderCodesList = new List<TaskDocIdViewModel>();
            var purchaseOrderNumbers =
                _dbContext.Set<PurchaseOrder>().Where(FilterByActiveTrueAndCompany).Select(x => new { x.Id, x.PurchaseOrderCode});
            foreach (var purchaseOrder in purchaseOrderNumbers)
            {
                purchaseOrderCodesList.Add(new TaskDocIdViewModel()
                {
                    Id = purchaseOrder.Id,
                    Name = purchaseOrder.PurchaseOrderCode
                });
            }
            return purchaseOrderCodesList;
        }

        public DateTime GetDueDate(DueDateViewModel dueDateViewModel)
        {
            var getDaysToBeAdded =
                _dbContext.Set<PaymentTerm>()
                    .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Id == dueDateViewModel.TermId)
                    .Select(x => x.DueDateValue).Single();
            var dueDate = dueDateViewModel.DateTime.Date.AddDays(getDaysToBeAdded);
            return dueDate;
        }

        public PagedTaskDataInquiryResponse SearchPurchaseOrders(PagedDataRequest requestInfo, Expression<Func<PurchaseOrder, bool>> @where = null)
        {
            var query = _dbContext.Set<PurchaseOrder>().Include(x => x.Vendor).ThenInclude(x => x.Addresses).Where(x => x.CompanyId == LoggedInUser.CompanyId 
            && x.Vendor.Active).ToList();

            if (requestInfo.Active)
                query = query.Where(x => x.Active).ToList();

            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query
                : query.Where(s
                        => s.OrderType.ToString().ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           || s.PurchaseOrderCode.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           || s.SalesOrderNumber.ToString().ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           || s.Vendor.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).FirstName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           || s.Vendor.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).LastName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           ).ToList();

            return FormatResultForPaging(requestInfo, result);
        }

        public bool Delete(int purchaseOrderId)
        {
            var doc = GetPurchaseOrder(purchaseOrderId);
            ValidateAuthorization(doc);
            if (doc == null) return false;

            //Remove Purchase Order Lines too
            var purchaseOrderLines = _dbContext.Set<PurchaseOrderLine>()
                .Where(x => x.Active && x.PurchaseOrderId == purchaseOrderId && x.CompanyId == LoggedInUser.CompanyId)
                .ToList();
            if (purchaseOrderLines != null && purchaseOrderLines.Count > 0)
                _dbContext.Set<PurchaseOrderLine>().RemoveRange(purchaseOrderLines);

            _dbContext.Set<PurchaseOrder>().Remove(doc);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Exists(Expression<Func<PurchaseOrder, bool>> where)
        {
            return _dbContext.Set<PurchaseOrder>().Any(where);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, List<PurchaseOrder> query)
        {
            var totalItemCount = query.Count;
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new PurchaseOrderToPurchaseOrderViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(
                        s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<PurchaseOrderViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public PurchaseOrder[] GetPurchaseOrders(Expression<Func<PurchaseOrder, bool>> where = null)
        {
            var query = _dbContext.Set<PurchaseOrder>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public string GeneratePurchaseOrderCode()
        {
            var fiscalYear = _fiscalYearQueryProcessor.GetFiscalYearByCurrentDate();
            var getCompany = _companyQueryProcessor.GetCompany(LoggedInUser.CompanyId);
            var getCompanyWebSetting = _companyWebSettingQueryProcessor.Get(LoggedInUser.CompanyId);
            var serialNo = getCompanyWebSetting.PurchaseOrderCode + 1;
            string purchaseOrderCode;
            switch (getCompany.FiscalYearFormat)
            {
                case FiscalYearFormat.WithPrefix:
                    {
                        purchaseOrderCode = serialNo.ToString("PI-" + fiscalYear.Name.Replace("/", "").Replace("FY-", "") + "-00000000");
                        while (true)
                        {
                            if (CheckIfPurchaseOrderCodeExistsOrNot(purchaseOrderCode))
                            {
                                serialNo = serialNo + 1;
                                purchaseOrderCode = serialNo.ToString("PI-" + fiscalYear.Name.Replace("/", "").Replace("FY-", "") + "-00000000");
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case FiscalYearFormat.WithoutPrefix:
                    {
                        purchaseOrderCode = serialNo.ToString("PI-" + fiscalYear.Name.Replace("/", "") + "-00000000");
                        while (true)
                        {
                            if (CheckIfPurchaseOrderCodeExistsOrNot(purchaseOrderCode))
                            {
                                serialNo = serialNo + 1;
                                purchaseOrderCode = serialNo.ToString("PI-" + fiscalYear.Name.Replace("/", "") + "-00000000");
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case FiscalYearFormat.WithPrefixSingleYear:
                    {
                        purchaseOrderCode = serialNo.ToString("PI-" + fiscalYear.Name.Replace("FY-", "") + "-00000000");
                        while (true)
                        {
                            if (CheckIfPurchaseOrderCodeExistsOrNot(purchaseOrderCode))
                            {
                                serialNo = serialNo + 1;
                                purchaseOrderCode = serialNo.ToString("PI-" + fiscalYear.Name.Replace("FY-", "") + "-00000000");
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateCompanyWebSetting(getCompanyWebSetting, serialNo);
            return purchaseOrderCode;
        }

        private void UpdateCompanyWebSetting(CompanyWebSetting getCompanyWebSetting, int serialNo)
        {
            var companyWebSetting = new CompanyWebSetting
            {
                CompanyId = LoggedInUser.CompanyId,
                Id = getCompanyWebSetting.Id,
                AllowGuest = true,
                CustomerSerialNo = getCompanyWebSetting.CustomerSerialNo,
                VendorSerialNo = getCompanyWebSetting.VendorSerialNo,
                PurchaseOrderCode = serialNo,
                SalesOrderCode = getCompanyWebSetting.SalesOrderCode,
                SalesRepId = LoggedInUser.Id,
                ShippingCalculationType = getCompanyWebSetting.ShippingCalculationType,
                DiscountCalculationType = getCompanyWebSetting.DiscountCalculationType,
                Active = getCompanyWebSetting.Active,
                WebActive = getCompanyWebSetting.WebActive,
                Version = getCompanyWebSetting.Version,
                IsEstoreInitialized = getCompanyWebSetting.IsEstoreInitialized,
                DeliveryMethodId = getCompanyWebSetting.DeliveryMethodId
            };
            _companyWebSettingQueryProcessor.Update(companyWebSetting);
        }

        public bool CheckIfPurchaseOrderCodeExistsOrNot(string purchaseOrderCode)
        {
            return _dbContext.Set<PurchaseOrder>().Any(x => x.PurchaseOrderCode == purchaseOrderCode);
        }

        public bool DeleteRange(List<int?> purchaseTermsId)
        {
            var purchaseTermList = purchaseTermsId.Select(purchaseTermId => _dbContext.Set<PurchaseOrder>().FirstOrDefault(x => x.Id == purchaseTermId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            purchaseTermList.ForEach(x => x.Active = false);
            _dbContext.Set<PurchaseOrder>().UpdateRange(purchaseTermList);
            _dbContext.SaveChanges();
            return true;
        }

        //public IQueryable<PurchaseOrder> GetActivePurchaseOrders()
        //{
        //    return _dbContext.Set<PurchaseOrder>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        //}

        //public IQueryable<PurchaseOrder> GetDeletedPurchaseOrders()
        //{
        //    return _dbContext.Set<PurchaseOrder>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        //}
        //public IQueryable<PurchaseOrder> GetAllPurchaseOrders()
        //{
        //    return _dbContext.Set<PurchaseOrder>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
        //}
    }
}
