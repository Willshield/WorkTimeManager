using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WorkTimeManager.Views
{
    public sealed partial class SettingsPage : Page
    {
        Template10.Services.SerializationService.ISerializationService _SerializationService;

        public SettingsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
            _SerializationService = Template10.Services.SerializationService.SerializationService.Json;

            ViewModel.SettingsPartViewModel.ClearDbCommand.CanExecuteChanged += ClearDbDisabled;
            ViewModel.SettingsPartViewModel.ResetSpareTimeCommand.CanExecuteChanged += ResetSpareDisabled;
        }

        private void ClearDbDisabled(object sender, EventArgs e)
        {
            ClearDb.Opacity = 0.6;
        }

        private void ResetSpareDisabled(object sender, EventArgs e)
        {
            ResetSpare.Opacity = 0.6;
        }

    }
}