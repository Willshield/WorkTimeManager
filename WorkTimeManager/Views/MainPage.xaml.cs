using System;
using WorkTimeManager.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Input;
using WorkTimeManager.Model.Enums;

namespace WorkTimeManager.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            TrackButton.Opacity = 0.6;
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
                    ViewModel.OrderCatName = WorktimeOrderBy.Subject;
                    break;
                case "Project":
                    ViewModel.OrderCatName = WorktimeOrderBy.ProjectName;
                    break;
                case "Updated":
                    ViewModel.OrderCatName = WorktimeOrderBy.StartTime;
                    break;
                case "Hours":
                    ViewModel.OrderCatName = WorktimeOrderBy.Hours;
                    break;
                case "Description":
                    ViewModel.OrderCatName = WorktimeOrderBy.Comment;
                    break;
            }
            ViewModel.OrderCats(OrderbyDesc);
            OrderbyDesc = !OrderbyDesc;
        }

        private void SetNormalOpícity()
        {
            Subject.Opacity = 1;
            Project.Opacity = 1;
            Updated.Opacity = 1;
            Hours.Opacity = 1;
            Description.Opacity = 1;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ViewModel.CanStartTracking())
            {
                TrackButton.Opacity = 0.6;
            }
            else
            {
                TrackButton.Opacity = 1;
            }
        }

    }
}