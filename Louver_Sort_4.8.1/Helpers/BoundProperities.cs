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
using OfficeOpenXml;
using System.Windows.Media;



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
        public Globals _globals = new Globals();

        //Global

        public string AdminPassword
        {
            get => _globals.AdminPassword;
            set
            {
                SetProperty(ref _globals.AdminPassword, value);
            }
        }
        public double RejectionSpec
        {
            get => _globals.RejectionSpec;
            set
            {
                SetProperty(ref _globals.RejectionSpec, value);
            }
        }

        private string _excelExportLocation;
        public string ExcelExportLocation
        {
            get => _excelExportLocation;
            set
            {
                SetProperty(ref _excelExportLocation, value);
                if (DateRangeStart != null && DateRangeEnd != null && ExcelExportLocation != null)
                {
                    IsEnabledExcelExport = true;
                }
            }
        }



        string Barcode1Regex = @"^\d{16}P\d$";
        string Barcode2Regex = @"^(PNL[1-9])\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$";
        string EmptyRegex = @"^$";

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
        private string _jSONSaveLocation = AppDomain.CurrentDomain.BaseDirectory;

        private bool _isCheckedUseFakeValues = false;
        public bool IsCheckedUseFakeValues
        {
            get => _isCheckedUseFakeValues;
            set { SetProperty(ref _isCheckedUseFakeValues, value); }
        }

        public int DataRetentionPeriod
        {
            get => _globals.DataRetentionPeriod;
            set { SetProperty(ref _globals.DataRetentionPeriod, value); }
        }

        //User Control Views
        private UserControl _selectedPopUp;
        public UserControl SelectedPopUp { get => _selectedPopUp; set => SetProperty(ref _selectedPopUp, value); }
        private UserControl _selectedView;
        public UserControl SelectedView { get => _selectedView; set => SetProperty(ref _selectedView, value); }
        private UserControl _reportingView;

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
        private bool _isEnabledEnterBarcode = true;
        public bool IsEnabledEnterBarcode
        {
            get => _isEnabledEnterBarcode;
            set { SetProperty(ref _isEnabledEnterBarcode, value); }
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
        private bool _isEnabledCancel = false;
        public bool IsEnabledCancel
        {
            get => _isEnabledCancel;
            set { SetProperty(ref _isEnabledCancel, value); }
        }

        private bool _isEnabledLouverCountOk = false;
        public bool IsEnabledLouverCountOk
        {
            get => _isEnabledLouverCountOk;
            set { SetProperty(ref _isEnabledLouverCountOk, value); }
        }
        private bool _isEnabledReCutBarcode = true;
        public bool IsEnabledReCutBarcode
        {
            get => _isEnabledReCutBarcode;
            set { SetProperty(ref _isEnabledReCutBarcode, value); }
        }

        private bool _isEnabledExcelExport = false;
        public bool IsEnabledExcelExport
        {
            get => _isEnabledExcelExport;
            set { SetProperty(ref _isEnabledExcelExport, value); }
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
        private Visibility _visilityPassword = Visibility.Visible;
        public Visibility VisilityPassword
        {
            get => _visilityPassword;
            set
            {
                SetProperty(ref _visilityPassword, value);
            }
        }
        private Visibility _visibilityAdmin = Visibility.Collapsed;
        public Visibility VisibilityAdmin
        {
            get => _visibilityAdmin;
            set
            {
                SetProperty(ref _visibilityAdmin, value);
            }
        }
        private Visibility _visibilityCalibRecord = Visibility.Collapsed;
        public Visibility VisibilityCalibRecord
        {
            get => _visibilityCalibRecord;
            set { SetProperty(ref _visibilityCalibRecord, value); }
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
        //private string _barcode1 = "1018652406000001L1";
        private string _barcode1;
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
                    if (Regex.IsMatch(value, Barcode1Regex))
                    {
                        SetProperty(ref _barcode1, value);
                    }
                    else if (Regex.IsMatch(value, EmptyRegex))
                    {
                        SetProperty(ref _barcode1, "");
                    }
                    else
                    {
                        SetProperty(ref _barcode1, "");
                    }
                }
            }
        }
        //private string _barcode2 = "PNL1/LXL/L4.5/L30.5188/LT";
        private string _barcode2;
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
                    if (Regex.IsMatch(value, Barcode2Regex))
                    {
                        SetProperty(ref _barcode2, value);

                        if (Regex.IsMatch(Barcode1, Barcode1Regex))
                        {
                            IsEnabledEnterBarcode = true;
                        }
                    }
                    else if (Regex.IsMatch(value, EmptyRegex))
                    {
                        SetProperty(ref _barcode2, "");
                    }
                    else
                    {
                        SetProperty(ref _barcode2, "");
                    }
                }
            }
        }
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
        private int? _txtLouverCount;
        public int? TxtLouverCount
        {
            get => _txtLouverCount;
            set
            {
                Regex regex = new Regex("[0-9]");
                if (regex.IsMatch(value.ToString()))
                {
                    IsEnabledLouverCountOk = true;
                    SetProperty(ref _txtLouverCount, value);
                }
                else if (value == null)
                {
                    SetProperty(ref _txtLouverCount, value);
                }
            }
        }
        private string _hintLouverCount = "Louver Count";
        public string HintLouverCount
        {
            get => _hintLouverCount;
            set { SetProperty(ref _hintLouverCount, value); }
        }
        private double _activeDeviation;
        public double ActiveDeviation
        {
            get => _activeDeviation;
            set { SetProperty(ref _activeDeviation, value); }
        }



        //ADMIN
        private string _password;
        public string Password
        {
            get => _password;
            set { SetProperty(ref _password, value); }
        }
        private string _passwordToolTip;
        public string PasswordToolTip
        {
            get => _passwordToolTip;
            set { SetProperty(ref _passwordToolTip, value); }
        }
        private DateTime? _dateRangeStart;
        public DateTime? DateRangeStart
        {
            get => _dateRangeStart;

            set
            {
                SetProperty(ref _dateRangeStart, value);

                if (DateRangeStart != null && DateRangeEnd != null && ExcelExportLocation != null)
                {
                    IsEnabledExcelExport = true;
                }
            }
        }
        private DateTime? _dateRangeEnd;
        public DateTime? DateRangeEnd
        {
            get => _dateRangeEnd;
            set
            {
                SetProperty(ref _dateRangeEnd, value);

                if (DateRangeStart != null && DateRangeEnd != null && ExcelExportLocation != null)
                {
                    IsEnabledExcelExport = true;
                }
            }
        }
        private DateTime _dateTimeToday = DateTime.Now;
        public DateTime DateTimeToday
        {
            get => _dateTimeToday;
            set { SetProperty(ref _dateTimeToday, value); }
        }







        //UserMessagePopUp
        private string _txtUserMessage;
        public string TxtUserMessage
        {
            get => _txtUserMessage;
            set { SetProperty(ref _txtUserMessage, value); }
        }


        //Calib
        private string _calibTxtBoxHint;
        public string CalibTxtBoxHint
        {
            get => _calibTxtBoxHint;
            set { SetProperty(ref _calibTxtBoxHint, value); }
        }
        private string _calibTxt;
        public string CalibTxt
        {
            get => _calibTxt;
            set { SetProperty(ref _calibTxt, value); }
        }
        private int _calibStep = 1;



        private string _calibImage;
        public string CalibImage
        {
            get => _calibImage;
            set { SetProperty(ref _calibImage, value); }
        }

        private Visibility _visibilityCalibImage = Visibility.Collapsed;
        public Visibility VisibilityCalibImage
        {
            get => _visibilityCalibImage;
            set { SetProperty(ref _visibilityCalibImage, value); }
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
                    IsEnabledRejectSelectedLouver = true;
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
                    IsEnabledCheckTop = true;
                    TopMinimumValue = (ActiveSet.Louvers[index].Reading1 - RejectionSpec).ToString();
                    TopMaximumValue = (ActiveSet.Louvers[index].Reading1 + RejectionSpec).ToString();
                    BottomMinimumValue = (ActiveSet.Louvers[index].Reading2 - RejectionSpec).ToString();
                    BottomMaximumValue = (ActiveSet.Louvers[index].Reading2 + RejectionSpec).ToString();
                    TopColor = Brushes.Red;
                    BottomColor = Brushes.Red;
                }
            }
        }
        private string _topMinimumValue;
        public string TopMinimumValue
        {
            get => _topMinimumValue;
            set { SetProperty(ref _topMinimumValue, value); }
        }

        private string _topMaximumValue;
        public string TopMaximumValue
        {
            get => _topMaximumValue;
            set { SetProperty(ref _topMaximumValue, value); }
        }

        private string _bottomMinimumValue;
        public string BottomMinimumValue
        {
            get => _bottomMinimumValue;
            set { SetProperty(ref _bottomMinimumValue, value); }
        }

        private string _bottomMaximumValue;
        public string BottomMaximumValue
        {
            get => _bottomMaximumValue;
            set { SetProperty(ref _bottomMaximumValue, value); }
        }


        private Brush _topColor;
        public Brush TopColor
        {
            get => _topColor;
            set { SetProperty(ref _topColor, value); }
        }

        private Brush _bottomColor;
        public Brush BottomColor
        {
            get => _bottomColor;
            set { SetProperty(ref _bottomColor, value); }
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
        public ICommand BrowseForJSONSaveLocation { get; set; }
        public ICommand Barcode1KeyDown { get; set; }
        public ICommand Barcode2KeyDown { get; set; }
        public ICommand ReCutBarcode2KeyDown { get; set; }
        public ICommand Calibrate { get; set; }
        public ICommand CalibRecord { get; set; }
        public ICommand ScanLoaded { get; set; }
        public ICommand ReCutLoaded { get; set; }
        public ICommand ReconnectToDataQ { get; set; }
        public ICommand EnterBarcodes { get; set; }
        public ICommand LouverCountPopUpLoaded { get; set; }
        public ICommand LouverCountOk { get; set; }
        public ICommand ClosePopUp { get; set; }
        public ICommand PrintUnsortedLabels { get; set; }
        public ICommand AcqReadingTop { get; set; }
        public ICommand AcqReadingBottom { get; set; }
        public ICommand ReviewLouverReport { get; set; }
        public ICommand CancelOrder { get; set; }
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
        public ICommand ExportExcel { get; set; }
        public ICommand ShutDown { get; set; }





        public ICommand TEMP { get; set; }

        #endregion

        #region CommandImplementation
        public BoundProperities()
        {


            TEMP = new BaseCommand(obj =>
            {
                foreach (var orderWithBarcode in _allOrders.OrdersWithBarcodes)
                {
                    Order order = orderWithBarcode.Order;
                    Debug.WriteLine($"Order Details: Barcode Set = {orderWithBarcode.BarcodeSet.ToString()}");

                    // Write additional details about each order
                    Debug.WriteLine($"Unit: {order.Unit}");
                    Debug.WriteLine($"Number of Openings: {order.Openings.Count}");
                    foreach (var opening in order.Openings)
                    {
                        Debug.WriteLine($"  Model Number: {opening.ModelNum}, Style: {opening.Style}");
                        Debug.WriteLine($"  Opening Line: {opening.Line}, Width: {opening.Width}, Length: {opening.Length}");
                        foreach (var panel in opening.Panels)
                        {
                            Debug.WriteLine($"    Panel ID: {panel.ID}, Sets: {panel.Sets.Count}");
                            foreach (var set in panel.Sets)
                            {
                                Debug.WriteLine($"      DateSortStarted: {set.DateSortStarted}, DateSortFinished: {set.DateSortFinished}");
                                Debug.WriteLine($"      Set ID: {set.ID}, Louver Count: {set.LouverCount}");
                                foreach (var louver in set.Louvers)
                                {
                                    Debug.WriteLine($"      ID: {louver.ID}, Reading1: {louver.Reading1}");
                                    Debug.WriteLine($"      Reading2: {louver.Reading2}, Deviation: {louver.Deviation}");
                                    Debug.WriteLine($"      AbsDeviation: {louver.AbsDeviation}, orienation: {louver.Orientation}");
                                    Debug.WriteLine($"      rejected: {louver.Rejected}, causeofrejection: {louver.CauseOfRejection}");
                                    Debug.WriteLine($"      sortedID: {louver.SortedID}");
                                }
                            }
                        }
                    }
                }
            });








            if (CheckFile(_jSONSaveLocation + "\\LouverSortData.ini") && CheckFile(_jSONSaveLocation + "\\Globals.ini"))
            {
                LoadFromJson();
            }


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
                     case "Reporting":
                         if (Password == AdminPassword)
                         {
                             VisilityPassword = Visibility.Collapsed;
                             VisibilityAdmin = Visibility.Visible;
                         }
                         else
                         {
                             PasswordToolTip = "Incorrect Password";
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
                        break;
                    case "SortedLabelsPopUp":
                        SelectedPopUp = new Views.PopUps.SortedLabelsPopUp();
                        break;
                    case "Calibrate":
                        SelectedPopUp = new Views.PopUps.CalibratePopUp();
                        break;
                    case "Message":
                        SelectedPopUp = new Views.PopUps.UserMessagePopUp();
                        break;
                    case "Await":
                        SelectedPopUp = new Views.PopUps.AwaitResult();
                        break;
                    case "Close":
                        SelectedPopUp = null;
                        VisibilityPopUp = Visibility.Hidden;
                        IsEnabledMain = true;
                        SelectedPopUp = null;
                        MainContentBlurRadius = 0;
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

            BrowseForJSONSaveLocation = new BaseCommand(obj =>
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer; // Set the initial folder to display
                folderBrowserDialog.Description = "Select a folder for saving the JSON file";
                folderBrowserDialog.ShowNewFolderButton = true; // Allow user to create new folder

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // User selected a folder and pressed OK
                    ExcelExportLocation = folderBrowserDialog.SelectedPath; // Get the selected folder path
                }
                else
                {
                    ExcelExportLocation = null; // User canceled the operation
                }
            });

            Barcode1KeyDown = new BaseCommand(obj =>
            {
                var focusedElement = Keyboard.FocusedElement as FrameworkElement;

                if (focusedElement is TextBox)
                {
                    BindingExpression be = focusedElement.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }
                if (Barcode1Correct())
                {
                    FocusBarcode2 = true;
                }
                else
                {
                    Barcode1 = "";
                    FocusBarcode1 = true;
                }
            });

            Barcode2KeyDown = new BaseCommand(obj =>
            {
                var focusedElement = Keyboard.FocusedElement as FrameworkElement;
                if (focusedElement is TextBox)
                {
                    BindingExpression be = focusedElement.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }
                EnterBarcodes.Execute("");
            });

            ReCutBarcode2KeyDown = new BaseCommand(obj =>
            {
                var focusedElement = Keyboard.FocusedElement as FrameworkElement;
                if (focusedElement is TextBox)
                {
                    BindingExpression be = focusedElement.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }
                SearchOrder.Execute("");
            });

            Calibrate = new BaseCommand(obj =>
            {
                switch (_calibStep)
                {
                    case 1:
                        UpdatePopUp.Execute("Calibrate");
                        CalibTxt = "Place laser centering plate on slide and adjust sensor until red dot is in the cross hair";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\1.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;
                    case 2:
                        UpdatePopUp.Execute("Calibrate");
                        CalibTxt = "Turn laser to teach mode";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\2.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;
                    case 3:
                        UpdatePopUp.Execute("Calibrate");
                        CalibTxt = "Set calibration plate on top of slide";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\3.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;
                    case 4:
                        UpdatePopUp.Execute("Calibrate");
                        CalibTxt = "Press plus on the laser";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\2.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;
                    case 5:
                        UpdatePopUp.Execute("Calibrate");
                        CalibTxt = "Set calibration plate on bottom of slide";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\4.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;
                    case 6:
                        UpdatePopUp.Execute("Calibrate");
                        CalibTxt = "Press minus on laser";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\2.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;
                    case 7:
                        UpdatePopUp.Execute("Calibrate");
                        CalibTxt = "Turn dial on laser back to run";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\5.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;
                    case 8:
                        VisibilityCalibImage = Visibility.Collapsed;
                        CalibTxt = "Place flat calibration plate on slide";
                        CalibTxtBoxHint = "";
                        VisibilityCalibRecord = Visibility.Collapsed;
                        _calibStep += 1;
                        break;
                    case 9:
                        Thread RecordThread = new Thread(() =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdatePopUp.Execute("Close");
                            UpdatePopUp.Execute("Await");
                        });


                            if (_dataQ == null)
                            {
                                ConnectToDataQ();
                            }

                            _dataQ.SetCalibrationFlat(_dataQ.RecordAndAverageReadings().Result);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                UpdatePopUp.Execute("Close");
                            });


                            
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CalibTxt = "Place stepped calibration plate on slide";
                                CalibTxtBoxHint = "";
                                VisibilityCalibRecord = Visibility.Collapsed;
                                UpdatePopUp.Execute("Calibrate");
                                _calibStep += 1;

                            });



                        });
                        RecordThread.Start();
                        break;
                    case 10:
                        Thread RecordThread1 = new Thread(() =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdatePopUp.Execute("Close");
                            UpdatePopUp.Execute("Await");
                        });
                            if (_dataQ == null)
                            {
                                ConnectToDataQ();
                            }
                           _dataQ.SetCalibrationStep(_dataQ.RecordAndAverageReadings().Result);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                UpdatePopUp.Execute("Close");
                                CalibTxt = "Calibartion value is:   " + _dataQ.GetSlope();
                                VisibilityCalibRecord = Visibility.Collapsed;
                                UpdatePopUp.Execute("Calibrate");
                                _calibStep++;
                            });
                        });
                        RecordThread1.Start();

                        break;
                    case 11:
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdatePopUp.Execute("Close");
                            _calibStep = 1;
                        });
                        break;
                    default:
                        break;
                }
            });

            ScanLoaded = new BaseCommand(obj =>
            {
                FocusBarcode1 = true;
            });

            ReCutLoaded = new BaseCommand(obj =>
            {
                FocusBarcode1 = true;
            });

            ReconnectToDataQ = new BaseCommand(obj =>
            {
                ConnectToDataQ();
            });

            EnterBarcodes = new BaseCommand(obj =>
            {
                if ((Barcode1 != null && Barcode2 != null))
                {
                    if ((Barcode1 != "" && Barcode2 != ""))
                    {
                        var o = _allOrders.CheckIfOrderExists(new BarcodeSet(Barcode1, Barcode2));
                        if (o != null)
                        {
                            TxtUserMessage = "Order Already Sorted";
                            UpdatePopUp.Execute("Message");
                            Barcode1 = "";
                            Barcode2 = "";
                            return;
                        }
                        else
                        {
                            IsReadOnlyBarcode = true;
                            IsEnabledBarcode = false;
                            IsEnabledEnterBarcode = false;
                            IsEnabledCancel = true;
                            UpdatePopUp.Execute("LouverCount");
                        }
                    }
                    else
                    {
                        TxtUserMessage = "Incorrect Barcode";
                        UpdatePopUp.Execute("Message");
                        FocusBarcode1 = true;
                    }
                }
                else
                {
                    TxtUserMessage = "Incorrect Barcode";
                    UpdatePopUp.Execute("Message");
                    FocusBarcode1 = true;
                }

            });

            LouverCountPopUpLoaded = new BaseCommand(obj =>
            {
                FocusLouverCount = true;
            });

            LouverCountOk = new BaseCommand(obj =>
            {

                var focusedElement = Keyboard.FocusedElement as FrameworkElement;

                if (focusedElement is TextBox)
                {
                    BindingExpression be = focusedElement.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }

                ClosePopUp.Execute("");
                var order = _allOrders.CreateOrderAfterScanAndFillAllVariables(new BarcodeSet(Barcode1, Barcode2), Convert.ToInt32(TxtLouverCount));
                ActiveLouverID = 1;
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

                if (!IsCheckedUseFakeValues)
                {
                    ZebraPrinter _Printer = _zebra.Connect();
                    List<Louver> ToPrint = new List<Louver>();
                    foreach (var sets in ActivePanel.GetAllSets())
                    {
                        ToPrint.AddRange(sets.GetLouverSet());
                    }
                    _zebra.PrintLouverIDs(_Printer, ToPrint);
                    _zebra.Disconnect(_Printer);
                }



                IsEnabledPrintUnsortedLabels = false;
                IsEnabledAcquareTop = true;
                IsEnabledAcquireBottom = false;

                TxtUserMessage = "Place Unsorted Labels on Louvers";
                UpdatePopUp.Execute("Message");
            });

            AcqReadingTop = new BaseCommand(obj =>
            {
                Random random = new Random();

                // Generate a random integer between -1000 and 1000
                int randomInt = random.Next(-50, 1001);

                double value;
                if (IsCheckedUseFakeValues)
                {
                    value = randomInt / 1000.0;
                    ActiveSet.Louvers[ActiveLouverID - 1].SetReading1(value);
                    ActiveTopReading = value;
                    ListViewContent = ActiveSet.GenerateRecordedLouvers();
                    IsEnabledAcquareTop = false;
                    IsEnabledAcquireBottom = true;
                    ListViewSelectedLouver = ListViewContent[(ListViewContent.IndexOf(ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID)) + 1)];
                    UpdatePopUp.Execute("Close");
                }
                else
                {
                    Thread RecordThread = new Thread(() =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdatePopUp.Execute("Await");
                        });
                        value = _dataQ.RecordAndAverageReadings().Result;
                        ActiveSet.Louvers[ActiveLouverID - 1].SetReading1(value);
                        ActiveTopReading = value;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ListViewContent = ActiveSet.GenerateRecordedLouvers();
                            IsEnabledAcquareTop = false;
                            IsEnabledAcquireBottom = true;
                            ListViewSelectedLouver = ListViewContent[(ListViewContent.IndexOf(ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID)) + 1)];
                            UpdatePopUp.Execute("Close");
                        });
                    });
                    RecordThread.Start();
                }
            });

            AcqReadingBottom = new BaseCommand(obj =>
            {
                Random random = new Random();

                // Generate a random integer between -1000 and 1000
                int randomInt = random.Next(-50, 1001);
                double value;
                if (IsCheckedUseFakeValues)
                {
                    value = randomInt / 1000.0;
                    ActiveSet.Louvers[ActiveLouverID - 1].SetReading2(value);
                    ActiveBottomReading = value;

                    ListViewContent = ActiveSet.GenerateRecordedLouvers();
                    ActiveSet.Louvers[ActiveLouverID - 1].CalcValues(RejectionSpec);
                    ActiveDeviation = ActiveSet.Louvers[ActiveLouverID - 1].Deviation;
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
                }
                else
                {
                    Thread RecordThread = new Thread(() =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdatePopUp.Execute("Await");
                        });
                        value = _dataQ.RecordAndAverageReadings().Result;
                        ActiveSet.Louvers[ActiveLouverID - 1].SetReading2(value);
                        ActiveBottomReading = value;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ListViewContent = ActiveSet.GenerateRecordedLouvers();
                            ActiveSet.Louvers[ActiveLouverID - 1].CalcValues(RejectionSpec);
                            ActiveDeviation = ActiveSet.Louvers[ActiveLouverID - 1].Deviation;
                            ListViewContent = ActiveSet.GenerateRecordedLouvers();
                            UpdatePopUp.Execute("Close");

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
                    });
                    RecordThread.Start();
                }


            });

            ReviewLouverReport = new BaseCommand(obj =>
            {
                ActiveSet.Sort();
                ListViewContent = null;
                ListViewContent = ActiveSet.GenerateRecordedLouvers();
                UpdateView.Execute("Report");
                IsEnabledReviewReport = false;
            });

            CancelOrder = new BaseCommand(obj =>
            {
                var orderToRemove = _allOrders.OrdersWithBarcodes.FirstOrDefault(orderWithBarcode => orderWithBarcode.BarcodeSet.Equals(new BarcodeSet(Barcode1, Barcode2)));
                if (orderToRemove != null)
                {
                    _allOrders.OrdersWithBarcodes.Remove(orderToRemove);
                }
                NextLouverSet.Execute("");
                IsEnabledCancel = false;
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



                foreach (var item in ReportContent)
                {
                    if (item.Status == "FAIL")
                    {
                        ReportSelectedLouver = item;
                        IsEnabledApproveSet = false;
                    }
                }
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
                if (!IsCheckedUseFakeValues)
                {
                    ZebraPrinter _Printer = _zebra.Connect();
                    List<Louver> ToPrint = new List<Louver>();
                    foreach (var sets in ActivePanel.GetAllSets())
                    {
                        ToPrint.AddRange(sets.GetLouverSet());
                    }
                    _zebra.PrintSortedLouverIDs(_Printer, ToPrint);
                    _zebra.Disconnect(_Printer);
                }

                IsEnabledPrintUnsortedLabels = false;
                IsEnabledNextLouverSet = true;
                IsEnabledExitReport = true;
                IsEnabledApproveSet = false;


                UpdatePopUp.Execute("SortedLabelsPopUp");
                SortedLabelsPopUpInitialize();
            });

            NextLouverSet = new BaseCommand(obj =>
            {
                if (ActiveSet != null)
                {
                    ActiveSet.StopSort(DateTime.Now);
                }


                TxtLouverCount = null;
                ActiveLouverID = 1;
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
                Barcode1 = "";
                Barcode2 = "";
                IsReadOnlyBarcode = false;
                IsEnabledBarcode = true;
                IsEnabledEnterBarcode = false;

                IsEnabledPrintUnsortedLabels = false;
                IsEnabledAcquareTop = false;
                IsEnabledAcquireBottom = false;
                IsEnabledReviewReport = false;
                IsEnabledPrintSortedLabels = false;
                IsEnabledNextLouverSet = false;
                ActiveDeviation = 0;


                FocusBarcode1 = true;
                IsEnabledCancel = false;
            });

            SearchOrder = new BaseCommand(obj =>
            {
                if ((Barcode1 != null && Barcode2 != null))
                {
                    if ((Barcode1 != "" && Barcode2 != ""))
                    {
                        var order = _allOrders.GetOrder(new BarcodeSet(Barcode1, Barcode2));
                        if (order != null)
                        {
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
                            ReCutContent = null;
                            ReCutContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));
                            IsEnabledReCutBarcode = false;
                        }
                        else
                        {
                            TxtUserMessage = "Order not found";
                            UpdatePopUp.Execute("Message");
                            Barcode1 = "";
                            Barcode2 = "";
                        }
                    }
                    else
                    {
                        TxtUserMessage = "Incorrect Barcode";
                        UpdatePopUp.Execute("Message");
                        FocusBarcode1 = true;
                    }
                }
                else
                {
                    TxtUserMessage = "Incorrect Barcode";
                    UpdatePopUp.Execute("Message");
                    FocusBarcode1 = true;
                }


            });

            CheckTop = new BaseCommand(obj =>
                {
                    Random random = new Random();

                    // Generate a random integer between -1000 and 1000
                    int randomInt = random.Next(-1000, 1001);

                    // Divide the random integer by 1000 to get increments of 0.001
                    double value = randomInt / 1000.0;
                    //double value = RecordWhenStable(0.001);
                    TxtTopAcceptableReplacement = value.ToString();
                    //ActiveSet.Louvers[ActiveLouverID].SetReading1(value);
                    //Reading1 = value;

                    if (Math.Abs(value - Convert.ToDouble(RecutReading1)) <= RejectionSpec)
                    {
                        TopColor = Brushes.Green;
                    }
                    else
                    {
                        TopColor = Brushes.Red;
                    }



                    IsEnabledCheckTop = false;
                    IsEnabledCheckBottom = true;
                });

            CheckBottom = new BaseCommand(obj =>
            {
                Random random = new Random();

                // Generate a random integer between -1000 and 1000
                int randomInt = random.Next(-1000, 1001);

                // Divide the random integer by 1000 to get increments of 0.001
                double value = randomInt / 1000.0;
                //double value = RecordWhenStable(0.001);
                TxtBottomAcceptableReplacement = value.ToString();
                //ActiveSet.Louvers[ActiveLouverID].SetReading1(value);
                //Reading1 = value;

                if (Math.Abs(value - Convert.ToDouble(RecutReading2)) <= RejectionSpec)
                {
                    BottomColor = Brushes.Green;
                    TxtUserMessage = "Louver is a good replacement";
                    UpdatePopUp.Execute("Message");
                }
                else
                {
                    BottomColor = Brushes.Red;
                    TxtUserMessage = "Louver is NOT a replacement";
                    UpdatePopUp.Execute("Message");




                }

                ReCutContent = null;
                TopMinimumValue = null;
                TopMaximumValue = null;
                BottomMinimumValue = null;
                BottomMaximumValue = null;
                IsEnabledCheckTop = false;
                IsEnabledCheckBottom = false;
                TopColor = Brushes.Gray;
                BottomColor = Brushes.Gray;
                TxtTopAcceptableReplacement = null;
                TxtBottomAcceptableReplacement = null;
                IsEnabledReCutBarcode = true;
                FocusBarcode1 = true;
                Barcode1 = null;
                Barcode2 = null;
            });

            ExportExcel = new BaseCommand(obj =>
            {
                string dateTimeFormat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string filename = $"LouverSortExport_{dateTimeFormat}.xlsx";
                string fullPath = Path.Combine(_excelExportLocation, filename);
                ExportToExcel(fullPath);

                TxtUserMessage = "Report Generated";
                UpdatePopUp.Execute("Message");
                DateRangeStart = null;
                DateRangeEnd = null;
                IsEnabledExcelExport = false;
                ExcelExportLocation = null;
            });

            ShutDown = new BaseCommand(obj =>
            {
                SaveToJson();

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
            if (!IsCheckedUseFakeValues)
            {
                ConnectToDataQ();
            }
            IsEnabledPrintUnsortedLabels = false;
            IsEnabledAcquareTop = false;
            IsEnabledAcquireBottom = false;
            IsEnabledReviewReport = false;
            IsEnabledPrintSortedLabels = false;
            IsEnabledNextLouverSet = false;
        }
        public void ReportInitialize()
        {
            var report = ActiveSet.GenerateReport();
            ReportContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));
            IsEnabledApproveSet = true;
            foreach (var item in report)
            {
                if (item.Status == "FAIL")
                {
                    ReportSelectedLouver = item;
                    IsEnabledApproveSet = false;
                }
            }
        }

        public void ReCutInitialize()
        {

        }

        //DataQ
        public void ConnectToDataQ()
        {
            Thread test = new Thread(() =>
            {
                if (_dataQ == null)
                {
                    _dataQ = new DataQHelper();
                    _dataQ.Connect();
                    _dataQ.Start();

                    VisibilityDisconnected = Visibility.Collapsed;
                    _stopwatch.Start();

                    _dataQ.AnalogUpdated += new EventHandler(DataQNewData);
                    _dataQ.LostConnection += new EventHandler(DataQLostConnection);
                }
            });
            test.Start();
        }

        public void DataQNewData(object sender, EventArgs e)
        {
            VoltageValues.Add(new MeasureModel
            {
                ElapsedMilliseconds = _stopwatch.Elapsed.TotalSeconds,
                Value = Math.Round(_dataQ.GetDistanceWCal(), 3)
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

        public static bool CheckFile(string filePath)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return false;
            }

            // Try to open the file
            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    Console.WriteLine("File is available and can be opened.");
                    return true;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error opening file: " + ex.Message);
                return false;
            }
        }


        public void LoadFromJson()
        {
            string json = File.ReadAllText(_jSONSaveLocation + "\\LouverSortData.ini");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            _allOrders = JsonConvert.DeserializeObject<OrderManager>(json, settings);

            json = File.ReadAllText(_jSONSaveLocation + "\\Globals.ini");

            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            _globals = JsonConvert.DeserializeObject<Globals>(json, settings);
            //AdminPassword = JsonConvert.DeserializeObject<string>(json, settings);
            //JSONSaveLocation = JsonConvert.DeserializeObject<string>(json, settings);
            //RejectionSpec = JsonConvert.DeserializeObject<double>(json, settings);
        }

        public void SaveToJson()
        {
            DeleteDataOlderThan90Days();
            //// Serialize to JSON
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(_jSONSaveLocation + "\\LouverSortData.ini"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, _allOrders);
                //serializer.Serialize(writer, AdminPassword);
                //serializer.Serialize(writer, JSONSaveLocation);
                //serializer.Serialize(writer, RejectionSpec);
            }
            using (StreamWriter sw = new StreamWriter(_jSONSaveLocation + "\\Globals.ini"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, _globals);
                //serializer.Serialize(writer, AdminPassword);
                //serializer.Serialize(writer, JSONSaveLocation);
                //serializer.Serialize(writer, RejectionSpec);
            }
        }

        /// <summary>
        /// Delete data older than 90 days.
        /// </summary>
        public void DeleteDataOlderThan90Days()
        {
            DateTime cutoffDate = DateTime.Now.AddDays(-90);
            List<OrderWithBarcode> OrderstoRemove = new List<OrderWithBarcode>();
            foreach (var order in _allOrders.OrdersWithBarcodes)
            {
                foreach (var openings in order.Order.Openings)
                {
                    foreach (var panels in openings.Panels)
                    {
                        foreach (var sets in panels.Sets)
                        {
                            if (sets.DateSortFinished < cutoffDate)
                            {
                                if (!OrderstoRemove.Contains(order))
                                {
                                    OrderstoRemove.Add(order);
                                }
                            }
                        }
                    }
                }
            }
            foreach (var orderToRemove in OrderstoRemove)
            {
                _allOrders.OrdersWithBarcodes.Remove(orderToRemove);
            }
        }

        public bool IsInSelectedRange(Order o)
        {
            DateTime cutoffDate = DateTime.Now.AddDays(-90);
            List<OrderWithBarcode> OrderstoRemove = new List<OrderWithBarcode>();
            foreach (var openings in o.Openings)
            {
                foreach (var panels in openings.Panels)
                {
                    foreach (var sets in panels.Sets)
                    {
                        if (sets.DateSortFinished < DateRangeEnd || sets.DateSortFinished > DateRangeStart)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ExportToExcel(string filename)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var l = 1;
                foreach (var order in _allOrders.OrdersWithBarcodes)
                {
                    if (IsInSelectedRange(order.Order))
                    {
                        var sheet = package.Workbook.Worksheets.Add(l.ToString());
                        l += 1;
                        sheet.Cells[1, 1].Value = "Barcode 1:";
                        sheet.Cells[2, 1].Value = order.BarcodeSet.Barcode1.ToString();
                        sheet.Cells[1, 2].Value = "Barcode2:";
                        sheet.Cells[2, 2].Value = order.BarcodeSet.Barcode2.ToString();
                        foreach (var openings in order.Order.Openings)
                        {
                            sheet.Cells[1, 3].Value = "Line:";
                            sheet.Cells[2, 3].Value = openings.Line.ToString();
                            sheet.Cells[1, 4].Value = "Model:";
                            sheet.Cells[2, 4].Value = openings.ModelNum.ToString();
                            sheet.Cells[1, 5].Value = "Style:";
                            sheet.Cells[2, 5].Value = openings.Style.ToString();
                            sheet.Cells[1, 6].Value = "Length:";
                            sheet.Cells[2, 6].Value = openings.Length.ToString();
                            sheet.Cells[1, 7].Value = "Width:";
                            sheet.Cells[2, 7].Value = openings.Width.ToString();
                            foreach (var panels in openings.Panels)
                            {
                                sheet.Cells[1, 8].Value = "ID:";
                                sheet.Cells[2, 8].Value = panels.ID.ToString();
                                foreach (var sets in panels.Sets)
                                {
                                    sheet.Cells[1, 9].Value = "Louver Count:";
                                    sheet.Cells[2, 9].Value = sets.LouverCount.ToString();
                                    sheet.Cells[1, 10].Value = "Date Sort Started:";
                                    sheet.Cells[2, 10].Value = sets.DateSortStarted.ToString();
                                    sheet.Cells[1, 11].Value = "Date Sort Finsihed:";
                                    sheet.Cells[2, 11].Value = sets.DateSortFinished.ToString();

                                    var i = 2;
                                    foreach (var louver in sets.Louvers)
                                    {
                                        if (i == 2)
                                        {
                                            sheet.Cells[4, 1].Value = "ID";
                                            sheet.Cells[5, 1].Value = "Sorted ID";
                                            sheet.Cells[6, 1].Value = "Orientation";
                                            sheet.Cells[7, 1].Value = "Processed";
                                            sheet.Cells[8, 1].Value = "Reading Top";
                                            sheet.Cells[9, 1].Value = "Reading Bottom";
                                            sheet.Cells[10, 1].Value = "Deviation";
                                            sheet.Cells[11, 1].Value = "Abs Deviation";
                                            sheet.Cells[12, 1].Value = "Rejected";
                                            sheet.Cells[13, 1].Value = "Cause of Rejection";
                                        }
                                        sheet.Cells[3, i].Value = "Louver";
                                        sheet.Cells[4, i].Value = louver.ID.ToString();
                                        sheet.Cells[5, i].Value = louver.SortedID.ToString();
                                        sheet.Cells[6, i].Value = louver.Orientation.ToString();
                                        sheet.Cells[7, i].Value = louver.Processed.ToString();
                                        sheet.Cells[8, i].Value = louver.Reading1.ToString();
                                        sheet.Cells[9, i].Value = louver.Reading2.ToString();
                                        sheet.Cells[10, i].Value = louver.Deviation.ToString();
                                        sheet.Cells[11, i].Value = louver.AbsDeviation.ToString();
                                        sheet.Cells[12, i].Value = louver.Rejected.ToString();
                                        sheet.Cells[13, i].Value = louver.CauseOfRejection.ToString();
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }

                if (package.Workbook.Worksheets.Count > 0)
                {
                    package.SaveAs(new FileInfo(filename));
                }

            }
        }

        //View Closing
        public void Closing()
        {
            try
            {
                if (!IsCheckedUseFakeValues)
                {
                    _dataQ.Stop();
                    _dataQ.Disconnect();
                    _stopwatch.Stop();
                    _stopwatch.Reset();
                }

                VisibilityDisconnected = Visibility.Visible;
            }
            catch (Exception)
            {
                throw;
            }
        }




        //Other
        public bool Barcode1Correct()
        {
            if (Barcode1 != null)
            {
                if (Barcode1 != "")
                {
                    if (Regex.IsMatch(Barcode1, Barcode1Regex))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Barcode2Correct()
        {
            if (Barcode2 != null)
            {
                if (Barcode2 != "")
                {
                    if (Regex.IsMatch(Barcode2, Barcode2Regex))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }




        #endregion



    }











}



