using LiveCharts;
using LiveCharts.Configurations;
using Louver_Sort_4._8._1.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Zebra.Sdk.Printer;
using System.Text.RegularExpressions;
using System.Globalization;
using Louver_Sort_4._8._1.Helpers.LouverStructure;

namespace Louver_Sort_4._8._1.Helpers
{
    internal class BoundProperities : INotifyPropertyChanged
    {
        #region SetProperty

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return;
            }
            storage = value;
            OnPropertyChanged(propertyName);
        }

        protected void SetProperty<T>(ref ChartValues<T> storage, ChartValues<T> value, [CallerMemberName] String propertyName = "")
        {
            if (ReferenceEquals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }



        #endregion

        #region Private Properities

        #region Global

        private DataQHelper _DataQ;
        private ZebraPrinterHelper _zebra = new ZebraPrinterHelper();

        #endregion

        #region MainWindow

        private UserControl _Popup;
        private UserControl _SelectedView;

        #endregion

        #region Menu



        #endregion

        #region Scan

        private Visibility _DisconnectedEnabled = Visibility.Visible;
        private Stopwatch stopwatch = new Stopwatch();
        private ChartValues<MeasureModel> _VoltageValues;

        #endregion

        #region Report



        #endregion

        #endregion

        #region Public Properities

        #region MainWindow

        public UserControl Popup { get => _Popup; set => SetProperty(ref _Popup, value); }
        public UserControl SelectedView { get => _SelectedView; set => SetProperty(ref _SelectedView, value); }

        #endregion

        #region Menu



        #endregion

        #region Scan


        public Visibility DisconnectedEnabled { get => _DisconnectedEnabled; set => SetProperty(ref _DisconnectedEnabled, value); }
        public ChartValues<MeasureModel> VoltageValues
        {
            get { return _VoltageValues ?? (_VoltageValues = new ChartValues<MeasureModel>()); }
            set { SetProperty<MeasureModel>(ref _VoltageValues, value); }
        }
        public class MeasureModel
        {
            public double ElapsedMilliseconds { get; set; }
            public double Value { get; set; }
        }

        #endregion

        #region Report



        #endregion

        #endregion

        #region Commands

        #region Global

        public ICommand TestZebra { get; set; }

        #endregion

        #region MainWindow

        public ICommand UpdateView { get; set; }

        #endregion

        #region Menu



        #endregion

        #region Scan



        #endregion

        #region Report



        #endregion

        #endregion

        #region Code Behind

        #region MainWindow

        public void Closing()
        {
            try
            {
                //_DataQ.Stop();
                //_DataQ.Disconnect();
                //stopwatch.Stop();
                //stopwatch.Reset();
                //DisconnectedEnabled = Visibility.Visible;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Menu



        #endregion

        #region Scan

        public void ScanInitialize()
        {
            Thread test = new Thread(() =>
            {
                _DataQ = new DataQHelper();
                _DataQ.Connect();
                _DataQ.Start();


                DisconnectedEnabled = Visibility.Collapsed;
                stopwatch.Start();

                _DataQ.AnalogUpdated += new EventHandler(DataQNewData);
                _DataQ.LostConnection += new EventHandler(DataQLostConnection);
            });
            test.Start();
        }

        public void DataQNewData(object sender, EventArgs e)
        {
            VoltageValues.Add(new MeasureModel
            {
                ElapsedMilliseconds = stopwatch.Elapsed.TotalSeconds,
                Value = _DataQ.GetDistance()
            });
            Debug.WriteLine(_DataQ.GetDistance());
            if (VoltageValues.Count > 25)
            {
                VoltageValues.RemoveAt(0);
            }
        }

        public void DataQLostConnection(object sender, EventArgs e)
        {
            Debug.WriteLine("Lost Conenction");
        }

        #endregion

        #region Report



        #endregion

        #endregion


        public ICommand test { get; set; }
        public ICommand ChangeLanguage { get; set; }





        private ItemsControl _ItemsToShowInCanvas;
        public ItemsControl ItemsToShowInCanvas { get => _ItemsToShowInCanvas; set => SetProperty(ref _ItemsToShowInCanvas, value); }


        public BoundProperities()
        {
            TestZebra = new BaseCommand(obj =>
            {
                ZebraPrinter _Printer = _zebra.Connect();
                List<double> Values = new List<double>();
                Values.Add(1);
                Values.Add(2);
                Values.Add(3);
                Values.Add(4);
                Values.Add(5);
                Values.Add(6);
                Values.Add(7);
                Values.Add(8);
                Values.Add(9);
                _zebra.PrintLouverIDs(_Printer, Values);
                _zebra.Disconnect(_Printer);
            });

            OrderManager o = new OrderManager();
            test = new BaseCommand(obj =>
            {
                o.CreateOrderAfterScanAndFillAllVariables(new BarcodeSet(Barcode1, Barcode2), 20);

                foreach (var order in o.GetAllOrders())
                {
                    Debug.WriteLine($"Order Barcode1: {order.BarcodeHelper.Barcode.Barcode1}, Barcode2: {order.BarcodeHelper.Barcode.Barcode2}");

                    foreach (var opening in order.Openings)
                    {
                        Debug.WriteLine($"\tOpening Line: {opening.Line}, ModelNum: {opening.ModelNum}, Style: {opening.Style}, Width: {opening.Width}, Length: {opening.Length}");

                        foreach (var panel in opening.Panels)
                        {
                            Debug.WriteLine($"\t\tPanel ID: {panel.ID}");

                            foreach (var set in panel.Sets)
                            {
                                Debug.WriteLine($"\t\t\tSet ID: {set.ID}, Date Sort Started: {set.DateSortStarted}, Date Sort Finished: {set.DateSortFinished}, Louver Count: {set.LouverCount}");

                                foreach (var louver in set.Louvers)
                                {
                                    Debug.WriteLine($"\t\t\t\tLouver ID: {louver.ID}, Processed: {louver.Processed}, Warp: {louver.Warp}, Rejected: {louver.Rejected}, Cause of Rejection: {louver.CauseOfRejection}");
                                }
                            }
                        }
                    }
                }
                Debug.WriteLine("\n\n\n\n\n\n\n\n\n\n\n");


                //var s = o.CreateOrder(new BarcodeSet(Barcode1, Barcode2)).Openings[0].Panels[0].Sort();
                //foreach (var set in s)
                //{
                //    Debug.WriteLine($"\t\t\tSet ID: {set.ID}, Date Sort Started: {set.DateSortStarted}, Date Sort Finished: {set.DateSortFinished}, Louver Count: {set.LouverCount}");

                //    foreach (var louver in set.Louvers)
                //    {
                //        Debug.WriteLine($"\t\t\t\tLouver ID: {louver.ID}, Processed: {louver.Processed}, Warp: {louver.Warp}, Rejected: {louver.Rejected}, Cause of Rejection: {louver.CauseOfRejection}");
                //    }
                //}
            });

            ChangeLanguage = new BaseCommand(obj =>
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(obj.ToString());
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(obj.ToString());

                    Application.Current.Resources.MergedDictionaries.Clear();
                    ResourceDictionary resdict = new ResourceDictionary()
                    {
                        Source = new Uri($"/Dictionary-{obj.ToString()}.xaml", UriKind.Relative)
                    };
                    Application.Current.Resources.MergedDictionaries.Add(resdict);

                    switch (obj.ToString())
                    {
                        case "en-US":
                            en_USEnabled = false;
                            es_ESEnabled = true;
                            break;
                        case "es-ES":
                            en_USEnabled = true;
                            es_ESEnabled = false;
                            break;
                        default:
                            break;
                    }
                });




            #region MainWindow

            UpdateView = new BaseCommand(obj =>
            {
                switch (obj)
                {
                    case "Menu":
                        SelectedView = new Views.Menu();
                        break;
                    case "Scan":
                        SelectedView = new Scan();
                        break;
                    case "Report":
                        SelectedView = new Report();
                        break;
                    case "Main":
                        SelectedView = null;
                        break;
                    case "Exit":
                        SelectedView = null;
                        break;
                    default:
                        break;
                }

                if (obj.ToString() != "Scan")
                {
                    //    _DataQ.Stop();
                    //    _DataQ.Disconnect();
                    //    DisconnectedEnabled = Visibility.Visible;
                }
            });

            #endregion

            #region Menu



            #endregion

            #region Scan

            var Mapper = Mappers.Xy<MeasureModel>()
                .X(x => x.ElapsedMilliseconds)
                .Y(x => x.Value);
            LiveCharts.Charting.For<MeasureModel>(Mapper);

            #endregion

            #region Report



            #endregion
        }

        private bool _en_USEnabled;
        public bool en_USEnabled
        {
            get => _en_USEnabled;
            set { SetProperty(ref _en_USEnabled, value); }
        }

        private bool _es_ESEnabled;
        public bool es_ESEnabled
        {
            get => _es_ESEnabled;
            set { SetProperty(ref _es_ESEnabled, value); }
        }



        //private Order TestSet = new Order();

        private string _Barcode1 = "1018652406000001L1";
        public string Barcode1
        {
            get => _Barcode1;
            set
            {
                if (Regex.IsMatch(value, @"^\d{16}L\d$"))
                {
                    SetProperty(ref _Barcode1, value);
                }
            }
        }

        private string _Barcode2;
        public string Barcode2
        {
            get => _Barcode2;
            set
            {
                if (Regex.IsMatch(value, @"^(PNL[1-9])\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$"))
                {
                    SetProperty(ref _Barcode2, value);
                }
            }
        }
    }
}