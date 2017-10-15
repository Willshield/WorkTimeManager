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
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Services.SettingsServices;

namespace WorkTimeManager.ViewModels
{
    public class WorkTimePageViewModel : ViewModelBase
    {

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { Set(ref searchText, value); }
        }


        IIssueService issueService;
        IWorkingTimeService workingTimeService;
        public List<WorktimeGroupBy> GroupByList { get; set; } = Enum.GetValues(typeof(WorktimeGroupBy)).Cast<WorktimeGroupBy>().ToList();

        public WorkTimePageViewModel()
        {
            RefreshCommand = new DelegateCommand(Refresh);
            StartTrackingCommand = new DelegateCommand(StartTracking, CanStartTracking);
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var dtservice = new DesignTimeDataService();
                issueService = dtservice;
                workingTimeService = dtservice;
                Refresh();
            }
            else
            {
                issueService = IssueService.Instance;
                workingTimeService = WorkingTimeService.Instance;
                Refresh();
            }
            SelectedGroupBy = (WorktimeGroupBy) UISettingsService.Instance.WorktimeGroupBy;

        }

        private WorktimeGroupBy selectedGroupBy;
        public WorktimeGroupBy SelectedGroupBy
        {
            get { return selectedGroupBy; }
            set
            {
                Set(ref selectedGroupBy, value);
                UISettingsService.Instance.WorktimeGroupBy = value.GetHashCode();
                //OrderCatName = StartTimeKey; ToDo: disable order other cats?
                //OrderCats(true);
                GroupItemsBy();
            }
        }

        private void GroupItemsBy()
        {
            Refresh();
            if (selectedGroupBy != WorktimeGroupBy.None)
            {
                ObservableCollection<WorkTime> newList = new ObservableCollection<WorkTime>();
                WorkTime lastItem = new WorkTime();
                lastItem = List[0];
                switch (selectedGroupBy)
                {
                    case WorktimeGroupBy.Day:
                        newList.Add(CreateDummy(lastItem.StartTime.Value.Date));
                        foreach (var item in List)
                        {
                            if (lastItem.StartTime.Value.Date != item.StartTime.Value.Date)
                            {
                                CreateNewGroupBy(newList, item);
                            }
                            newList.Add(item);
                            lastItem = item;
                        }
                        break;
                    case WorktimeGroupBy.Week:
                        newList.Add(CreateDummy(lastItem.StartTime.Value.Date));
                        foreach (var item in List)
                        {
                            int lastDayNum = (lastItem.StartTime.Value.DayOfWeek.GetHashCode() + 6) % 7;
                            int itemDayNum = (item.StartTime.Value.DayOfWeek.GetHashCode() + 6) % 7;
                            var ts = lastItem.StartTime.Value.Subtract(item.StartTime.Value);
                            if (ts.Days >= 7 || (lastDayNum < itemDayNum && ts.Days < 7))
                            {
                                CreateNewGroupBy(newList, item);
                            }
                            newList.Add(item);
                            lastItem = item;
                        }
                        break;
                    case WorktimeGroupBy.Month:
                        newList.Add(CreateDummy(lastItem.StartTime.Value.Date));
                        foreach (var item in List)
                        {
                            if ((lastItem.StartTime.Value.Date.Year == item.StartTime.Value.Date.Year && lastItem.StartTime.Value.Date.Month != item.StartTime.Value.Date.Month)
                             || (lastItem.StartTime.Value.Date.Year != item.StartTime.Value.Date.Year && lastItem.StartTime.Value.Date.Month != item.StartTime.Value.Date.Month))
                            {
                                CreateNewGroupBy(newList, item);
                            }
                            newList.Add(item);
                            lastItem = item;
                        }
                        break;
                    default:
                        return;
                }
                List = newList;

            }

        }
        private void CreateNewGroupBy(ObservableCollection<WorkTime> newList, WorkTime item)
        {
            newList.Add(CreateEmptyDummy());
            newList.Add(CreateDummy(item.StartTime.Value.Date));
        }
        private WorkTime CreateEmptyDummy()
        {
            WorkTime dummy = new WorkTime();
            dummy.Issue = new Issue();
            dummy.Issue.Project = new Project();
            return dummy;
        }
        private WorkTime CreateDummy(DateTime thisDate)
        {
            WorkTime dummy = new WorkTime();
            dummy.Issue = new Issue();
            dummy.IssueID = -1;
            dummy.Issue.Project = new Project();
            string align = "     ";
            switch (selectedGroupBy)
            {
                case WorktimeGroupBy.Day:
                    if (thisDate.Date == DateTime.Now.Date)
                    {
                        dummy.Issue.Subject = align + "Today:";
                    }
                    else
                    {
                        dummy.Issue.Subject = align + "On " + thisDate.Date.Year + "-" + thisDate.Date.Month + "-" + thisDate.Date.Day + ":";
                    }
                    break;
                case WorktimeGroupBy.Week:
                    //int lastdayNum = (lastDate.DayOfWeek.GetHashCode() + 1) % 7;
                    int dayNum = (thisDate.DayOfWeek.GetHashCode() + 6) % 7;
                    var date = thisDate.Date.AddDays(-dayNum);
                    if (DateTime.Now.CompareTo(date) == -1 || DateTime.Now.CompareTo(date.AddDays(7)) == 1)
                        dummy.Issue.Subject = align + "On the week started on " + date.Date.Year + "-" + date.Date.Month + "-" + date.Date.Day + ":";
                    else
                        dummy.Issue.Subject = align + "This week:";
                    break;
                case WorktimeGroupBy.Month:
                    if (thisDate.Date.Year == DateTime.Now.Year && thisDate.Date.Month == DateTime.Now.Month)
                    {
                        dummy.Issue.Subject = align + "This month:";
                    }
                    else
                    {
                        dummy.Issue.Subject = align + "On " + thisDate.Date.Year + "-" + thisDate.Date.Month + ":";
                    }
                    break;
            }

            return dummy;
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
        private WorkTime selectedWorktime;
        public WorkTime SelectedWorkTime
        {
            get { return selectedWorktime; }
            set
            {
                Set(ref selectedWorktime, value);
                StartTrackingCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand StartTrackingCommand { get; }
        public async void StartTracking()
        {
            await issueService.StartTracking(await issueService.GetIssueById(SelectedWorkTime.IssueID));
            NavigationService.Navigate(typeof(Views.ActuallyTrackingPage));
        }
        public bool CanStartTracking()
        {
            if (SelectedWorkTime == null)
                return false;
            if (SelectedWorkTime.Hours == 0)
                return false;

            return true;
        }



        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand OrderCommand { get; }
        private WorktimeOrderBy orderCatName;
        public WorktimeOrderBy OrderCatName
        {
            get { return orderCatName; }
            set { orderCatName = value; }
        }
        public void OrderCats(bool byDesc)
        {
            SelectedGroupBy = WorktimeGroupBy.None;
            switch (OrderCatName)
            {
                case WorktimeOrderBy.Subject:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Issue.Subject)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Issue.Subject)); }
                    break;
                case WorktimeOrderBy.ProjectName:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Issue.Project.Name)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Issue.Project.Name)); }
                    break;
                case WorktimeOrderBy.StartTime:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.StartTime)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.StartTime)); }
                    break;
                case WorktimeOrderBy.Hours:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Hours)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Hours)); }
                    break;
                case WorktimeOrderBy.Comment:
                    if (byDesc)
                    { List = new ObservableCollection<WorkTime>(List.OrderByDescending(i => i.Comment)); }
                    else
                    { List = new ObservableCollection<WorkTime>(List.OrderBy(i => i.Comment)); }
                    break;
            }
        }


        public async void Refresh()
        {
            List = await workingTimeService.GetWorkTimes();
        }
    }
}
