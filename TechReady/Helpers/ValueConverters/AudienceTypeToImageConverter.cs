using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TechReady.Helpers.ValueConverters
{
    public class AudienceTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = value as string;
            if (boolValue != null)
            {
                switch (boolValue)
                {
                    case "Developer":
                        return "/Assets/Images/developer.png";
                    case "IT Implementer":
                        return "/Assets/Images/itProfessional.png";
                    case "Student":
                        return "/Assets/Images/student.png";
                    case "Architect / Consultant":
                        return "/Assets/Images/other.png";
                    case "Key Decision Maker":
                        return "/Assets/Images/other.png";
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
