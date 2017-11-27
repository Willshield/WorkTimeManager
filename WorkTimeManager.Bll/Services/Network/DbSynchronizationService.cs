using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WorkTimeManager.Bll.Factories;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.NetworkInterfaces;
using WorkTimeManager.LocalDB.Context;
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Model.Exceptions;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Redmine.Service;

namespace WorkTimeManager.Bll.Services.Network
{
    public class DbSynchronizationService : IDbSynchronizationService
    {
        private readonly BllSettingsService bllSettingsService = BllSettingsService.Instance;
        private readonly PopupService popupService = new PopupService();
        private static INetworkDataService NetworkDataService;
        private string token { get { return bllSettingsService.CurrentUser?.ConnectionKey; } }

        public DbSynchronizationService()
        {
            if(bllSettingsService.CurrentUser != null)
            {
                NetworkDataService = new RedmineService(new Uri(bllSettingsService.CurrentUser.Url));
            } else
            {
                NetworkDataService = new RedmineService(new Uri($"http://www.redmine.org/"));
            }
        }

        public async Task PullAll()
        {      
            using (var db = new WorkTimeContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    await ResetWorktimes(db);

                    await PullProjects(db);
                    await PullIssues(db);
                    await PullTimeEntries(db);
                    transaction.Commit();
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
                }
            }
            await db.SaveChangesAsync();
        }

        private async Task PullTimeEntries(WorkTimeContext db)
        {

            var from = GetIntervalFrom();
            var times = await NetworkDataService.GetTimeEntriesAsync(token, bllSettingsService.CurrentUser?.ProfileID ?? 0, from, DateTime.Now);
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
                var wtList = db.WorkTimes.Where(wt=> wt.Dirty).Include(wt => wt.Issue).ThenInclude(i => i.Project).OrderByDescending(i => i.StartTime).ToList();
                foreach (WorkTimeManager.Model.Models.WorkTime wt in wtList)
                {
                    await NetworkDataService.PostTimeEntry(token, wt); 
                    wt.Dirty = false;
                    await db.SaveChangesAsync();
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

    }
}
