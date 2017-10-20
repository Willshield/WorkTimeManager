using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.CommonInterfaces
{
    public interface INetworkDataService
    {
        Task<List<Issue>> GetIssuesAsync();
        Task<List<Project>> GetProjectsAsync();
        Task<List<WorkTime>> GetTimeEntriesAsync(DateTime? from = null, DateTime? to = null);
        Task PostTimeEntry(WorkTime t, string UploadKey);
    }
}
