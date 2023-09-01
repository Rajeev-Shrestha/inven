using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HrevertCRM.Data
{
    public interface IDbContext
    { 
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        void Dispose();
        DatabaseFacade Database { get; }
    }
}
