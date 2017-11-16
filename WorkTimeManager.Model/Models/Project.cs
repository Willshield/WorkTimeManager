using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int ParentProjectID { get; set; }

        [ForeignKey(nameof(ParentProjectID))]
        public Project ParentProject { get; set; }

        public List<Project> ChildrenProjects { get; set; }

        public List<Issue> Issues { get; set; }
    }
}
