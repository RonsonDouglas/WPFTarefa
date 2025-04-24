using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using Tarefas.Presentation.Enums;

namespace Tarefas.Presentation.Helpers
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && Enum.TryParse(typeof(StatusTarefa), stringValue, out var enumValue))
            {
                return EnumHelper.GetDescription((Enum)enumValue);
            }

            if (value is Enum enumVal)
            {
                return EnumHelper.GetDescription(enumVal);
            }

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
