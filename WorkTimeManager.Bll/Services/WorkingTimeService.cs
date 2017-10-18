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
    public class WorkingTimeService : IWorkingTimeService
    {
        private static WorkingTimeService instance = null;
        WorkingTimeService() { }
        public static WorkingTimeService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkingTimeService();
                }

                return instance;
            }
        }

        public async Task AddTimeEntry(WorkTime workTime)
        {
            using (var db = new WorkTimeContext())
            {
                var issue = await db.Issues.Where(i => i.IssueID == workTime.IssueID).SingleAsync();
                workTime.Issue = issue;
                workTime.Dirty = true;
                db.WorkTimes.Add(workTime);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<WorkTime>> GetDirtyWorkTimes()
        {
            using (var db = new WorkTimeContext())
            {
                return await db.WorkTimes.Include(wt => wt.Issue).Where(wt => wt.Dirty).Include(i => i.Issue.Project).OrderByDescending(i => i.StartTime).ToListAsync();
            }
            
        }

        public double GetWorkingHoursTodaySync()
        {
            using (var db = new WorkTimeContext())
            {
                return db.WorkTimes.Where(wt => OnToday(wt.StartTime)).Sum(wt => wt.Hours);
            }
        }

        public async Task<double> GetWorkingHoursToday()
        {
            using (var db = new WorkTimeContext())
            {
                return await db.WorkTimes.Where(wt => OnToday(wt.StartTime)).SumAsync(wt => wt.Hours);
            }
        }

        private bool OnToday(DateTime? checkedDay)
        {
            if (checkedDay == null)
                return false;
            var now = DateTime.Now;
            if(now.Year == checkedDay.Value.Year && now.Month == checkedDay.Value.Month && now.Day == checkedDay.Value.Day)
            {
                return true;
            }
            return false;
        }

        public async Task<List<WorkTime>> GetWorkTimes()
        {
            using (var db = new WorkTimeContext())
            {
                return await db.WorkTimes.Include(wt => wt.Issue).Include(i => i.Issue.Project).OrderByDescending(i => i.StartTime).ToListAsync();
            }
        }

    }
}
