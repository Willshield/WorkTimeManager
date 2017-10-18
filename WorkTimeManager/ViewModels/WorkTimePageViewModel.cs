using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool OrderbyDesc = false;
        IIssueService issueService;
        IWorkingTimeService workingTimeService;
        public List<WorktimeGroupBy> GroupByList { get; set; } = Enum.GetValues(typeof(WorktimeGroupBy)).Cast<WorktimeGroupBy>().ToList();

        public WorkTimePageViewModel()
        {
            RefreshCommand = new DelegateCommand(RefreshDbList);
            StartTrackingCommand = new DelegateCommand(StartTracking, CanStartTracking);
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var dtservice = new DesignTimeDataService();
                issueService = dtservice;
                workingTimeService = dtservice;
                RefreshDbList();
            }
            else
            {
                issueService = IssueService.Instance;
                workingTimeService = WorkingTimeService.Instance;
                RefreshDbList();
            }
            SelectedGroupBy = (WorktimeGroupBy) UISettingsService.Instance.WorktimeGroupBy;
        }


        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {

                Set(ref searchText, value);
                SelectedGroupBy = (WorktimeGroupBy)UISettingsService.Instance.WorktimeGroupBy;
            }
        }

        private void InitManipulatedListBySeatchtext()
        {
            if (searchText == null || searchText == "")
            {
                ManipulatedList = new ObservableCollection<WorkTime>(FromDbList);
            }
            else
            {
                ManipulatedList = new ObservableCollection<WorkTime>(FromDbList.Where(wt => wt.Issue.Subject.ToLower().Contains(searchText.ToLower())));
            }
        }

        private WorktimeGroupBy selectedGroupBy;
        public WorktimeGroupBy SelectedGroupBy
        {
            get { return selectedGroupBy; }
            set
            {
                Set(ref selectedGroupBy, value);
                UISettingsService.Instance.WorktimeGroupBy = value.GetHashCode();
                FilterOrderGroupby(); 
            }
        }
        
        private void FilterOrderGroupby()
        {
            InitManipulatedListBySeatchtext();
            GroupOrderItemsBy();
        }

        private void GroupOrderItemsBy()
        {
            if (ManipulatedList.Count == 0)
            {
                ManipulatedList.Add(new WorkTime() { Issue = new Issue() { Subject = "--- no results ---" } });
                return;
            }
            if (selectedGroupBy != WorktimeGroupBy.None )
            {
                List<WorkTime> newList = new List<WorkTime>();
                List<WorkTime> orderList = new List<WorkTime>();
                WorkTime lastItem = new WorkTime();
                lastItem = ManipulatedList[0];
                switch (selectedGroupBy)
                {
                    case WorktimeGroupBy.Day:
                        newList.Add(CreateDummy(lastItem.StartTime.Value.Date));
                        foreach (var item in ManipulatedList)
                        {
                            if (lastItem.StartTime.Value.Date != item.StartTime.Value.Date)
                            {
                                orderList = OrderGivenListCats(OrderbyDesc, orderList);
                                newList.AddRange(orderList);
                                orderList = new List<WorkTime>();
                                CreateNewGroupBy(newList, item);
                            }
                            orderList.Add(item);
                            lastItem = item;
                        }
                        newList.AddRange(orderList);
                        break;
                    case WorktimeGroupBy.Week:
                        newList.Add(CreateDummy(lastItem.StartTime.Value.Date));
                        foreach (var item in ManipulatedList)
                        {
                            int lastDayNum = (lastItem.StartTime.Value.DayOfWeek.GetHashCode() + 6) % 7;
                            int itemDayNum = (item.StartTime.Value.DayOfWeek.GetHashCode() + 6) % 7;
                            var ts = lastItem.StartTime.Value.Subtract(item.StartTime.Value);
                            if (ts.Days >= 7 || (lastDayNum < itemDayNum && ts.Days < 7))
                            {
                                orderList = OrderGivenListCats(OrderbyDesc, orderList);
                                newList.AddRange(orderList);
                                orderList = new List<WorkTime>();
                                CreateNewGroupBy(newList, item);
                            }
                            orderList.Add(item);
                            lastItem = item;
                        }
                        newList.AddRange(orderList);
                        break;
                    case WorktimeGroupBy.Month:
                        newList.Add(CreateDummy(lastItem.StartTime.Value.Date));
                        foreach (var item in ManipulatedList)
                        {
                            if ((lastItem.StartTime.Value.Date.Year == item.StartTime.Value.Date.Year && lastItem.StartTime.Value.Date.Month != item.StartTime.Value.Date.Month)
                             || (lastItem.StartTime.Value.Date.Year != item.StartTime.Value.Date.Year && lastItem.StartTime.Value.Date.Month != item.StartTime.Value.Date.Month))
                            {
                                orderList = OrderGivenListCats(OrderbyDesc, orderList);
                                newList.AddRange(orderList);
                                orderList = new List<WorkTime>();
                                CreateNewGroupBy(newList, item);
                            }
                            orderList.Add(item);
                            lastItem = item;
                        }
                        newList.AddRange(orderList);
                        break;
                    default:
                        return;
                }
                ManipulatedList = new ObservableCollection<WorkTime>(newList);
            } else
            {
                OrderCats(OrderbyDesc);
            }

        }
        private void CreateNewGroupBy(List<WorkTime> newList, WorkTime item)
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


        private ObservableCollection<WorkTime> manipulatedList;
        public ObservableCollection<WorkTime> ManipulatedList
        {
            get { return manipulatedList; }
            set
            {
                Set(ref manipulatedList, value);
            }
        }
        public List<WorkTime> FromDbList { get; set; }


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
        public void OrderCats()
        {
            SelectedGroupBy = (WorktimeGroupBy)UISettingsService.Instance.WorktimeGroupBy; //handles ordering
            OrderbyDesc = !OrderbyDesc;
        }
        private void OrderCats(bool byDesc)
        {
            if (SelectedGroupBy == WorktimeGroupBy.None)
            {
                switch (OrderCatName)
                {
                    case WorktimeOrderBy.Subject:
                        OrderByManipulate(byDesc, i => i.Issue.Subject);
                        break;
                    case WorktimeOrderBy.ProjectName:
                        OrderByManipulate(byDesc, i => i.Issue.Project.Name);
                        break;
                    case WorktimeOrderBy.StartTime:
                        OrderByManipulate(byDesc, i => i.StartTime);
                        break;
                    case WorktimeOrderBy.Hours:
                        OrderByManipulate(byDesc, i => i.Hours);
                        break;
                    case WorktimeOrderBy.Comment:
                        OrderByManipulate(byDesc, i => i.Comment);
                        break;
                }
            }
            
        }

        private List<WorkTime> OrderGivenListCats(bool byDesc, List<WorkTime> list)
        {
            switch (OrderCatName)
            {
                case WorktimeOrderBy.Subject:
                    return OrderListByManipulate(byDesc, list, i => i.Issue.Subject);
                case WorktimeOrderBy.ProjectName:
                    return OrderListByManipulate(byDesc, list, i => i.Issue.Project.Name);
                case WorktimeOrderBy.StartTime:
                    return OrderListByManipulate(byDesc, list, i => i.StartTime);
                case WorktimeOrderBy.Hours:
                    return OrderListByManipulate(byDesc, list, i => i.Hours);
                case WorktimeOrderBy.Comment:
                    return OrderListByManipulate(byDesc, list, i => i.Comment);
                default:
                    return list;
            }
        }

        private List<WorkTime> OrderListByManipulate(bool byDesc, List<WorkTime> list, Func<WorkTime, object> lambda)
        {
            if (byDesc)
            {
                return list.OrderByDescending(lambda).ToList();
            }
            else
            {
                return list.OrderBy(lambda).ToList();
            }
        }

        private void OrderByManipulate(bool byDesc, Func<WorkTime, object> lambda)
        {
            if (byDesc)
            {
                ManipulatedList = new ObservableCollection<WorkTime>(ManipulatedList.OrderByDescending(lambda));
            } else
            {
                ManipulatedList = new ObservableCollection<WorkTime>(ManipulatedList.OrderBy(lambda));
            }
        }

        public async void RefreshDbList()
        {
            FromDbList = await workingTimeService.GetWorkTimes();
        }
    }
}
