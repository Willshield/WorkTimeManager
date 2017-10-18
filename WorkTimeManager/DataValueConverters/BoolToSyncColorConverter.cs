using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WorkTimeManager.DataValueConverters
{
    public class BoolToSyncColorConverter : IValueConverter
    {
        private static byte trueA = 255;
        private static byte trueR = 166;
        private static byte trueG = 0;
        private static byte trueB = 24;

        private static byte falseA = 255;
        private static byte falseR = 24;
        private static byte falseG = 166;
        private static byte falseB = 24;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Boolean.Parse(value.ToString()))
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(trueA, trueR, trueG, trueB));
            }
            else
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(falseA, falseR, falseG, falseB));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
