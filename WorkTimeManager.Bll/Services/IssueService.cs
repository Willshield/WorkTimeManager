using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.Services
{
    public class IssueService : IIssueService
    {
        private static IssueService instance = null;
        IssueService()
        { }
        public static IssueService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IssueService();
                }

                return instance;
            }
        }

        public double GetAllTrackedIssueTime(int IssueID)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Issue>> GetFavouriteIssues()
        {
            throw new NotImplementedException();
        }

        public Task<Issue> GetIssueById(int IssueID)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Issue>> GetIssues()
        {
            throw new NotImplementedException();
        }

        public Task SetFavourite(int IssueID, bool isFav)
        {
            throw new NotImplementedException();
        }

        public Task StartTracking(int IssueID)
        {
            throw new NotImplementedException();
        }
    }
}
