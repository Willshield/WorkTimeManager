using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Model.Enums.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WorkTimeManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ActuallyTrackingPage : Page
    {
        public string LoadInterval { get; set; }

        public ActuallyTrackingPage()
        {
            this.InitializeComponent();
            LoadInterval = ((DataLoadInterval)BllSettingsService.Instance.PullLastNDays).GetDisplayName();
            SetButtonOpacities();
            ViewModel.CanExecutesChanged += SetButtonOpacities;

        }

        public void SetButtonOpacities()
        {
            StopSave.Opacity = ViewModel.CanStopSave() ? 1 : 0.6;
            Restart.Opacity = ViewModel.CanRestart() ? 1 : 0.6;
            Abort.Opacity = ViewModel.CanAbort() ? 1 : 0.6;
            Pause.Opacity = ViewModel.CanPause() ? 1 : 0.6;
        }
    }
}
