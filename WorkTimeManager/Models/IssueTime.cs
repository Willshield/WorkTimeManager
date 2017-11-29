using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using WorkTimeManager.Bll;
using WorkTimeManager.Bll.Factories;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Services;

namespace WorkTimeManager.Models
{
    public class IssueTime
    {
        public int IssueID { get; set; }
        public string Tracker { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        private bool isFavourite { get; set; }
        public bool FavouriteSetter
        {
            get { return this.isFavourite; }
            set {
                isFavourite = value;
                Task.Run(() => { IssueService.Instance.SetFavourite(this.IssueID, value); });
            }
        }

        private readonly PopupService popupService;
        public string Priority { get; set; }
        public DateTime Updated { get; set; }

        //Navigation & counted
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public double AllTrackedTime { get; set; }

        //Actions
        public AwaitableDelegateCommand TrackingCommand { get { return new AwaitableDelegateCommand(StartTracking); } }
        public async Task StartTracking(AwaitableDelegateCommandParameter arg)
        {
            var tracker = new TrackingSafeStarterService();
            await tracker.AskStartTracking(this.ToEntity());    
        }

        //Ctors
        public IssueTime()
        {

        }

        public IssueTime(Issue issue, double trackedTime)
        {
            IssueID = issue.IssueID;
            Tracker = issue.Tracker;
            Subject = issue.Subject;
            Description = issue.Description;
            isFavourite = issue.IsFavourite;
            Priority = issue.Priority;
            Updated = issue.Updated;
            ProjectID = issue.ProjectID;
            Project = issue.Project;
            AllTrackedTime = trackedTime;
        }

        public Issue ToEntity()
        {
            return new Issue()
            {
                IssueID = this.IssueID,
                Tracker = this.Tracker,
                Subject = this.Subject,
                Description = this.Description,
                IsFavourite = this.isFavourite,
                Priority = this.Priority,
                Updated = this.Updated,
                ProjectID = this.ProjectID,
                Project = this.Project
            };
        }
    }
}
