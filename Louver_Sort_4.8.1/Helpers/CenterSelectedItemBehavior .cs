using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows;

namespace Louver_Sort_4._8._1.Helpers
{
    public class CenterSelectedItemBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem != null)
            {
                listView.Dispatcher.Invoke(() =>
                {
                    listView.UpdateLayout();
                    if (listView.ItemContainerGenerator.ContainerFromItem(listView.SelectedItem) is ListViewItem item)
                    {
                        item.BringIntoView(); // You might need to adjust this to actually center the item
                    }
                });
            }
        }
    }

}
