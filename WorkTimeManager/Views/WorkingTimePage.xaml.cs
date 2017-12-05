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
using WorkTimeManager.Model.Enums;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Views
{
    public sealed partial class WorkingTimePage : Page
    {
        public WorkingTimePage()
        {
            this.InitializeComponent();

            WorkTimeListView.ContainerContentChanging += HighlightHeaders;
            TrackButton.Opacity = 0.6;
        }
        
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
                case "StartTime":
                    ViewModel.OrderCatName = WorktimeOrderBy.StartTime;
                    break;
                case "Hours":
                    ViewModel.OrderCatName = WorktimeOrderBy.Hours;
                    break;
                case "Comment":
                    ViewModel.OrderCatName = WorktimeOrderBy.Comment;
                    break;
            }
            ViewModel.OrderCats();
            
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
                args.ItemContainer.Background = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush; 
            } else
            {
                args.ItemContainer.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
            }

        }

        private void WorkTimeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ViewModel.IsValidWorktime())
            {
                TrackButton.Opacity = 0.6;
            } else
            {
                TrackButton.Opacity = 1;
            }
        }
    }
}
