using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class EmailSettingQueryProcessor : QueryBase<EmailSetting>, IEmailSettingQueryProcessor
    {
        public EmailSettingQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public EmailSetting Update(EmailSetting emailSetting)
        {
            var original = GetValidEmailSetting(emailSetting.Id);
            ValidateAuthorization(emailSetting);
            CheckVersionMismatch(emailSetting, original);

            original.Host = emailSetting.Host;
            original.Port = emailSetting.Port;
            original.Sender = emailSetting.Sender;
            original.UserName = emailSetting.UserName;
            original.Password = emailSetting.Password;
            original.Name = emailSetting.Name;
            original.EncryptionType = emailSetting.EncryptionType;
            original.RequireAuthentication = emailSetting.RequireAuthentication;
            original.Active = emailSetting.Active;
            original.WebActive = emailSetting.WebActive;
            original.CompanyId = emailSetting.CompanyId;

            _dbContext.Set<EmailSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual EmailSetting GetValidEmailSetting(int emailSettingId)
        {
            var emailSetting = _dbContext.Set<EmailSetting>().FirstOrDefault(sc => sc.Id == emailSettingId);
            if (emailSetting == null)
            {
                throw new RootObjectNotFoundException(EmailSettingConstants.EmailSettingQueryProcessorConstants.EmailSettingNotFound);
            }
            return emailSetting;
        }

        public EmailSetting GetEmailSetting(int emailSettingId)
        {
            var emailSetting = _dbContext.Set<EmailSetting>().FirstOrDefault(d => d.Id == emailSettingId);
            return emailSetting;
        }
        public void SaveAllEmailSetting(List<EmailSetting> emailSettings)
        {
            _dbContext.Set<EmailSetting>().AddRange(emailSettings);
            _dbContext.SaveChanges();
        }

        public EmailSetting Save(EmailSetting emailSetting)
        {
            emailSetting.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<EmailSetting>().Add(emailSetting);
            _dbContext.SaveChanges();
            return emailSetting;
        }

        public int SaveAll(List<EmailSetting> emailSettings)
        {
            _dbContext.Set<EmailSetting>().AddRange(emailSettings);
            return _dbContext.SaveChanges();
        }

        public EmailSetting ActivateEmailSetting(int id)
        {
            var original = GetValidEmailSetting(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<EmailSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public EmailSettingViewModel GetEmailSettingViewModel(int id)
        {
            var emailSetting = _dbContext.Set<EmailSetting>().Single(s => s.Id == id);
            var mapper = new EmailSettingToEmailSettingViewModelMapper();
            return mapper.Map(emailSetting);
        }

        public bool Delete(int emailSettingId)
        {
            var doc = GetEmailSetting(emailSettingId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<EmailSetting>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<EmailSetting, bool>> @where)
        {
            return _dbContext.Set<EmailSetting>().Any(@where);
        }

        public EmailSetting[] GetEmailSettings(Expression<Func<EmailSetting, bool>> @where = null)
        {

            var query = _dbContext.Set<EmailSetting>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public EmailSetting CheckIfDeletedEmailSettingWithSameNameExists(string name)
        {
            var emailSetting =
                _dbContext.Set<EmailSetting>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.UserName == name && x.Active ||
                             x.Active == false);
            return emailSetting;
        }

        public IQueryable<EmailSetting> SearchEmailSettings(bool active, string searchText)
        {
            var emailSettings = _dbContext.Set<EmailSetting>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                emailSettings = emailSettings.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? emailSettings
                : emailSettings.Where(s => s.UserName.ToUpper().Contains(searchText.ToUpper())
                                           || s.Sender.ToUpper().Contains(searchText.ToUpper())
                                           || s.Host.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public bool DeleteRange(List<int?> emailsId)
        {
            var emailList = emailsId.Select(emailId => _dbContext.Set<EmailSetting>().FirstOrDefault(x => x.Id == emailId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            emailList.ForEach(x => x.Active = false);
            _dbContext.Set<EmailSetting>().UpdateRange(emailList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
