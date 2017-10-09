using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WorkTimeManager.DataValueConverters
{
    public class DoubleOrNoneFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;
            Double? d = value as Double?;
            if (d == 0)
                return "";

            return string.Format("{0:0.00}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            string val = value as string;
            double ret;
            if (double.TryParse(val, out ret))
                return ret;

            return 0;
        }
    }
}
