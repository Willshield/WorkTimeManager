﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.Interfaces
{
    public interface IWorkingTimeService
    {
        Task AddTimeEntry(WorkTime workTime);

        Task<double> GetWorkingHoursToday();

        Task<List<WorkTime>> GetWorkTimes();

        Task<WorkTime> GetWorkTime(int workTimeId);

        Task<List<WorkTime>> GetDirtyWorkTimes();

        Task<bool> GetIsAnyDirty();


        Task RoundWorktime(int workTimeId);

        Task RoundDirtyWorktimes();


        Task MergeWorktimeWithDirty(int issueID);

        Task GroupMergeWorktimesWithDirty();


        Task UpdateWorktime(WorkTime workTime);

        Task UpdateWorktimes(IEnumerable<WorkTime> workTimes);

        Task DeleteWorktime(int workTimeId);
    }
}
