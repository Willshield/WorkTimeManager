using Windows.UI.Xaml;
using System.Threading.Tasks;
using WorkTimeManager.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using WorkTimeManager.Dal.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WorkTimeManager.Bll.Services.Network;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Bll.Factories;

namespace WorkTimeManager
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region app settings

            // some settings must be set in app.constructor
            var settings = UISettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;

            #endregion

            using (var db = new WorkTimeContext())
            {
                db.Database.Migrate();
            }

            
            
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            //Todo: check if crashed
            if(!(await WorkingTimeService.Instance.GetIsAnyDirty()))
            {
                await Task.Run(() =>
                {
                    return DbSynchronizationService.Instance.PullAll();
                });

                await NavigationService.NavigateAsync(typeof(Views.MainPage));
            } else
            {
                var popupService = new PopupService();
                popupService.GetDefaultNotification("You have some unsynchronized worktimes, that pulling data would delete. Automatic database sync aborted.", "Database sync failed").ShowAsync();
                await NavigationService.NavigateAsync(typeof(Views.SyncPage));
            }

        }
    }
}
