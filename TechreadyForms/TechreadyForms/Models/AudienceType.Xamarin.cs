using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace TechReady.Common.Models
{
    public partial class AudienceType
    {
        [JsonIgnore]
        public ImageSource AudienceTypeImageSource
        {
            get
            {
                var boolValue = this.AudienceTypeName;
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
        }
    }
}
