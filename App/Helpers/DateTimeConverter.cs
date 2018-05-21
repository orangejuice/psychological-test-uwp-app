using System;

using Windows.UI.Xaml.Data;

namespace App.Helpers
{
    public class DateTimeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime time)
            {
                var now = DateTime.Now;
                var days = (now - time).TotalDays;
                if (days <= 1)
                {
                    return "Today".GetLocalized() + ", " + time.Hour + ":" + time.Minute;
                }
                else if (days <= 2)
                {
                    return "Yesterday".GetLocalized() + ", " + time.Hour + ":" + time.Minute;
                }
                else if (time.Year == now.Year)
                {
                    return time.ToString("MM/dd, HH:mm");
                }
                else
                {
                    return time.ToString("yyyy/MM/dd, HH:mm");
                }
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
