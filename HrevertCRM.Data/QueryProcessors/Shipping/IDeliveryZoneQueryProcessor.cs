using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IDeliveryZoneQueryProcessor
    {
        DeliveryZone Update(DeliveryZone deliveryZone);
        DeliveryZone GetDeliveryZone(int deliveryZoneId);
        DeliveryZoneViewModel GetDeliveryZoneViewModel(int deliveryZoneId);
        void SaveAll(List<DeliveryZone> deliveryZones);
        bool Delete(int deliveryZoneId);
        bool Exists(Expression<Func<DeliveryZone, bool>> where);
        DeliveryZone ActivateDeliveryZone(int id);
        DeliveryZone[] GetDeliveryZones(Expression<Func<DeliveryZone, bool>> where = null);
        DeliveryZone Save(DeliveryZone deliveryZone);
        DeliveryZone CheckIfDeletedDeliveryZoneWithSameCodeExists(string code);
        IQueryable<DeliveryZone> SearchDeliveryZones(bool active, string searchText);
        bool DeleteRange(List<int?> deliveryZoneId);
    }
}