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

        public Task<ObservableCollection<WorkTime>> GetDirtyWorkTimes()
        {
            return GetWorkTimes();
        }

        public Task<ObservableCollection<Issue>> GetFavouriteIssues()
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

        public Task<ObservableCollection<Issue>> GetIssues()
        {
            ObservableCollection<Issue> list = new ObservableCollection<Issue>();

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

            list.Add(tmp);

            return Task.FromResult(list);

        }

        public Task<ObservableCollection<WorkTime>> GetWorkTimes()
        {
            ObservableCollection<WorkTime> list = new ObservableCollection<WorkTime>();

            WorkTime tmp = new WorkTime();
            tmp.WorkTimeID = 1;
            tmp.Hours = 1.0;
            tmp.StartTime = DateTime.Now;
            tmp.IssueID = 1;
            tmp.Comment = "Comment";

            list.Add(tmp);

            return Task.FromResult(list);
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
    }
}
