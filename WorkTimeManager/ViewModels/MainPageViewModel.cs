using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using WorkTimeManager.LocalDB.Context;
using WorkTimeManager.Model.Models;
using System.Collections.ObjectModel;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.DesignTimeServices;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Models;
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Services;

namespace WorkTimeManager.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IIssueService issueService;
        private readonly TrackingSafeStarterService trackingSafeStarterService;
        private readonly IWorkingTimeService workingTimeService;
        private ObservableCollection<IssueTime> list;
        public ObservableCollection<IssueTime> List
        {
            get { return list; }
            set { Set(ref list, value); }
        }


        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var designtimeService = new DesignTimeDataService();
                issueService = designtimeService;
                workingTimeService = designtimeService;
                getData();
            }
            else
            {
                issueService = IssueService.Instance;
                workingTimeService = WorkingTimeService.Instance;
                trackingSafeStarterService = new TrackingSafeStarterService();
                getData();
                StartTrackingCommand = new DelegateCommand(StartTracking, CanStartTracking);
            }
        }

        public bool CanStartTracking()
        {
            return (SelectedIssue != null && SelectedIssue.IssueID > 0);
        }

        private async void getData()
        {
            var issueList = await issueService.GetFavouriteIssues();
            List = new ObservableCollection<IssueTime>(issueList.Select(i => new IssueTime(i, i.WorkTimes.Sum(t => t.Hours))).ToList());

            if (List.Count == 0)
            {
                List.Add(new IssueTime() { IssueID = -1, Subject = "--- No favourite issues ---", Project = new Project() });
            }
        }

        public DelegateCommand StartTrackingCommand { get; }
        public async void StartTracking()
        {
            if (SelectedIssue == null)
                return;
            
            var started_new = await trackingSafeStarterService.AskStartTracking(SelectedIssue.ToEntity());
            if (started_new)
                NavigationService.Navigate(typeof(Views.ActuallyTrackingPage));
        }
        private IssueTime selectedIssue;
        public IssueTime SelectedIssue
        {
            get { return selectedIssue; }
            set
            {
                Set(ref selectedIssue, value);
                StartTrackingCommand.RaiseCanExecuteChanged();
            }
        }

        private WorktimeOrderBy orderCatName;
        public WorktimeOrderBy OrderCatName
        {
            get { return orderCatName; }
            set { orderCatName = value; }
        }
        public void OrderCats(bool byDesc)
        {
            switch (OrderCatName)
            {
                case WorktimeOrderBy.Subject:
                    if (byDesc)
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.Subject)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.Subject)); }
                    break;
                case WorktimeOrderBy.ProjectName:
                    if (byDesc)
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.Project.Name)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.Project.Name)); }
                    break;
                case WorktimeOrderBy.StartTime:
                    if (byDesc)
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.Updated)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.Updated)); }
                    break;
                case WorktimeOrderBy.Hours:
                    if (byDesc)
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.AllTrackedTime)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.AllTrackedTime)); }
                    break;
                case WorktimeOrderBy.Comment:
                    if (byDesc)
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.Description)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.Description)); }
                    break;
            }
        }

        public void Refresh()
        {
            getData();
        }
        
    }

}
