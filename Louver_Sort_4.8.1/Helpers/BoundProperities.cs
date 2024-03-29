﻿using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using Louver_Sort_4._8._1.Views;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using Zebra.Sdk.Printer;
using System.Text.RegularExpressions;
using PolyNester;
using System.Windows.Markup;
using Louver_Sort_4._8._1.Helpers;
using Microsoft.Win32;
using System.Windows.Shapes;
using ClipperLib;
using System.Windows.Media;
using ClipperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using Louver_Sort_4._8._1.Properties;

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

        List<Louver> louvers = new List<Louver>();
        LouverSet LouverSetT = new LouverSet();
        LouverSet LouverSetM = new LouverSet();
        LouverSet LouverSetB = new LouverSet();



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

            test = new BaseCommand(obj =>
            {
                for (int i = 1; i < 18 + 1; i++)
                {
                    louvers.Add(new Louver(i, true));
                }
                for (int i = 18; i < 18 + 19; i++)
                {
                    louvers.Add(new Louver(i, false));
                }
                LouverOrder LouverOrder = new LouverOrder(LouverSetT, LouverSetM, LouverSetB);

                LouverOrder = LouverOrder.Sort(louvers, LouverOrder);


                int iset = 0;
                foreach (LouverSet set in LouverOrder)
                {
                    Debug.WriteLine("Set " + (iset+1));
                    foreach (Louver louver in set)
                    {
                        Debug.WriteLine(louver.AbsWarp.ToString());
                    }
                    Debug.WriteLine("End Set " + (iset + 1));
                    iset++;
                }

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
            set{SetProperty(ref _en_USEnabled, value);}
        }

        private bool _es_ESEnabled;
        public bool es_ESEnabled
        {
            get => _es_ESEnabled;
            set { SetProperty(ref _es_ESEnabled, value); }
        }



        private LouverSet TestSet = new LouverSet();

        private string _Barcode1;
        public string Barcode1
        {
            get => _Barcode1;
            set
            {
                if (Regex.IsMatch(value, @"^\d{16}L\d$"))
                {
                    SetProperty(ref _Barcode1, value);
                    TestSet.AssignFromBarcode1(_Barcode1);
                }
            }
        }

        private string _Barcode2;
        public string Barcode2
        {
            get => _Barcode2;
            set
            {
                if (Regex.IsMatch(value, @"^PST\d\sP\d/L[A-Z]L/L\d\.\d/L\d+\.\d+/LT$"))
                {
                    SetProperty(ref _Barcode2, value);
                    TestSet.AssignFromBarcode2(_Barcode2);
                }
            }
        }
    }
}