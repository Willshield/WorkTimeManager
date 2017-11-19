using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.UI.Xaml;
using WorkTimeManager.Bll.Interfaces;
using WorkTimeManager.Bll.Interfaces.Network;
using WorkTimeManager.Bll.Services;
using WorkTimeManager.Bll.Services.Network;
using WorkTimeManager.Model.Enums;
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
        public bool Is001 { get { return _Bllsettings.RoundingTo == Rounding.Round001.GetHashCode(); } set { if (value) _Bllsettings.RoundingTo = Rounding.Round001.GetHashCode(); } }
        public bool Is005 { get { return _Bllsettings.RoundingTo == Rounding.Round005.GetHashCode(); } set { if (value) _Bllsettings.RoundingTo = Rounding.Round005.GetHashCode(); } }
        public bool Is010 { get { return _Bllsettings.RoundingTo == Rounding.Round010.GetHashCode(); } set { if (value) _Bllsettings.RoundingTo = Rounding.Round010.GetHashCode(); } }
        public bool Is025 { get { return _Bllsettings.RoundingTo == Rounding.Round025.GetHashCode(); } set { if (value) _Bllsettings.RoundingTo = Rounding.Round025.GetHashCode(); } }
        public bool Is050 { get { return _Bllsettings.RoundingTo == Rounding.Round050.GetHashCode(); } set { if (value) _Bllsettings.RoundingTo = Rounding.Round050.GetHashCode(); } }
        public bool Is100 { get { return _Bllsettings.RoundingTo == Rounding.Round100.GetHashCode(); } set { if (value) _Bllsettings.RoundingTo = Rounding.Round100.GetHashCode(); } }

        public bool IsLastWeek       { get { return _Bllsettings.PullLastNDays == DataLoadInterval.IsLastWeek.GetHashCode(); }       set { if (value) _Bllsettings.PullLastNDays = DataLoadInterval.IsLastWeek.GetHashCode(); } }
        public bool IsLastTwoWeek    { get { return _Bllsettings.PullLastNDays == DataLoadInterval.IsLastTwoWeek.GetHashCode(); }    set { if (value) _Bllsettings.PullLastNDays = DataLoadInterval.IsLastTwoWeek.GetHashCode(); } }
        public bool IsLastMonth      { get { return _Bllsettings.PullLastNDays == DataLoadInterval.IsLastMonth.GetHashCode(); }      set { if (value) _Bllsettings.PullLastNDays = DataLoadInterval.IsLastMonth.GetHashCode(); } }
        public bool IsLastTwoMonth   { get { return _Bllsettings.PullLastNDays == DataLoadInterval.IsLastTwoMonth.GetHashCode(); }   set { if (value) _Bllsettings.PullLastNDays = DataLoadInterval.IsLastTwoMonth.GetHashCode(); } }
        public bool IsLastYear       { get { return _Bllsettings.PullLastNDays == DataLoadInterval.IsLastYear.GetHashCode(); }       set { if (value) _Bllsettings.PullLastNDays = DataLoadInterval.IsLastYear.GetHashCode(); } }
        public bool IsAll            { get { return _Bllsettings.PullLastNDays == DataLoadInterval.IsAll.GetHashCode(); }            set { if (value) _Bllsettings.PullLastNDays = DataLoadInterval.IsAll.GetHashCode(); } }

        public bool AskIfStop { get { return _Bllsettings.AskIfStop; } set { _Bllsettings.AskIfStop = value; } }
        public bool AlwaysUp { get { return _Bllsettings.AlwaysUp; } set { _Bllsettings.AlwaysUp = value; } }

        private readonly IDbClearService DbService;
        public DelegateCommand ResetSpareTimeCommand { get; }
        private bool _canReset = true;
        public bool CanReset()
        {
            return _canReset;
        }
        public void ResetSpare()
        {
            _Bllsettings.SpareTime = 0.0;
            _canReset = false;
            ResetSpareTimeCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand ClearDbCommand { get; }
        private bool _canClear = true;
        public bool CanClear()
        {
            return _canClear;
        }
        public void ClearDb()
        {
            DbService.ClearDb();
            _canClear = false;
            ClearDbCommand.RaiseCanExecuteChanged();
        }

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                DbService = DbClearService.Instance;
                _UIsettings = UISettingsService.Instance;
                _Bllsettings = BllSettingsService.Instance;
                ResetSpareTimeCommand = new DelegateCommand(ResetSpare, CanReset);
                ClearDbCommand = new DelegateCommand(ClearDb, CanClear);
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
