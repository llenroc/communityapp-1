using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;
using Xamarin.Forms;

namespace TechReady.Helpers.ValueConverters
{
    public class BoolToInverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var boolValue = (bool)value;
            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            //var boolValue = (bool) value;
            //if (boolValue)
            //    return Visibility.Visible;

            //return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class EventTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as string;
            if (boolValue != null)
            {
                switch (boolValue)
                {
                    case "Firstparty_ProDev":
                    case "Firstparty_StudentDev":
                        return ImageSource.FromResource("TechreadyForms.Images.microsoftEvents.png");

                    case "Thirdparty":
                        return  ImageSource.FromResource("TechreadyForms.Images.industryEvents.png");
                    case "Community_ProDev":
                    case "Community_StudentDev":
                        return  ImageSource.FromResource("TechreadyForms.Images.communityEvents.png");
                    case "Webinar":
                        return ImageSource.FromResource("TechreadyForms.Images.webinarEvents.png");
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
