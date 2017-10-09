using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.CommonInterfaces;
using WorkTimeManager.Dal.Context;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Redmine.Service;

namespace WorkTimeManager.Bll.Services.Network
{
    public class DbSynchronizationService : IDbSynchronizationService
    {
        private static INetworkDataService NetworkDataService;
        private static DbSynchronizationService instance = null;
        DbSynchronizationService()
        {
            NetworkDataService = new RedmineService();
        }
        public static DbSynchronizationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbSynchronizationService();
                    instance.PullAll(); //Todo: kell?
                }

                return instance;
            }
        }

        public async Task PullAll()
        {
            try
            {
                using (var db = new WorkTimeContext())
                {
                    await PullProjects(db);
                    await PullIssues(db);
                    await PullTimeEntries(db);
                }
            }
            catch (HttpRequestException e)
            {
                await new MessageDialog("You are currently not connected to the internet. Syncing data failed. You can use offline mode. Error message: " + e.Message , "Network Error").ShowAsync();
            }
        }


        private async Task PullProjects(WorkTimeContext db)
        {
            foreach (var project in await NetworkDataService.GetProjectsAsync())
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
            foreach (var issue in await NetworkDataService.GetIssuesAsync())
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
            foreach (var timeEntry in await NetworkDataService.GetTimeEntriesAsync())
            {
                var exists = await db.WorkTimes.Where(w => w.WorkTimeID == timeEntry.WorkTimeID).SingleOrDefaultAsync();
                if (exists is null)
                {
                    db.WorkTimes.Add(timeEntry);
                }
                else
                {
                    //exists but can be changed
                    exists.IssueID = timeEntry.IssueID;
                    exists.WorkTimeID = timeEntry.WorkTimeID;
                    exists.StartTime = timeEntry.StartTime;

                    var issue = db.Issues.Where(i => i.IssueID == timeEntry.IssueID).Single();
                    exists.Issue = issue;
                    exists.Dirty = false;
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
                    var wtList = db.WorkTimes.Include(wt => wt.Issue).Include(i => i.Issue.Project).OrderByDescending(i => i.StartTime).ToList();
                    foreach (var wt in wtList)
                    {
                        if (wt.Dirty)
                        {
                            wt.Dirty = false;
                            await NetworkDataService.PostTimeEntry(wt, "4f56fb8188c5f48811efe9a47b7ef50ad3443318"); //todo: get key as parameter                        
                        }
                    }

                    await db.SaveChangesAsync();
                }
                catch (HttpRequestException e)
                {
                    await new MessageDialog("You are currently not connected to the internet. Syncing data failed. You can use offline mode. Error message: " + e.Message, "Network Error").ShowAsync();
                }
            }
        }

    }
}
