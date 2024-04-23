using Louver_Sort_4._8._1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Louver_Sort_4._8._1.Views.PopUps
{
    /// <summary>
    /// Interaction logic for LouverCount.xaml
    /// </summary>
    public partial class LouverCount : UserControl
    {
        BoundProperities boundProperities;
        public LouverCount()
        {
            InitializeComponent();



            boundProperities = (BoundProperities)Application.Current.MainWindow.DataContext;
            this.DataContext = boundProperities;
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //boundProperities.LouverCountPopUpLoaded();
            //InputTextBox.Focus();
        }
    }
}
