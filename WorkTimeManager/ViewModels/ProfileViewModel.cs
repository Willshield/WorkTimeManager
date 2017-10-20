using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WorkTimeManager.Bll.Factories;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Views;

namespace WorkTimeManager.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        BllSettingsService settingService;
        public DelegateCommand SaveCommand { get; }

        private string profileName;
        public string ProfileName
        {
            get { return profileName == null ? settingService.ProfileName : profileName; }
            set {
                Set(ref profileName, value);
            }
        }
        private string url;
        public string URL
        {
            get { return url == null ? settingService.URL : url; }
            set {
                Set(ref url, value);
            }
        }

        private string key;
        public string Key
        {
            get { return key == null ? settingService.UploadKey : key; }
            set {
                Set(ref key, value);
            }
        }


        public ProfileViewModel()
        {
            settingService = BllSettingsService.Instance;
            SaveCommand = new DelegateCommand(SetProfile);
        }

        public void SetProfile()
        {
            try
            {
                var url = new Uri(URL);
            } catch (UriFormatException)
            {
                var popup = new PopupService();
                popup.GetDefaultNotification("Invalid url, saving data failed.", "Invalid url error").ShowAsync();
                return;
            }

            settingService.UploadKey = Key;
            settingService.URL = URL;
            settingService.ProfileName = ProfileName;
        }
    }
}
