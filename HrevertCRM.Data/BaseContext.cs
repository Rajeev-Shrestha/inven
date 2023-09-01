using System;
using System.Linq;
using Hrevert.Common.Security;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data
{
    public class BaseContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IUserSession _userSession;
        private readonly IServiceProvider _serviceProvider;

        public BaseContext(IUserSession userSession, IServiceProvider serviceProvider)
        {
            _userSession = userSession;
            _serviceProvider = serviceProvider;
            
        }

       
        public override int SaveChanges()
        {
            var loggedInUser = new ApplicationUser();
            var userQueryProcessor = (UserQueryProcessor) _serviceProvider.GetService(typeof(IUserQueryProcessor));
            try
            {
                //var user = userQueryProcessor.GetUser(a => a.Active).FirstOrDefault();
                loggedInUser = userQueryProcessor.GetUser(x => x.UserName == _userSession.Username).FirstOrDefault() ??
                               userQueryProcessor.GetUser(x => x.UserName == "admin@hrevertcrm.com").SingleOrDefault();
            }
            catch (Exception)
            {
                loggedInUser = userQueryProcessor.GetUser(x => x.UserName == "admin@hrevertcrm.com").SingleOrDefault();
            }

            foreach (var history in this.ChangeTracker.Entries()
         .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified)).Select(e => e.Entity as BaseEntity)
         )
            {
                //this.Entry(history).State = EntityState.Detached;
                //if (loggedInUser.CompanyId == 0)
                //    loggedInUser.CompanyId = 1;  //We have set the companyId to 7 for now, we will change it to 1.
                   
                history.LastUpdatedBy = loggedInUser.Id;
                history.DateModified = DateTime.Now;


                if (history.DateCreated != DateTime.MinValue) continue;
                //if(history.GetType() != typeof(Company))
                //    history.CompanyId = loggedInUser.CompanyId;
                history.Active = true;
                history.DateCreated = DateTime.Now;
                history.CreatedBy = loggedInUser.Id;
            }
            var result = base.SaveChanges();
            foreach (var history in this.ChangeTracker.Entries()
                                          .Where(e => e.Entity is IModificationHistory)
                                          .Select(e => e.Entity as IModificationHistory)
              )
            {
                //TODO: history.IsDirty = false;
            }
            return result;
        }
    }
}
