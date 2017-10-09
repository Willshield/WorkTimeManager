using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using WorkTimeManager.Dal.Context;
using WorkTimeManager.Model.Models;
using System.Collections.ObjectModel;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.DesignTimeServices;
using WorkTimeManager.Bll.Services;

namespace WorkTimeManager.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        IIssueService issueService;
        IWorkingTimeService workingTimeService;
        private ObservableCollection<Issue> list;
        public ObservableCollection<Issue> List
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
                getData();
                StartTrackingCommand = new DelegateCommand(StartTracking, CanStartTracking);
            }
        }
        public bool CanStartTracking()
        {
            return !(SelectedIssue == null);
        }

        private async void getData()
        {
            List = await issueService.GetFavouriteIssues();
        }

        public DelegateCommand StartTrackingCommand { get; }
        public async void StartTracking()
        {
            if (SelectedIssue == null)
                return;

            await issueService.StartTracking(SelectedIssue);
            //NavigationService.Navigate(typeof(Views.ActuallyTrackingPage)); //Todo: Uncomment

            WorkTime wt = new WorkTime();
            wt.IssueID = 1;
            wt.Hours = 1;
            wt.Comment = "posttest";
            await workingTimeService.AddTimeEntry(wt);
        }
        private Issue selectedIssue;
        public Issue SelectedIssue
        {
            get { return selectedIssue; }
            set
            {
                Set(ref selectedIssue, value);
                StartTrackingCommand.RaiseCanExecuteChanged();
            }
        }

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
                    { List = new ObservableCollection<Issue>(List.OrderByDescending(i => i.Subject)); }
                    else
                    { List = new ObservableCollection<Issue>(List.OrderBy(i => i.Subject)); }
                    break;
                case 1:
                    if (byDesc)
                    { List = new ObservableCollection<Issue>(List.OrderByDescending(i => i.Project.Name)); }
                    else
                    { List = new ObservableCollection<Issue>(List.OrderBy(i => i.Project.Name)); }
                    break;
                case 2:
                    if (byDesc)
                    { List = new ObservableCollection<Issue>(List.OrderByDescending(i => i.Updated)); }
                    else
                    { List = new ObservableCollection<Issue>(List.OrderBy(i => i.Updated)); }
                    break;
                case 3:
                    //Todo: own model with worktimes
                    if (byDesc)
                    { List = new ObservableCollection<Issue>(List.OrderByDescending(i => i.Description)); }
                    else
                    { List = new ObservableCollection<Issue>(List.OrderBy(i => i.Description)); }
                    break;
                    //if (byDesc)
                    //{ List = new ObservableCollection<Issue>(List.OrderByDescending(i => i.AllTrackedTime)); }
                    //else
                    //{ List = new ObservableCollection<Issue>(List.OrderBy(i => i.AllTrackedTime)); }
                    //break;
                case 4:
                    if (byDesc)
                    { List = new ObservableCollection<Issue>(List.OrderByDescending(i => i.Description)); }
                    else
                    { List = new ObservableCollection<Issue>(List.OrderBy(i => i.Description)); }
                    break;
            }
        }

        public async void Refresh()
        {
            List = await issueService.GetFavouriteIssues();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Refresh();
            return base.OnNavigatedToAsync(parameter, mode, state);

        }

    }

}
