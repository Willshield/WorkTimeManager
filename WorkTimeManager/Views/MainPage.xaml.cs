using System;
using WorkTimeManager.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Input;

namespace WorkTimeManager.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

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
                case "Updated":
                    ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.StartTimeKey;
                    break;
                case "Hours":
                    ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.HoursKey;
                    break;
                case "Description":
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
            Updated.Opacity = 1;
            Hours.Opacity = 1;
            Description.Opacity = 1;
        }
    }
}