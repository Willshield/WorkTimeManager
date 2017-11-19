using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.ViewModels
{
    public class ProfileUserControlViewModel : ViewModelBase
    {
        private readonly BllSettingsService bllSettingsService = BllSettingsService.Instance;

        private Profile profile;
        public Profile UserProfile
        {
            get { return profile; }
            set { Set(ref profile, value); }
        }

        private double hours;
        public double Hours
        {
            get { return hours; }
            set { Set(ref hours, value); }
        }

        public ProfileUserControlViewModel()
        {
            UserProfile = bllSettingsService.CurrentUser;
            SetHours();
        }

        public async void SetHours()
        {
            Hours = await WorkingTimeService.Instance.GetWorkingHoursToday();
        }
    }
}
