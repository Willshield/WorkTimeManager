using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WorkTimeManager.DataValueConverters
{
    public class IssueIdToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return 0;
            }
            int result;
            if (int.TryParse(value.ToString(), out result))
            {
                if (result <= 0)
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
