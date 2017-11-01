using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Profile Profile
        {
            get  { return new Profile() { Name = ProfileName, Url = URL }; }
            set
            {
                ProfileName = value.Name;
                URL = value.Url;
            }
        }

        public int Rounding
        {
            get { return _helper.Read<int>(nameof(Rounding), 25); }
            set { _helper.Write(nameof(Rounding), value); }
        }

        public bool AutoTrack
        {
            get { return _helper.Read<bool>(nameof(AutoTrack), false); }
            set { _helper.Write(nameof(AutoTrack), value); }
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
            get { return _helper.Read<string>("ProfileName", "Gáspár Vilmos"); }
            set { _helper.Write("ProfileName", value); }
        }

        public string URL
        {
            get { return _helper.Read<string>("URL", "http://onlab.m.redmine.org"); }
            set { _helper.Write("URL", value); }
        }

        public string UploadKey
        {
            get { return _helper.Read<string>(nameof(UploadKey), "4f56fb8188c5f48811efe9a47b7ef50ad3443318"); }
            set { _helper.Write(nameof(UploadKey), value); }
        }
    }
}
