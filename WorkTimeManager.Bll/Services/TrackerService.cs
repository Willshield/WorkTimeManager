using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll
{
    public class TrackerService
    {

        private static TrackerService instance = null;

        public delegate void ChangedEventHandler(TimeSpan t);
        public delegate void NewTrackingStartedEventHandler();
        public event ChangedEventHandler TimeChanged;
        public event NewTrackingStartedEventHandler NewTracking;

        private readonly IWorkingTimeService workingTimeService;
        private readonly BllSettingsService bllSettingsService;

        DispatcherTimer stopWatch;
        DispatcherTimer backupWatch;

        private WorkTime trackedTime;
        public Issue TrackedIssue { get; private set; }

        private TimeSpan time;
        public TimeSpan Time
        {
            get { return time; }
            private set
            {
                time = value;
                TimeChanged?.Invoke(value);
            }
        }

        public string Comment
        {
            get { return trackedTime.Comment; }
            set { trackedTime.Comment = value; }
        }

        public DateTime? StartTime
        {
            get { return trackedTime.StartTime; }
        }

        public string Priority
        {
            get { return TrackedIssue.Priority; }
        }

        public string IssueDescription
        {
            get { return TrackedIssue.Description; }
        }

        public string IssueTracker
        {
            get { return TrackedIssue.Tracker; }
        }

        public string ProjectName
        {
            get { return TrackedIssue.Project == null ? "" : TrackedIssue.Project.Name; }
        }

        public string IssueSubject
        {
            get { return TrackedIssue.Subject; }
        }

        public bool IsTracking { get { return stopWatch.IsEnabled; } }
        public bool Paused { get; private set; } = false;
        public bool HasPendingTrack { get { return IsTracking || Paused; } }

        private TrackerService()
        {
            SetStartValues();
            stopWatch = new DispatcherTimer();
            stopWatch.Tick += Stopwatch_Tick;
            stopWatch.Interval = new TimeSpan(0, 0, 1);

            backupWatch = new DispatcherTimer();
            backupWatch.Tick += Backupwatch_Tick;
            backupWatch.Interval = new TimeSpan(0, 1, 0);

            workingTimeService = WorkingTimeService.Instance;
            bllSettingsService = BllSettingsService.Instance;
        }

        public static TrackerService Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = new TrackerService();
                }

                return instance;
            }
        }

        private void SetStartValues()
        {
            time = new TimeSpan(0, 0, 0);
            var issue = new Issue();
            issue.IssueID = -1;
            TrackedIssue = issue;
            trackedTime = new WorkTime();
        }


        public void StartTracking(Issue issue)
        {
            if (!HasPendingTrack)
            {
                TrackedIssue = issue;
                trackedTime = new WorkTime();
                trackedTime.StartTime = DateTime.Now;
                trackedTime.Comment = "";
                trackedTime.IssueID = issue.IssueID;
                stopWatch.Start();
                backupWatch.Start();
                Paused = false;
                NewTracking();
            }
        }

        private void Stopwatch_Tick(object sender, object e)
        {
            Time = time.Add(new TimeSpan(0, 0, 1));
        }

        private void Backupwatch_Tick(object sender, object e)
        {
            var backupTime = trackedTime;
            backupTime.IssueID = TrackedIssue.IssueID;
            backupTime.Hours = ((double)time.Hours) + ((double)time.Minutes / 60.0) + ((double)time.Seconds / 3600.0);

            bllSettingsService.ActualTrackBackup = backupTime;
        }

        public async Task StopAndSaveTracking()
        {
            if (HasPendingTrack)
            {
                stopWatch.Stop();
                backupWatch.Stop();
                Paused = false;

                trackedTime.IssueID = TrackedIssue.IssueID;
                trackedTime.Hours = ((double)time.Hours) + ((double)time.Minutes / 60.0) + ((double)time.Seconds / 3600.0);
                await workingTimeService.AddTimeEntry(trackedTime);

                SetStartValues();
                NewTracking();
                bllSettingsService.ActualTrackBackup = null;
            }
        }

        public void AbortTracking()
        {
            if (HasPendingTrack)
            {
                stopWatch.Stop();
                backupWatch.Stop();
                Paused = false;
                SetStartValues();
                NewTracking();
                bllSettingsService.ActualTrackBackup = null;
            }

        }

        public void PauseTracking()
        {
            if (stopWatch.IsEnabled)
            {
                stopWatch.Stop();
                backupWatch.Stop();
                Backupwatch_Tick(null, null); //backup if paused
                Paused = true;
            }
        }

        public void RestartTracking()
        {
            if (!stopWatch.IsEnabled && Paused == true)
            {
                stopWatch.Start();
                backupWatch.Start();
                Paused = false;
            }
        }
       
    }
}
