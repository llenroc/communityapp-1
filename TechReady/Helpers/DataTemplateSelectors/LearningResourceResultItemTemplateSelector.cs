using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TechReady.Common.Models;
using TechReady.Models;

namespace TechReady.Helpers.DataTemplateSelectors
{
    internal class LearningResourceResultItemTemplateSelector : DataTemplateSelector
    {

        public DataTemplate SmallItemTemplate { get; set; }

        public DataTemplate SmallLoadMoreTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is LoadMoreLearningResource)
            {
                return SmallLoadMoreTemplate;
            }
            if (item is LearningResource)
            {
                return SmallItemTemplate;
            }
            return SmallItemTemplate;
        }
    }
}