using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IDeliveryRateQueryProcessor
    {
        DeliveryRate Update(DeliveryRate deliveryRate);
        DeliveryRate GetDeliveryRate(int deliveryRateId);
        DeliveryRateViewModel GetDeliveryRateViewModel(int deliveryRateId);
        void SaveAllDeliveryRate(List<DeliveryRate> deliveryRates);
        bool Delete(int deliveryRateId);
        bool Exists(Expression<Func<DeliveryRate, bool>> where);
        DeliveryRate ActivateDeliveryRate(int id);
        DeliveryRate[] GetDeliveryRates(Expression<Func<DeliveryRate, bool>> where = null);
        DeliveryRate Save(DeliveryRate deliveryRate);
        IQueryable<DeliveryRate> GetActiveDeliveryRates(int distributorId);
        IQueryable<DeliveryRate> SearchDeliveryRates(bool active, string searchText);
        bool DeleteRange(List<int?> deliveryRatesId);

    }
}