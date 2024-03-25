using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Louver_Sort_4._8._1.Helpers;
using Louver_Sort_4._8._1.Views;
using Menu = Louver_Sort_4._8._1.Views.Menu;

namespace Louver_Sort_4._8._1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BoundProperities boundProperities = new BoundProperities();
        public MainWindow()
        {
            InitializeComponent();


            this.DataContext = boundProperities;


            boundProperities.SelectedView = new Menu();
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            boundProperities.Closing();
        }
    }

}
