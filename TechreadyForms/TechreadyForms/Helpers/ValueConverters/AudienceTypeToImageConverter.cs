using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechReady.Helpers.ValueConverters
{
    public class AudienceTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var boolValue = value as string;
            if (boolValue != null)
            {
                switch (boolValue)
                {
                    case "Developer":
                        return ImageSource.FromResource("TechreadyForms.Images.developer.png");
                    case "IT Implementer":
                        return ImageSource.FromResource("TechreadyForms.Images.itProfessional.png");
                    case "Student":
                        return ImageSource.FromResource("TechreadyForms.Images.student.png");
                    case "Architect / Consultant":
                        return ImageSource.FromResource("TechreadyForms.Images.other.png");
                    case "Key Decision Maker":
                        return ImageSource.FromResource("TechreadyForms.Images.other.png");
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
