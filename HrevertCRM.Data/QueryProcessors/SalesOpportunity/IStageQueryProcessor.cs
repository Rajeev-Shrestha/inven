using HrevertCRM.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IStageQueryProcessor
    {

        Stage Save(Stage stage);
        Stage Update(Stage stage);
        int SaveAll(List<Stage> stages);
        bool Delete(int stageId);
        Stage GetStageById(int stageId);
        IQueryable<Stage> GetAll();
        IQueryable<Stage> GetAllActive();
    }
}
