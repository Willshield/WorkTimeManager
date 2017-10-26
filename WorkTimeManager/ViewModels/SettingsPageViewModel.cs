using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.UI.Xaml;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Services.SettingsServices;

namespace WorkTimeManager.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {

        UISettingsService _UIsettings;
        BllSettingsService _Bllsettings;
        public bool Is1 { get { return _Bllsettings.Rounding == 1; } set { if (value) _Bllsettings.Rounding = 1; } }
        public bool Is5 { get { return _Bllsettings.Rounding == 5; } set { if (value) _Bllsettings.Rounding = 5; } }
        public bool Is15 { get { return _Bllsettings.Rounding == 15; } set { if (value) _Bllsettings.Rounding = 15; } }
        public bool Is30 { get { return _Bllsettings.Rounding == 30; } set { if (value) _Bllsettings.Rounding = 30; } }
        public bool Is60 { get { return _Bllsettings.Rounding == 60; } set { if (value) _Bllsettings.Rounding = 60; } }

        public bool AutoTrack { get { return _Bllsettings.AutoTrack; } set { _Bllsettings.AutoTrack = value; } }
        public bool AskIfStop { get { return _Bllsettings.AskIfStop; } set { _Bllsettings.AskIfStop = value; } }
        public bool AlwaysUp { get { return _Bllsettings.AlwaysUp; } set { _Bllsettings.AlwaysUp = value; } }


        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _UIsettings = UISettingsService.Instance;
                _Bllsettings = BllSettingsService.Instance;
            }
        }

        public bool ShowHamburgerButton
        {
            get { return _UIsettings.ShowHamburgerButton; }
            set { _UIsettings.ShowHamburgerButton = value; base.RaisePropertyChanged(); }
        }

        public bool IsFullScreen
        {
            get { return _UIsettings.IsFullScreen; }
            set
            {
                _UIsettings.IsFullScreen = value;
                base.RaisePropertyChanged();
                if (value)
                {
                    ShowHamburgerButton = false;
                }
                else
                {
                    ShowHamburgerButton = true;
                }
            }
        }


        public bool UseShellBackButton
        {
            get { return _UIsettings.UseShellBackButton; }
            set { _UIsettings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _UIsettings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _UIsettings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }

        private string _BusyText = "Please wait...";
        public string BusyText
        {
            get { return _BusyText; }
            set
            {
                Set(ref _BusyText, value);
                _ShowBusyCommand.RaiseCanExecuteChanged();
            }
        }

        DelegateCommand _ShowBusyCommand;
        public DelegateCommand ShowBusyCommand
            => _ShowBusyCommand ?? (_ShowBusyCommand = new DelegateCommand(async () =>
            {
                Views.Busy.SetBusy(true, _BusyText);
                await Task.Delay(5000);
                Views.Busy.SetBusy(false);
            }, () => !string.IsNullOrEmpty(BusyText)));
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }
}
