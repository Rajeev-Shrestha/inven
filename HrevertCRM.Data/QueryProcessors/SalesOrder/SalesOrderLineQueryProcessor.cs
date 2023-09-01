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
using PagedTaskDataInquiryResponse =
    HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.SalesOrderLineViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class SalesOrderLineQueryProcessor : QueryBase<SalesOrderLine>, ISalesOrderLineQueryProcessor
    {
        public SalesOrderLineQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public SalesOrderLine Update(SalesOrderLine salesOrderLine)
        {
            var original = GetValidSalesOrderLine(salesOrderLine.Id);
            ValidateAuthorization(salesOrderLine);
            CheckVersionMismatch(salesOrderLine, original);

            original.Description = salesOrderLine.Description;
            original.DescriptionType = salesOrderLine.DescriptionType;
            original.ItemPrice = salesOrderLine.ItemPrice;
            original.Shipped = salesOrderLine.Shipped;
            original.ItemQuantity = salesOrderLine.ItemQuantity;
            original.ShippedQuantity = salesOrderLine.ShippedQuantity;
            original.LineOrder = salesOrderLine.LineOrder;
            original.Discount = salesOrderLine.Discount;
            original.TaxAmount = salesOrderLine.TaxAmount;
            original.DiscountType = salesOrderLine.DiscountType;
            original.TaxId = salesOrderLine.TaxId;
            original.SalesOrderId = salesOrderLine.SalesOrderId;
            original.ProductId = salesOrderLine.ProductId;
            original.CompanyId = salesOrderLine.CompanyId;
            original.Active = salesOrderLine.Active;
            original.WebActive = salesOrderLine.WebActive;

            _dbContext.Set<SalesOrderLine>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual SalesOrderLine GetValidSalesOrderLine(int salesOrderLineId)
        {
            var salesOrderLine = _dbContext.Set<SalesOrderLine>().FirstOrDefault(sc => sc.Id == salesOrderLineId);
            if (salesOrderLine == null)
                throw new RootObjectNotFoundException(SalesOrderConstants.SalesOrderLineQueryProcessorConstants.SalesOrderLineNotFound);
            return salesOrderLine;
        }

        public SalesOrderLine GetSalesOrderLine(int salesOrderLineId)
        {
            var salesOrderLine = _dbContext.Set<SalesOrderLine>().Include(o => o.SalesOrder).FirstOrDefault(d => d.Id == salesOrderLineId);
            return salesOrderLine;
        }

        public void SaveAllSalesOrderLine(ICollection<SalesOrderLine> salesOrderLines)
        {
             salesOrderLines.ToList().ForEach(x => x.CompanyId = LoggedInUser.CompanyId);   
            _dbContext.Set<SalesOrderLine>().AddRange(salesOrderLines);
            _dbContext.SaveChanges();
        }

        public SalesOrderLine Save(SalesOrderLine salesOrderLine)
        {
            salesOrderLine.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SalesOrderLine>().Add(salesOrderLine);
            _dbContext.SaveChanges();
            return salesOrderLine;
        }
        public void SaveAllSeed(List<SalesOrderLine> salesOrderLines)
        {
            _dbContext.Set<SalesOrderLine>().AddRange(salesOrderLines);
            _dbContext.SaveChanges();
        }

        public void SaveAll(ICollection<SalesOrderLineViewModel> salesOrderLines)
        {
            var mapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
            _dbContext.Set<SalesOrderLine>().AddRange(salesOrderLines.Select(x => mapper.Map(x)));
            _dbContext.SaveChanges();
        }

        public void UpdateQuantityInProduct(IEnumerable<SalesOrderLineViewModel> salesOrderLines)
        {
            foreach (var orderLine in salesOrderLines)
            {
                var product = _dbContext.Set<Product>().FirstOrDefault(x => x.Id == orderLine.ProductId && x.Active && x.CompanyId == LoggedInUser.CompanyId);
                product.QuantityOnHand = product.QuantityOnHand - (int)orderLine.ItemQuantity;

                _dbContext.Set<Product>().Update(product);
                _dbContext.SaveChanges();
            }
        }

        public void UpdateQuantityOnOrderInProduct(SalesOrderLineViewModel salesOrderLineViewModel, SalesOrderType orderType)
        {
            var product = _dbContext.Set<Product>().FirstOrDefault(x => x.Id == salesOrderLineViewModel.ProductId && x.Active && x.CompanyId == LoggedInUser.CompanyId);
            if (salesOrderLineViewModel.Id == 0 || salesOrderLineViewModel.Id == null)
            {
                switch (orderType)
                {
                    case SalesOrderType.Order:
                        product.QuantityOnOrder = product.QuantityOnOrder + (int)salesOrderLineViewModel.ItemQuantity;
                        break;
                    case SalesOrderType.Invoice:
                        product.QuantityOnHand = product.QuantityOnHand - (int)salesOrderLineViewModel.ItemQuantity;
                        break;
                    case SalesOrderType.Quote:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null);
                }
            }
            else
            {
                var salesOrderType = _dbContext.Set<SalesOrderLine>().Include(x => x.SalesOrder)
                    .FirstOrDefault(x => x.Id == salesOrderLineViewModel.Id).SalesOrder.OrderType;
                if (salesOrderType == SalesOrderType.Invoice)
                {
                        product.QuantityOnOrder = product.QuantityOnOrder - (int)salesOrderLineViewModel.ItemQuantity;
                        product.QuantityOnHand = product.QuantityOnHand - (int)salesOrderLineViewModel.ItemQuantity;
                }
                if (salesOrderType == SalesOrderType.Order)
                {
                    product.QuantityOnOrder = product.QuantityOnOrder + (int)salesOrderLineViewModel.ItemQuantity;
                }
                if (salesOrderType == SalesOrderType.Quote)
                {
                    product.QuantityOnOrder = product.QuantityOnOrder - (int)salesOrderLineViewModel.ItemQuantity;
                }
            }
            _dbContext.Set<Product>().Update(product);
            _dbContext.SaveChanges();
        }

        public PagedTaskDataInquiryResponse SearchSalesOrderLines(PagedDataRequest requestInfo, Expression<Func<SalesOrderLine, bool>> @where = null)
        {
            var query = _dbContext.Set<SalesOrderLine>().Include(x => x.Product).Where(c => c.CompanyId == LoggedInUser.CompanyId);
            if (requestInfo.Active)
                query = query.Where(x => x.Active);
            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query
                : query.Where(x => x.Product.Name.ToUpper().Contains(requestInfo.SearchText.ToUpper()) ||
                                   x.Product.Code.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, result);
        }

        private void SaveOrUpdateItemCount(SalesOrderLineViewModel orderLineViewModel)
        {
            var order = _dbContext.Set<SalesOrder>().SingleOrDefault(x => x.Id == orderLineViewModel.SalesOrderId);
            var item = _dbContext.Set<ItemCount>().AsNoTracking()
                .FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.ItemId == orderLineViewModel.ProductId);

            if (order.OrderType == SalesOrderType.Order)
            {
                SaveOrUpdateSalesOrderItemCount(orderLineViewModel, item);
            }

            if (order.OrderType == SalesOrderType.Invoice)
            {
                SaveOrUpdateSalesInvoiceItemCount(orderLineViewModel, item);
            }
            _dbContext.SaveChanges();
        }

        private void SaveOrUpdateSalesInvoiceItemCount(SalesOrderLineViewModel orderLineViewModel, ItemCount item)
        {
            if (item != null)
            {
                item.QuantityOnInvoice = item.QuantityOnInvoice + (int)orderLineViewModel.ItemQuantity;
                _dbContext.Set<ItemCount>().Update(item);
            }
            else
            {
                _dbContext.Set<ItemCount>().Add(new ItemCount
                {
                    Active = true,
                    CompanyId = LoggedInUser.CompanyId,
                    ItemId = orderLineViewModel.ProductId,
                    QuantityOnInvoice = (int)orderLineViewModel.ItemQuantity
                });
            }
        }

        private void SaveOrUpdateSalesOrderItemCount(SalesOrderLineViewModel orderLineViewModel, ItemCount item)
        {
            if (item != null)
            {
                item.QuantityOnOrder = item.QuantityOnOrder + (int)orderLineViewModel.ItemQuantity;
                _dbContext.Set<ItemCount>().Update(item);
            }
            else
            {
                _dbContext.Set<ItemCount>().Add(new ItemCount
                {
                    Active = true,
                    CompanyId = LoggedInUser.CompanyId,
                    ItemId = orderLineViewModel.ProductId,
                    QuantityOnOrder = (int)orderLineViewModel.ItemQuantity
                });
            }
        }

        public SalesOrderLine ActivateSalesOrderLine(int id)
        {
            var original = GetValidSalesOrderLine(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<SalesOrderLine>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public SalesOrderLineViewModel GetSalesOrderLineViewModel(int id)
        {
            var salesOrderLine = _dbContext.Set<SalesOrderLine>().Single(s => s.Id == id);
            var mapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
            return mapper.Map(salesOrderLine);
        }

        public bool Delete(int salesOrderLineId)
        {
            var doc = GetSalesOrderLine(salesOrderLineId);
            ValidateAuthorization(doc);

            if (doc == null) return false;

            //Restore values to maintain stock and QOO
            var orderType = doc.SalesOrder.OrderType;
            var productId = doc.ProductId;
            var itemQuantity = doc.ItemQuantity;

            _dbContext.Set<SalesOrderLine>().Remove(doc);

            //Update Quantity in Stock or Quantity on Hand
            var product = _dbContext.Set<Product>().FirstOrDefault(x => x.Id == productId && x.Active && x.CompanyId == LoggedInUser.CompanyId);

            switch (orderType)
            {
                case SalesOrderType.Order:
                    product.QuantityOnOrder = product.QuantityOnOrder - (int)itemQuantity;
                    break;
                case SalesOrderType.Invoice:
                    product.QuantityOnHand = product.QuantityOnHand + (int)itemQuantity;
                    break;
                case SalesOrderType.Quote:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null);
            }
            _dbContext.Set<Product>().Update(product);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Exists(Expression<Func<SalesOrderLine, bool>> where)
        {
            return _dbContext.Set<SalesOrderLine>().Any(where);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<SalesOrderLine> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(s => mapper.Map(s))
                    .ToList();

            var queryResult = new QueryResult<SalesOrderLineViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public SalesOrderLine[] GetSalesOrderLines(Expression<Func<SalesOrderLine, bool>> where = null)
        {
            var query = _dbContext.Set<SalesOrderLine>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        //public void SaveItemCount()
        //{
        //    var orderSalesOrder =
        //        _dbContext.Set<SalesOrder>().AsNoTracking().Include(x => x.SalesOrderLines)
        //            .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.OrderType == SalesOrderType.Order)
        //            .Select(x => x.SalesOrderLines);

        //    var invoiceSalesOrder =
        //        _dbContext.Set<SalesOrder>().AsNoTracking().Include(x => x.SalesOrderLines)
        //            .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.OrderType == SalesOrderType.Invoice)
        //            .Select(x => x.SalesOrderLines);

        //    var orderSalesOrderLines =
        //        orderSalesOrder.Select(oso => oso.GroupBy(x => x.ProductId)
        //            .Select(
        //                g =>
        //                    new
        //                    {
        //                        g.Key,
        //                        ItemQuantity = g.Sum(item => item.ItemQuantity),
        //                        ShippedQuantity = g.Sum(item => item.ShippedQuantity)
        //                    })
        //            .ToList())
        //            ;

        //    var invoiceSalesOrderLines =
        //        invoiceSalesOrder.Select(iso => iso.GroupBy(x => x.ProductId)
        //            .Select(
        //                g =>
        //                    new
        //                    {
        //                        g.Key,
        //                        ItemQuantity = g.Sum(item => item.ItemQuantity),
        //                        ShippedQuantity = g.Sum(item => item.ShippedQuantity)
        //                    })
        //            .ToList());

        //    var counts = new List<ItemCount>();

        //    foreach (var orderLine in orderSalesOrderLines)
        //    {
        //        counts.AddRange(orderLine.Select(line => new ItemCount
        //        {
        //            ItemId = line.Key, QuantityOnOrder = (int) (line.ItemQuantity - line.ShippedQuantity), CompanyId = LoggedInUser.CompanyId
        //        }));
        //    }

        //    foreach (var itemCount in invoiceSalesOrderLines)
        //    {
        //         var i = 0;

        //        foreach (var item in itemCount)
        //        {
        //            counts[i].QuantityOnInvoice = (int)(item.ItemQuantity - item.ShippedQuantity);
        //            i++;
        //        }
        //    }

        //    foreach (var itemCount in counts)
        //    {
        //            var item = _dbContext.Set<ItemCount>().AsNoTracking()
        //                .FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.ItemId == itemCount.ItemId);

        //            if (item != null)
        //            {
        //                item.QuantityOnOrder = item.QuantityOnOrder + itemCount.QuantityOnOrder;
        //                _dbContext.Set<ItemCount>().Update(item);
        //            }
        //            else
        //            {
        //                _dbContext.Set<ItemCount>().Add(itemCount);
        //            }


        //        _dbContext.SaveChanges();
        //    }
        //}
    }
}
