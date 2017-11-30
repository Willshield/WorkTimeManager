using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.LocalDB.Context;
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.Services
{
    public class WorkingTimeService : IWorkingTimeService
    {
        private readonly BllSettingsService bllSettingsService = BllSettingsService.Instance;

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
                return await db.WorkTimes.Where(wt => wt.Dirty).Include(wt => wt.Issue).ThenInclude(i => i.Project).OrderByDescending(i => i.StartTime).ToListAsync();
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
            if (now.Year == checkedDay.Value.Year && now.Month == checkedDay.Value.Month && now.Day == checkedDay.Value.Day)
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

        public async Task RoundWorktime(int workTimeId)
        {
            using (var db = new WorkTimeContext())
            {
                var worktime = await db.WorkTimes.Where(wt => wt.WorkTimeID == workTimeId).SingleAsync();
                var rounding = (Rounding)bllSettingsService.RoundingTo;
                worktime.Hours = round(rounding, worktime.Hours, bllSettingsService.AlwaysUp);
                await db.SaveChangesAsync();
                bllSettingsService.SpareTime += SpareTimeChange;
            }
            SpareTimeChange = 0.0;
        }

        private double SpareTimeChange = 0.0;
        public async Task RoundDirtyWorktimes()
        {
            using (var db = new WorkTimeContext())
            {
                var worktimes = await db.WorkTimes.Where(wt => wt.Dirty).ToListAsync();
                var rounding = (Rounding)bllSettingsService.RoundingTo;
                foreach (var worktime in worktimes)
                {
                    worktime.Hours = round(rounding, worktime.Hours, bllSettingsService.AlwaysUp);
                }
                await db.SaveChangesAsync();
                bllSettingsService.SpareTime += SpareTimeChange;
            }
            SpareTimeChange = 0.0;
        }

        private double round(Rounding r, double d, bool onlyUp)
        {
            switch (r)
            {
                case Rounding.Round001:
                    var dr001 = Math.Round(d, 2);
                    if ((onlyUp && dr001 < d) || dr001 == 0.0)
                        dr001 += 0.01;

                    SpareTimeChange += d - dr001;
                    return dr001;

                case Rounding.Round005:
                    var dr005 = Math.Round(d * 2.0, 1);
                    dr005 /= 2.0;
                    if ((onlyUp && dr005 < d) || dr005 == 0.0)
                        dr005 += 0.05;

                    SpareTimeChange += d - dr005;
                    return dr005;

                case Rounding.Round010:
                    var dr010 = Math.Round(d, 1);
                    if ((onlyUp && dr010 < d) || dr010 == 0.0)
                        dr010 += 0.10;

                    SpareTimeChange += d - dr010;
                    return dr010;

                case Rounding.Round025:
                    var dr025 = Math.Round(d * 4.0, 0);
                    dr025 /= 4.0;
                    if ((onlyUp && dr025 < d) || dr025 == 0.0)
                        dr025 += 0.25;

                    SpareTimeChange += d - dr025;
                    return dr025;

                case Rounding.Round050:
                    var dr050 = Math.Round(d * 2.0, 0);
                    dr050 /= 2.0;
                    if ((onlyUp && dr050 < d) || dr050 == 0.0)
                        dr050 += 0.50;

                    SpareTimeChange += d - dr050;
                    return dr050;

                case Rounding.Round100:
                    var dr100 = Math.Round(d, 0);
                    if ((onlyUp && dr100 < d) || dr100 == 0.0)
                        dr100 += 1.00;

                    SpareTimeChange += d - dr100;
                    return dr100;
            }
            return 0;
        }

        public async Task MergeWorktimeWithDirty(int issueID)
        {
            using (var db = new WorkTimeContext())
            {
                var worktimes = await db.WorkTimes.Where(wt => wt.Dirty && wt.IssueID == issueID).ToListAsync();
                if (worktimes.Count <= 1)
                    return;
                MergeWorktimes(db, worktimes);

                await db.SaveChangesAsync();
            }
        }

        public async Task GroupMergeWorktimesWithDirty()
        {
            using (var db = new WorkTimeContext())
            {
                var worktimes = await db.WorkTimes.Where(wt => wt.Dirty).ToListAsync();
                if (worktimes.Count <= 1)
                    return;

                foreach (var issue in worktimes.GroupBy(wt => wt.IssueID))
                {
                    var issueWts = issue.ToList();
                    if (issueWts.Count > 1)
                        MergeWorktimes(db, issueWts);
                }

                await db.SaveChangesAsync();
            }
        }

        private void MergeWorktimes(WorkTimeContext db, List<WorkTime> issueWts)
        {
            WorkTime merged = issueWts.First();
            merged.Hours = issueWts.Sum(wt => wt.Hours);
            merged.Comment = String.Join("; ", issueWts.Select(wt => wt.Comment));
            db.WorkTimes.RemoveRange(issueWts.Where(wt => wt.WorkTimeID != merged.WorkTimeID));
        }

        public async Task UpdateWorktime(WorkTime workTime)
        {
            using (var db = new WorkTimeContext())
            {
                var originalHours = await db.WorkTimes.Where(wt => wt.WorkTimeID == workTime.WorkTimeID).Select(wt => wt.Hours).SingleAsync();
                bllSettingsService.SpareTime += originalHours - workTime.Hours;

                db.Update(workTime);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateWorktimes(IEnumerable<WorkTime> workTimes)
        {
            using (var db = new WorkTimeContext())
            {
                var originalHours = await db.WorkTimes.Where(wt => workTimes.Any(w => w.WorkTimeID == wt.WorkTimeID)).SumAsync(wt => wt.Hours);
                var newHours = workTimes.Sum(wt => wt.Hours);
                bllSettingsService.SpareTime += originalHours - newHours;

                db.UpdateRange(workTimes);
                await db.SaveChangesAsync();
            }
        }

        public async Task<WorkTime> GetWorkTime(int workTimeId)
        {
            using (var db = new WorkTimeContext())
            {
                return await db.WorkTimes.Where(wt => wt.WorkTimeID == workTimeId).Include(wt => wt.Issue).ThenInclude(i => i.Project).SingleAsync();
            }
        }

        public async Task<bool> GetIsAnyDirty()
        {
            using (var db = new WorkTimeContext())
            {
                return await db.WorkTimes.Where(wt => wt.Dirty).AnyAsync();
            }
        }

        public async Task DeleteWorktime(int workTimeId)
        {
            using (var db = new WorkTimeContext())
            {
                var deleteWT = await db.WorkTimes.Where(wt => wt.WorkTimeID == workTimeId).SingleAsync();
                bllSettingsService.SpareTime += deleteWT.Hours;
                db.WorkTimes.Remove(deleteWT);
                await db.SaveChangesAsync();
            }
            
        }
    }
}
