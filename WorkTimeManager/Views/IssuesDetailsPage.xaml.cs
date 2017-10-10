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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WorkTimeManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IssuesDetailsPage : Page
    {
        public IssuesDetailsPage()
        {
            this.InitializeComponent();
        }

        private bool OrderbyDesc = false;
        private void OrderData(object sender, TappedRoutedEventArgs e)
        {
            TextBlock ClickedBox = sender as TextBlock;
            SetNormalOpícity();
            ClickedBox.Opacity = 0.6;
            switch (ClickedBox.Name)
            {
                case "Subject":
                    ViewModel.OrderCatName = ViewModels.IssuesDetailsPageViewModel.SubjectKey;
                    break;
                case "ProjectName":
                    ViewModel.OrderCatName = ViewModels.IssuesDetailsPageViewModel.ProjectNameKey;
                    break;
                case "Tracker":
                    ViewModel.OrderCatName = ViewModels.IssuesDetailsPageViewModel.TrackerKey;
                    break;
            }
            ViewModel.OrderCats(OrderbyDesc);
            OrderbyDesc = !OrderbyDesc;
        }

        private void SetNormalOpícity()
        {
            Subject.Opacity = 1;
            ProjectName.Opacity = 1;
            Tracker.Opacity = 1;
        }
    }
}
