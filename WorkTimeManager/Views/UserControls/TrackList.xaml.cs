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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WorkTimeManager.Views.UserControls
{
    public sealed partial class TrackList : UserControl
    {
        public TrackList()
        {
            this.InitializeComponent();
        }

        private bool OrderbyDesc = false;
        private void OrderData(object sender, TappedRoutedEventArgs e)
        {
            TextBlock ClickedBox = sender as TextBlock;
            SetNormalOpícity();
            ClickedBox.Opacity = 0.6;
            //Todo: uncomment
            //switch (ClickedBox.Name)
            //{
            //    case "Subject":
            //        ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.SubjectKey;
            //        break;
            //    case "Project":
            //        ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.ProjectNameKey;
            //        break;
            //    case "StartTime":
            //        ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.StartTimeKey;
            //        break;
            //    case "Hours":
            //        ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.HoursKey;
            //        break;
            //    case "Comment":
            //        ViewModel.OrderCatName = ViewModels.WorkTimePageViewModel.CommentKey;
            //        break;
            //}
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
    }
}
