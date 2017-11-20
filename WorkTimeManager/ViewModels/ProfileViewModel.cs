using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using WorkTimeManager.Bll.Factories;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Bll.Services.Network;
using WorkTimeManager.Model.Exceptions;
using WorkTimeManager.Model.Models;
using WorkTimeManager.Views;

namespace WorkTimeManager.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        private readonly PopupService popupService;
        private readonly BllSettingsService settingService;
        private readonly IDbClearService dbClearService;
        private IAuthenticationService authenticationService;
        private IDbSynchronizationService dbSynchronizationService = null;
        public DelegateCommand SaveCommand { get; }

        private string url;
        public string URL
        {
            get { return url; }
            set {
                Set(ref url, value);
            }
        }

        private string key;
        public string Key
        {
            get { return key; }
            set {
                Set(ref key, value);
            }
        }

        private string profileName;
        public string ProfileName
        {
            get { return profileName; }
            set
            {
                Set(ref profileName, value);
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                Set(ref email, value);
            }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                Set(ref userName, value);
            }
        }



        public ProfileViewModel()
        {
            settingService = BllSettingsService.Instance;
            popupService = new PopupService();
            SaveCommand = new DelegateCommand(SetProfile);
            dbClearService = DbClearService.Instance;
            RefreshDisplayedProfile();
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
            } catch (ArgumentNullException)
            {
                popupService.GetDefaultNotification("Url is required", "Invalid url error").ShowAsync();
                return;
            }
            
            authenticationService = new AuthenticationService();
            try
            {
                Views.Busy.SetBusy(true, "Refreshing profile...");
                var profile = await authenticationService.GetProfile(URL, Key);
                profile.Url = URL;
                settingService.CurrentUser = profile;
                RefreshDisplayedProfile();
                Views.Busy.SetBusy(false);
            }
            catch (RequestStatusCodeException rex)
            {
                Views.Busy.SetBusy(false);
                popupService.GetDefaultNotification(rex.GetErrorMessage(), "Connection error").ShowAsync();
                return;
            }

            MessageDialog dialog = popupService.GetDefaultAskDialog("If you change user, you may have to refresh the database for proper usability. Do you want to refresh it now? (Favourite issues will be lost!)", "Database refresh required", false);

            var cmd = await dialog.ShowAsync();
            if (cmd.Label == PopupService.YES)
            {
                Views.Busy.SetBusy(true, "Refreshing database...");
                await dbClearService.ClearDb();
                dbSynchronizationService = new DbSynchronizationService();
                await dbSynchronizationService.PullAll();
                Views.Busy.SetBusy(false);
            }           

        }

        private void RefreshDisplayedProfile()
        {
            var currProfile = settingService.CurrentUser;
            UserName = currProfile?.UserName;
            Email = currProfile?.Email;
            ProfileName = currProfile?.Name;
            URL = currProfile?.Url;
            Key = currProfile?.ConnectionKey;
        }
    }
}
