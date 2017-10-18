using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll.DesignTimeServices;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.ViewModels
{
    class SyncPageViewModel : ViewModelBase
    {

        IWorkingTimeService workingTimeService;
        private ObservableCollection<WorkTime> list;
        public ObservableCollection<WorkTime> List
        {
            get { return list; }
            set
            {
                Set(ref list, value);
            }
        }

        public SyncPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                workingTimeService = new DesignTimeDataService();
                Refresh();
            }
            else
            {
                workingTimeService = WorkingTimeService.Instance;
                Refresh();
            }
        }

        public async void Refresh()
        {
            List = new ObservableCollection<WorkTime>(await workingTimeService.GetDirtyWorkTimes());
        }

    }
}
