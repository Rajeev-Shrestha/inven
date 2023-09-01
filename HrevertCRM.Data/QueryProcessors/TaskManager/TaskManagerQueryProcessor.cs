using HrevertCRM.Entities;
using System.Linq;
using System.Linq.Dynamic.Core;
using Hrevert.Common.Security;
using System;
using System.Collections.Generic;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using System.Linq.Expressions;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.TaskManagerViewModel>;
using HrevertCRM.Data.Mapper;

namespace HrevertCRM.Data.QueryProcessors
{
    public class TaskManagerQueryProcessor : QueryBase<TaskManager>, ITaskManagerQueryProcessor
    {
        public TaskManagerQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public TaskManager Insert(TaskManager taskManager)
        {
            taskManager.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<TaskManager>().Add(taskManager);
            _dbContext.SaveChanges();
            return taskManager;
        }

        public IQueryable<TaskManager> GetAll()
        {
            return _dbContext.Set<TaskManager>().Where(FilterByActiveTrueAndCompany);
            //throw new NotImplementedException();
        }

        public TaskManager GetTaskManagerByTaskId(int taskId)
        {
            var taskmanager = _dbContext.Set<TaskManager>().FirstOrDefault(x => x.TaskId == taskId);
            return taskmanager;
        }

        public bool ActiveTask(int id)
        {
            var doc = GetTaskManagerByTaskId(id);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = true;
            _dbContext.Set<TaskManager>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public TaskManager Update(TaskManager taskManager)
        {
            var original = GetTaskManagerByTaskId(taskManager.TaskId);
            ValidateAuthorization(taskManager);
            CheckVersionMismatch(taskManager, original);   //TODO: to test this method comment this out
            original.TaskTitle = taskManager.TaskTitle;
            original.TaskDescription = taskManager.TaskDescription;
            original.TaskStartDateTime = taskManager.TaskStartDateTime;
            original.TaskEndDateTime = taskManager.TaskEndDateTime;
            original.TaskAssignedToUser = taskManager.TaskAssignedToUser;
            original.Status = taskManager.Status;
            original.Reminder = taskManager.Reminder;
            original.TaskPriority = taskManager.TaskPriority;
            original.DocType = taskManager.DocType;
            original.DocId = taskManager.DocId;
            original.Active = taskManager.Active;
            original.CompletePercentage = taskManager.CompletePercentage;
            _dbContext.Set<TaskManager>().Update(original);
            _dbContext.SaveChanges();
            return taskManager;
        }
        public bool Delete(int taskId)
        {
            var doc = GetTaskManagerByTaskId(taskId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<TaskManager>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<TaskManager> GetAllActive()
        {
            return _dbContext.Set<TaskManager>().Where(FilterByActiveTrueAndCompany);
        }

        public int SaveAll(List<TaskManager> taskManagers)
        {
            taskManagers.ForEach(x => x.TaskAssignedToUser = LoggedInUser.Id);
            _dbContext.Set<TaskManager>().AddRange(taskManagers);
            return _dbContext.SaveChanges();
        }

        public PagedDataInquiryResponse<TaskManagerViewModel> SearchTasks(PagedDataRequest requestInfo, Expression<Func<TaskManager, bool>> where = null)
        {

            var filteredTask = @where != null ? _dbContext.Set<TaskManager>().Where(@where).Where(w => w.CompanyId == LoggedInUser.CompanyId) : 
                _dbContext.Set<TaskManager>().Where(w => w.CompanyId == LoggedInUser.CompanyId);

            if (requestInfo.Active)
                filteredTask = filteredTask.Where(req => req.Active);

            if (!string.IsNullOrEmpty(requestInfo.SortColumn))
            {
                switch (requestInfo.SortColumn)
                {
                    case "StartDate":
                    case "StartTime":
                        filteredTask.OrderBy(t=>t.TaskStartDateTime + " " + requestInfo.SortOrder);
                        break;
                    case "EndDate":
                    case "EndTime":
                        filteredTask.OrderBy(t=>t.TaskEndDateTime + " " + requestInfo.SortOrder);
                        break;
                    default:
                        filteredTask.OrderBy(requestInfo.SortColumn + " " + requestInfo.SortOrder);
                        break;
                }
            }

            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredTask : filteredTask.Where(s
                                                                  => s.TaskTitle.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.DocId.ToUpper().Contains(requestInfo.SearchText.ToUpper()) || s.TaskDescription.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, query);
        }
        private PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<TaskManager> query)
        {
            var docs = new List<TaskManagerViewModel>();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            if (totalItemCount <= startIndex)
            {
                startIndex = 0;
            }

            var mapper = new TaskManagerToTaskManagerViewModelMapper();
            if (requestInfo.SortColumn == "Status" && requestInfo.SortOrder == "DESC")
            {
                docs = query.OrderByDescending(x => x.Status).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            }
            else if (requestInfo.SortColumn == "Status" && requestInfo.SortOrder == "ASC")
            {
                docs = query.OrderBy(x => x.Status).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            }

            else if (requestInfo.SortColumn == "TaskPriority" && requestInfo.SortOrder == "DESC")
            {
                docs = query.OrderByDescending(x => x.TaskPriority).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            }
            else if (requestInfo.SortColumn == "TaskPriority" && requestInfo.SortOrder == "ASC")
            {
                docs = query.OrderBy(x => x.TaskPriority).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            }
            else
            {
                docs = query.OrderByDescending(x => x.TaskStartDateTime).Skip(startIndex).Take(requestInfo.PageSize).Select(
               s => mapper.Map(s)).ToList();
            }
            var queryResult = new QueryResult<TaskManagerViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public IQueryable<TaskManager> SearchActiveTasks(string searchText)
        {
            var filteredTask = _dbContext.Set<TaskManager>().Where(FilterByActiveTrueAndCompany);
            var query = string.IsNullOrEmpty(searchText) ? filteredTask : filteredTask.Where(s
                                                                  => s.TaskTitle.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public IQueryable<TaskManager> SortTask(int sortTask)
        {
            var filteredTask = _dbContext.Set<TaskManager>().Where(FilterByActiveTrueAndCompany);
            if (sortTask == 1)
            {
                var query = filteredTask.OrderByDescending(t => t.TaskPriority);
                return query;
            }
            else
            {
                var query = filteredTask.OrderByDescending(t => t.TaskPriority).ThenByDescending(t => t.TaskStartDateTime);
                return query;
            }
        }
    }
}