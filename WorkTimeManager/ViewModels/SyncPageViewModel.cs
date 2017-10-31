using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using WorkTimeManager.Bll.DesignTimeServices;
using WorkTimeManager.Bll.Factories;
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
        public DelegateCommand RoundCommand { get; }
        public DelegateCommand MergeCommand { get; }
        public DelegateCommand EditWorktimeCommand { get; }

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
                RoundCommand = new DelegateCommand(RoundWorktimes);
                MergeCommand = new DelegateCommand(MergeWorktimes);
                EditWorktimeCommand = new DelegateCommand(EditWorktime);
                RefreshFromLocal();
            }
        }

        public async void RefreshFromLocal()
        {
            var dbList = await workingTimeService.GetDirtyWorkTimes();
            if (dbList.Count != 0)
            {
                DirtyList = new ObservableCollection<WorkTime>(dbList);
                PivotEnabled = true;
                EditList = new ObservableCollection<WorkTime>(dbList);
            }
            else
            {
                DirtyList = new ObservableCollection<WorkTime>();
                DirtyList.Add(new WorkTime() { Issue = new Issue() { Subject = "---No dirty time entries---", IssueID = 0 } });
                PivotEnabled = false;
                EditList = null;
            }
        }

        #region first pivot

        private ObservableCollection<WorkTime> dirtyList;
        public ObservableCollection<WorkTime> DirtyList
        {
            get { return dirtyList; }
            set
            {
                Set(ref dirtyList, value);
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
            if (DirtyList[0].IssueID != -1)
            {
                PopupService p = new PopupService();
                MessageDialog dialog = p.GetDefaultAskDialog("You have some unpushed worktimes. Pulling data overwrites all locally stored worktimes, unpushed data will be deleted.", "Confirm pull", false);

                var cmd = await dialog.ShowAsync();
                if (cmd.Label == PopupService.NO)
                {
                    return;
                }
            }

            Views.Busy.SetBusy(true, "Pulling data...");
            await Syncer.PullAll();
            RefreshFromLocal();
            Views.Busy.SetBusy(false);
        }
        #endregion  

        #region second pivot

        private bool pivotEnabled;
        public bool PivotEnabled {
                get { return pivotEnabled; }
                set { Set(ref pivotEnabled, value); }
            }

        private ObservableCollection<WorkTime> editList;
        public ObservableCollection<WorkTime> EditList
        {
            get { return editList; }
            set { Set(ref editList, value); }
        }

        private WorkTime editWorkTime;
        public WorkTime EditWorkTime {
            get { return editWorkTime;  }
            set {
                Set(ref editWorkTime, value);
                //Raise executechanged events
            }
        }

        public void RoundWorktimes()
        {

        }

        public void MergeWorktimes()
        {

        }

        public void EditWorktime()
        {
            NavigationService.Navigate(typeof(Views.EditWorktimes), EditWorkTime.WorkTimeID);
        }


        #endregion


    }
}
