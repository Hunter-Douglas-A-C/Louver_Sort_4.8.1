using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Louver_Sort_4._8._1.Helpers
{
    public static class TabControlSelectionChangedBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(TabControlSelectionChangedBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {
                tabControl.SelectionChanged -= OnSelectionChanged;
                if (e.NewValue is ICommand)
                {
                    tabControl.SelectionChanged += OnSelectionChanged;
                }
            }
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabControl)
            {
                var command = GetCommand(tabControl);
                if (command != null && command.CanExecute(e))
                {
                    command.Execute(e);
                }
            }
        }
    }

}
