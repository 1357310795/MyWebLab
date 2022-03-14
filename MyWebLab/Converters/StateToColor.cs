using MyWebLab.Hack;
using MyWebLab.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MyWebLab.Converters
{
    public class StateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReportState state)
            {
                switch (state)
                {
                    case ReportState.Cancelled:
                        return ThemeHelper.StringToColor("#607D8B");//灰色
                    case ReportState.Done:
                        return ThemeHelper.StringToColor("#4CAF50");//绿色
                    case ReportState.Error:
                        return ThemeHelper.StringToColor("#F44336");//红色
                    case ReportState.Running:
                        return ThemeHelper.StringToColor("#2196F3");//蓝色
                    case ReportState.Waiting:
                        return ThemeHelper.StringToColor("#673AB7");//紫色
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StateToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReportState state)
            {
                switch (state)
                {
                    case ReportState.Cancelled:
                        return new SolidColorBrush(ThemeHelper.StringToColor("#607D8B"));//灰色
                    case ReportState.Done:
                        return new SolidColorBrush(ThemeHelper.StringToColor("#4CAF50"));//绿色
                    case ReportState.Error:
                        return new SolidColorBrush(ThemeHelper.StringToColor("#F44336"));//红色
                    case ReportState.Running:
                        return new SolidColorBrush(ThemeHelper.StringToColor("#2196F3"));//蓝色
                    case ReportState.Waiting:
                        return new SolidColorBrush(ThemeHelper.StringToColor("#673AB7"));//紫色
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
