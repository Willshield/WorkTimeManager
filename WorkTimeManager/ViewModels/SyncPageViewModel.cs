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
        public delegate void EditListRefreshedEvent();
        public event EditListRefreshedEvent EditListRefreshed;

        IDbSynchronizationService Syncer;
        IWorkingTimeService workingTimeService;
        PopupService popupService = new PopupService();

        public DelegateCommand PushCommand { get; }
        public DelegateCommand PullCommand { get; }
        public DelegateCommand RoundAllCommand { get; }
        public DelegateCommand MergeAllCommand { get; }
        public DelegateCommand RoundCommand { get; }
        public DelegateCommand MergeCommand { get; }
        public DelegateCommand EditWorktimeCommand { get; }
        public DelegateCommand SaveChangesCommand { get; }
        public DelegateCommand UndoChangesCommand { get; }

        public SyncPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                workingTimeService = new DesignTimeDataService();
                RefreshFromLocal();
            }
            else
            {
                DirtyList = new ObservableCollection<WorkTime>();
                EditList = new ObservableCollection<WorkTime>();

                workingTimeService = WorkingTimeService.Instance;
                Syncer = DbSynchronizationService.Instance;

                PushCommand = new DelegateCommand(Push);
                PullCommand = new DelegateCommand(Pull);
                RoundAllCommand = new DelegateCommand(RoundWorktimes);
                MergeAllCommand = new DelegateCommand(MergeWorktimes, IsAllMergeable);
                RoundCommand = new DelegateCommand(RoundSelected, IsSelectionValid);
                MergeCommand = new DelegateCommand(MergeSelected, IsSelectedMergeable);
                EditWorktimeCommand = new DelegateCommand(EditWorktime, IsSelectionValid);
                SaveChangesCommand = new DelegateCommand(SaveChanges, IsEdited);
                UndoChangesCommand = new DelegateCommand(UndoChanges, IsEdited);

                RefreshFromLocal();
            }
        }

        public async void RefreshFromLocal()
        {
            var dbList = await workingTimeService.GetDirtyWorkTimes();
            if (dbList.Count != 0)
            {
                DirtyList = new ObservableCollection<WorkTime>(dbList);
                EditList = new ObservableCollection<WorkTime>(dbList);
                EditListRefreshed?.Invoke();
            }
            else
            {
                DirtyList = new ObservableCollection<WorkTime>();
                DirtyList.Add(new WorkTime() { Issue = new Issue() { Subject = "---No dirty time entries---", IssueID = 0 } });
                EditList = new ObservableCollection<WorkTime>();
            }
            NotifyListChanged();

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
            if (DirtyList[0].IssueID <= 0)
            {
                MessageDialog dialog = popupService.GetDefaultAskDialog("You have some unpushed worktimes. Pulling data overwrites all locally stored worktimes, unpushed data will be deleted.", "Confirm pull", false);

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

        private double spareTime;
        public double SpareTime
        {
            get { return spareTime; }
            set { Set(ref spareTime, value); }
        }

        private ObservableCollection<WorkTime> editList;
        public ObservableCollection<WorkTime> EditList
        {
            get { return editList; }
            set { Set(ref editList, value); }
        }

        private WorkTime editWorkTime;
        public WorkTime EditWorkTime
        {
            get { return editWorkTime; }
            set
            {
                Set(ref editWorkTime, value);
                NotifySelectionChanged();
            }
        }

        private void NotifySelectionChanged()
        {
            RoundCommand.RaiseCanExecuteChanged();
            MergeCommand.RaiseCanExecuteChanged();
            EditWorktimeCommand.RaiseCanExecuteChanged();
        }

        private void NotifyListChanged()
        {
            MergeAllCommand.RaiseCanExecuteChanged();
            NotifyEditingChanged();
            NotifySelectionChanged();
            SpareTime = BllSettingsService.Instance.SpareTime;
        }

        private void NotifyEditingChanged()
        {
            SaveChangesCommand.RaiseCanExecuteChanged();
            UndoChangesCommand.RaiseCanExecuteChanged();
        }

        public async void RoundWorktimes()
        {
            await workingTimeService.RoundDirtyWorktimes();
            RefreshFromLocal();
        }

        public async void MergeWorktimes()
        {
            await workingTimeService.GroupMergeWorktimesWithDirty();
            RefreshFromLocal();
        }

        public async void RoundSelected()
        {
            await workingTimeService.RoundWorktime(EditWorkTime.WorkTimeID);
            RefreshFromLocal();
        }

        public async void MergeSelected()
        {
            await workingTimeService.MergeWorktimeWithDirty(EditWorkTime.IssueID);
            RefreshFromLocal();
        }

        public async void SaveChanges()
        {
            if(EditList.Any(wt => wt.Hours == 0))
            {
                var popup = popupService.GetDefaultNotification("There's some invalid edited workingtime. Use only numbers '.' and the wokingtime can't be zero!", "Invalid edited item(s)");
                await popup.ShowAsync();
                return;
            }

            await workingTimeService.UpdateWorktimes(EditList);
            EditFinished();
        }

        public async void UndoChanges()
        {
            var popup = popupService.GetDefaultAskDialog("All changes will be lost. Are you sure?", "Undo confirmation", false);
            var cmd = await popup.ShowAsync();
            if (cmd.Label == PopupService.NO)
            {
                return;
            }
            EditFinished();
        }

        private void EditFinished()
        {
            isEdited = false;
            RefreshFromLocal();
        }

        private bool isEdited = false;
        public void CommentEdited()
        {
            isEdited = true;
            NotifyEditingChanged();
        }
        public void WorkHourEdited()
        {
            //todo: calculate spare time
            isEdited = true;
            NotifyEditingChanged();
        }


        public bool IsEdited()
        {
            return isEdited;
        }

        public bool IsAllMergeable()
        {
            if (EditList == null)
                return false;
            var checklist = EditList.OrderBy(e => e.IssueID).ToList();
            for (int i = 0; i < checklist.Count - 1; i++)
            {
                if (checklist[i].IssueID == checklist[i + 1].IssueID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSelectedMergeable()
        {
            if (IsSelectionValid())
            {
                var checklist = EditList.Where(e => e.IssueID == EditWorkTime.IssueID).ToList();
                return checklist.Count > 1;
            }
            return false;
        }


        public void EditWorktime()
        {
            NavigationService.Navigate(typeof(Views.EditWorktimes), EditWorkTime.WorkTimeID);
        }
        public bool IsSelectionValid()
        {
            if (EditWorkTime == null || EditWorkTime.IssueID <= 0)
            {
                return false;
            }
            return true;
        }


        #endregion


    }
}
