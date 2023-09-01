using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{ 
    public class ThemeImage
    {
        public string FileName { get; set; }
        public string ImageBase64 { get; set; }
        public ThemeSettingImageType ImageType { get; set; }
    }
}
