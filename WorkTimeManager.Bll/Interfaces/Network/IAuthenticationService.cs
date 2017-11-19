using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.Interfaces.Network
{
    public interface IAuthenticationService
    {
        Task<Profile> GetProfile(string url, string connectionKey);
    }
}
