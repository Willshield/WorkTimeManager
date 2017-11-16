using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.CommonInterfaces;
using WorkTimeManager.Dal.Context;
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Redmine.Service;

namespace WorkTimeManager.Bll.Services.Network
{
    public class DbSynchronizationService : IDbSynchronizationService, IDbClearService
    {
        private static INetworkDataService NetworkDataService;
        private static DbSynchronizationService instance = null;
        private string token { get { return BllSettingsService.Instance.UploadKey; } }
        DbSynchronizationService()
        {
            NetworkDataService = new RedmineService(new Uri(BllSettingsService.Instance.URL));
        }
        public static DbSynchronizationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbSynchronizationService();
                }

                return instance;
            }
        }

        public async Task PullAll()
        {
            
            using (var db = new WorkTimeContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        await ResetWorktimes(db);

                        await PullProjects(db);
                        await PullIssues(db);
                        await PullTimeEntries(db);
                        transaction.Commit();
                    }
                    catch (HttpRequestException e)
                    {
                        await new MessageDialog("You are currently not connected to the internet. Syncing data failed. You can use offline mode. Error message: " + e.Message, "Network Error").ShowAsync();
                    }
                }
            }
        }

        private async Task ResetWorktimes(WorkTimeContext db)
        {
            db.WorkTimes.RemoveRange(db.WorkTimes);
            await db.SaveChangesAsync();
        }

        private async Task PullProjects(WorkTimeContext db)
        {
            var projectList = await NetworkDataService.GetProjectsAsync(token);
            foreach (var project in projectList)
            {
                var exists = await db.Projects.Where(p => p.ProjectID == project.ProjectID).SingleOrDefaultAsync();
                if (exists is null)
                {
                    db.Projects.Add(project);
                }
                else
                {
                    //exists but can be changed
                    exists.ProjectID = project.ProjectID;
                    exists.Name = project.Name;
                    exists.Description = project.Description;
                }
            }
            await db.SaveChangesAsync();
        }

        private async Task PullIssues(WorkTimeContext db)
        {
            foreach (var issue in await NetworkDataService.GetIssuesAsync(token))
            {
                var exists = await db.Issues.Where(i => i.IssueID == issue.IssueID).SingleOrDefaultAsync();
                if (exists is null)
                {
                    var project = db.Projects.Where(p => p.ProjectID == issue.ProjectID).Single();
                    issue.Project = project;
                    db.Issues.Add(issue);
                }
                else
                {
                    //exists but can change
                    exists.Updated = issue.Updated;
                    exists.Tracker = issue.Tracker;
                    exists.Subject = issue.Subject;
                    exists.ProjectID = issue.ProjectID;
                    exists.Priority = issue.Priority;
                    exists.IssueID = issue.IssueID;
                    exists.Description = issue.Description;

                    var project = db.Projects.Where(p => p.ProjectID == issue.ProjectID).Single();
                    exists.Project = project;
                    exists.Dirty = false;
                }
            }
            await db.SaveChangesAsync();
        }

        private async Task PullTimeEntries(WorkTimeContext db)
        {

            var from = GetIntervalFrom();
            var times = await NetworkDataService.GetTimeEntriesAsync(token, from, DateTime.Now);
            foreach (var timeEntry in times)
            {
                var exists = await db.WorkTimes.Where(w => w.WorkTimeID == timeEntry.WorkTimeID).SingleOrDefaultAsync();
                if (exists is null)
                {
                    db.WorkTimes.Add(timeEntry);
                }
            }
            await db.SaveChangesAsync();
        }

        public async Task PushAll()
        {
            using (var db = new WorkTimeContext())
            {
                try
                {
                    var wtList = db.WorkTimes.Where(wt=> wt.Dirty).Include(wt => wt.Issue).ThenInclude(i => i.Project).OrderByDescending(i => i.StartTime).ToList();
                    foreach (WorkTimeManager.Model.Models.WorkTime wt in wtList)
                    {
                        await NetworkDataService.PostTimeEntry(token, wt); 
                        wt.Dirty = false;
                        await db.SaveChangesAsync();
                    }
                    
                }
                catch (HttpRequestException e)
                {
                    await new MessageDialog("You are currently not connected to the internet. Syncing data failed. You can use offline mode. Error message: " + e.Message, "Network Error").ShowAsync();
                }
            }
        }

        private DateTime? GetIntervalFrom()
        {
            var loadinterval = (DataLoadInterval) BllSettingsService.Instance.PullLastNDays;

            switch (loadinterval)
            {
                case DataLoadInterval.IsLastWeek:
                    return DateTime.Now.AddDays(-7);
                case DataLoadInterval.IsLastTwoWeek:
                    return DateTime.Now.AddDays(-14);
                case DataLoadInterval.IsLastMonth:
                    return DateTime.Now.AddMonths(-1);
                case DataLoadInterval.IsLastTwoMonth:
                    return DateTime.Now.AddMonths(-2);
                case DataLoadInterval.IsLastYear:
                    return DateTime.Now.AddYears(-1);
                case DataLoadInterval.IsAll:
                    return null;
                default:
                    return DateTime.Now;
            }
        }

        public async Task ClearDb()
        {
            using (var db = new WorkTimeContext())
            {
                db.WorkTimes.RemoveRange(db.WorkTimes);
                db.Issues.RemoveRange(db.Issues);
                db.Projects.RemoveRange(db.Projects);
                await db.SaveChangesAsync();
            }
        }
    }
}
