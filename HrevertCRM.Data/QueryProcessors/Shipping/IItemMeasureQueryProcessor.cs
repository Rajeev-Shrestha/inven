using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IItemMeasureQueryProcessor
    {
        ItemMeasure Update(ItemMeasure itemMeasure);
        ItemMeasure GetItemMeasure(int itemMeasureId);
        ItemMeasureViewModel GetItemMeasureViewModel(int itemMeasureId);
        void SaveAll(List<ItemMeasure> itemMeasures);
        bool Delete(int itemMeasureId);
        bool Exists(Expression<Func<ItemMeasure, bool>> where);
        ItemMeasure ActivateItemMeasure(int id);
        ItemMeasure[] GetItemMeasures(Expression<Func<ItemMeasure, bool>> where = null);
        ItemMeasure Save(ItemMeasure itemMeasure);
        IQueryable<ItemMeasure> GetActiveItemMeasures(int distributorId);
        ItemMeasure CheckIfDeletedItemMeasureWithSameProductIdExists(int productId);
        IQueryable<ItemMeasure> SearchItemMeasures(bool active, string searchText);
        bool DeleteRange(List<int?> itemMeasuresId);
    }
}