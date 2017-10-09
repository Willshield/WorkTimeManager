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
using WorkTimeManager.Model.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WorkTimeManager.Views.UserControls
{
    public sealed partial class ProfileUserControl : UserControl
    {
        Profile profile;
        public ProfileUserControl()
        {
            this.InitializeComponent();
            profile = BllSettingsService.Instance.Profile;
        }
    }
}
