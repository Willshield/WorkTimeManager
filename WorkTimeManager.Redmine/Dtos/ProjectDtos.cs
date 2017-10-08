using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Redmine.Dto
{
    public class ProjectListDto
    {
        public Project[] projects { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }

        public List<WorkTimeManager.Model.Models.Project> ToEntityList()
        {
            var list = new List<WorkTimeManager.Model.Models.Project>();
            for (int i = 0; i < total_count; i++)
            {
                var tmp = projects[i].ToEntity();
                list.Add(tmp);
            }
            return list;
        }
    }

    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
        public string identifier { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public ProjectOf parent { get; set; }

        public WorkTimeManager.Model.Models.Project ToEntity()
        {
            var tmp = new WorkTimeManager.Model.Models.Project();
            tmp.Description = description;
            tmp.Name = name;
            tmp.ProjectID = id;
            return tmp;
        }
    }

}
