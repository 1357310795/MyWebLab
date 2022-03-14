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
    public class StateToStr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReportState state)
            {
                switch (state)
                {
                    case ReportState.Cancelled:
                        return "已取消";
                    case ReportState.Done:
                        return "已完成";
                    case ReportState.Error:
                        return "错误...";
                    case ReportState.Running:
                        return "运行中";
                    case ReportState.Waiting:
                        return "等待中";
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
