using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using LiveCharts.Configurations;
using LiveCharts;
using Louver_Sort_4._8._1.Helpers.LouverStructure;
using Louver_Sort_4._8._1.Views;
using Newtonsoft.Json;
using System.Diagnostics;
using Zebra.Sdk.Printer;

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

        #region Properities



        //Instances
        private DataQHelper _dataQ;
        private ZebraPrinterHelper _zebra = new ZebraPrinterHelper();
        public OrderManager _allOrders = new OrderManager();

        //Global
        private string _adminPassword = "55555";
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

        //User Control Views
        private UserControl _selectedPopUp;
        public UserControl SelectedPopUp { get => _selectedPopUp; set => SetProperty(ref _selectedPopUp, value); }
        private UserControl _selectedView;
        public UserControl SelectedView { get => _selectedView; set => SetProperty(ref _selectedView, value); }
        private UserControl _reportingView;
        private UserControl ReportingView
        {
            get => _reportingView;
            set { SetProperty(ref _reportingView, value); }
        }
        private UserControl _settingsView;
        private UserControl SettingsView
        {
            get => _settingsView;
            set { SetProperty(ref _settingsView, value); }
        }

        //IsEnabled
        private bool _isEnabledAcquareTop;
        public bool IsEnabledAcquareTop
        {
            get => _isEnabledAcquareTop;
            set { SetProperty(ref _isEnabledAcquareTop, value); }
        }

        private bool _isEnabledAcquireBottom;
        public bool IsEnabledAcquireBottom
        {
            get => _isEnabledAcquireBottom;
            set { SetProperty(ref _isEnabledAcquireBottom, value); }
        }
        private bool _isEnabledPrintUnsortedLabels;
        public bool IsEnabledPrintUnsortedLabels
        {
            get => _isEnabledPrintUnsortedLabels;
            set { SetProperty(ref _isEnabledPrintUnsortedLabels, value); }
        }
        private bool _isEnabledReviewReport;
        public bool IsEnabledReviewReport
        {
            get => _isEnabledReviewReport;
            set { SetProperty(ref _isEnabledReviewReport, value); }
        }
        private bool _isEnabledPrintSortedLabels;
        public bool IsEnabledPrintSortedLabels
        {
            get => _isEnabledPrintSortedLabels;
            set { SetProperty(ref _isEnabledPrintSortedLabels, value); }
        }
        private bool _isEnabledNextLouverSet;
        public bool IsEnabledNextLouverSet
        {
            get => _isEnabledNextLouverSet;
            set { SetProperty(ref _isEnabledNextLouverSet, value); }
        }
        private bool _isEnabledMain;
        public bool IsEnabledMain
        {
            get => _isEnabledMain;
            set { SetProperty(ref _isEnabledMain, value); }
        }
        private bool _isEnabledApproveSet = true;
        public bool IsEnabledApproveSet
        {
            get => _isEnabledApproveSet;
            set { SetProperty(ref _isEnabledApproveSet, value); }
        }
        private bool _isEnabledRejectSelectedLouver = false;
        public bool IsEnabledRejectSelectedLouver
        {
            get => _isEnabledRejectSelectedLouver;
            set { SetProperty(ref _isEnabledRejectSelectedLouver, value); }
        }
        private bool _isEnabledReworkSet = false;
        public bool IsEnabledReworkSet
        {
            get => _isEnabledReworkSet;
            set { SetProperty(ref _isEnabledReworkSet, value); }
        }
        private bool _isEnabledBarcode = true;
        public bool IsEnabledBarcode
        {
            get => _isEnabledBarcode;
            set { SetProperty(ref _isEnabledBarcode, value); }
        }
        private bool _isEnabledExitReport = false;
        public bool IsEnabledExitReport
        {
            get => _isEnabledExitReport;
            set { SetProperty(ref _isEnabledExitReport, value); }
        }
        private bool _isEnabledCheckTop;
        public bool IsEnabledCheckTop
        {
            get => _isEnabledCheckTop;
            set { SetProperty(ref _isEnabledCheckTop, value); }
        }
        private bool _isEnabledCheckBottom;
        public bool IsEnabledCheckBottom
        {
            get => _isEnabledCheckBottom;
            set { SetProperty(ref _isEnabledCheckBottom, value); }
        }


        //Visibility
        private Visibility _visibilityDisconnected = Visibility.Visible;
        public Visibility VisibilityDisconnected { get => _visibilityDisconnected; set => SetProperty(ref _visibilityDisconnected, value); }
        private Visibility _visibilityPopUp;
        public Visibility VisibilityPopUp
        {
            get => _visibilityPopUp;
            set { SetProperty(ref _visibilityPopUp, value); }
        }
        private int _mainContentBlurRadius;
        public int MainContentBlurRadius
        {
            get => _mainContentBlurRadius;
            set { SetProperty(ref _mainContentBlurRadius, value); }
        }
        private Visibility _visilitySettingsPassword = Visibility.Visible;
        public Visibility VisilitySettingsPassword
        {
            get => _visilitySettingsPassword;
            set
            {
                SetProperty(ref _visilitySettingsPassword, value);
            }
        }
        private Visibility _visibilitySettings = Visibility.Collapsed;
        public Visibility VisibilitySettings
        {
            get => _visibilitySettings;
            set
            {
                SetProperty(ref _visibilitySettings, value);
            }
        }
        private Visibility _visilityReportingPassword = Visibility.Visible;
        public Visibility VisilityReportingPassword
        {
            get => _visilityReportingPassword;
            set
            {
                SetProperty(ref _visilityReportingPassword, value);
            }
        }
        private Visibility _visibilityReporting = Visibility.Collapsed;
        public Visibility VisibilityReporting
        {
            get => _visibilityReporting;
            set
            {
                SetProperty(ref _visibilityReporting, value);
            }
        }

        //Focus
        private bool _focusBarcode1;
        public bool FocusBarcode1
        {
            get => _focusBarcode1;
            set { SetProperty(ref _focusBarcode1, value); }
        }
        private bool _focusBarcode2;
        public bool FocusBarcode2
        {
            get => _focusBarcode2;
            set { SetProperty(ref _focusBarcode2, value); }
        }
        private bool _focusLouverCount;
        public bool FocusLouverCount
        {
            get => _focusLouverCount;
            set { SetProperty(ref _focusLouverCount, value); }
        }



        //IsReadOnly
        private bool _isReadOnlyBarcode;
        public bool IsReadOnlyBarcode
        {
            get => _isReadOnlyBarcode;
            set
            {
                SetProperty(ref _isReadOnlyBarcode, value);
            }
        }




        //Active Order
        private string _barcode1 = "1018652406000001L1";
        public string Barcode1
        {
            get => _barcode1;
            set
            {
                if (value == null)
                {
                    SetProperty(ref _barcode1, "");
                }
                else
                {
                    if (Regex.IsMatch(value, @"^\d{16}L\d$"))
                    {
                        SetProperty(ref _barcode1, value);
                    }
                    else if (Regex.IsMatch(value, @"^$"))
                    {
                        SetProperty(ref _barcode1, value);
                    }
                }


            }
        }
        private string _barcode2 = "PNL1/LXL/L4.5/L30.5188/LT";
        public string Barcode2
        {
            get => _barcode2;
            set
            {
                if (value == null)
                {
                    SetProperty(ref _barcode2, "");
                }
                else
                {
                    if (Regex.IsMatch(value, @"^(PNL[1-9])\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$"))
                    {
                        SetProperty(ref _barcode2, value);
                    }
                    else if (Regex.IsMatch(value, @"^$"))
                    {
                        SetProperty(ref _barcode2, value);
                    }
                }

            }
        }
        //private string _barcode1;
        //private string _barcode2;
        private int _activeLouverId;
        public int ActiveLouverID
        {
            get => _activeLouverId;
            set
            {
                SetProperty(ref _activeLouverId, value);
            }
        }
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Panel _activePanel;
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Panel ActivePanel
        {
            get => _activePanel;
            set { SetProperty(ref _activePanel, value); }
        }
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Set _activeSet;
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Set ActiveSet
        {
            get => _activeSet;
            set { SetProperty(ref _activeSet, value); }
        }
        public double _activeTopReading;
        public double ActiveTopReading
        {
            get => _activeTopReading;
            set { SetProperty(ref _activeTopReading, value); }
        }
        public double _activeBottomReading;
        public double ActiveBottomReading
        {
            get => _activeBottomReading;
            set { SetProperty(ref _activeBottomReading, value); }
        }
        private string _curOrder;
        public string CurOrder
        {
            get => _curOrder;
            set { SetProperty(ref _curOrder, value); }
        }
        private string _curLine;
        public string CurLine
        {
            get => _curLine;
            set { SetProperty(ref _curLine, value); }
        }
        private string _curUnit;
        public string CurUnit
        {
            get => _curUnit;
            set { SetProperty(ref _curUnit, value); }
        }
        private string _curPanelID;
        public string CurPanelID
        {
            get => _curPanelID;
            set { SetProperty(ref _curPanelID, value); }
        }
        private string _curLouverSet;
        public string CurLouverSet
        {
            get => _curLouverSet;
            set { SetProperty(ref _curLouverSet, value); }
        }
        private bool _curXL;
        public bool CurXL
        {
            get => _curXL;
            set { SetProperty(ref _curXL, value); }
        }
        private string _curWidth;
        public string CurWidth
        {
            get => _curWidth;
            set { SetProperty(ref _curWidth, value); }
        }
        private string _curLength;
        public string CurLength
        {
            get => _curLength;
            set { SetProperty(ref _curLength, value); }
        }
        private string _curBarcode1;
        public string CurBarcode1
        {
            get => _curBarcode1;
            set { SetProperty(ref _curBarcode1, value); }
        }
        private string _curBarcode2;
        public string CurBarcode2
        {
            get => _curBarcode2;
            set { SetProperty(ref _curBarcode2, value); }
        }
        private int? _txtLouverCount = 5;
        public int? TxtLouverCount
        {
            get => _txtLouverCount;
            set
            {
                SetProperty(ref _txtLouverCount, value);
            }
        }
        private double _activeDeviation;
        public double ActiveDeviation
        {
            get => _activeDeviation;
            set { SetProperty(ref _activeDeviation, value); }
        }

        //Scan
        private LouverListView _listViewSelectedLouver;
        public LouverListView ListViewSelectedLouver
        {
            get => _listViewSelectedLouver;
            set
            {
                SetProperty(ref _listViewSelectedLouver, value);
                if (_listViewSelectedLouver != null)
                {
                    ActiveLouverID = _listViewSelectedLouver.LouverID;

                }
            }
        }
        private Stopwatch _stopwatch = new Stopwatch();
        public class MeasureModel
        {
            public double ElapsedMilliseconds { get; set; }
            public double Value { get; set; }
        }
        private ChartValues<MeasureModel> _voltageValues;
        public ChartValues<MeasureModel> VoltageValues
        {
            get { return _voltageValues ?? (_voltageValues = new ChartValues<MeasureModel>()); }
            set { SetProperty<MeasureModel>(ref _voltageValues, value); }
        }

        //Report
        private ReportListView _reportSelectedLouver;
        public ReportListView ReportSelectedLouver
        {
            get => _reportSelectedLouver;
            set
            {
                SetProperty(ref _reportSelectedLouver, value);
                if (_reportSelectedLouver != null)
                {
                    // _reportSelectedLouver = true;
                }
            }
        }

        //Recut
        private ReportListView _reCutSelectedLouver;
        public ReportListView ReCutSelectedLouver
        {
            get => _reCutSelectedLouver;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _reCutSelectedLouver, value);
                }

                // Find the index of the Louver object with the same ID as ReportSelectedLouver.
                int index = ActiveSet.Louvers.FindIndex(louver => louver.ID == ReCutSelectedLouver.LouverID);

                // If the Louver with the same ID is found, remove it from the collection.
                if (index != -1)
                {
                    RecutReading1 = ActiveSet.Louvers[index].Reading1.ToString();
                    RecutReading2 = ActiveSet.Louvers[index].Reading2.ToString();
                }
            }
        }
        private string _reCutOrder;
        public string ReCutOrder
        {
            get => _reCutOrder;
            set { SetProperty(ref _reCutOrder, value); }
        }
        private string _reCutLine;
        public string ReCutLine
        {
            get => _reCutLine;
            set { SetProperty(ref _reCutLine, value); }
        }
        private string _reCutUnit;
        public string ReCutUnit
        {
            get => _reCutUnit;
            set { SetProperty(ref _reCutUnit, value); }
        }
        private string _reCutPanelID;
        public string ReCutPanelID
        {
            get => _reCutPanelID;
            set { SetProperty(ref _reCutPanelID, value); }
        }
        private string _reCutLouverSet;
        public string ReCutLouverSet
        {
            get => _reCutLouverSet;
            set { SetProperty(ref _reCutLouverSet, value); }
        }
        private bool _reCutXL;
        public bool ReCutXL
        {
            get => _reCutXL;
            set { SetProperty(ref _reCutXL, value); }
        }
        private string _reCutWidth;
        public string ReCutWidth
        {
            get => _reCutWidth;
            set { SetProperty(ref _reCutWidth, value); }
        }
        private string _reCutLength;
        public string ReCutLength
        {
            get => _reCutLength;
            set { SetProperty(ref _reCutLength, value); }
        }
        private string _reCutBarcode1;
        public string ReCutBarcode1
        {
            get => _reCutBarcode1;
            set { SetProperty(ref _reCutBarcode1, value); }
        }
        private string _reCutBarcode2;
        public string ReCutBarcode2
        {
            get => _reCutBarcode2;
            set { SetProperty(ref _reCutBarcode2, value); }
        }
        private string _recutReading1;
        public string RecutReading1
        {
            get => _recutReading1;
            set { SetProperty(ref _recutReading1, value); }
        }
        private string _recutReading2;
        public string RecutReading2
        {
            get => _recutReading2;
            set { SetProperty(ref _recutReading2, value); }
        }
        private string _txtTopAcceptableReplacement;
        public string TxtTopAcceptableReplacement
        {
            get => _txtTopAcceptableReplacement;
            set { SetProperty(ref _txtTopAcceptableReplacement, value); }
        }
        private string _txtBottomAcceptableReplacement;
        public string TxtBottomAcceptableReplacement
        {
            get => _txtBottomAcceptableReplacement;
            set { SetProperty(ref _txtBottomAcceptableReplacement, value); }
        }

        //Settings
        private string _passwordSettings;
        public string PasswordSettings
        {
            get => _passwordSettings;
            set
            {
                SetProperty(ref _passwordSettings, value);
            }
        }
        private string _settingsPasswordToolTip = "Enter your password";
        public string SettingsPasswordToolTip
        {
            get => _settingsPasswordToolTip;
            set { SetProperty(ref _settingsPasswordToolTip, value); }
        }

        //Reporting
        private string _passwordReporting;
        public string PasswordReporting
        {
            get => _passwordReporting;
            set
            {
                SetProperty(ref _passwordReporting, value);
            }
        }
        private string _reportingPasswordToolTip = "Enter your password";
        public string ReportingPasswordToolTip
        {
            get => _reportingPasswordToolTip;
            set { SetProperty(ref _reportingPasswordToolTip, value); }
        }

        //Observable Collections
        private ObservableCollection<LouverListView> _listViewContent = new ObservableCollection<LouverListView>();
        public ObservableCollection<LouverListView> ListViewContent
        {
            get => _listViewContent;
            set { SetProperty(ref _listViewContent, value); }
        }
        private ObservableCollection<ReportListView> _reportContent = new ObservableCollection<ReportListView>();
        public ObservableCollection<ReportListView> ReportContent
        {
            get => _reportContent;
            set { SetProperty(ref _reportContent, value); }
        }
        private ObservableCollection<ReportListView> _reCutContent;
        public ObservableCollection<ReportListView> ReCutContent
        {
            get => _reCutContent;
            set { SetProperty(ref _reCutContent, value); }
        }
        private ObservableCollection<LabelID> _labelIDContent;
        public ObservableCollection<LabelID> LabelIDContent
        {
            get => _labelIDContent;
            set { SetProperty(ref _labelIDContent, value); }
        }

        #endregion

        #region Commands
        public ICommand UpdateView { get; set; }
        public ICommand UpdatePopUp { get; set; }
        public ICommand ChangeLanguage { get; set; }
        public ICommand Barcode1KeyDown { get; set; }
        public ICommand ScanLoaded { get; set; }
        public ICommand ReconnectToDataQ { get; set; }
        public ICommand EnterBarcodes { get; set; }
        public ICommand LouverCountOk { get; set; }
        public ICommand ClosePopUp { get; set; }
        public ICommand PrintUnsortedLabels { get; set; }
        public ICommand AcqReadingTop { get; set; }
        public ICommand AcqReadingBottom { get; set; }
        public ICommand ReviewLouverReport { get; set; }
        public ICommand RejectSelected { get; set; }
        public ICommand ReworkSet { get; set; }
        public ICommand ReportApproved { get; set; }
        public ICommand SortedLabelsComplete { get; set; }
        public ICommand PrintSortedLabels { get; set; }
        public ICommand NextLouverSet { get; set; }
        public ICommand SearchOrder { get; set; }
        public ICommand CheckTop { get; set; }
        public ICommand CheckBottom { get; set; }
        public ICommand LostFocusSettings { get; set; }
        public ICommand LostFocusReporting { get; set; }
        public ICommand ShutDown { get; set; }
        #endregion

        #region CommandImplementation
        public BoundProperities()
        {

            var Mapper = Mappers.Xy<MeasureModel>()
        .X(x => x.ElapsedMilliseconds)
        .Y(x => x.Value);
            LiveCharts.Charting.For<MeasureModel>(Mapper);


            UpdateView = new BaseCommand(obj =>
             {
                 VisibilityPopUp = Visibility.Hidden;
                 IsEnabledMain = true;
                 SelectedPopUp = null;
                 MainContentBlurRadius = 0;
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
                     case "ReCut":
                         SelectedView = new ReCut();
                         break;
                     case "Main":
                         SelectedView = null;
                         break;
                     case "Exit":
                         SelectedView = null;
                         break;
                     case "Settings":
                         if (PasswordSettings == _adminPassword)
                         {
                             VisilitySettingsPassword = Visibility.Collapsed;
                             VisibilitySettings = Visibility.Visible;
                             SettingsView = new Settings();
                         }
                         else
                         {
                             SettingsPasswordToolTip = "Incorrect Password";
                         }
                         break;
                     case "Reporting":
                         if (PasswordReporting == _adminPassword)
                         {
                             VisilityReportingPassword = Visibility.Collapsed;
                             VisibilityReporting = Visibility.Visible;
                             ReportingView = new Reporting();
                         }
                         else
                         {
                             ReportingPasswordToolTip = "Incorrect Password";
                         }
                         break;
                     default:
                         break;
                 }
             });

            UpdatePopUp = new BaseCommand(obj =>
        {
            IsEnabledMain = false;
            VisibilityPopUp = Visibility.Visible;
            MainContentBlurRadius = 50;
            switch (obj)
            {
                case "LouverCount":
                    SelectedPopUp = new Views.PopUps.LouverCount();
                    FocusLouverCount = true;
                    break;
                case "SortedLabelsPopUp":
                    SelectedPopUp = new Views.PopUps.SortedLabelsPopUp();
                    break;
                default:
                    break;
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

            Barcode1KeyDown = new BaseCommand(obj =>
    {
        FocusBarcode2 = true;
    });

            ScanLoaded = new BaseCommand(obj =>
    {
        FocusBarcode1 = true;
    });

            ReconnectToDataQ = new BaseCommand(obj =>
        {
            ConnectToDataQ();
        });


            EnterBarcodes = new BaseCommand(obj =>
        {
            if (Barcode1 != "" && Barcode2 != "")
            {
                var o = _allOrders.CheckIfOrderExists(new BarcodeSet(Barcode1, Barcode2));
                if (o != null)
                {
                    MessageBox.Show("Already Sorted Order");
                    //Barcode1 = "";
                    //Barcode2 = "";
                    return;
                }
                else
                {
                    IsReadOnlyBarcode = true;
                    IsEnabledBarcode = false;
                    UpdatePopUp.Execute("LouverCount");
                }
            }
            else
            {
                //CHANGE
                //Add warning to user that they need barcode values
            }

        });


            LouverCountOk = new BaseCommand(obj =>
        {
            var focusedElement = Keyboard.FocusedElement as FrameworkElement;

            if (focusedElement is TextBox)
            {
                BindingExpression be = focusedElement.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }


            //PopUpVisible = Visibility.Hidden;
            //MainEnabled = true;
            //SelectedPopUp = null;
            //MainContentBlurRadius = 0;
            //SelectedView = new Scan();
            ClosePopUp.Execute("");
            var order = _allOrders.CreateOrderAfterScanAndFillAllVariables(new BarcodeSet(Barcode1, Barcode2), Convert.ToInt32(TxtLouverCount));
            ActiveLouverID = 0;
            ActivePanel = order.GetOpeningByLine(order.BarcodeHelper.Line).GetPanel(order.BarcodeHelper.PanelID);
            ActiveSet = ActivePanel.GetSet(order.BarcodeHelper.Set);



            CurBarcode1 = order.BarcodeHelper.BarcodeSet.Barcode1.ToString();
            CurBarcode2 = order.BarcodeHelper.BarcodeSet.Barcode2.ToString();
            CurOrder = order.BarcodeHelper.Order.ToString();
            CurLine = order.BarcodeHelper.Line.ToString();
            CurUnit = order.BarcodeHelper.Unit.ToString();
            CurPanelID = order.BarcodeHelper.PanelID.ToString();
            CurLouverSet = order.BarcodeHelper.Set.ToString();
            CurXL = order.BarcodeHelper.Style == LouverStructure.LouverStyle.LouverStyles.XL;
            CurWidth = order.BarcodeHelper.Width.ToString();
            CurLength = order.BarcodeHelper.Length.ToString();

            IsEnabledPrintUnsortedLabels = true;

            ListViewContent = ActiveSet.GenerateRecordedLouvers();
            ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);
        });

            ClosePopUp = new BaseCommand(obj =>
        {
            VisibilityPopUp = Visibility.Hidden;
            IsEnabledMain = true;
            SelectedPopUp = null;
            MainContentBlurRadius = 0;
        });



            PrintUnsortedLabels = new BaseCommand(obj =>
        {
            ActiveSet.StartSort(DateTime.Now);


            ZebraPrinter _Printer = _zebra.Connect();
            List<Louver> ToPrint = new List<Louver>();
            foreach (var sets in ActivePanel.GetAllSets())
            {
                ToPrint.AddRange(sets.GetLouverSet());
            }
            _zebra.PrintLouverIDs(_Printer, ToPrint);
            _zebra.Disconnect(_Printer);

            //Debug.WriteLine("Printed first set of labels");


            IsEnabledPrintUnsortedLabels = false;
            IsEnabledAcquareTop = true;
            IsEnabledAcquireBottom = false;
        });

            AcqReadingTop = new BaseCommand(obj =>
        {
            Random random = new Random();

            // Generate a random integer between -1000 and 1000
            int randomInt = random.Next(-1000, 1001);

            // Divide the random integer by 1000 to get increments of 0.001
            //double value = randomInt / 1000.0;
            double value = _dataQ.RecordAndAverageReadings();
            ActiveSet.Louvers[ActiveLouverID].SetReading1(value);
            ActiveTopReading = value;

            ListViewContent = ActiveSet.GenerateRecordedLouvers();
            IsEnabledAcquareTop = false;
            IsEnabledAcquireBottom = true;
            ListViewSelectedLouver = ListViewContent[(ListViewContent.IndexOf(ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID)) + 1)];
        });

            AcqReadingBottom = new BaseCommand(obj =>
            {
                Random random = new Random();

                // Generate a random integer between -1000 and 1000
                int randomInt = random.Next(-1000, 1001);

                // Divide the random integer by 1000 to get increments of 0.001
                //double value = randomInt / 1000.0;
                double value = _dataQ.RecordAndAverageReadings();
                ActiveSet.Louvers[ActiveLouverID].SetReading2(value);
                ActiveBottomReading = value;



                ActiveSet.Louvers[ActiveLouverID].CalcValues();
                ActiveDeviation = ActiveSet.Louvers[ActiveLouverID].Deviation;
                ListViewContent = ActiveSet.GenerateRecordedLouvers();




                foreach (var louver in ActiveSet.Louvers)
                {
                    if (louver.Reading1 == 0 && louver.Reading2 == 0)
                    {
                        ActiveLouverID = louver.ID;
                        IsEnabledAcquareTop = true;
                        IsEnabledAcquireBottom = false;
                        ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);
                        return;
                    }
                }
                IsEnabledAcquareTop = false;
                IsEnabledAcquireBottom = false;
                IsEnabledReviewReport = true;
                ListViewSelectedLouver = null;
            });

            ReviewLouverReport = new BaseCommand(obj =>
            {
                ActiveSet.Sort();
                ListViewContent = null;
                ListViewContent = ActiveSet.GenerateRecordedLouvers();
                UpdateView.Execute("Report");
                IsEnabledReviewReport = false;
            });

            RejectSelected = new BaseCommand(obj =>
        {
            // Check if ReportSelectedLouver is not null and ActiveSet is initialized.
            if (ReportSelectedLouver != null && ActiveSet != null)
            {
                // Find the index of the Louver object with the same ID as ReportSelectedLouver.
                int index = ActiveSet.Louvers.FindIndex(louver => louver.ID == ReportSelectedLouver.LouverID);

                // If the Louver with the same ID is found, remove it from the collection.
                if (index != -1)
                {
                    ReportContent.Remove(ReportSelectedLouver);
                    ActiveSet.Louvers[index].SetReading1(0);
                    ActiveSet.Louvers[index].SetReading2(0);
                }
            }

            IsEnabledReworkSet = true;
            IsEnabledApproveSet = false;
            IsEnabledRejectSelectedLouver = false;
        });

            ReworkSet = new BaseCommand(obj =>
        {
            ListViewContent = ActiveSet.GenerateRecordedLouvers();
            IsEnabledAcquareTop = true;
            IsEnabledPrintSortedLabels = false;

            UpdateView.Execute("Scan");
            IsEnabledReworkSet = false;
            foreach (var louver in ActiveSet.Louvers)
            {
                if (louver.Reading1 == 0 && louver.Reading2 == 0)
                {
                    ActiveLouverID = louver.ID;
                    IsEnabledAcquareTop = true;
                    IsEnabledAcquireBottom = false;
                    ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);
                    return;
                }
            }
        });


            ReportApproved = new BaseCommand(obj =>
        {
            //UpdateView.Execute("Scan");
            //PrintLouverSortedLabels = true;
            //NextLouverSet.Execute("");
            SortedLabelsPopUpInitialize();
        });

















            SortedLabelsComplete = new BaseCommand(obj =>
        {
            NextLouverSet.Execute("");
            UpdateView.Execute("Scan");
        });




            PrintSortedLabels = new BaseCommand(obj =>
            {
                ZebraPrinter _Printer = _zebra.Connect();
                List<Louver> ToPrint = new List<Louver>();
                foreach (var sets in ActivePanel.GetAllSets())
                {
                    ToPrint.AddRange(sets.GetLouverSet());
                }
                _zebra.PrintSortedLouverIDs(_Printer, ToPrint);
                _zebra.Disconnect(_Printer);

                //Debug.WriteLine("Printed sorted set of labels");

                IsEnabledPrintUnsortedLabels = false;
                IsEnabledNextLouverSet = true;
                IsEnabledExitReport = true;
                IsEnabledApproveSet = false;


                UpdatePopUp.Execute("SortedLabelsPopUp");
                SortedLabelsPopUpInitialize();
            });

            NextLouverSet = new BaseCommand(obj =>
            {
                ActiveSet.StopSort(DateTime.Now);


                ActiveLouverID = 0;
                CurOrder = "";
                CurLine = "";
                CurUnit = "";
                CurPanelID = "";
                CurLouverSet = "";
                CurXL = false;
                CurWidth = "";
                CurLength = "";
                ActiveTopReading = 0;
                ActiveBottomReading = 0;
                ListViewContent.Clear();
                CurBarcode1 = "";
                CurBarcode2 = "";
                //CHANGE
                //Barcode1 = "";
                //Barcode2 = "";
                IsReadOnlyBarcode = false;
                IsEnabledBarcode = true;

                IsEnabledPrintUnsortedLabels = false;
                IsEnabledAcquareTop = false;
                IsEnabledAcquireBottom = false;
                IsEnabledReviewReport = false;
                IsEnabledPrintSortedLabels = false;
                IsEnabledNextLouverSet = false;
                ActiveDeviation = 0;


                FocusBarcode1 = true;
            });












            SearchOrder = new BaseCommand(obj =>
        {
            var order = _allOrders.GetOrder(new BarcodeSet(Barcode1, Barcode2));
            ActivePanel = order.GetOpeningByLine(order.BarcodeHelper.Line).GetPanel(order.BarcodeHelper.PanelID);
            ActiveSet = ActivePanel.GetSet(order.BarcodeHelper.Set);

            ReCutBarcode1 = order.BarcodeHelper.BarcodeSet.Barcode1.ToString();
            ReCutBarcode2 = order.BarcodeHelper.BarcodeSet.Barcode2.ToString();
            ReCutOrder = order.BarcodeHelper.Order.ToString();
            ReCutLine = order.BarcodeHelper.Line.ToString();
            ReCutUnit = order.BarcodeHelper.Unit.ToString();
            ReCutPanelID = order.BarcodeHelper.PanelID.ToString();
            ReCutLouverSet = order.BarcodeHelper.Set.ToString();
            ReCutXL = order.BarcodeHelper.Style == LouverStructure.LouverStyle.LouverStyles.XL;
            ReCutWidth = order.BarcodeHelper.Width.ToString();
            ReCutLength = order.BarcodeHelper.Length.ToString();

            var report = ActiveSet.GenerateReport();
            ReCutContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));
        });

            CheckTop = new BaseCommand(obj =>
                {
                    Random random = new Random();

                    // Generate a random integer between -1000 and 1000
                    int randomInt = random.Next(-1000, 1001);

                    // Divide the random integer by 1000 to get increments of 0.001
                    //double value = randomInt / 1000.0;
                    double value = RecordWhenStable(0.001);
                    //ActiveSet.Louvers[ActiveLouverID].SetReading1(value);
                    //Reading1 = value;

                    if (Math.Abs(value - Convert.ToDouble(RecutReading1)) <= 0.1)
                    {
                        TxtTopAcceptableReplacement = "Yes";
                    }
                    else
                    {
                        TxtTopAcceptableReplacement = "no";
                    }
                });

            CheckBottom = new BaseCommand(obj =>
            {
                Random random = new Random();

                // Generate a random integer between -1000 and 1000
                int randomInt = random.Next(-1000, 1001);

                // Divide the random integer by 1000 to get increments of 0.001
                //double value = randomInt / 1000.0;
                double value = RecordWhenStable(0.001);
                //ActiveSet.Louvers[ActiveLouverID].SetReading1(value);
                //Reading1 = value;

                if (Math.Abs(value - Convert.ToDouble(RecutReading2)) <= 0.1)
                {
                    TxtBottomAcceptableReplacement = "Yes";
                }
                else
                {
                    TxtBottomAcceptableReplacement = "no";
                }
            });









            LostFocusReporting = new BaseCommand(obj =>
        {
            VisilityReportingPassword = Visibility.Visible;
            VisibilityReporting = Visibility.Collapsed;
            ReportingPasswordToolTip = "Enter Your Password";
        });

            LostFocusSettings = new BaseCommand(obj =>
            {
                VisilitySettingsPassword = Visibility.Visible;
                VisibilitySettings = Visibility.Collapsed;
                SettingsPasswordToolTip = "Enter Your Password";
            });











            ShutDown = new BaseCommand(obj =>
         {













             //// Serialize the object to JSON
             //string j = JsonConvert.SerializeObject(AllOrders, Formatting.Indented);

             //// Specify the file path where you want to write the JSON
             //string filePath = System.IO.Directory.GetCurrentDirectory() + @"\json.txt";

             //// Write the JSON string to the file
             //File.WriteAllText(filePath, j);


             Console.WriteLine("JSON exported and saved to file.");

             Application.Current.Shutdown();


         });
        }
        #endregion

        #region Code Behind

        //View Intialize
        public void SortedLabelsPopUpInitialize()
        {
            ObservableCollection<LabelID> lables = new ObservableCollection<LabelID>();
            foreach (var louver in ReportContent)
            {
                lables.Add(new LabelID(louver.LouverID, louver.LouverOrder, louver.Orientation));
            }
            LabelIDContent = lables;
        }
        public void ScanInitialize()
        {
            ConnectToDataQ();
            IsEnabledPrintUnsortedLabels = false;
            IsEnabledAcquareTop = false;
            IsEnabledAcquireBottom = false;
            IsEnabledReviewReport = false;
            IsEnabledPrintSortedLabels = false;
            IsEnabledNextLouverSet = false;
        }
        public void MenuInitialize()
        {

        }
        public void ReportInitialize()
        {
            var report = ActiveSet.GenerateReport();
            ReportContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));
            IsEnabledApproveSet = true;
        }

        public void ReCutInitialize()
        {

        }
        public void LouverCountPopUpLoaded()
        {
            FocusLouverCount = true;
        }




        //DataQ
        public void ConnectToDataQ()
        {
            Thread test = new Thread(() =>
            {
                _dataQ = new DataQHelper();
                _dataQ.Connect();
                _dataQ.Start();

                VisibilityDisconnected = Visibility.Collapsed;
                _stopwatch.Start();

                _dataQ.AnalogUpdated += new EventHandler(DataQNewData);
                _dataQ.LostConnection += new EventHandler(DataQLostConnection);
            });
            test.Start();
        }

        public double RecordWhenStable(double variationallowed)
        {
            double currentValue = 0; // Variable to record the stabilized value

            // Create an array to store the readings
            List<double> readings = new List<double>();

            // Simulate readings (replace with actual reading logic)
            do
            {
                // Store the reading in the array
                readings.Add(_dataQ.GetDistance());
                readings.Add(_dataQ.GetDistance());


                // Compare with previous reading (if available)
                int count = readings.Count;
                if (count > 1)
                {
                    double difference = Math.Abs(readings[count - 1] - readings[count - 2]);
                    if (difference <= variationallowed)
                    {
                        // Value is stabilized within acceptable range
                        currentValue = readings[count - 1];
                    }
                }

            }
            while (currentValue == 0);
            return currentValue;
        }





        public void DataQNewData(object sender, EventArgs e)
        {
            VoltageValues.Add(new MeasureModel
            {
                ElapsedMilliseconds = _stopwatch.Elapsed.TotalSeconds,
                Value = _dataQ.GetDistance()
            });
            //Debug.WriteLine(_DataQ.GetDistance());
            if (VoltageValues.Count > 25)
            {
                VoltageValues.RemoveAt(0);
            }
            //CurrentReading = VoltageValues[VoltageValues.Count].Value.ToString();
        }

        public void DataQLostConnection(object sender, EventArgs e)
        {
            Debug.WriteLine("Lost Conenction");
        }



        //JSON
        public void LoadFromJson()
        {
            //string json = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\json.txt");

            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            //AllOrders = JsonConvert.DeserializeObject<OrderManager>(json, settings);
        }

        public void SaveToJson()
        {
            //// Serialize to JSON
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\json.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, _allOrders);
            }
        }

        //View Closing
        public void Closing()
        {
            try
            {
                _dataQ.Stop();
                _dataQ.Disconnect();
                _stopwatch.Stop();
                _stopwatch.Reset();
                VisibilityDisconnected = Visibility.Visible;
            }
            catch (Exception)
            {
                throw;
            }
        }









        #endregion



    }
}