using System;
using System.Globalization;
using System.Windows.Data;
using Tarefas.Presentation.Enums;

namespace Tarefas.Presentation.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusTarefa status)
            {
                return status.ToString(); 
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
