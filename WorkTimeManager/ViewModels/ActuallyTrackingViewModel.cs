using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll;
using WorkTimeManager.Bll.Factories;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Services;

namespace WorkTimeManager.ViewModels
{
    public class ActuallyTrackingViewModel : ViewModelBase
    {

        private readonly PopupService popupService;
        private readonly Tracker tracker;
        private readonly IIssueService issueService;

        public delegate void ExecuteChangingDelegate();
        public event ExecuteChangingDelegate CanExecutesChanged;

        private string timeStamp;
        public string TimeStamp
        {
            get { return timeStamp; }
            set { Set(ref timeStamp, value); }
        }

        private string comment;
        public string Comment
        {
            get { return tracker.Comment; }
            set
            {
                Set(ref comment, value);
                tracker.Comment = value;
            }
        }

        public DateTime? StartTime
        {
            get { return tracker.StartTime; }
        }

        public string Priority
        {
            get { return tracker.Priority; }
        }

        public double AllWorkingTime
        {
            get
            {
                if (tracker.GetTrackedIssue() != null)
                {

                    return Task.Run(() => {

                        return issueService.GetAllTrackedIssueTime(tracker.GetTrackedIssue().IssueID);

                    }).Result;
                }
                else { return 0; }
            }
        }

        public string Description
        {
            get { return tracker.IssueDescription; }
        }

        public string IssueTracker
        {
            get { return tracker.IssueTracker; }
        }

        public string ProjectName
        {
            get { return tracker.ProjectName; }
        }

        public string Subject
        {
            get { return tracker.IssueSubject; }
        }


        public ActuallyTrackingViewModel()
        {
            popupService = new PopupService();
            AbortCommand = new DelegateCommand(AbortTracking, CanAbort);
            StopSaveCommand = new DelegateCommand(StopSaveTracking, CanStopSave);
            RestartCommand = new DelegateCommand(RestartTracking, CanRestart);
            PauseCommand = new DelegateCommand(PauseTracking, CanPause);

            issueService = IssueService.Instance;
            tracker = Tracker.Instance;
            timeStamp = "00:00:00";
            tracker.TimeChanged += TimeChangedEventHandler;
            tracker.NewTracking += RefreshDisplayedData;

            TimeStamp = tracker.Time.ToString();

            CanExecutesChanged += PauseCommand.RaiseCanExecuteChanged;
            CanExecutesChanged += RestartCommand.RaiseCanExecuteChanged;
            CanExecutesChanged += AbortCommand.RaiseCanExecuteChanged;
            CanExecutesChanged += StopSaveCommand.RaiseCanExecuteChanged;
        }

        public void RefreshDisplayedData()
        {
            Comment = "";
            TimeStamp = "00:00:00";

        }

        private void TimeChangedEventHandler(TimeSpan t)
        {
            TimeStamp = t.ToString();
        }

        public DelegateCommand AbortCommand { get; }
        public async void AbortTracking()
        {
            await tracker.AskAbortTracking();
            CanExecutesChanged.Invoke();
        }

        public bool CanAbort()
        {
            return tracker.IsTracking || tracker.Paused;
        }

        public DelegateCommand StopSaveCommand { get; }
        public async void StopSaveTracking()
        {
            await tracker.AskStopTracking();
            CanExecutesChanged.Invoke();
        }
        public bool CanStopSave()
        {
            return tracker.IsTracking || tracker.Paused;
        }

        public DelegateCommand RestartCommand { get; }
        public async void RestartTracking()
        {
            await tracker.RestartTracking();
            CanExecutesChanged.Invoke();
        }
        public bool CanRestart()
        {
            return (tracker.Paused && !tracker.IsTracking);
        }

        public DelegateCommand PauseCommand { get; }
        public void PauseTracking()
        {
            tracker.PauseTracking();
            CanExecutesChanged.Invoke();
        }
        public bool CanPause()
        {
            return tracker.IsTracking;
        }

    }
}
