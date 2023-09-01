using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ITaskManagerQueryProcessor
    {
        TaskManager Insert(TaskManager taskManager);
        IQueryable<TaskManager> GetAll();
        IQueryable<TaskManager> GetAllActive();
        TaskManager GetTaskManagerByTaskId(int taskId);
        TaskManager Update(TaskManager taskManager);
        bool Delete(int  taskId);
        int SaveAll(List<TaskManager> taskManagers);
        bool ActiveTask(int id);
        PagedDataInquiryResponse<TaskManagerViewModel> SearchTasks(PagedDataRequest requestInfo, Expression<Func<TaskManager, bool>> @where = null);
        IQueryable<TaskManager> SearchActiveTasks(string searchText);
        IQueryable<TaskManager> SortTask(int sortTask);

    }
}
