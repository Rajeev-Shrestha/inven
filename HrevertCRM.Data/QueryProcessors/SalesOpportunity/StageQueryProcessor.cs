using System.Collections.Generic;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class StageQueryProcessor : QueryBase<Stage>, IStageQueryProcessor
    {
        public StageQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {

        }
        public bool Delete(int stageId)
        {
            var doc = GetStageById(stageId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Stage>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<Stage> GetAll()
        {
            return _dbContext.Set<Stage>().Where(x=>x.CompanyId==LoggedInUser.CompanyId);
        }

        public IQueryable<Stage> GetAllActive()
        {
            return _dbContext.Set<Stage>().Include(x => x.SalesOpportunities).Where(FilterByActiveTrueAndCompany);

        }
        public Stage GetStageById(int stageId)
        {
            var stage = _dbContext.Set<Stage>().FirstOrDefault(x => x.Id == stageId);
            return stage ;
        }

        public Stage Save(Stage stage)
        {
            stage.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Stage>().Add(stage);
            _dbContext.SaveChanges();
            return stage;
        }
        public int SaveAll(List<Stage> stages)
        {
            _dbContext.Set<Stage>().AddRange(stages);
            return _dbContext.SaveChanges();
        }
        public Stage Update(Stage stage)
        {
            var original = GetStageById(stage.Id);
            ValidateAuthorization(stage);
            CheckVersionMismatch(stage, original);   //TODO: to test this method comment this out
            original.StageName = stage.StageName;
            original.Rank = stage.Rank;
            original.Probability = stage.Probability;
            original.Active = stage.Active;
            _dbContext.Set<Stage>().Update(original);
            _dbContext.SaveChanges();
            return stage;
        }
    }
}
