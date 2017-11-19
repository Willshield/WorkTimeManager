using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.CommonInterfaces;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Redmine.Service;

namespace WorkTimeManager.Bll.Services.Network
{
    public class AuthenticationService : IAuthenticationService
    {

        public AuthenticationService()
        { }

        public async Task<Profile> GetProfile(string url, string connectionKey)
        {
            INetworkDataService NetworkDataService = new RedmineService(new Uri(url));
            return await NetworkDataService.GetCurrentProfileAsync(connectionKey);         
        }
    }
}
