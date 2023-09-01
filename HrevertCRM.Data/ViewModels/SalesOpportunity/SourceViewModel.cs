namespace HrevertCRM.Data.ViewModels
{
    public class SourceViewModel
    {
        public int? Id { get; set; }
        public string SourceName { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
    }
}
