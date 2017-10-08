using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Model.Models
{
    public class Project
    {

        public int ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Navigation
        //todo:Subprojects
        public List<Issue> Issues { get; set; }
    }
}
