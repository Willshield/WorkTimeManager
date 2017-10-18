using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.Interfaces
{
    public interface IIssueService
    {
        Task<List<Issue>> GetIssues();

        Task<List<Issue>> GetIssuesWithWorkTimes();        

        Task<List<Issue>> GetFavouriteIssues();

        Task<Issue> GetIssueById(int IssueID);

        Task SetFavourite(int IssueID, bool isFav);

        Task<double> GetAllTrackedIssueTime(int IssueID);

        Task StartTracking(Issue issue);


    }
}
