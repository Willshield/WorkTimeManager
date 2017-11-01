using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WorkTimeManager.DataValueConverters
{
    public class DoubleToStringValidatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var strVal = value.ToString();
            double result;
            if(!double.TryParse(strVal, out result))
            {
                return 0.0;
             
            }
            return result;
        }
    }
}
