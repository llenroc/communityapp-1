using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TechReady.Common.Models;

namespace TechReady.Helpers.DataTemplateSelectors
{
    internal class EventResultItemTemplateSelector : DataTemplateSelector
    {

        public DataTemplate SmallItemTemplate { get; set; }

        public DataTemplate SmallLoadMoreTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
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