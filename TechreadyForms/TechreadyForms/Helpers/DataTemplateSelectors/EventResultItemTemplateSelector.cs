using TechReady.Common.Models;
using Xamarin.Forms;

namespace TechReady.Helpers.DataTemplateSelectors
{
    internal class EventResultItemTemplateSelector : DataTemplateSelector
    {
        
        public DataTemplate SmallItemTemplate { get; set; }

        public DataTemplate SmallLoadMoreTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, BindableObject container)
        {
            if (item is LoadMore)
            {
                return SmallLoadMoreTemplate;
            }
            if (item is Event)
            {
                return SmallItemTemplate;
            }
            return SmallItemTemplate;
        }
    }
}