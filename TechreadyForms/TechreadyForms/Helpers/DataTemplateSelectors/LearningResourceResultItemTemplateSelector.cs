using TechReady.Models;
using Xamarin.Forms;

namespace TechReady.Helpers.DataTemplateSelectors
{
    internal class LearningResourceResultItemTemplateSelector : DataTemplateSelector
    {

        public DataTemplate SmallItemTemplate { get; set; }

        public DataTemplate SmallLoadMoreTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, BindableObject container)
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