using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Http;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IEmailSettingQueryProcessor
    {
        EmailSetting Update(EmailSetting emailSetting);
        void SaveAllEmailSetting(List<EmailSetting> emailSettings);
        bool Delete(int emailSettingId);
        bool Exists(Expression<Func<EmailSetting, bool>> @where);
        EmailSetting[] GetEmailSettings(Expression<Func<EmailSetting, bool>> @where = null);
        EmailSetting Save(EmailSetting emailSetting);
        int SaveAll(List<EmailSetting> emailSettings);
        EmailSetting ActivateEmailSetting(int id);
        EmailSettingViewModel GetEmailSettingViewModel(int id);
        EmailSetting CheckIfDeletedEmailSettingWithSameNameExists(string name);
        IQueryable<EmailSetting> SearchEmailSettings(bool active, string searchText);
        bool DeleteRange(List<int?> emailsId);
    }
}