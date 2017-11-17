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
using WorkTimeManager.Models;
using WorkTimeManager.Model.Enums.Extensions;
using WorkTimeManager.Model.Enums;

namespace WorkTimeManager.ViewModels
{
    class IssuesDetailsPageViewModel : ViewModelBase
    {

        private bool OrderbyDesc = false;
        public static readonly int SubjectKey = 0;
        public static readonly int ProjectNameKey = 1;
        public static readonly int TrackerKey = 2;

        private string loadInterval;
        public string LoadInterval
        {
            get { return loadInterval; }
            set { Set(ref loadInterval, value); }
        }

        private readonly IIssueService issueService;

        public IssuesDetailsPageViewModel()
        {
            
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                issueService = new DesignTimeDataService();
            }
            else
            {
                issueService = IssueService.Instance;
                LoadInterval = ((DataLoadInterval)BllSettingsService.Instance.PullLastNDays).GetDisplayName();
            }
            Refresh();
            ManipulateList = new ObservableCollection<IssueTime>(FromDbList);
        }

        public List<IssueTime> FromDbList { get; set; }
        private ObservableCollection<IssueTime> list;
        public ObservableCollection<IssueTime> ManipulateList
        {
            get { return list; }
            set { Set(ref list, value); }
        }


        private int orderCatName;
        public int OrderCatName
        {
            get { return orderCatName; }
            set
            {
                Set(ref orderCatName, value);
                FilterOrderList();
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {

                Set(ref searchText, value);
                FilterOrderList();
            }
        }

        private bool onlyWithTimelog;
        public bool OnlyWithTimelog
        {
            get { return onlyWithTimelog; }
            set
            {
                Set(ref onlyWithTimelog, value);
                FilterOrderList();
            }
        }

        public async void Refresh()
        {
            var issueList = await issueService.GetIssuesWithWorkTimes();
            FromDbList = issueList.Select(i => new IssueTime(i, i.WorkTimes.Sum(t => t.Hours))).ToList();
        }

        private void FilterOrderList()
        {
            if (SearchText == null || SearchText == "")
            {
                if (OnlyWithTimelog)
                {
                    OrderCats(FromDbList.Where(i => i.AllTrackedTime != 0.0));
                }
                else
                {
                    OrderCats(FromDbList);
                }
            }
            else
            {
                if (OnlyWithTimelog)
                {
                    OrderCats(FromDbList.Where(i => (i.AllTrackedTime != 0.0 && i.Subject.ToLower().Contains(SearchText.ToLower()))));
                }
                else
                {
                    OrderCats(FromDbList.Where(i => i.Subject.ToLower().Contains(SearchText.ToLower())));
                }
            }
        }

        private void OrderCats(IEnumerable<IssueTime> filteredList)
        {
            switch (OrderCatName)
            {
                case 0:
                    if (OrderbyDesc)
                    { ManipulateList = new ObservableCollection<IssueTime>(filteredList.OrderByDescending(i => i.Subject)); }
                    else
                    { ManipulateList = new ObservableCollection<IssueTime>(filteredList.OrderBy(i => i.Subject)); }
                    break;
                case 1:
                    if (OrderbyDesc)
                    { ManipulateList = new ObservableCollection<IssueTime>(filteredList.OrderByDescending(i => i.Project.Name)); }
                    else
                    { ManipulateList = new ObservableCollection<IssueTime>(filteredList.OrderBy(i => i.Project.Name)); }
                    break;
                case 2:
                    if (OrderbyDesc)
                    { ManipulateList = new ObservableCollection<IssueTime>(filteredList.OrderByDescending(i => i.Tracker)); }
                    else
                    { ManipulateList = new ObservableCollection<IssueTime>(filteredList.OrderBy(i => i.Tracker)); }
                    break;
            }

            OrderbyDesc = !OrderbyDesc;
        }

    }
}
