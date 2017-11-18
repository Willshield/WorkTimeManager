using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Bll.Services
{
    public class BllSettingsService
    {
        public static BllSettingsService Instance { get; } = new BllSettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;
        private BllSettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public WorkTime ActualTrackBackup
        {
            get { return _helper.Read<WorkTime>(nameof(ActualTrackBackup), null); }
            set { _helper.Write(nameof(ActualTrackBackup), value); }
        }

        public double SpareTime
        {
            get { return _helper.Read<double>(nameof(SpareTime), 0.0); }
            set { _helper.Write(nameof(SpareTime), value); }
        }

        public int RoundingTo
        {
            get { return _helper.Read<int>(nameof(RoundingTo), Rounding.Round025.GetHashCode()); }
            set { _helper.Write(nameof(RoundingTo), value); }
        }

        public int PullLastNDays
        {
            get { return _helper.Read<int>(nameof(PullLastNDays), DataLoadInterval.IsLastTwoWeek.GetHashCode()); }
            set { _helper.Write(nameof(PullLastNDays), value); }
        }

        public bool AskIfStop
        {
            get { return _helper.Read<bool>(nameof(AskIfStop), false); }
            set { _helper.Write(nameof(AskIfStop), value); }
        }

        public bool AlwaysUp
        {
            get { return _helper.Read<bool>(nameof(AlwaysUp), true); }
            set { _helper.Write(nameof(AlwaysUp), value); }
        }

        public string ProfileName
        {
            get { return _helper.Read<string>(nameof(ProfileName), "Gáspár Vilmos"); }
            set { _helper.Write(nameof(ProfileName), value); }
        }

        public string URL
        {
            get { return _helper.Read<string>(nameof(URL), "http://szakdolgozat.m.redmine.org"); }
            set { _helper.Write(nameof(URL), value); }
        }

        public string UploadKey
        {
            get { return _helper.Read<string>(nameof(UploadKey), "4f56fb8188c5f48811efe9a47b7ef50ad3443318"); }
            set { _helper.Write(nameof(UploadKey), value); }
        }
    }
}
