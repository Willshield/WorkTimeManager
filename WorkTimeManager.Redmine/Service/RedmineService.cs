﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.CommonInterfaces;
using WorkTimeManager.Redmine.Dto;

namespace WorkTimeManager.Redmine.Service
{
    public class RedmineService : INetworkDataService
    {
        public readonly Uri serverUrl = new Uri("http://onlab.m.redmine.org");

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

                return default(T);
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

        public async Task<List<WorkTimeManager.Model.Models.Issue>> GetIssuesAsync()
        {
            return (await GetAsync<IssueListDto>(new Uri(serverUrl, $"issues.json"))).ToEntityList();
        }

        public async Task<List<WorkTimeManager.Model.Models.Project>> GetProjectsAsync()
        {
            return (await GetAsync<ProjectListDto>(new Uri(serverUrl, $"projects.json"))).ToEntityList();
        }

        public async Task<List<WorkTimeManager.Model.Models.WorkTime>> GetTimeEntriesAsync()
        {
            return (await GetAsync<TimeEntryListDto>(new Uri(serverUrl, $"time_entries.json"))).ToEntityList();
        }

        public async Task PostTimeEntry(WorkTimeManager.Model.Models.WorkTime t, string UploadKey)
        {
            var dto = new Post_Time_Entry(t);
            dto.key = UploadKey;
            await PostTAsync<Post_Time_Entry>(new Uri(serverUrl, $"time_entries.json"), dto);
        }
    }
}