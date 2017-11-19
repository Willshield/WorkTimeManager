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

        public Profile CurrentUser
        {
            get { return _helper.Read<Profile>(nameof(CurrentUser), null); }
            set { _helper.Write(nameof(CurrentUser), value); }
        }

    }
}
