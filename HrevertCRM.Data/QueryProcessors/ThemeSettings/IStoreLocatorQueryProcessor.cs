using HrevertCRM.Entities;
using System.Collections.Generic;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IStoreLocatorQueryProcessor
    {
        StoreLocator Update(StoreLocator storeLocator);
        StoreLocator GetStoreLocator(int storeLocatorId);
        StoreLocator Save(StoreLocator storeLocator);
        void SaveListOfStoreLocators(List<StoreLocator> storeLocators);
        void UpdateListOfStoreLocators(List<StoreLocator> storeLocators);


    }
}
