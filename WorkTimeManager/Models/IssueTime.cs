using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Models
{
    public class IssueTime
    {
        public int IssueID { get; set; }
        public string Tracker { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool Dirty { get; set; }
        public bool IsFavourite { get; set; }
        public string Priority { get; set; }
        public DateTime Updated { get; set; }

        //Navigation & counted
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public double AllTrackedTime { get; set; }

        public IssueTime()
        {

        }

        public IssueTime(Issue issue, double trackedTime)
        {
            IssueID = issue.IssueID;
            Tracker = issue.Tracker;
            Subject = issue.Subject;
            Description = issue.Description;
            Dirty = issue.Dirty;
            IsFavourite = issue.IsFavourite;
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
                Dirty = this.Dirty,
                IsFavourite = this.IsFavourite,
                Priority = this.Priority,
                Updated = this.Updated,
                ProjectID = this.ProjectID,
                Project = this.Project
            };
        }
    }
}
