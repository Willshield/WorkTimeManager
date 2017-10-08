using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Redmine.Dto
{
    public class TimeEntryListDto
    {
        public Time_Entries[] time_entries { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }

        public List<WorkTimeManager.Model.Models.WorkTime> ToEntityList()
        {
            var list = new List<WorkTimeManager.Model.Models.WorkTime>();
            for (int i = 0; i < total_count; i++)
            {
                var tmp = time_entries[i].ToEntity();
                list.Add(tmp);
            }
            return list;
        }
    }

    public class Time_Entries
    {
        public int id { get; set; }
        public ProjectOf project { get; set; }
        public IssueOf issue { get; set; }
        public User user { get; set; }
        public Activity activity { get; set; }
        public float hours { get; set; }
        public string comments { get; set; }
        public string spent_on { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }

        public WorkTimeManager.Model.Models.WorkTime ToEntity()
        {
            var tmp = new WorkTimeManager.Model.Models.WorkTime();
            tmp.WorkTimeID = id;
            tmp.Hours = hours;
            tmp.StartTime = (created_on).AddHours(-tmp.Hours);
            tmp.IssueID = issue.id;
            tmp.Comment = comments;
            tmp.Dirty = false;
            return tmp;
        }
    }

    public class IssueOf
    {
        public int id { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Activity
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Post_Time_Entry
    {
        public string key { get; set; }
        public Time_Entry time_entry { get; set; }

        public Post_Time_Entry()
        {

        }

        public Post_Time_Entry(WorkTimeManager.Model.Models.WorkTime wt)
        {
            Post_Time_Entry pt = new Post_Time_Entry();
            pt.time_entry = new Time_Entry();
            pt.time_entry.issue_id = wt.IssueID;
            pt.time_entry.hours = (float)wt.Hours;
            pt.time_entry.comments = wt.Comment;
            pt.time_entry.activity_id = 1; //Todo: miért const?
        }
    }


    public class Time_Entry
    {
        public int issue_id { get; set; }
        public float hours { get; set; }
        public int activity_id { get; set; }
        public string comments { get; set; }
    }
}
