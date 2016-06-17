using Xamarin.Forms;

public class DataTemplateSelector
{
    public virtual DataTemplate SelectTemplate(object item, BindableObject container)
    {
        return null;
    }
}
