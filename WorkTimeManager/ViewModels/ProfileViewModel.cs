using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using WorkTimeManager.Bll.Factories;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Bll.Services.Network;
using WorkTimeManager.Views;

namespace WorkTimeManager.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        PopupService popupService;
        BllSettingsService settingService;
        IDbClearService dbClearService;
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
            popupService = new PopupService();
            SaveCommand = new DelegateCommand(SetProfile);
            dbClearService = DbSynchronizationService.Instance;
        }

        public async void SetProfile()
        {
            try
            {
                var url = new Uri(URL);
            } catch (UriFormatException)
            {
                popupService.GetDefaultNotification("Invalid url, saving data failed.", "Invalid url error").ShowAsync();
                return;
            }

            MessageDialog dialog = popupService.GetDefaultAskDialog("If you change user, you may have to refresh the database for proper usability. Do you want to refresh it now? (Favourite issues will be lost!)", "Database refresh required", true);

            var cmd = await dialog.ShowAsync();
            if (cmd.Label == PopupService.YES)
            {
                await dbClearService.ClearDb();
            }
            else if (cmd.Label == PopupService.CANCEL)
            {
                Key = settingService.UploadKey;
                URL = settingService.URL;
                ProfileName = settingService.ProfileName;
                return;
            } else
            {
                settingService.UploadKey = Key;
                settingService.URL = URL;
                settingService.ProfileName = ProfileName;
            }

        }
    }
}
