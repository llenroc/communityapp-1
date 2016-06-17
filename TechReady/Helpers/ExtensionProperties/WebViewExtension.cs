using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TechReady.Helpers.ExtensionProperties
{
    public class WebViewExtension
    {
        public static string GetHTML(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLProperty);
        }


        public static void SetHTML(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLProperty, value);
        }


        // Using a DependencyProperty as the backing store for HTML.  This enables animation, styling, binding, etc...  
        public static readonly DependencyProperty HTMLProperty =
            DependencyProperty.RegisterAttached("HTML", typeof(string), typeof(WebViewExtension), new PropertyMetadata("", new PropertyChangedCallback(OnHTMLChanged)));

        private static void OnHTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView wv = d as WebView;
            if (wv != null)
            {
                string contentString = (string)e.NewValue;
                //                string newHtmlString = @"<head></head><body    
                //      onLoad=""window.external.notify('rendered_height='+document.getElementById('content').offsetHeight);"">
                //      <div id='content'>";
                //                newHtmlString += contentString + @"</div></body>";

                wv.NavigateToString(e.NewValue.ToString());
            }
        }
    }

}
