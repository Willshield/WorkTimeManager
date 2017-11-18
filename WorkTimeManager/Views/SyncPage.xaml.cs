using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public sealed partial class SyncPage : Page
    {


        public SyncPage()
        {
            this.InitializeComponent();

            ViewModel.EditListRefreshed += SetButtonOpacities;

            SetButtonOpacities();
        }

        private void WorkhourChanged(object sender, KeyRoutedEventArgs e)
        {
            SaveButton.Opacity = 1;
            UndoButton.Opacity = 1;
            ViewModel.WorkHourEdited();
        }

        private void CommentChanged(object sender, KeyRoutedEventArgs e)
        {
            SaveButton.Opacity = 1;
            UndoButton.Opacity = 1;
            ViewModel.CommentEdited();
        }

        public void SetButtonOpacities()
        {
            MergeButton.Opacity = 1;
            RoundSelectedButton.Opacity = 1;
            MergeSelectedButton.Opacity = 1;
            EditButton.Opacity = 1;
            DeleteButton.Opacity = 1;
            SaveButton.Opacity = 1;
            UndoButton.Opacity = 1;

            if (!ViewModel.IsAllMergeable())
            {
                MergeButton.Opacity = 0.6;
            }
            if (!ViewModel.IsEdited())
            {
                SaveButton.Opacity = 0.6;
                UndoButton.Opacity = 0.6;
            }
            if (!ViewModel.IsSelectedMergeable())
            {
                MergeSelectedButton.Opacity = 0.6;
            }
            if (!ViewModel.IsSelectionValid())
            {
                EditButton.Opacity = 0.6;
                DeleteButton.Opacity = 0.6;
                RoundSelectedButton.Opacity = 0.6;
            }

        }

        private void WorkTimeEditor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MergeButton.Opacity = 1;
            EditButton.Opacity = 1;
            DeleteButton.Opacity = 1;
            RoundSelectedButton.Opacity = 1;

            if (!ViewModel.IsSelectedMergeable())
            {
                MergeButton.Opacity = 0.6;
            }
            if (!ViewModel.IsSelectionValid())
            {
                EditButton.Opacity = 0.6;
                DeleteButton.Opacity = 0.6;
                RoundSelectedButton.Opacity = 0.6;
            }
        }

    }
}
