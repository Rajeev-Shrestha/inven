using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ImageBase64 { get; set; }
        public ImageSize ImageSize { get; set; }
        public ImageType ImageType { get; set; }
    }
}
