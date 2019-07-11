using System;
using System.Globalization;
using System.Windows.Data;

namespace RSSReader.converters
{
    [ValueConversion( typeof(DateTimeOffset), typeof(string))]
    public class DataToStringConverter: IValueConverter
    {
        // ReSharper disable once TooManyArguments
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            var date = (DateTimeOffset) value;
            var offset = DateTime.Now - date;

            if (offset.TotalMinutes < 60) return offset.TotalMinutes.ToString("F0") + " minutes ago";
            if (offset.TotalHours < 30) return offset.TotalMinutes.ToString("H0") + " hours ago";
            return offset.TotalDays.ToString("D0") + " days ago";
        }

        // ReSharper disable once TooManyArguments
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DateTime.Now;
    }
}
