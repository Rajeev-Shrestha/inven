using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class ProductMetadata : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public MediaType MediaType { get; set; }
        public string MediaUrl { get; set; }
        public bool WebActive { get; set; }
        public Product Product { get; set; }
        public ImageSize ImageSize { get; set; }
    }
}
