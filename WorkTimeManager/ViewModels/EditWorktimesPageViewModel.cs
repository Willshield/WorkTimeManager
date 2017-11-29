using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll.DesignTimeServices;
using WorkTimeManager.Services;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.ViewModels
{
    public class EditWorktimesPageViewModel : ViewModelBase
    {
        private readonly IWorkingTimeService workingTimeService;
        private readonly IIssueService issueService;
        private readonly PopupService popupService = new PopupService();

        public List<Issue> Issues { get; set; }

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
                issueService = IssueService.Instance;

                RoundCommand = new DelegateCommand(RoundSelected);
                SaveChangesCommand = new DelegateCommand(SaveChanges);
                UndoChangesCommand = new DelegateCommand(UndoChanges);

                GetIssues();
            }
        }

        public async void GetIssues()
        {
            Issues = await issueService.GetIssues();
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
                RefreshDisplayedWorkTime();
            }
        }

        private void RefreshDisplayedWorkTime()
        {
            Comment = editWorkTime.Comment;
            StartTime = editWorkTime.StartTime.Value;
            Description = editWorkTime.Issue.Description;
            Priority = editWorkTime.Issue.Priority;
            IssueTracker = editWorkTime.Issue.Tracker;
            ProjectName = editWorkTime.Issue.Project.Name;
            Subject = editWorkTime.Issue.Subject;
            TrackedTime = editWorkTime.Hours;
        }

        private int issueID;
        public int IssueID {
            get { return issueID; }
            set {
                Set(ref issueID, value);
                var issue = Issues.Where(i => i.IssueID == value).Single();
                WorkTime freshIssueWt = EditWorkTime;
                freshIssueWt.IssueID = value;
                freshIssueWt.Issue = issue;
                EditWorkTime = freshIssueWt;
            }
        }

        public async void RoundSelected()
        {
            await workingTimeService.RoundWorktime(EditWorkTime.WorkTimeID);
        }

        public async void SaveChanges()
        {
            if (EditWorkTime.Hours <= 0)
            {
                var popup = popupService.GetDefaultNotification("There's some invalid edited workingtime. Use only numbers '.' and the wokingtime can't be zero or negative!", "Invalid edited item(s)");
                await popup.ShowAsync();
                return;
            }

            await workingTimeService.UpdateWorktime(EditWorkTime);
            NavigationService.GoBack();
        }

        public async void UndoChanges()
        {
            var popup = popupService.GetDefaultAskDialog("All changes will be lost. Are you sure?", "Undo confirmation", false);
            var cmd = await popup.ShowAsync();
            if (cmd.Label == PopupService.NO)
            {
                return;
            }
            EditWorkTime = await workingTimeService.GetWorkTime(EditWorkTime.WorkTimeID);
        }

        private string comment;
        public string Comment
        {
            get { return EditWorkTime?.Comment; }
            set
            {
                Set(ref comment, value);
                EditWorkTime.Comment = value;
            }
        }

        private double trackedTime;
        public double TrackedTime
        {
            get {
                if (EditWorkTime == null)
                    return 0;
                return EditWorkTime.Hours;
            }
            set
            {
                Set(ref trackedTime, value);
                EditWorkTime.Hours = value;
            }
        }

        private DateTimeOffset startTime;
        public DateTimeOffset StartTime
        {
            get
            {
                if (EditWorkTime == null)
                    return new DateTimeOffset();
                return EditWorkTime.StartTime.Value; }
            set {
                Set(ref startTime, value);
                EditWorkTime.StartTime = value.DateTime;
            }
        }

        private string priority;
        public string Priority
        {
            get { return EditWorkTime?.Issue.Priority; }
            private set
            {
                Set(ref priority, value);
            }
        }
        
        private string description;
        public string Description
        {
            get { return EditWorkTime?.Issue.Description; }
            private set
            {
                Set(ref description, value);
            }
        }

        private string issueTracker;
        public string IssueTracker
        {
            get { return EditWorkTime?.Issue.Tracker; }
            private set
            {
                Set(ref issueTracker, value);
            }
        }

        private string projectName;
        public string ProjectName
        {
            get { return EditWorkTime?.Issue.Project.Name; }
            private set
            {
                Set(ref projectName, value);
            }
        }

        private string subject;
        public string Subject
        {
            get { return EditWorkTime?.Issue.Subject; }
            private set
            {
                Set(ref subject, value);
            }
        }

    }
}
