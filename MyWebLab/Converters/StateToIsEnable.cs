using MyWebLab.Hack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyWebLab.Converters
{
    public class StateToIsEnable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReportState state)
            {
                switch (state)
                {
                    case ReportState.Cancelled:
                        return true;
                    case ReportState.Done:
                        return true;
                    case ReportState.Error:
                        return true;
                    case ReportState.Running:
                        return false;
                    case ReportState.Waiting:
                        return true;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
