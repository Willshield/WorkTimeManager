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
    class TrackListViewModel : ViewModelBase
    {
        public static readonly int SubjectKey = 0;
        public static readonly int ProjectNameKey = 1;
        public static readonly int StartTimeKey = 2;
        public static readonly int HoursKey = 3;
        public static readonly int CommentKey = 4;

        private ObservableCollection<WorkTime> list;
        public ObservableCollection<WorkTime> List
        {
            get { return list; }
            set { Set(ref list, value); }
        }

        IIssueService issueService;
        IWorkingTimeService workingTimeService;

        public TrackListViewModel()
        {
            RefreshCommand = new DelegateCommand(Refresh);
            StartTrackingCommand = new DelegateCommand(StartTracking);
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                issueService = new DesignTimeDataService();
                Refresh();
            }
            else
            {
                issueService = IssueService.Instance;
                workingTimeService = WorkingTimeService.Instance;
                Refresh();
            }

        }

        public DelegateCommand StartTrackingCommand { get; }
        public async void StartTracking()
        {
            if (SelectedWorkTime == null)
                return;

            await issueService.StartTracking(await issueService.GetIssueById(SelectedWorkTime.IssueID));
            NavigationService.Navigate(typeof(Views.ActuallyTrackingPage));
        }
        public WorkTime SelectedWorkTime { get; set; }

        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand OrderCommand { get; }

        private int orderCatName;
        public int OrderCatName
        {
            get { return orderCatName; }
            set { orderCatName = value; }
        }
        public void OrderCats(bool byDesc)
        {
            switch (OrderCatName)
            {
                case 0:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Issue.Subject)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Issue.Subject)); }
                    break;
                case 1:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Issue.Project.Name)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Issue.Project.Name)); }
                    break;
                case 2:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.StartTime)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.StartTime)); }
                    break;
                case 3:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Hours)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Hours)); }
                    break;
                case 4:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Comment)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Comment)); }
                    break;
            }
        }


        public async void Refresh()
        {
            List = new ObservableCollection<WorkTime>(await workingTimeService.GetWorkTimes());
        }

    }
}
