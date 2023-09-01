using HrevertCRM.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
  public  interface IGradeQueryProcessor
    {
        Grade Save(Grade grade);
        Grade Update(Grade grade);
        int SaveAll(List<Grade> grades);
        bool Delete(int gradeId);
        Grade GetGradeById(int gradeId);
        IQueryable<Grade> GetAll();
        IQueryable<Grade> GetAllActive();
    }
}
