using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Hrevert.Common.Constants;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Http;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace HrevertCRM.Data.QueryProcessors
{
    public class EmailSenderQueryProcessor : QueryBase<EmailSender>, IEmailSenderQueryProcessor
    {
        private readonly IUserSession _userSession;
        private readonly IHostingEnvironment _env;
        private readonly IServiceProvider _serviceProvider;

        public EmailSenderQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env, IServiceProvider serviceProvider) 
            : base(userSession, dbContext)
        {
            _userSession = userSession;
            _dbContext = dbContext;
            _env = env;
            _serviceProvider = serviceProvider;
        }

        public EmailSender Save(EmailSenderViewModel emailSenderViewModel)
        {
            var mapper = new EmailSenderToEmailSenderViewModelMapper();
            var newEmail = mapper.Map(emailSenderViewModel);
            newEmail.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<EmailSender>().Add(newEmail);
            _dbContext.SaveChanges();
            return newEmail;
        }
        public EmailSender Update(EmailSender emailSender)
        {
            var original = GetValidEmailSender(emailSender.Id);
            ValidateAuthorization(emailSender);
            //pass value to original
            original.Cc = emailSender.Cc;
            original.MailTo = emailSender.MailTo;
            original.MailFrom = emailSender.MailFrom;
            original.Message = emailSender.Message;
            original.Subject = emailSender.Subject;
            original.IsEmailSent = emailSender.IsEmailSent;
            original.CompanyId = LoggedInUser.CompanyId;
            original.Active = emailSender.Active;

            _dbContext.Set<EmailSender>().Update(original);
            _dbContext.SaveChanges();
            return emailSender;
        }

        public virtual EmailSender GetValidEmailSender(int emailSenderId)
        {
            var emailSender = _dbContext.Set<EmailSender>().FirstOrDefault(sc => sc.Id == emailSenderId);
            if (emailSender == null)
            {
                throw new RootObjectNotFoundException(EmailSenderConstants.EmailSenderQueryProcessorConstants.EmailNotFound);
            }
            return emailSender;
        }

        public async void SendEmail(EmailSettingViewModel emailSetting, EmailSender newEmail,
            IFormFileCollection files)
        {
            var retirievedEmailSetting =
                _dbContext.Set<EmailSetting>()
                    .FirstOrDefault(x => x.Port == 8889 && x.CompanyId == LoggedInUser.CompanyId && x.Active);
            var emailMessage = new MimeMessage();
            var disconnectClient = new SmtpClient();

            string[] emailRecipients = null;
            if (newEmail.MailTo != null)
            {
                emailRecipients = newEmail.MailTo.Remove(newEmail.MailTo.Length - 1).Split(';');
            }

            if (newEmail.Cc != null)
            {
                var ccs = newEmail.Cc.Remove(newEmail.Cc.Length - 1).Split(';');
                foreach (var cca in ccs)
                {
                    emailMessage.Cc.Add(new MailboxAddress(cca));
                }
            }

            emailMessage.From.Add(new MailboxAddress(retirievedEmailSetting.Sender, retirievedEmailSetting.UserName));
            emailMessage.Subject = newEmail.Subject;
            var builder = new BodyBuilder { TextBody = newEmail.Message };

            if (files != null)
            {
                foreach (var attachment in files.ToList())
                {
                    builder.Attachments.Add(attachment.FileName, attachment.OpenReadStream());
                }
            }

            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    var credentials = new NetworkCredential
                    {
                        UserName = retirievedEmailSetting.UserName,
                        Password = retirievedEmailSetting.Password
                    };

                    await
                        client.ConnectAsync(retirievedEmailSetting.Host, retirievedEmailSetting.Port).ConfigureAwait(false);
                    await client.AuthenticateAsync(credentials);

                    if (emailRecipients != null)
                        foreach (var recipient in emailRecipients)
                        {
                            emailMessage.To.Add(new MailboxAddress(recipient));
                            await client.SendAsync(emailMessage).ConfigureAwait(false);
                        }
                    newEmail.IsEmailSent = true;
                    using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var db = serviceScope.ServiceProvider.GetService<IDbContext>();
                        db.Set<EmailSender>().Update(newEmail);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var db = serviceScope.ServiceProvider.GetService<IDbContext>();
                        newEmail.EmailNotSentCause = ex.Message;
                        newEmail.IsEmailSent = false;
                        db.Set<EmailSender>().Update(newEmail);
                        db.SaveChanges();
                    }
                }
            }
            await disconnectClient.DisconnectAsync(true).ConfigureAwait(false);
        }

        public async void EmailCartDetail(EmailSettingViewModel emailSetting, EmailSenderViewModel emailViewModel,
            string fileBase64)
        {
            var retirievedEmailSetting =
                _dbContext.Set<EmailSetting>()
                    .FirstOrDefault(x => x.Port == 8889 && x.CompanyId == LoggedInUser.CompanyId && x.Active);
            var emailMessage = new MimeMessage();
            var disconnectClient = new SmtpClient();

            string[] emailRecipients = null;
            if (emailViewModel.MailTo != null)
            {
                emailRecipients = emailViewModel.MailTo.Remove(emailViewModel.MailTo.Length - 1).Split(';');
            }

            emailMessage.From.Add(new MailboxAddress(retirievedEmailSetting.Sender, retirievedEmailSetting.UserName));
            emailMessage.Subject = emailViewModel.Subject;

            var builder = new BodyBuilder { TextBody = emailViewModel.Message };
            if (fileBase64 != null)
            {
                var attachmentPathUrl = SaveEmailAttachment(fileBase64);
                builder.Attachments.Add(attachmentPathUrl);
            }
            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {

                try
                {
                    var credentials = new NetworkCredential
                    {
                        UserName = retirievedEmailSetting.UserName,
                        Password = retirievedEmailSetting.Password
                    };

                    await
                        client.ConnectAsync(retirievedEmailSetting.Host, retirievedEmailSetting.Port).ConfigureAwait(false);
                    await client.AuthenticateAsync(credentials);

                    if (emailRecipients != null)
                        foreach (var recipient in emailRecipients)
                        {
                            emailMessage.To.Add(new MailboxAddress(recipient));
                            await client.SendAsync(emailMessage).ConfigureAwait(false);
                        }
                    var mapper = new EmailSenderToEmailSenderViewModelMapper();
                    var email = mapper.Map(emailViewModel);
                    email.IsEmailSent = true;
                    using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var db = serviceScope.ServiceProvider.GetService<IDbContext>();
                        db.Set<EmailSender>().Add(email);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var db = serviceScope.ServiceProvider.GetService<IDbContext>();
                        emailViewModel.EmailNotSentCause = ex.Message;
                        emailViewModel.IsEmailSent = false;
                        var mapper = new EmailSenderToEmailSenderViewModelMapper();
                        db.Set<EmailSender>().Add(mapper.Map(emailViewModel));
                        db.SaveChanges();
                    }
                }

            }
            await disconnectClient.DisconnectAsync(true).ConfigureAwait(false);
        }

        public async void ReportBug(EmailSenderViewModel emailViewModel)
        {
            var retirievedEmailSetting =
                _dbContext.Set<EmailSetting>()
                    .FirstOrDefault(x => x.Port == 8889 && x.CompanyId == LoggedInUser.CompanyId && x.Active);
            var emailMessage = new MimeMessage();
            var disconnectClient = new SmtpClient();

            string[] emailRecipients = null;
            if (emailViewModel.MailTo != null)
            {
                emailRecipients = emailViewModel.MailTo.Remove(emailViewModel.MailTo.Length - 1).Split(';');
            }

            emailMessage.From.Add(new MailboxAddress(retirievedEmailSetting.Sender, retirievedEmailSetting.UserName));
            emailMessage.Subject = emailViewModel.Subject;

            var builder = new BodyBuilder { TextBody = emailViewModel.Message };
            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    var credentials = new NetworkCredential
                    {
                        UserName = retirievedEmailSetting.UserName,
                        Password = retirievedEmailSetting.Password
                    };

                    await
                        client.ConnectAsync(retirievedEmailSetting.Host, retirievedEmailSetting.Port, SecureSocketOptions.None).ConfigureAwait(false);
                    await client.AuthenticateAsync(credentials, CancellationToken.None);

                    if (emailRecipients != null)
                        foreach (var recipient in emailRecipients)
                        {
                            emailMessage.To.Add(new MailboxAddress(recipient));
                            await client.SendAsync(emailMessage).ConfigureAwait(false);
                        }
                    var mapper = new EmailSenderToEmailSenderViewModelMapper();
                    var email = mapper.Map(emailViewModel);
                    email.IsEmailSent = true;
                    using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var db = serviceScope.ServiceProvider.GetService<IDbContext>();
                        db.Set<EmailSender>().Add(email);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var db = serviceScope.ServiceProvider.GetService<DataContext>();
                        emailViewModel.CompanyId = LoggedInUser.CompanyId;
                        emailViewModel.EmailNotSentCause = ex.Message;
                        emailViewModel.IsEmailSent = false;
                        var mapper = new EmailSenderToEmailSenderViewModelMapper();
                        db.Set<EmailSender>().Add(mapper.Map(emailViewModel));
                        db.SaveChanges();
                    }
                }

            }
            await disconnectClient.DisconnectAsync(true).ConfigureAwait(false);
        }

        public async void SendUsernamePassToCustomer(string email, string userName, string password)
        {
            var retirievedEmailSetting =
                _dbContext.Set<EmailSetting>()
                    .FirstOrDefault(x => x.Port == 8889 && x.CompanyId == LoggedInUser.CompanyId && x.Active);
            var emailMessage = new MimeMessage();
            var disconnectClient = new SmtpClient();

            emailMessage.From.Add(new MailboxAddress(retirievedEmailSetting.Sender, retirievedEmailSetting.UserName));

            string[] emailRecipients = null;
            if (email != null)
            {
                email = email + ";";
                emailRecipients = email.Remove(email.Length - 1).Split(';');
            }

            emailMessage.Subject = "Here is your UserName And Password:";
            var message = "UserName: " + userName + " and Password: " + password;
            var builder = new BodyBuilder { TextBody = message };
            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = retirievedEmailSetting.UserName,
                    Password = retirievedEmailSetting.Password
                };

                await client.ConnectAsync(retirievedEmailSetting.Host, retirievedEmailSetting.Port).ConfigureAwait(false);
                await client.AuthenticateAsync(credentials);

                if (emailRecipients != null)
                    foreach (var recipient in emailRecipients)
                    {
                        emailMessage.To.Add(new MailboxAddress(recipient));
                        await client.SendAsync(emailMessage).ConfigureAwait(false);
                    }
            }
            await disconnectClient.DisconnectAsync(true).ConfigureAwait(false);
        }

        public string SaveEmailAttachment(string fileBase64)
        {
            if (fileBase64 == null) return "Nothing to save";
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var emailAttachmentsPath = Path.Combine(_env.WebRootPath, @"companyMM\" + LoggedInUser.CompanyId + @"\EmailAttachments");
                    CreateDirectory(emailAttachmentsPath);
                    var filePath = "";
                    //Check if the directory exists or create a new one
                    var bytes = Convert.FromBase64String(fileBase64);
                    var filename = "EmailAttachment";
                    const string fileExtension = ".pdf";

                    var fileWritten = false;
                    var i = 0;
                    while (!fileWritten)
                    {
                        if (i > 0)
                        {
                            if (!filename.Contains("_") && i == 1)
                            {
                                filename = filename + "_" + i;
                            }
                            else
                            {
                                filename = filename.Remove(filename.LastIndexOf("_", StringComparison.Ordinal)) + "_" + i;
                            }
                        }

                        var copyFileName = filename + fileExtension;
                        filePath = Path.Combine(emailAttachmentsPath, copyFileName);

                        if (File.Exists(filePath)) i++;
                        else
                        {
                            using (var pdfFile = new FileStream(filePath, FileMode.Create))
                            {
                                pdfFile.Write(bytes, 0, bytes.Length);
                                pdfFile.Flush();
                                fileWritten = true;
                            }
                        }
                    }
                    trans.Commit();
                    return filePath;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return ex.Message;
                }
            }
        }

        private static void CreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists == false)
            {
                directory.Create();
            }
        }


        #region SendEmail with ViewModel

        //public async void SendEmailAsync(string mailFrom, string mailTo, string subject, string message, List<IFormFile> files)
        //{
        //    var emailMessage = new MimeMessage();
        //    var disconnectClient = new SmtpClient();

        //    emailMessage.From.Add(new MailboxAddress(mailFrom));
        //    emailMessage.Subject = subject;
        //    var emailRecipients = mailTo.Split(';');

        //    var builder = new BodyBuilder
        //    {
        //        TextBody = message,
        //    };
        //    foreach (var attachment in files)
        //    {
        //        builder.Attachments.Add(attachment.FileName, attachment.OpenReadStream());
        //    }
        //    emailMessage.Body = builder.ToMessageBody();

        //    using (var client = new SmtpClient())
        //    {
        //        var credentials = new NetworkCredential
        //        {
        //            UserName = "thakurkamlesh10@gmail.com",
        //        };

        //        client.LocalDomain = "domain.com";
        //        //fetch the values from EmailSettings Table and amend accordingly
        //        await client.ConnectAsync("smtp.gmail.com", 587).ConfigureAwait(false);
        //        await client.AuthenticateAsync(credentials);

        //        foreach (var recipient in emailRecipients)
        //        {
        //            emailMessage.To.Add(new MailboxAddress(mailTo));
        //            await client.SendAsync(emailMessage).ConfigureAwait(false);
        //        }
        //    }
        //    await disconnectClient.DisconnectAsync(true).ConfigureAwait(false);
        //}

        #endregion
    }
}
