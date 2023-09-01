namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class StoreLocatorViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public int HeaderSettingId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
