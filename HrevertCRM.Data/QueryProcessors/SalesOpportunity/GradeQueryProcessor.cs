using System;
using System.Collections.Generic;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public class GradeQueryProcessor : QueryBase<Grade>, IGradeQueryProcessor
    {
        public GradeQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {

        }

        public bool Delete(int gradeId)
        {
            var doc = GetGradeById(gradeId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Grade>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<Grade> GetAll()
        {
            return _dbContext.Set<Grade>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
        }

        public IQueryable<Grade> GetAllActive()
        {
            return _dbContext.Set<Grade>().Where(FilterByActiveTrueAndCompany);
        }

        public Grade GetGradeById(int gradeId)
        {
            var grade = _dbContext.Set<Grade>().FirstOrDefault(x => x.Id == gradeId);
            return grade;
        }

        public Grade Save(Grade grade)
        {
            grade.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Grade>().Add(grade);
            _dbContext.SaveChanges();
            return grade;
        }

        public int SaveAll(List<Grade> grades)
        {
            _dbContext.Set<Grade>().AddRange(grades);
            return _dbContext.SaveChanges();
        }

        public Grade Update(Grade grade)
        {
            var original = GetGradeById(grade.Id);
            ValidateAuthorization(grade);
           CheckVersionMismatch(grade, original);   //TODO: to test this method comment this out
            original.GradeName = grade.GradeName;
            original.Rank = grade.Rank;
            original.Active = grade.Active;
            _dbContext.Set<Grade>().Update(original);
            _dbContext.SaveChanges();
            return grade;
        }
    }
}
