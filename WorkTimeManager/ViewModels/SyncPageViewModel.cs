using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll.DesignTimeServices;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Bll.Services.Network;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.ViewModels
{
    class SyncPageViewModel : ViewModelBase
    {
        IDbSynchronizationService Syncer;
        IWorkingTimeService workingTimeService;

        public DelegateCommand PushCommand { get; }
        public DelegateCommand PullCommand { get; }

        public SyncPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                workingTimeService = new DesignTimeDataService();
                RefreshFromLocal();
            }
            else
            {
                workingTimeService = WorkingTimeService.Instance;
                Syncer = DbSynchronizationService.Instance;
                PushCommand = new DelegateCommand(Push);
                PullCommand = new DelegateCommand(Pull);
                RefreshFromLocal();
            }
        }

        private ObservableCollection<WorkTime> list;
        public ObservableCollection<WorkTime> List
        {
            get { return list; }
            set
            {
                Set(ref list, value);
            }
        }


        public async void RefreshFromLocal()
        {
            var dbList = await workingTimeService.GetDirtyWorkTimes();
            if(dbList.Count != 0)
            {
                List = new ObservableCollection<WorkTime>(dbList);
            } else
            {
                List = new ObservableCollection<WorkTime>();
                List.Add(new WorkTime() { Issue = new Issue() { Subject = "---No dirty time entries---" } });
            }
        }

        public async void Push()
        {
            Views.Busy.SetBusy(true, "Pushing worktimes...");
            await Syncer.PushAll();
            RefreshFromLocal();
            Views.Busy.SetBusy(false);
        }

        public async void Pull()
        {
            Views.Busy.SetBusy(true, "Pulling data...");
            await Syncer.PullAll();
            Views.Busy.SetBusy(false);
        }

    }
}
