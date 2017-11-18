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
using WorkTimeManager.Model.Models;
using Newtonsoft.Json;
using WorkTimeManager.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WorkTimeManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditWorktimes : Page
    {
        public EditWorktimes()
        {
            this.InitializeComponent();
        }
        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationData data = JsonConvert.DeserializeObject<NavigationData>(e.Parameter.ToString());
            var WorktimeId = int.Parse(data.Data);
            ViewModel.SetEditedWorktime(WorktimeId);
            base.OnNavigatedTo(e);
        }
    }
}
