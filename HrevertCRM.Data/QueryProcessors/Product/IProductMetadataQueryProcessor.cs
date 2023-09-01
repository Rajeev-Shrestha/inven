using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IProductMetadataQueryProcessor
    {
        ProductMetadata Update(ProductMetadata productMetadata);
        ProductMetadata GetValidProductMetadata(int productMetadataId);
        ProductMetadataViewModel GetProductMetadata(int productMetadataId);
        ProductMetadata GetProductMetadataById(int productMetadataId);
        void SaveAllProductMetadata(List<ProductMetadata> productMetadatas);
        ProductMetadata Save(ProductMetadata productMetadata);
        int SaveAll(List<ProductMetadata> productMetadatas);
        ProductMetadata ActivateProductMetadata(int id);
        bool Delete(int productMetadataId);
        bool DeleteProductImage(int productId, string imageUrl);
        bool Exists(Expression<Func<ProductMetadata, bool>> @where);
        ProductMetadata[] GetProductMetadatas(Expression<Func<ProductMetadata, bool>> @where = null);
        //string SaveImage(int productId, IFormFileCollection files);
        string SaveProductMetadatas(int productId, List<Image> images);
        IQueryable<ProductMetadata> GetActiveProductMetadatas(int distributorId);
        PagedDataInquiryResponse<ProductMetadataViewModel> SearchProductMetadatas(PagedDataRequest requestInfo, 
            Expression<Func<ProductMetadata, bool>> @where = null);

        IEnumerable<string> GetProductMetadatasByProductId(int productId);
    }
}