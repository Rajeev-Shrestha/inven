using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.SalesOrder;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.SalesOrderViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class SalesOrderQueryProcessor : QueryBase<SalesOrder>, ISalesOrderQueryProcessor
    {
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;
        private readonly IFiscalYearQueryProcessor _fiscalYearQueryProcessor;
        private readonly ICompanyQueryProcessor _companyQueryProcessor;
        private readonly ISalesOrderLineQueryProcessor _salesOrderLineQueryProcessor;

        public SalesOrderQueryProcessor(IUserSession userSession, IDbContext dbContext,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor,
            IFiscalYearQueryProcessor fiscalYearQueryProcessor,
            ICompanyQueryProcessor companyQueryProcessor,
            ISalesOrderLineQueryProcessor salesOrderLineQueryProcessor
            ) : base(userSession, dbContext)
        {
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
            _fiscalYearQueryProcessor = fiscalYearQueryProcessor;
            _companyQueryProcessor = companyQueryProcessor;
            _salesOrderLineQueryProcessor = salesOrderLineQueryProcessor;
        }

        public SalesOrder Update(SalesOrder salesOrder)
        {
            var original = GetValidSalesOrder(salesOrder.Id);
            ValidateAuthorization(salesOrder);
            CheckVersionMismatch(salesOrder, original);

            original.PurchaseOrderNumber = salesOrder.PurchaseOrderNumber;
            original.DueDate = salesOrder.DueDate;
            original.Status = salesOrder.Status;
            original.SalesPolicy = salesOrder.SalesPolicy;
            original.IsWebOrder = salesOrder.IsWebOrder;
            original.FullyPaid = salesOrder.FullyPaid;
            original.TotalAmount = salesOrder.TotalAmount;
            original.PaidAmount = salesOrder.PaidAmount;
            original.BillingAddressId = salesOrder.BillingAddressId;
            original.ShippingAddressId = salesOrder.ShippingAddressId;
            original.PaymentTermId = salesOrder.PaymentTermId;
            original.FiscalPeriodId = salesOrder.FiscalPeriodId;
            original.OrderType = salesOrder.OrderType;
            original.DeliveryMethodId = salesOrder.DeliveryMethodId;
            original.SalesRepId = salesOrder.SalesRepId;
            original.CustomerId = salesOrder.CustomerId;
            original.CompanyId = salesOrder.CompanyId;
            original.Active = salesOrder.Active;
            original.WebActive = salesOrder.WebActive;

            _dbContext.Set<SalesOrder>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual SalesOrder GetValidSalesOrder(int salesOrderId)
        {
            var salesOrder = _dbContext.Set<SalesOrder>().FirstOrDefault(sc => sc.Id == salesOrderId);
            if (salesOrder == null)
                throw new RootObjectNotFoundException(SalesOrderConstants.SalesOrderQueryProcessorConstants.SalesOrderNotFound);
            return salesOrder;
        }

        public SalesOrder GetSalesOrder(int salesOrderId)
        {
            var salesOrder = _dbContext.Set<SalesOrder>().FirstOrDefault(d => d.Id == salesOrderId);
            return salesOrder;
        }

        public void SaveAllSalesOrder(List<SalesOrder> salesOrders)
        {
            _dbContext.Set<SalesOrder>().AddRange(salesOrders);
            _dbContext.SaveChanges();
        }

        public SalesOrder Save(SalesOrder salesOrder)
        {
            salesOrder.SalesRepId = LoggedInUser.Id;
            salesOrder.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SalesOrder>().Add(salesOrder);
            _dbContext.SaveChanges();

            return salesOrder;
        }

        public int SaveAll(List<SalesOrder> salesOrders)
        {
            _dbContext.Set<SalesOrder>().AddRange(salesOrders);
            return _dbContext.SaveChanges();
        }

        public SalesOrder ActivateSalesOrder(int id)
        {
            var original = GetValidSalesOrder(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<SalesOrder>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public SalesOrderViewModel GetSalesOrderViewModel(int id)
        {
            var salesOrder = _dbContext.Set<SalesOrder>().Include(s => s.SalesOrderLines).Single(s => s.Id == id);
            var mapper = new SalesOrderToSalesOrderViewModelMapper();
            return mapper.Map(salesOrder);
        }

        public SalesOrderDefaultValuesViewModel GetDefaultValues(int customerId)
        {
            var mapper = new AddressToAddressViewModelMapper();
            var custMapper = new CustomerToCustomerViewModelMapper();

            var customer = _dbContext.Set<Customer>().Include(a=>a.Addresses).FirstOrDefault(x => x.Id == customerId);
            var billingAddress = _dbContext.Set<Address>().FirstOrDefault(x => x.CustomerId == customerId && x.IsDefault && x.AddressType==AddressType.Billing);
            var shippingAddress = _dbContext.Set<Address>().FirstOrDefault(x => x.CustomerId == customerId && x.AddressType==AddressType.Shipping);
            var paymentTerms = _dbContext.Set<SalesOrder>().FirstOrDefault(x => x.CustomerId == customerId);
            var billingAddresses = customer.Addresses.Where(c => c.AddressType == AddressType.Billing).ToList();
            var shippingAddresses = customer.Addresses.Where(c => c.AddressType != AddressType.Billing).ToList();

            customer.BillingAddress = null;
            customer.Addresses = null;
            customer.SalesOrders = null;

            var res = new SalesOrderDefaultValuesViewModel
            {
                LoggedInUserId = LoggedInUser.Id,
                BillingAddresses = billingAddresses == null ? null : mapper.Map(billingAddresses),
                ShippingAddresses = shippingAddresses == null ? null : mapper.Map(shippingAddresses),
                BillingAddressId = billingAddress?.Id,
                ShippingAddressId = shippingAddress?.Id,
                PaymentTermsId = paymentTerms?.Id,
                Customer = custMapper.Map(customer)
            };

            return res;
        }

        public DateTime GetDueDate(DateTime? date, int termId)
        {
            var dueDateValue = _dbContext.Set<PaymentTerm>().SingleOrDefault(x => x.Id == termId).DueDateValue;
            date = date?.AddDays(dueDateValue) ?? DateTime.Now.AddDays(dueDateValue);
            return (DateTime) date;
        }

        public List<TaskDocIdViewModel> GetSalesOrderNumbers()
        {
            var salesOrderCodesList = new List<TaskDocIdViewModel>();
            var salesOrderNumbers =
                _dbContext.Set<SalesOrder>().Where(FilterByActiveTrueAndCompany).Select(x => new {x.Id, x.SalesOrderCode});
            foreach (var salesOrderNumber in salesOrderNumbers)
            {
                salesOrderCodesList.Add(new TaskDocIdViewModel()
                {
                    Id = salesOrderNumber.Id,
                    Name = salesOrderNumber.SalesOrderCode
                });
            }
            return salesOrderCodesList;
        }

        public PagedTaskDataInquiryResponse SearchSalesOrders(PagedDataRequest requestInfo, Expression<Func<SalesOrder, bool>> @where = null)
        {
            var query = _dbContext.Set<SalesOrder>()
                .Include(x => x.SalesOrderLines)
                .Include(x => x.Customer).Where(c => c.CompanyId == LoggedInUser.CompanyId && c.Customer.Active).ToList();
            if (requestInfo.Active)
                query = query.Where(x => x.Active).ToList();
            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query 
                : query.Where(s
                        => s.OrderType.ToString().ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           || s.SalesOrderCode.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           || s.Customer.DisplayName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                           || s.PurchaseOrderNumber.ToString().Contains(requestInfo.SearchText)).ToList();
            return FormatResultForPaging(requestInfo, result);
        }

        public SalesOrderType GetOrderType(int? id)
        {
            var salesOrder = _dbContext.Set<SalesOrder>()
                .FirstOrDefault(x => x.Id == id && x.CompanyId == LoggedInUser.CompanyId && x.Active);
            return salesOrder.OrderType;
        }

        public bool Delete(int salesOrderId)
        {
            var doc = GetSalesOrder(salesOrderId);
            ValidateAuthorization(doc);
            if (doc == null) return false;
            #region Delete Sales Order Lines, and maintain Stock if OrderType is Order

            // If the Sales Order is deleted, we need to delete the SalesOrderLines as well
            // and if the  OrderType is Order then we need to maintain the Stock and QOO too
            var salesOrderLines = _dbContext.Set<SalesOrderLine>()
                .Where(x => x.Active && x.SalesOrderId == salesOrderId && x.CompanyId == LoggedInUser.CompanyId).ToList();
            if (salesOrderLines != null && salesOrderLines.Count > 0)
            {
                if (doc.OrderType == SalesOrderType.Order)
                {
                    foreach (var salesOrderLine in salesOrderLines)
                    {
                        _salesOrderLineQueryProcessor.Delete(salesOrderLine.Id);
                    }
                }
                else
                {
                    _dbContext.Set<SalesOrderLine>().RemoveRange(salesOrderLines);
                    _dbContext.SaveChanges();
                }
            }
            #endregion
            
            _dbContext.Set<SalesOrder>().Remove(doc);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Exists(Expression<Func<SalesOrder, bool>> where)
        {
            return _dbContext.Set<SalesOrder>().Any(where);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, List<SalesOrder> query)
        {
            var totalItemCount = query.Count;
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new SalesOrderToSalesOrderViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(s => mapper.Map(s))
                    .ToList();

            #region Determine Shipping Status of Sales Orders

            foreach (var salesOrderViewModel in docs)
            {
                if (salesOrderViewModel.SalesOrderLines == null || salesOrderViewModel.SalesOrderLines.Count <= 0) continue;
                if (salesOrderViewModel.SalesOrderLines.All(x => x.Shipped == false))
                {
                    salesOrderViewModel.ShippingStatus = ShippingStatus.Pending;
                }
                else if (salesOrderViewModel.SalesOrderLines.All(x => x.Shipped))
                {
                    salesOrderViewModel.ShippingStatus = ShippingStatus.Shipped;
                }
                else
                {
                    salesOrderViewModel.ShippingStatus = ShippingStatus.PartiallyShipped;
                }
            }


            #endregion

            var queryResult = new QueryResult<SalesOrderViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public SalesOrder[] GetSalesOrders(Expression<Func<SalesOrder, bool>> where = null)
        {
            var query = _dbContext.Set<SalesOrder>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public string GenerateSalesOrderCode()
        {
            var fiscalYear = _fiscalYearQueryProcessor.GetFiscalYearByCurrentDate();
            var getCompany = _companyQueryProcessor.GetCompany(LoggedInUser.CompanyId);
            var getCompanyWebSetting = _companyWebSettingQueryProcessor.Get(LoggedInUser.CompanyId);
            var serialNo = getCompanyWebSetting.SalesOrderCode + 1;
            var salesOrderCode = "";
            switch (getCompany.FiscalYearFormat)
            {
                case FiscalYearFormat.WithPrefix:
                    {
                        salesOrderCode = serialNo.ToString("SI-" + fiscalYear.Name.Replace("/", "").Replace("FY-", "") + "-00000000");
                        while (true)
                        {
                            if (CheckIfSalesOrderCodeExistsOrNot(salesOrderCode))
                            {
                                serialNo = serialNo + 1;
                                salesOrderCode = serialNo.ToString("SI-" + fiscalYear.Name.Replace("/", "").Replace("FY-", "") + "-00000000");
                            }
                            break;
                        }
                    }
                    break;
                case FiscalYearFormat.WithoutPrefix:
                    {
                        salesOrderCode = serialNo.ToString("SI-" + fiscalYear.Name.Replace("/", "") + "-00000000");
                        while (true)
                        {
                            if (CheckIfSalesOrderCodeExistsOrNot(salesOrderCode))
                            {
                                serialNo = serialNo + 1;
                                salesOrderCode = serialNo.ToString("SI-" + fiscalYear.Name.Replace("/", "") + "-00000000");
                            }
                            break;
                        }
                    }
                    break;
                case FiscalYearFormat.WithPrefixSingleYear:
                    {
                        salesOrderCode = serialNo.ToString("SI-" + fiscalYear.Name.Replace("FY-", "") + "-00000000");
                        while (true)
                        {
                            if (CheckIfSalesOrderCodeExistsOrNot(salesOrderCode))
                            {
                                serialNo = serialNo + 1;
                                salesOrderCode = serialNo.ToString("SI-" + fiscalYear.Name.Replace("FY-", "") + "-00000000");
                            }
                            break;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateCompanyWebSetting(getCompanyWebSetting, serialNo);
            return salesOrderCode;
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
                SalesOrderCode = serialNo,
                PurchaseOrderCode = getCompanyWebSetting.PurchaseOrderCode,
                SalesRepId = LoggedInUser.Id,
                ShippingCalculationType = getCompanyWebSetting.ShippingCalculationType,
                DiscountCalculationType = getCompanyWebSetting.DiscountCalculationType,
                Active = getCompanyWebSetting.Active,
                WebActive = getCompanyWebSetting.WebActive,
                Version = getCompanyWebSetting.Version,
                IsEstoreInitialized = getCompanyWebSetting.IsEstoreInitialized,
                PaymentMethodId = getCompanyWebSetting.PaymentMethodId,
                DeliveryMethodId = getCompanyWebSetting.DeliveryMethodId
            };
            _companyWebSettingQueryProcessor.Update(companyWebSetting);
        }

        public bool CheckIfSalesOrderCodeExistsOrNot(string salesOrderCode)
        {
            return _dbContext.Set<SalesOrder>().Any(x => x.SalesOrderCode == salesOrderCode);
        }

        public bool DeleteRange(List<int?> salesOrderId)
        {
            var saleOrderList = salesOrderId.Select(saleOrderId => _dbContext.Set<SalesOrder>().FirstOrDefault(x => x.Id == saleOrderId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            saleOrderList.ForEach(x => x.Active = false);
            _dbContext.Set<SalesOrder>().UpdateRange(saleOrderList);
            _dbContext.SaveChanges();
            return true;
        }

        //public IQueryable<SalesOrder> GetActiveSalesOrders()
        //{
        //    return _dbContext.Set<SalesOrder>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        //}

        //public IQueryable<SalesOrder> GetDeletedSalesOrders()
        //{
        //    return _dbContext.Set<SalesOrder>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        //}
        //public IQueryable<SalesOrder> GetAllSalesOrders()
        //{
        //    return _dbContext.Set<SalesOrder>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
        //}
    }
}
