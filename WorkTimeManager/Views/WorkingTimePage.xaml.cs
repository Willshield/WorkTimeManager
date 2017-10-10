using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WorkTimeManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkingTimePage : Page
    {
        ObservableCollection<string> OrderingCats;

        public WorkingTimePage()
        {
            this.InitializeComponent();

            WorkTimeListView.ContainerContentChanging += HighlightHeaders;

            OrderingCats = new ObservableCollection<string>
            {
                "None", "Day", "Week", "Month"
            };

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
                    ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.SubjectKey;
                    break;
                case "Project":
                    ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.ProjectNameKey;
                    break;
                case "StartTime":
                    ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.StartTimeKey;
                    break;
                case "Hours":
                    ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.HoursKey;
                    break;
                case "Comment":
                    ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.CommentKey;
                    break;
            }
            ViewModel.OrderCats(OrderbyDesc);
            OrderbyDesc = !OrderbyDesc;
        }

        private void SetNormalOpícity()
        {
            Subject.Opacity = 1;
            Project.Opacity = 1;
            StartTime.Opacity = 1;
            Hours.Opacity = 1;
            Comment.Opacity = 1;
        }

        private void HighlightHeaders(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            WorkTime wt = args.Item as WorkTime;
            if (wt.IssueID == -1)
            {
                args.ItemContainer.Background = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;  //(SolidColorBrush)Application.Current.Resources["grey"];
            }

        }
    }
}
