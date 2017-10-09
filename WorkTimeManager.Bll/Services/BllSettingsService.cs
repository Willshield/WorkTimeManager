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

            //todo: load it
            //Profile = new Models.Profile() { Name = "Gáspár Vilmos", Url = "onlab.m.redmine.org" };
        }

        public Profile Profile
        {
            get
            {
                string name = _helper.Read<string>("ProfileName", "Gáspár Vilmos");
                string url = _helper.Read<string>("URL", "Gáspár Vilmos");
                return new Profile() { Name = name, Url = url };
            }
            set
            {
                _helper.Write("ProfileName", value.Name);
                _helper.Write("URL", value.Url);
            }
        }

        public int Rounding
        {
            get { return _helper.Read<int>(nameof(Rounding), 30); }
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

        public string UploadKey
        {
            get { return _helper.Read<string>(nameof(UploadKey), "4f56fb8188c5f48811efe9a47b7ef50ad3443318"); }
            set { _helper.Write(nameof(UploadKey), value); }
        }
    }
}
