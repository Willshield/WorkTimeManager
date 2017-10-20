using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Redmine.Interfaces;

namespace WorkTimeManager.Redmine.Dto
{
    public class IssueListDto : IFetchableDto<WorkTimeManager.Model.Models.Issue>
    {
        public Issue[] issues { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }

        public int getFetchedCount()
        {
            return issues.Length;
        }

        public int getTotalCount()
        {
            return total_count;
        }

        public List<WorkTimeManager.Model.Models.Issue> ToEntityList()
        {
            var list = new List<WorkTimeManager.Model.Models.Issue>();
            for (int i = 0; i < issues.Length; i++)
            {
                var tmp = issues[i].ToEntity();
                list.Add(tmp);
            }
            return list;
        }
    }

    public class Issue
    {
        public int id { get; set; }
        public ProjectOf project { get; set; }
        public Tracker tracker { get; set; }
        public Status status { get; set; }
        public Priority priority { get; set; }
        public Author author { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public int done_ratio { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }

        public WorkTimeManager.Model.Models.Issue ToEntity()
        {
            var tmp = new WorkTimeManager.Model.Models.Issue();
            tmp.IssueID = id;
            tmp.Tracker = tracker.name;
            tmp.Project = new WorkTimeManager.Model.Models.Project();
            tmp.ProjectID = project.id;
            tmp.Project.Name = project.name; //removable
            tmp.Subject = subject;
            tmp.Description = description;
            tmp.Priority = priority.name;
            tmp.Updated = updated_on;
            return tmp;
        }
    }

    public class ProjectOf
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Tracker
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Priority
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
