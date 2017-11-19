﻿using Windows.UI.Xaml;
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
        private readonly PopupService popupService = new PopupService();

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
            if (BllSettingsService.Instance.CurrentUser == null)
            {
                await NavigationService.NavigateAsync(typeof(Views.ProfilePage));
                popupService.GetDefaultNotification("It seems that this is your first time you use the application. Set profile data to get started!", "Set profile").ShowAsync();
                return;
            }

            var RecoveryWorkTime = BllSettingsService.Instance.ActualTrackBackup;
            if (RecoveryWorkTime == null)
            {
                if (!(await WorkingTimeService.Instance.GetIsAnyDirty()))
                {
                    await Task.Run(() =>
                    {
                        var syncer = new DbSynchronizationService();
                        return syncer.PullAll();
                    });

                    await NavigationService.NavigateAsync(typeof(Views.MainPage));
                }
                else
                {
                    await NavigationService.NavigateAsync(typeof(Views.SyncPage));
                    popupService.GetDefaultNotification("You have some unsynchronized worktimes, that pulling data would delete. Automatic database sync aborted.", "Database sync failed").ShowAsync();
                }
            } else
            {
                await WorkingTimeService.Instance.AddTimeEntry(RecoveryWorkTime);
                await NavigationService.NavigateAsync(typeof(Views.SyncPage));
                popupService.GetDefaultNotification("It seems that the app crashed while tracking. A backup is recovered and added to your worktimes list.", "Crash recovery").ShowAsync();
                BllSettingsService.Instance.ActualTrackBackup = null;
            }

        }
    }
}
