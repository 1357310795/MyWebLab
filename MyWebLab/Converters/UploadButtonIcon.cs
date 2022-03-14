using MaterialDesignThemes.Wpf;
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
    public class UploadButtonIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReportState state)
            {
                switch (state)
                {
                    case ReportState.Cancelled:
                        return PackIconKind.Upload;
                    case ReportState.Done:
                        return PackIconKind.Refresh;
                    case ReportState.Error:
                        return PackIconKind.Refresh;
                    case ReportState.Running:
                        return PackIconKind.Upload;
                    case ReportState.Waiting:
                        return PackIconKind.Upload;
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
