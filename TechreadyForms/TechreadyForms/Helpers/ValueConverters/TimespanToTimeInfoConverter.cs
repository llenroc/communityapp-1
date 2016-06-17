using System;
using System.Globalization;
using Xamarin.Forms;

namespace TechReady.Helpers.ValueConverters
{
    public class TimespanToTimeInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan t = System.DateTime.UtcNow - (DateTime)value;

            var time = (TimeSpan)t;
            if (time.Days > 0)
            {
                if (time.Days > 1)
                {
                    return string.Format("{0} days ago", time.Days);
                }
                else
                {
                    return string.Format("{0} day ago", time.Days);
                }
            }
            else if (time.Hours > 0)
            {
                if (time.Hours > 1)
                {
                    return string.Format("{0} hours ago", time.Hours);
                }
                else
                {
                    return string.Format("{0} hour ago", time.Hours);
                }
            }
            else if (time.Minutes > 0)
            {
                if (time.Minutes > 1)
                {
                    return string.Format("{0} minutes ago", time.Minutes);
                }
                else
                {
                    return string.Format("{0} minute ago", time.Minutes);
                }
            }
            else if (time.Seconds > 0)
            {
                if (time.Seconds > 1)
                {
                    return string.Format("{0} seconds ago", time.Seconds);
                }
                else
                {
                    return string.Format("{0} second ago", time.Seconds);
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
