using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Dal.Context;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.Services
{
    public class IssueService : IIssueService
    {
        private static IssueService instance = null;
        IssueService()
        { }
        public static IssueService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IssueService();
                }

                return instance;
            }
        }

        public async Task<double> GetAllTrackedIssueTime(int IssueID)
        {
            using (var db = new WorkTimeContext())
            {
                return await db.WorkTimes.Where(wt => wt.IssueID == IssueID).SumAsync(wt => wt.Hours);
            }
        }

        public async Task<ObservableCollection<Issue>> GetFavouriteIssues()
        {
            using (var db = new WorkTimeContext())
            {
                return new ObservableCollection<Issue>(await db.Issues.Where(i => i.IsFavourite).Include(i => i.Project).ToListAsync());
            }
        }

        public async Task<Issue> GetIssueById(int IssueID)
        {
            using (var db = new WorkTimeContext())
            {
                return await db.Issues.Where(i => i.IssueID == IssueID).Include(i => i.Project).SingleAsync();
            }
        }

        public async Task<ObservableCollection<Issue>> GetIssues()
        {
            using (var db = new WorkTimeContext())
            {
                return new ObservableCollection<Issue>(await db.Issues.Include(i => i.Project).ToListAsync());
            }
        }

        public async Task SetFavourite(int IssueID, bool isFav)
        {
            using (var db = new WorkTimeContext())
            {
                Issue issue = db.Issues.Where(i => i.IssueID == IssueID).Include(i => i.Project).Single();
                issue.IsFavourite = isFav;
                db.SaveChanges();
            }
        }

        public async Task StartTracking(int IssueID)
        {
            throw new NotImplementedException();
        }

    }
}
