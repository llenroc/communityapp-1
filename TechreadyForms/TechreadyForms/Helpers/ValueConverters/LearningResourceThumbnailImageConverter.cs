using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Models;
using Xamarin.Forms;

namespace TechReady.Helpers.ValueConverters
{
    public class LearningResourceThumbnailImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultImage = "TechreadyForms.Images.videoVisualStudio.png";
            var lr = value as LearningResource;
            if (lr == null)
            {
                return ImageSource.FromResource(defaultImage);
            }
            if (string.IsNullOrEmpty(lr.Thumbnail))
            {
                switch (lr.LearningResourceType)
                {
                    case "Channel 9":
                        return ImageSource.FromResource(defaultImage);
                    case "Microsoft Virtual Academy":
                        return ImageSource.FromResource(defaultImage);
                    case "Article":
                        return ImageSource.FromResource(defaultImage);
                }
            }
            else
            {
                try {
                    return ImageSource.FromUri(new Uri(lr.Thumbnail));
                }
                catch { }
            }
            return ImageSource.FromResource(defaultImage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
