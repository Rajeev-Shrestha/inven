﻿namespace HrevertCRM.Data.ViewModels.Enumerations
{
    public class AccountCashFlowTypeViewModel
    {
        public int? Id { get; set; }
        public string Value { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}