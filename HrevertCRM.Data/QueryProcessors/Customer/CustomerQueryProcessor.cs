using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.EditCustomerViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class CustomerQueryProcessor : QueryBase<Customer>, ICustomerQueryProcessor
    {
        private readonly IAddressQueryProcessor _addressQueryProcessor;
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;

        public CustomerQueryProcessor(IUserSession userSession, IDbContext dbContext, 
            IAddressQueryProcessor addressQueryProcessor, 
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor) : base(userSession, dbContext)
        {
            _addressQueryProcessor = addressQueryProcessor;
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
        }
        public Customer Update(Customer customer)
        {
            var original = GetValidCustomer(customer.Id);
            ValidateAuthorization(customer);
            CheckVersionMismatch(customer, original); 
           
            original.CustomerCode = customer.CustomerCode;
            original.Password = customer.Password;
            original.CustomerLevelId = customer.CustomerLevelId;
            original.PaymentTermId = customer.PaymentTermId;
            original.PaymentMethodId = customer.PaymentMethodId;
            original.OpeningBalance = customer.OpeningBalance;
            original.PaymentTermId = customer.PaymentTermId;
            original.Note = customer.Note;
            original.TaxRegNo = customer.TaxRegNo;
            original.DisplayName = customer.DisplayName;
            original.IsCodEnabled = customer.IsCodEnabled;
            original.IsPrepayEnabled = customer.IsPrepayEnabled;
            original.OnAccountId = customer.OnAccountId;
            original.WebActive = customer.WebActive;
            original.Active = customer.Active;
            original.VatNo = customer.VatNo;
            original.PanNo = customer.PanNo;
            _addressQueryProcessor.Update(customer.BillingAddress);
            if (customer.Addresses.Count > 0)
            {
                foreach (var address in customer.Addresses)
                {
                    if (address.Id == 0)
                    {
                        address.CustomerId = customer.Id;
                        address.CompanyId = LoggedInUser.CompanyId;
                        _addressQueryProcessor.Save(address);
                    }
                    else
                    {
                        _addressQueryProcessor.Update(address);
                    }
                }
            }
           
            _dbContext.Set<Customer>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual Customer GetValidCustomer(int customerId)
        {
            var customer = _dbContext.Set<Customer>().FirstOrDefault(sc => sc.Id == customerId);
            if (customer == null)
            {
                throw new RootObjectNotFoundException(CustomerConstants.CustomerQueryProcessorConstants.CustomerNotFound);
            }
            return customer;
        }

        public EditCustomerViewModel GetCustomerViewModel(int customerId)
        {
            var addresses =
                _dbContext.Set<Address>()
                    .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.CustomerId == customerId && x.Active && x.AddressType != AddressType.Billing).ToList();
            var customer = _dbContext.Set<Customer>().FirstOrDefault(d => d.CompanyId == LoggedInUser.CompanyId && d.Id == customerId);
            var billingAddress= _dbContext.Set<Address>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.CustomerId == customerId && x.Active).FirstOrDefault(d => d.AddressType == AddressType.Billing);

            if (billingAddress != null && addresses != null)
            {
                customer.BillingAddress = billingAddress;
                customer.Addresses = addresses;
            }
                
            var mapper = new CustomerToEditCustomerViewModelMapper();
            return mapper.Map(customer);
        }
        public Customer GetCustomer(int customerId)
        {
            var customer = _dbContext.Set<Customer>().Include(c => c.Addresses).FirstOrDefault(d => d.Id == customerId);
            return customer;
        }
        public void SaveAllCustomer(List<Customer> customers)
        {
            _dbContext.Set<Customer>().AddRange(customers);
            _dbContext.SaveChanges();
        }

        public Customer Save(Customer customer)
        {
            customer.CompanyId = LoggedInUser.CompanyId;
            customer.Active = true;
            _dbContext.Set<Customer>().Add(customer);
            _dbContext.SaveChanges();
            return customer;
        }

        public int SaveAll(List<Customer> customers)
        {
            _dbContext.Set<Customer>().AddRange(customers);
            return _dbContext.SaveChanges();
        }

        public Customer ActivateCustomer(int id)
        {
            var original = GetValidCustomer(id);
            ValidateAuthorization(original);
          
            original.Active = true;
           _dbContext.Set<Customer>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public Address SaveBillingAddress(Address address)
        {
            address.CompanyId = LoggedInUser.CompanyId;
            return _addressQueryProcessor.Save(address);
        }

        public bool Delete(int customerId)
        {
            var doc = GetCustomer(customerId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Customer>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Customer, bool>> where)
        {
            return _dbContext.Set<Customer>().Include(x => x.Addresses).Any(where);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<Customer> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new CustomerToEditCustomerViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<EditCustomerViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public Customer[] GetCustomers(Expression<Func<Customer, bool>> where = null)
        {
          
            var query = _dbContext.Set<Customer>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public string GenerateCustomerCode()
        {
            var getCompanyWebSetting = _companyWebSettingQueryProcessor.Get(LoggedInUser.CompanyId);
            var serialNo = getCompanyWebSetting.CustomerSerialNo + 1;

            var customerCode = serialNo.ToString("00000000");
           
            while (true)
            {
                if (CheckIfCustomerCodeExistsOrNot(customerCode))
                {
                    serialNo = serialNo + 1;
                    customerCode = serialNo.ToString("00000000");
                }
                else
                {
                    break;
                }
            }
            UpdateCompanyWebSetting(getCompanyWebSetting, serialNo); 
            return "C-" + customerCode;
        }

        private void UpdateCompanyWebSetting(CompanyWebSetting getCompanyWebSetting, int serialNo)
        {
            var companyWebSetting = new CompanyWebSetting
            {
                CompanyId = LoggedInUser.CompanyId,
                Id = getCompanyWebSetting.Id,
                AllowGuest = true,
                CustomerSerialNo = serialNo,
                VendorSerialNo = getCompanyWebSetting.VendorSerialNo,
                SalesRepId = LoggedInUser.Id,
                ShippingCalculationType = getCompanyWebSetting.ShippingCalculationType,
                DiscountCalculationType = getCompanyWebSetting.DiscountCalculationType,
                Active = getCompanyWebSetting.Active,
                WebActive = getCompanyWebSetting.WebActive,
                Version = getCompanyWebSetting.Version
            };
            _companyWebSettingQueryProcessor.Update(companyWebSetting);
        }

        public bool CheckIfCustomerCodeExistsOrNot(string customerCode)
        {
            var getCustomerCodes = _dbContext.Set<Customer>().Where(x => x.CompanyId == LoggedInUser.CompanyId).Select(x => x.CustomerCode).ToList();
            var value = getCustomerCodes.Contains("C-"+customerCode);
            return value;
        }

        public CustomerLoginResultViewModel CustomerLogin(string email, string password)
        {
            var address = _dbContext.Set<Address>().SingleOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.AddressType == AddressType.Billing && x.Email == email && x.Active);
            if (address == null)
            {
                return new CustomerLoginResultViewModel
                {
                    LoggedIn = false
                };
            }
            var isCustomerPasswordMatches = _dbContext.Set<Customer>().Where(x => x.Id == address.CustomerId && x.Active && x.WebActive).Select(x => x.Password).Single() == password;
            if (isCustomerPasswordMatches == false)
            {
                return new CustomerLoginResultViewModel
                {
                    LoggedIn = false
                };
            }
            return new CustomerLoginResultViewModel
            {
                // ReSharper disable once PossibleInvalidOperationException
                CustomerId = (int) address.CustomerId,
                FirstName = address.FirstName,
                MiddleName = address.MiddleName,
                LastName = address.LastName,
                Email = address.Email,
                LoggedIn = true
            };
        }

        public List<SalesOrderViewModel> GetOrdersSummary(int customerId)
        {
            var salesOrdersValues = _dbContext.Set<SalesOrder>()
                .Include(x => x.SalesOrderLines)
                .Where(x => x.CustomerId == customerId).ToList();
            var mapper = new SalesOrderToSalesOrderViewModelMapper();
            var salesOrderViewModels = salesOrdersValues.Select(x => mapper.Map(x)).ToList();

            foreach (var salesOrder in salesOrderViewModels)
            {
                if (salesOrder.SalesOrderLines == null || salesOrder.SalesOrderLines.Count <= 0) continue;
                if (salesOrder.SalesOrderLines.All(x => x.Shipped == false))
                {
                    salesOrder.ShippingStatus = ShippingStatus.Pending;
                }
                else if (salesOrder.SalesOrderLines.All(x => x.Shipped))
                {
                    salesOrder.ShippingStatus = ShippingStatus.Shipped;
                }
                else
                {
                    salesOrder.ShippingStatus = ShippingStatus.PartiallyShipped;
                }
            }
            return salesOrderViewModels;
        }

        public IQueryable<Customer> GetActiveCustomersWithoutPaging()
        {
            var billingAddress = _dbContext.Set<Address>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.AddressType == AddressType.Billing).ToList();

            var customer = _dbContext.Set<Customer>().Where(FilterByActiveTrueAndCompany);

            foreach (var item in customer)
            {
                item.BillingAddress = billingAddress.FirstOrDefault(x => x.CustomerId == item.Id);
            }
           
            return customer;
        }
        public IQueryable<Customer> GetDeletedCustomersWithoutPaging()
        {
            return _dbContext.Set<Customer>().Where(FilterByActiveFalseAndCompany);
        }

        public void ImportCustomers(List<Customer> customers, bool updateExisting)
        {
            if (updateExisting)
            {
                foreach (var customer in customers)
                {
                    if (_dbContext.Set<Customer>().Any(cust => cust.CustomerCode == customer.CustomerCode))
                    {
                        Update(customer);
                    }
                    else
                    {
                        Save(customer);
                    }
                }
            }
            else
            {
                SaveAll(customers);
            }
        }


        public IQueryable<Address> GetCustomerAllAddresses(int customerId)
        {
            var addresses = _dbContext.Set<Address>()
                .Where(x => x.CustomerId == customerId && x.CompanyId == LoggedInUser.CompanyId && x.AddressType!=AddressType.Billing);

            return addresses;
        }


        public Customer CheckIfDeletedCustomerWithSameCodeExists(string customerCode)
        {
            var customer =
                _dbContext.Set<Customer>()
                    .FirstOrDefault(
                        x =>
                            x.CustomerCode == customerCode && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                            x.Active == false));
            return customer;
        }

        public PagedTaskDataInquiryResponse SearchCustomers(PagedDataRequest requestInfo, Expression<Func<Customer, bool>> @where = null)
        {
            var filteredCustomers = _dbContext.Set<Customer>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            var billingAddress = _dbContext.Set<Address>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.AddressType == AddressType.Billing);

            foreach (var customer in filteredCustomers)
            {
                customer.BillingAddress = billingAddress.FirstOrDefault(x => x.CustomerId == customer.Id);
            }
            if (requestInfo.Active)
                filteredCustomers = filteredCustomers.Where(req => req.Active);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredCustomers : filteredCustomers.Where(s
                                                                   => s.CustomerCode.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.DisplayName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.Addresses.Single(x => x.AddressType == AddressType.Billing).Email.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, query);
        }

        public bool DeleteRange(List<int?> customersId)
        {
            var customerList = customersId.Select(customerId => _dbContext.Set<Customer>().FirstOrDefault(x => x.Id == customerId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            customerList.ForEach(x => x.Active = false);
            _dbContext.Set<Customer>().UpdateRange(customerList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
