using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WorkTimeManager.DataValueConverters
{
    class BoolToSyncIconTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            if(Boolean.Parse(value.ToString()))
            {
                string codePoint = "EA6A";
                int code = int.Parse(codePoint, System.Globalization.NumberStyles.HexNumber);
                return char.ConvertFromUtf32(code);
            } else
            {
                string codePoint = "E8FB";
                int code = int.Parse(codePoint, System.Globalization.NumberStyles.HexNumber);
                return char.ConvertFromUtf32(code);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if(value.ToString() == "&#xEA6A;")
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
