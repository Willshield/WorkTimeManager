using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Models
{
    public class EditableWorktime
    {

        private WorkTime original { get; set; }
        
        public DateTime? StartTime { get; set; }
        public double Hours { get; set; }
        public string Comment { get; set; }
        public int IssueID { get; set; }

        public EditableWorktime(WorkTime wt)
        {
            original = wt;
            StartTime = wt.StartTime;
            Hours = wt.Hours;
            Comment = wt.Comment;
            IssueID = wt.IssueID;
        }

        public WorkTime ToEntity()
        {
            original.StartTime = StartTime;
            original.Hours = Hours;
            original.Comment = Comment;
            original.IssueID = IssueID;
            return original;
        }
    }
}
