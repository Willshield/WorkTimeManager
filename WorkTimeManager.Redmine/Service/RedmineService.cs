using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.CommonInterfaces;
using WorkTimeManager.Redmine.Dto;
using WorkTimeManager.Redmine.Interfaces;

namespace WorkTimeManager.Redmine.Service
{
    public class RedmineService : INetworkDataService
    {
        //public readonly Uri serverUrl = new Uri("http://onlab.m.redmine.org");         //Todo: ?key=4f56fb8188c5f48811efe9a47b7ef50ad3443318
        private Uri serverUrl;
        public RedmineService(Uri _serverUrl)
        {
            serverUrl = _serverUrl;
        }

        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    T result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
                else
                {
                    throw new HttpRequestException("Network error: Loading data failed. Try again later.");
                }
            }
        }
        
        private async Task PostTAsync<T>(Uri uri, T t)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(t);
                HttpResponseMessage rpmsg = await client.PostAsync(uri, new StringContent(json, new UTF8Encoding(), "application/json"));
                if (rpmsg.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    throw new HttpRequestException("Network error: Sending data failed. Try again later.");
                }

            }
        }

        public async Task<List<WorkTimeManager.Model.Models.Issue>> GetIssuesAsync(string token)
        {
            token = getTokenString(token);
            return await FetchAll<WorkTimeManager.Model.Models.Issue, IssueListDto>($"issues.json?" + token);
        }

        public async Task<List<WorkTimeManager.Model.Models.Project>> GetProjectsAsync(string token)
        {
            token = getTokenString(token);
            return await FetchAll<WorkTimeManager.Model.Models.Project, ProjectListDto>($"projects.json?" + token);
        }

        public async Task<List<WorkTimeManager.Model.Models.WorkTime>> GetTimeEntriesAsync(string token, DateTime? from = null, DateTime? to = null)
        {
            token = getTokenString(token);
            return await FetchAll<WorkTimeManager.Model.Models.WorkTime, TimeEntryListDto>($"time_entries.json?" + token, getDateFilterString(from, to));
        }

        private async Task<List<TEntity>> FetchAll<TEntity,TDto>(string urlEnd, string additionalFilter = "") where TDto : IFetchableDto<TEntity>
        {
            var response = await GetAsync<TDto>(new Uri(serverUrl, urlEnd + additionalFilter));
            var returnList = response.ToEntityList();
            if (response.getTotalCount() > response.getFetchedCount())
            {
                var offset = response.getFetchedCount();
                while (offset < response.getTotalCount())
                {
                    var offsetFilter = getOffsetFilterString(offset);
                    var nextFetch = await GetAsync<TDto>(new Uri(serverUrl, urlEnd + additionalFilter + offsetFilter));
                    returnList.AddRange(nextFetch.ToEntityList());
                    offset += nextFetch.getFetchedCount();
                }
            }
            return returnList;
        }

        public async Task PostTimeEntry(string token, WorkTimeManager.Model.Models.WorkTime t)
        {
            var dto = new Post_Time_Entry(t, token);
            await PostTAsync<Post_Time_Entry>(new Uri(serverUrl, $"time_entries.json"), dto);
        }

        private string getTokenString(string token)
        {
            return "&key=" + token;
        }

        private string getOffsetFilterString(int offset)
        {
            return "&offset=" + offset;
        }

        private string getDateFilterString(DateTime? from, DateTime? to)
        {
            if(from == null || to == null)
            {
                return "";
            }
            return String.Join("", "&spent_on=><", from.Value.ToString("yyyy-MM-dd"), "|", to.Value.ToString("yyyy-MM-dd"));
        }
    }
}
