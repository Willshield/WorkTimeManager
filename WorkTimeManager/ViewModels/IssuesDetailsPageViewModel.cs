using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Models;

namespace WorkTimeManager.ViewModels
{
    class IssuesDetailsPageViewModel : ViewModelBase
    {

        public static readonly int SubjectKey = 0;
        public static readonly int ProjectNameKey = 1;
        public static readonly int TrackerKey = 2;

        private ObservableCollection<IssueTime> list;
        public ObservableCollection<IssueTime> List
        {
            get { return list; }
            set { Set(ref list, value); }
        }

        IIssueService issueService;

        public IssuesDetailsPageViewModel()
        {

            issueService = IssueService.Instance;

            List = new ObservableCollection<IssueTime>();
            Refresh();
        }

        public async void Refresh()
        {
            var issueList = await issueService.GetFavouriteIssues();
            List = new ObservableCollection<IssueTime>(issueList.Select(i => new IssueTime(i, i.WorkTimes.Sum(t => t.Hours))).ToList());
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
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.Subject)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.Subject)); }
                    break;
                case 1:
                    if (byDesc)
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.Project.Name)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.Project.Name)); }
                    break;
                case 2:
                    if (byDesc)
                    { List = new ObservableCollection<IssueTime>(List.OrderByDescending(i => i.Tracker)); }
                    else
                    { List = new ObservableCollection<IssueTime>(List.OrderBy(i => i.Tracker)); }
                    break;
            }
        }

    }
}
