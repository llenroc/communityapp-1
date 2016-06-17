using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using TechReady.Common.Models;

namespace TechReady.Helpers.ValueConverters
{
    public class BoolToInverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = (bool)value;
            if (boolValue)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = (bool)value;
            if (boolValue)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

 

    public class EventTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = value as string;
            if (boolValue != null)
            {
                switch (boolValue)
                {
                    case "Firstparty_ProDev":
                    case "Firstparty_StudentDev":
                        return "/Assets/Images/microsoftEvents.png";

                    case "Thirdparty":
                        return "/Assets/Images/industryEvents.png";
                    case "Community_ProDev":
                    case "Community_StudentDev":
                        return "/Assets/Images/communityEvents.png";
                    case "Webinar":
                        return "/Assets/Images/webinarEvents.png";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }




}
