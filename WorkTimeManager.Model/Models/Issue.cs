using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Model.Models
{
    public class Issue
    {
        public int IssueID { get; set; }
        public string Tracker { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool IsFavourite { get; set; }
        public string Priority { get; set; }
        public DateTime Updated { get; set; }

        //Navigation
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public List<WorkTime> WorkTimes { get; set; }
    }
}
