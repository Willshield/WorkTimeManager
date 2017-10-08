using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Bll.Interfaces;
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

        public Task AddTimeEntry(WorkTime workTime)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<WorkTime>> GetDirtyWorkTimes()
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<WorkTime>> GetWorkTimes()
        {
            throw new NotImplementedException();
        }
    }
}
