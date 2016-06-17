using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using TechReady.Models;

namespace TechReady.Helpers.ValueConverters
{
    public class LearningResourceThumbnailImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var defaultImage = "/Assets/Images/videovisualStudio.png";
            var lr = value as LearningResource;
            if (lr == null)
            {
                return defaultImage;
            }
            if (string.IsNullOrEmpty(lr.Thumbnail))
            {
                switch (lr.LearningResourceType)
                {
                    case "Channel 9":
                        return defaultImage;
                    case "Microsoft Virtual Academy":
                        return defaultImage;
                    case "Article":
                        return defaultImage;
                }
            }
            else
            {
                return lr.Thumbnail;
            }
            return defaultImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
