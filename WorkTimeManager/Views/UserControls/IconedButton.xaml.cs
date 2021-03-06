﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Template10.Mvvm;
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
    public sealed partial class IconedButton : UserControl
    {
        public IconedButton()
        {
            this.InitializeComponent();
        }

        public string IconText { get; set; }
        public string LabelText { get; set; }
        public FontFamily Font { get; set; } = new FontFamily("Segoe MDL2 Assets");

    }
}
