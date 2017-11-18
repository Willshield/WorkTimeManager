using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.PopupService;
using WorkTimeManager.Bll.DesignTimeServices;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Bll.Services.Network;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.ViewModels
{
    public class EditWorktimesPageViewModel : ViewModelBase
    {
        IDbSynchronizationService Syncer;
        IWorkingTimeService workingTimeService;
        PopupService popupService = new PopupService();
        
        public DelegateCommand RoundCommand { get; }
        public DelegateCommand SaveChangesCommand { get; }
        public DelegateCommand UndoChangesCommand { get; }

        public EditWorktimesPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                workingTimeService = new DesignTimeDataService();
            }
            else
            {
                workingTimeService = WorkingTimeService.Instance;
                Syncer = DbSynchronizationService.Instance;
                
                RoundCommand = new DelegateCommand(RoundSelected);
                SaveChangesCommand = new DelegateCommand(SaveChanges);
                UndoChangesCommand = new DelegateCommand(UndoChanges);
            }
        }

        public async void SetEditedWorktime(int id)
        {
            EditWorkTime = await workingTimeService.GetWorkTime(id);
        }

        private WorkTime editWorkTime;
        public WorkTime EditWorkTime
        {
            get { return editWorkTime; }
            set
            {
                Set(ref editWorkTime, value);
            }
        }


        public async void RoundSelected()
        {
            await workingTimeService.RoundWorktime(EditWorkTime.WorkTimeID);
        }

        public async void SaveChanges()
        {
            //if (EditList.Any(wt => wt.Hours == 0))
            //{
            //    var popup = popupService.GetDefaultNotification("There's some invalid edited workingtime. Use only numbers '.' and the wokingtime can't be zero!", "Invalid edited item(s)");
            //    await popup.ShowAsync();
            //    return;
            //}

            //await workingTimeService.UpdateWorktimes(EditList);
        }

        public async void UndoChanges()
        {
            //var popup = popupService.GetDefaultAskDialog("All changes will be lost. Are you sure?", "Undo confirmation", false);
            //var cmd = await popup.ShowAsync();
            //if (cmd.Label == PopupService.NO)
            //{
            //    return;
            //}
            //EditFinished();
        }


        //public async void Push()
        //{
        //    Views.Busy.SetBusy(true, "Pushing worktimes...");
        //    await Syncer.PushAll();
        //    RefreshFromLocal();
        //    Views.Busy.SetBusy(false);
        //}

    }
}
