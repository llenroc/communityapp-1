using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechreadyForms.Helpers
{
    public class CustomLabel : Label
    {
        public CustomLabel()
        {

        }

        public static BindableProperty MaxLinesProperty =
            BindableProperty.Create<CustomLabel, int>(o => o.MaxLines, default(int));


        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }
    }

}
