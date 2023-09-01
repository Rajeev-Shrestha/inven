using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Carousel : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public ProductOrCategory ProductOrCategory { get; set; }
        public int ItemId { get; set; }
        public Image Image { get; set; }
        public string ImageUrl { get; set; }
        public bool WebActive { get; set; }
    }
}
