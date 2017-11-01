using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.DesignTimeServices
{
    public class DesignTimeDataService : IIssueService, IWorkingTimeService
    {
        public Task AddTimeEntry(WorkTime workTime)
        {
            throw new NotImplementedException();
        }

        public Task<Issue> GetActuallyTracked()
        {
            Issue tmp = new Issue();
            tmp.IssueID = 1;
            tmp.Tracker = "Tracker";
            tmp.Project = new Project();
            tmp.ProjectID = 1;
            tmp.Project.Name = "ProjectName";
            tmp.Subject = "Subject";
            tmp.Description = "Description";
            tmp.Priority = "Priority";
            tmp.Updated = DateTime.Now;
            return Task.FromResult(tmp);
        }

        public Task<double> GetAllTrackedIssueTime(int IssueID)
        {
            return Task.FromResult(10.4);
        }

        public Task<List<WorkTime>> GetDirtyWorkTimes()
        {
            return GetWorkTimes();
        }

        public Task<List<Issue>> GetFavouriteIssues()
        {
            return GetIssues();
        }

        public Task<Issue> GetIssueById(int id)
        {
            Issue tmp = new Issue();
            tmp.IssueID = 1;
            tmp.Tracker = "Tracker";
            tmp.Project = new Project();
            tmp.ProjectID = 1;
            tmp.Project.Name = "ProjectName";
            tmp.Subject = "Subject";
            tmp.Description = "Description";
            tmp.Priority = "Priority";
            tmp.Updated = DateTime.Now;
            return Task.FromResult(tmp);
        }

        public Task<List<Issue>> GetIssues()
        {
            List<Issue> list = new List<Issue>();

            Issue tmp = new Issue();
            tmp.IssueID = 1;
            tmp.Tracker = "Tracker";
            tmp.Project = new Project();
            tmp.ProjectID = 1;
            tmp.Project.Name = "ProjectName";
            tmp.Subject = "Subject";
            tmp.Description = "Description";
            tmp.Priority = "Priority";
            tmp.Updated = DateTime.Now;
            tmp.WorkTimes = new List<WorkTime>() { new WorkTime() { IssueID = 1, Dirty = false, Hours = 11, WorkTimeID = 1, Comment = "comment", StartTime = DateTime.Now } };

            list.Add(tmp);

            return Task.FromResult(list);

        }

        public Task<List<Issue>> GetIssuesWithWorkTimes()
        {
            return GetIssues();
        }

        public Task<double> GetWorkingHoursToday()
        {
            return Task.FromResult(7.5);
        }

        public Task<List<WorkTime>> GetWorkTimes()
        {
            List<WorkTime> list = new List<WorkTime>();

            WorkTime tmp = new WorkTime();
            tmp.WorkTimeID = 1;
            tmp.Hours = 1.0;
            tmp.StartTime = DateTime.Now;
            tmp.IssueID = 1;
            tmp.Comment = "Comment";

            list.Add(tmp);

            return Task.FromResult(list);
        }

        public Task GroupMergeWorktimesWithDirty()
        {
            throw new NotImplementedException();
        }

        public Task MergeWorktimeWithDirty(int workTimeId)
        {
            throw new NotImplementedException();
        }

        public Task RoundDirtyWorktimes()
        {
            throw new NotImplementedException();
        }

        public Task RoundWorktime(int workTimeId)
        {
            throw new NotImplementedException();
        }

        public Task SetFavourite(int id, bool isFavourite)
        {
            throw new NotImplementedException();
        }

        public Task StartTracking(int IssueID)
        {
            throw new NotImplementedException();
        }

        public Task StartTracking(Issue issue)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWorktime(WorkTime workTime)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWorktimes(IEnumerable<WorkTime> workTimes)
        {
            throw new NotImplementedException();
        }
    }
}
