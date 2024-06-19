using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using ClipperLib;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using Louver_Sort_4._8._1.Helpers;
using Louver_Sort_4._8._1.Views;
using Microsoft.Win32;

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

            boundProperities.EnUSEnabled = false;
            boundProperities.EsESEnabled = true;
            boundProperities.VisibilityPopUp = Visibility.Hidden;
            boundProperities.IsEnabledMain = true;
            boundProperities.MainContentBlurRadius = 0;

            Chart.DataContextChanged += OnDataContextChanged;
            ReCutChart.DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is CartesianChart chart)
            {
                if (IsChartVisible(chart))
                {
                    // Resume chart updates
                    chart.DisableAnimations = false;
                }
                else
                {
                    // Pause chart updates
                    chart.DisableAnimations = true;
                }
            }
        }

        private bool IsChartVisible(CartesianChart chart)
        {
            // Determine if the chart is visible (this can be customized based on your tab control implementation)
            var parent = VisualTreeHelper.GetParent(chart);
            while (parent != null && !(parent is TabItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is TabItem tabItem)
            {
                var tabControl = VisualTreeHelper.GetParent(tabItem) as TabControl;
                return tabControl?.SelectedItem == tabItem;
            }

            return false;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            boundProperities.ClosingAsync();
        }

        private void ListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
        }


        //    public void test()
        //    {
        //        List<Louver> louvers = new List<Louver>();
        //        LouverSet LouverSetT = new LouverSet();
        //        LouverSet LouverSetM = new LouverSet();
        //        LouverSet LouverSetB = new LouverSet();

        //        for (int i = 1; i < 18 + 1; i++)
        //        {
        //            louvers.Add(new Louver(i, true));
        //        }
        //        for (int i = 18; i < 18 + 19; i++)
        //        {
        //            louvers.Add(new Louver(i, false));
        //        }
        //        LouverOrder LouverOrder = new LouverOrder(LouverSetT, LouverSetM, LouverSetB);




        //        Vector64[] vertices = new Vector64[]
        //{
        //                    new Vector64(0, 0), // Bottom-left
        //                    new Vector64(10, 0), // Bottom-right
        //                    new Vector64(10, 5), // Top-right
        //                    new Vector64(0, 5) // Top-left
        //};















        //        // get the canvas container
        //        Rect64 container = new Rect64(0, ((LouverOrder.LouverSetTop.LouverCount + LouverOrder.LouverSetMiddle.LouverCount + LouverOrder.LouverSetBottom.LouverCount) * 0.461), LouverOrder.LouverSetTop.Width, 0);

        //        int[] _handles;
        //        Nester _nester = new Nester();
        //        // create a new nester object and populate its data
        //        //_handles = _nester.AddUVPolygons(vertices, new int[0], 0.0);

        //        // get the path of the .obj
        //        string file_path = string.Empty;

        //        OpenFileDialog open_file_dialog = new OpenFileDialog();
        //        open_file_dialog.Filter = "Object files (*.obj; *.OBJ)|*.obj;*.OBJ";
        //        if (open_file_dialog.ShowDialog() == true)
        //            file_path = open_file_dialog.FileName;

        //        // if no path then return
        //        if (string.IsNullOrEmpty(file_path))
        //            return;

        //        // try to get the model data
        //        UVObjData data = null;
        //        try
        //        {
        //            data = ObjHandler.GetData(file_path);
        //        }
        //        catch { return; }

        //        // if data is null return
        //        if (data == null)
        //            return;

        //        // get the uv verts and triangles
        //        Vector64[] pts = data.uvs.Select(p => new Vector64(p.X, p.Y)).ToArray();
        //        int[] tris = data.tris;

        //        // create a new nester object and populate its data
        //        _nester = new Nester();
        //        _handles = _nester.AddUVPolygons(pts, tris, 0.0);









        //        // set the nesting commands
        //        _nester.ClearCommandBuffer();
        //        _nester.ResetTransformLib();

        //        _nester.CMD_OptimalRotation(null);

        //        _nester.CMD_Nest(null, NFPQUALITY.Full);

        //        _nester.CMD_Refit(container, false, null);

        //        _nester.ExecuteCommandBuffer(NesterProgress, NesterComplete);



        //        void NesterProgress(ProgressChangedEventArgs e)
        //        {
        //            Debug.WriteLine(e.ProgressPercentage);
        //        }

        //        void NesterComplete(AsyncCompletedEventArgs e)
        //        {
        //            if (e.Cancelled)
        //            {
        //                Debug.WriteLine("cancelled");
        //                return;
        //            }
        //            for (int i = 0; i < _nester.LibSize; i++)
        //            {
        //                //CanvasMain.AddNgon(_nester.GetTransformedPoly(i), Helpers.WPFHelper.RandomColor());
        //            }
        //        }





        //    }

        private void Button_play_Click(object sender, RoutedEventArgs e)
        {
            //test();
        }
    }
}

