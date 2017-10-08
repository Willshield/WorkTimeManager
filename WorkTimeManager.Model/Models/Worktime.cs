using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Model.Models
{
    public class WorkTime
    {

        public int WorkTimeID { get; set; }
        public DateTime? StartTime { get; set; }
        public double Hours { get; set; }
        public string Comment { get; set; }
        public bool Dirty { get; set; }

        //navigációs
        public int IssueID { get; set; }
        public Issue Issue { get; set; }
    }
}
