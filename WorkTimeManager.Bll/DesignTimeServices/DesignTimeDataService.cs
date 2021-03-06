﻿using System;
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
            return Task.CompletedTask;
        }

        public Task DeleteWorktime(int workTimeId)
        {
            return Task.CompletedTask;
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

        public Task<bool> GetIsAnyDirty()
        {
            return Task.FromResult(false);
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

        public async Task<WorkTime> GetWorkTime(int id)
        {
            return (await GetWorkTimes()).First();
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
            return Task.CompletedTask;
        }

        public Task MergeWorktimeWithDirty(int workTimeId)
        {
            return Task.CompletedTask;
        }

        public Task RoundDirtyWorktimes()
        {
            return Task.CompletedTask;
        }

        public Task RoundWorktime(int workTimeId)
        {
            return Task.CompletedTask;
        }

        public Task SetFavourite(int id, bool isFavourite)
        {
            return Task.CompletedTask;
        }

        public Task StartTracking(int IssueID)
        {
            return Task.CompletedTask;
        }

        public Task StartTracking(Issue issue)
        {
            return Task.CompletedTask;
        }

        public Task UpdateWorktime(WorkTime workTime)
        {
            return Task.CompletedTask;
        }

        public Task UpdateWorktimes(IEnumerable<WorkTime> workTimes)
        {
            return Task.CompletedTask;
        }
    }
}
