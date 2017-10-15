using System;
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

        Task<ObservableCollection<WorkTime>> GetWorkTimes();

        Task<ObservableCollection<WorkTime>> GetDirtyWorkTimes();
    }
}
