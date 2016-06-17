using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechreadyForms.Helpers
{
    public class TracksPicker : Picker
    {
        public TracksPicker()
        {
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<TracksPicker, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);

        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create<TracksPicker, object>(o => o.SelectedItem, default(object), propertyChanged: OnSelectedItemChanged);


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var picker = bindable as TracksPicker;
            picker.Items.Clear();
            if (newvalue != null)
            {
                //now it works like "subscribe once" but you can improve
                foreach (var item in newvalue)
                {
                    picker.Items.Add(item.ToString());
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = Items[SelectedIndex];
            }
        }
        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as TracksPicker;
            if (newvalue != null)
            {
                picker.SelectedIndex = picker.Items.IndexOf(newvalue.ToString());
            }
        }
    }

}
