using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.NetworkInterfaces
{
    public interface INetworkDataService
    {
        Task<Profile> GetCurrentProfileAsync(string token);
        Task<List<Issue>> GetIssuesAsync(string token);
        Task<List<Project>> GetProjectsAsync(string token);
        Task<List<WorkTime>> GetTimeEntriesAsync(string token, int userId, DateTime? from = null, DateTime? to = null);
        Task PostTimeEntry(string token, WorkTime t);
    }
}
