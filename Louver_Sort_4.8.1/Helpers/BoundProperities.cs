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
using Newtonsoft.Json;
using System.Diagnostics;
using OfficeOpenXml;
using System.Windows.Media;
using Louver_Sort_4._8._1.Views.PopUps;
using System.Reflection;
using Zebra.Sdk.Printer;
using System.IO.Ports;
using System.Threading.Tasks;

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

        #region Class Instances
        //Class Instances
        private DataQHelper _dataQ;
        private ZebraPrinterHelper _zebra = new ZebraPrinterHelper();
        public OrderManager _allOrders = new OrderManager();
        public Globals _globals = new Globals();
        #endregion

        #region Generic
        //Generic
        string Barcode1Regex = @"^\d{16}P\d$";
        string Barcode2Regex = @"^(PNL[1-9])\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$";
        string EmptyRegex = @"^$";
        private string _jSONSaveLocation = AppDomain.CurrentDomain.BaseDirectory;
        string cultureCode = "en-US";
        #endregion

        #region Globals
        // Globals
        public string AdminPassword
        {
            get => _globals.AdminPassword;
            set => SetProperty(ref _globals.AdminPassword, value);
        }

        public double RejectionSpec
        {
            get => _globals.RejectionSpec;
            set => SetProperty(ref _globals.RejectionSpec, value);
        }

        public double ReCutRailSpec
        {
            get => _globals.ReCutRailSpec;
            set => SetProperty(ref _globals.ReCutRailSpec, value);
        }

        public double ReCutLouverToLouverSpec
        {
            get => _globals.ReCutLouverToLouverSpec;
            set => SetProperty(ref _globals.ReCutLouverToLouverSpec, value);
        }

        public double GapSpecRailToLouver
        {
            get => _globals.GapSpecRailToLouver;
            set => SetProperty(ref _globals.GapSpecRailToLouver, value);
        }

        public double GapSpecLouverToLouver
        {
            get => _globals.GapSpecLouverToLouver;
            set => SetProperty(ref _globals.GapSpecLouverToLouver, value);
        }

        //CHANGE
        public int RecalibrationPeriod
        {
            get => _globals.RecalibrationPeriod;
            set => SetProperty(ref _globals.RecalibrationPeriod, value);
        }

        public double CalibrationRejectionSpec
        {
            get => _globals.CalibrationRejectionSpec;
            set => SetProperty(ref _globals.CalibrationRejectionSpec, value);
        }

        //CHANGE
        public int DataRetentionPeriod
        {
            get => _globals.DataRetentionPeriod;
            set => SetProperty(ref _globals.DataRetentionPeriod, value);
        }

        private bool _en_USEnabled;
        public bool EnUSEnabled
        {
            get => _en_USEnabled;
            set => SetProperty(ref _en_USEnabled, value);
        }

        private bool _es_ESEnabled;
        public bool EsESEnabled
        {
            get => _es_ESEnabled;
            set => SetProperty(ref _es_ESEnabled, value);
        }

        public List<string> UserIDs
        {
            get => _globals.UserIDs;
            set => SetProperty(ref _globals.UserIDs, value);
        }

        #endregion

        #region User Control Views
        // User Control Views
        private UserControl _selectedPopUp;
        public UserControl SelectedPopUp
        {
            get => _selectedPopUp;
            set => SetProperty(ref _selectedPopUp, value);
        }
        #endregion

        #region IsEnabled
        // IsEnabled
        private bool _isEnabledMain = true;
        public bool IsEnabledMain
        {
            get => _isEnabledMain;
            set => SetProperty(ref _isEnabledMain, value);
        }

        private bool _isEnabledAcquireBottom;
        public bool IsEnabledAcquireBottom
        {
            get => _isEnabledAcquireBottom;
            set => SetProperty(ref _isEnabledAcquireBottom, value);
        }

        private bool _isEnabledAcquareTop;
        public bool IsEnabledAcquareTop
        {
            get => _isEnabledAcquareTop;
            set => SetProperty(ref _isEnabledAcquareTop, value);
        }

        private bool _isEnabledApproveSet = true;
        public bool IsEnabledApproveSet
        {
            get => _isEnabledApproveSet;
            set => SetProperty(ref _isEnabledApproveSet, value);
        }

        private bool _isEnabledBarcode = true;
        public bool IsEnabledBarcode
        {
            get => _isEnabledBarcode;
            set => SetProperty(ref _isEnabledBarcode, value);
        }

        private bool _isEnabledCalibrate = false;
        public bool IsEnabledCalibrate
        {
            get => _isEnabledCalibrate;
            set => SetProperty(ref _isEnabledCalibrate, value);
        }

        private bool _isEnabledCancel = false;
        public bool IsEnabledCancel
        {
            get => _isEnabledCancel;
            set => SetProperty(ref _isEnabledCancel, value);
        }

        private bool _isEnabledCheckBottom;
        public bool IsEnabledCheckBottom
        {
            get => _isEnabledCheckBottom;
            set => SetProperty(ref _isEnabledCheckBottom, value);
        }

        private bool _isEnabledCheckTop;
        public bool IsEnabledCheckTop
        {
            get => _isEnabledCheckTop;
            set => SetProperty(ref _isEnabledCheckTop, value);
        }

        private bool _isEnabledEnterBarcode = true;
        public bool IsEnabledEnterBarcode
        {
            get => _isEnabledEnterBarcode;
            set => SetProperty(ref _isEnabledEnterBarcode, value);
        }

        private bool _isEnabledExcelExport = false;
        public bool IsEnabledExcelExport
        {
            get => _isEnabledExcelExport;
            set => SetProperty(ref _isEnabledExcelExport, value);
        }

        private bool _isEnabledExitReport = false;
        public bool IsEnabledExitReport
        {
            get => _isEnabledExitReport;
            set => SetProperty(ref _isEnabledExitReport, value);
        }

        private bool _isEnabledLouverCountOk = false;
        public bool IsEnabledLouverCountOk
        {
            get => _isEnabledLouverCountOk;
            set => SetProperty(ref _isEnabledLouverCountOk, value);
        }

        private bool _isEnabledNextLouverSet;
        public bool IsEnabledNextLouverSet
        {
            get => _isEnabledNextLouverSet;
            set => SetProperty(ref _isEnabledNextLouverSet, value);
        }

        private bool _isEnabledPrintSortedLabels;
        public bool IsEnabledPrintSortedLabels
        {
            get => _isEnabledPrintSortedLabels;
            set => SetProperty(ref _isEnabledPrintSortedLabels, value);
        }

        private bool _isEnabledPrintUnsortedLabels;
        public bool IsEnabledPrintUnsortedLabels
        {
            get => _isEnabledPrintUnsortedLabels;
            set => SetProperty(ref _isEnabledPrintUnsortedLabels, value);
        }

        private bool _isEnabledReCutCancel = false;
        public bool IsEnabledReCutCancel
        {
            get => _isEnabledReCutCancel;
            set => SetProperty(ref _isEnabledReCutCancel, value);
        }

        private Visibility _isEnabledReCut = Visibility.Visible;
        public Visibility IsEnabledReCut
        {
            get => _isEnabledReCut;
            set => SetProperty(ref _isEnabledReCut, value);
        }

        private bool _isEnabledRejectSelectedLouver = false;
        public bool IsEnabledRejectSelectedLouver
        {
            get => _isEnabledRejectSelectedLouver;
            set => SetProperty(ref _isEnabledRejectSelectedLouver, value);
        }

        private bool _isEnabledReviewReport;
        public bool IsEnabledReviewReport
        {
            get => _isEnabledReviewReport;
            set => SetProperty(ref _isEnabledReviewReport, value);
        }

        private bool _isEnabledReworkSet = false;
        public bool IsEnabledReworkSet
        {
            get => _isEnabledReworkSet;
            set => SetProperty(ref _isEnabledReworkSet, value);
        }

        private bool _isEnabledScan = true;
        public bool IsEnabledScan
        {
            get => _isEnabledScan;
            set => SetProperty(ref _isEnabledScan, value);
        }

        private bool _isEnabledUserID = false;
        public bool IsEnabledUserID
        {
            get => _isEnabledUserID;
            set => SetProperty(ref _isEnabledUserID, value);
        }

        private bool _isEnabledNewUser = false;
        public bool IsEnabledNewUser
        {
            get => _isEnabledNewUser;
            set => SetProperty(ref _isEnabledNewUser, value);
        }
        #endregion

        #region Visibility
        // Visibility
        private Visibility _visibilityAdjustCalib = Visibility.Collapsed;
        public Visibility VisibilityAdjustCalib
        {
            get => _visibilityAdjustCalib;
            set => SetProperty(ref _visibilityAdjustCalib, value);
        }

        private Visibility _visibilityAdmin = Visibility.Collapsed;
        public Visibility VisibilityAdmin
        {
            get => _visibilityAdmin;
            set => SetProperty(ref _visibilityAdmin, value);
        }

        private Visibility _visibilityCalibRecord = Visibility.Collapsed;
        public Visibility VisibilityCalibRecord
        {
            get => _visibilityCalibRecord;
            set => SetProperty(ref _visibilityCalibRecord, value);
        }

        private Visibility _visibilityDisconnected = Visibility.Visible;
        public Visibility VisibilityDisconnected
        {
            get => _visibilityDisconnected;
            set => SetProperty(ref _visibilityDisconnected, value);
        }

        private Visibility _visibilityPopUp;
        public Visibility VisibilityPopUp
        {
            get => _visibilityPopUp;
            set => SetProperty(ref _visibilityPopUp, value);
        }

        private Visibility _visibilityReCutData = Visibility.Collapsed;
        public Visibility VisibilityReCutData
        {
            get => _visibilityReCutData;
            set => SetProperty(ref _visibilityReCutData, value);
        }

        private Visibility _visibilityScan;
        public Visibility VisibilityScan
        {
            get => _visibilityScan;
            set => SetProperty(ref _visibilityScan, value);
        }

        private Visibility _visibilitySortSet = Visibility.Visible;
        public Visibility VisibilitySortSet
        {
            get => _visibilitySortSet;
            set => SetProperty(ref _visibilitySortSet, value);
        }

        private Visibility _visilityPassword = Visibility.Visible;
        public Visibility VisilityPassword
        {
            get => _visilityPassword;
            set => SetProperty(ref _visilityPassword, value);
        }
        #endregion

        #region Focus
        // Focus
        private bool _focusBarcode1 = true;
        public bool FocusBarcode1
        {
            get => _focusBarcode1;
            set => SetProperty(ref _focusBarcode1, value);
        }

        private bool _focusBarcode2;
        public bool FocusBarcode2
        {
            get => _focusBarcode2;
            set => SetProperty(ref _focusBarcode2, value);
        }

        private bool _focusLouverCount;
        public bool FocusLouverCount
        {
            get => _focusLouverCount;
            set => SetProperty(ref _focusLouverCount, value);
        }

        private bool _reCutFocusBarcode1;
        public bool ReCutFocusBarcode1
        {
            get => _reCutFocusBarcode1;
            set => SetProperty(ref _reCutFocusBarcode1, value);
        }

        private bool _reCutFocusBarcode2;
        public bool ReCutFocusBarcode2
        {
            get => _reCutFocusBarcode2;
            set => SetProperty(ref _reCutFocusBarcode2, value);
        }

        private bool _focusUserBadgeIn;
        public bool FocusUserBadgeIn
        {
            get => _focusUserBadgeIn;
            set => SetProperty(ref _focusUserBadgeIn, value);
        }
        #endregion

        #region IsReadOnly
        // IsReadOnly
        private bool _isReadOnlyBarcode;
        public bool IsReadOnlyBarcode
        {
            get => _isReadOnlyBarcode;
            set => SetProperty(ref _isReadOnlyBarcode, value);
        }
        #endregion

        #region Main
        // Main
        private int _mainContentBlurRadius;
        public int MainContentBlurRadius
        {
            get => _mainContentBlurRadius;
            set => SetProperty(ref _mainContentBlurRadius, value);
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }
        #endregion

        #region Variables for Calibration
        //Variables for Calibration
        private string _calibImage;
        public string CalibImage
        {
            get => _calibImage;
            set => SetProperty(ref _calibImage, value);
        }

        private int _calibStep = 1;

        private string _calibTxt;
        public string CalibTxt
        {
            get => _calibTxt;
            set => SetProperty(ref _calibTxt, value);
        }

        private string _calibTxtBoxHint;
        public string CalibTxtBoxHint
        {
            get => _calibTxtBoxHint;
            set => SetProperty(ref _calibTxtBoxHint, value);
        }

        private Visibility _visibilityCalibImage = Visibility.Collapsed;
        public Visibility VisibilityCalibImage
        {
            get => _visibilityCalibImage;
            set => SetProperty(ref _visibilityCalibImage, value);
        }
        #endregion

        #region Variables for Scan
        //Variables for Scan
        private string _barcode1;
        public string Barcode1
        {
            get => _barcode1;
            set
            {
                if (value == null || !Regex.IsMatch(value, Barcode1Regex))
                {
                    SetProperty(ref _barcode1, "");
                }
                else
                {
                    SetProperty(ref _barcode1, value);
                }
            }
        }

        private string _barcode2;
        public string Barcode2
        {
            get => _barcode2;
            set
            {
                if (value == null || !Regex.IsMatch(value, Barcode2Regex))
                {
                    SetProperty(ref _barcode2, "");
                }
                else
                {
                    SetProperty(ref _barcode2, value);

                    if (Regex.IsMatch(Barcode1, Barcode1Regex))
                    {
                        IsEnabledEnterBarcode = true;
                    }
                }
            }
        }

        private int _activeLouverId;
        public int ActiveLouverID
        {
            get => _activeLouverId;
            set => SetProperty(ref _activeLouverId, value);
        }

        public Louver_Sort_4._8._1.Helpers.LouverStructure.Panel _activePanel;
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Panel ActivePanel
        {
            get => _activePanel;
            set => SetProperty(ref _activePanel, value);
        }

        public Louver_Sort_4._8._1.Helpers.LouverStructure.Set _activeSet;
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Set ActiveSet
        {
            get => _activeSet;
            set => SetProperty(ref _activeSet, value);
        }

        private double _activeBottomReading;
        public double ActiveBottomReading
        {
            get => Math.Round(_activeBottomReading, 3);
            set => SetProperty(ref _activeBottomReading, Math.Round(value, 3));
        }

        private double _activeDeviation;
        public double ActiveDeviation
        {
            get => Math.Round(_activeDeviation, 3);
            set => SetProperty(ref _activeDeviation, Math.Round(value, 3));
        }

        private double _activeTopReading;
        public double ActiveTopReading
        {
            get => Math.Round(_activeTopReading, 3);
            set => SetProperty(ref _activeTopReading, Math.Round(value, 3));
        }

        private string _curBarcode1;
        public string CurBarcode1
        {
            get => _curBarcode1;
            set => SetProperty(ref _curBarcode1, value);
        }

        private string _curBarcode2;
        public string CurBarcode2
        {
            get => _curBarcode2;
            set => SetProperty(ref _curBarcode2, value);
        }

        private string _curLine;
        public string CurLine
        {
            get => _curLine;
            set => SetProperty(ref _curLine, value);
        }

        private string _curLouverSet;
        public string CurLouverSet
        {
            get => _curLouverSet;
            set => SetProperty(ref _curLouverSet, value);
        }

        private string _curLength;
        public string CurLength
        {
            get => _curLength;
            set => SetProperty(ref _curLength, value);
        }

        private string _curOrder;
        public string CurOrder
        {
            get => _curOrder;
            set => SetProperty(ref _curOrder, value);
        }

        private string _curPanelID;
        public string CurPanelID
        {
            get => _curPanelID;
            set => SetProperty(ref _curPanelID, value);
        }

        private string _curUnit;
        public string CurUnit
        {
            get => _curUnit;
            set => SetProperty(ref _curUnit, value);
        }

        private string _curWidth;
        public string CurWidth
        {
            get => _curWidth;
            set => SetProperty(ref _curWidth, value);
        }

        private bool _curXL;
        public bool CurXL
        {
            get => _curXL;
            set => SetProperty(ref _curXL, value);
        }

        private int? _txtLouverCount;
        public int? TxtLouverCount
        {
            get => _txtLouverCount;
            set
            {
                Regex regex = new Regex("[0-9]");
                if (regex.IsMatch(value.ToString()) && value > 0)
                {
                    IsEnabledLouverCountOk = true;
                    SetProperty(ref _txtLouverCount, value);
                }
                else if (value == null)
                {
                    IsEnabledLouverCountOk = false;
                    SetProperty(ref _txtLouverCount, value);
                }
                else
                {
                    IsEnabledLouverCountOk = false;
                }
            }
        }

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

        private ChartValues<MeasureModel> _voltageValues;
        public ChartValues<MeasureModel> VoltageValues
        {
            get => _voltageValues ?? (_voltageValues = new ChartValues<MeasureModel>());
            set => SetProperty(ref _voltageValues, value);
        }

        public class MeasureModel
        {
            public double ElapsedMilliseconds { get; set; }
            public double Value { get; set; }
        }
        #endregion

        #region Variables for Recut
        //Variables for Recut
        private string _reCutbarcode1;
        public string ReCutBarcode1
        {
            get => _reCutbarcode1;
            set
            {
                if (value == null || !Regex.IsMatch(value, Barcode1Regex))
                {
                    SetProperty(ref _reCutbarcode1, "");
                }
                else
                {
                    SetProperty(ref _reCutbarcode1, value);
                }
            }
        }

        private string _reCutBarcode2;
        public string ReCutBarcode2
        {
            get => _reCutBarcode2;
            set
            {
                if (value == null || !Regex.IsMatch(value, Barcode2Regex))
                {
                    SetProperty(ref _reCutBarcode2, "");
                }
                else
                {
                    SetProperty(ref _reCutBarcode2, value);

                    if (Regex.IsMatch(ReCutBarcode2, Barcode1Regex))
                    {
                        IsEnabledEnterBarcode = true;
                    }
                }
            }
        }
        private Brush _bottomColor;
        public Brush BottomColor
        {
            get => _bottomColor;
            set => SetProperty(ref _bottomColor, value);
        }

        private string _bottomMaximumValue;
        public string BottomMaximumValue
        {
            get => _bottomMaximumValue;
            set => SetProperty(ref _bottomMaximumValue, value);
        }

        private string _bottomMinimumValue;
        public string BottomMinimumValue
        {
            get => _bottomMinimumValue;
            set => SetProperty(ref _bottomMinimumValue, value);
        }

        private ReportListView _reCutSelectedLouver;
        public ReportListView ReCutSelectedLouver
        {
            get => _reCutSelectedLouver;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _reCutSelectedLouver, value);

                    // Find the index of the Louver object with the same ID as ReportSelectedLouver.
                    int index = ActiveSet.Louvers.FindIndex(louver => louver.ID == ReCutSelectedLouver.LouverID);

                    ZebraPrinter _Printer = _zebra.Connect();
                    List<Louver> toPrint = new List<Louver> { ActiveSet.Louvers[index] };
                    _zebra.PrintLouverIDs(_Printer, toPrint);
                    _zebra.Disconnect(_Printer);

                    // If the Louver with the same ID is found, remove it from the collection.
                    if (index != -1)
                    {
                        if (ReCutSelectedLouver.LouverOrder == 1 || ReCutSelectedLouver.LouverOrder == ActiveSet.LouverCount)
                        {
                            TopMinimumValue = (ActiveSet.Louvers[index].Readings.Reading1 - ReCutRailSpec).ToString().Substring(0, 4);
                            TopMaximumValue = (ActiveSet.Louvers[index].Readings.Reading1 + ReCutRailSpec).ToString().Substring(0, 4);
                            BottomMinimumValue = (ActiveSet.Louvers[index].Readings.Reading2 - ReCutRailSpec).ToString().Substring(0, 4);
                            BottomMaximumValue = (ActiveSet.Louvers[index].Readings.Reading2 + ReCutRailSpec).ToString().Substring(0, 4);
                        }
                        else
                        {
                            TopMinimumValue = (ActiveSet.Louvers[index].Readings.Reading1 - ReCutLouverToLouverSpec).ToString();
                            TopMaximumValue = (ActiveSet.Louvers[index].Readings.Reading1 + ReCutLouverToLouverSpec).ToString();
                            BottomMinimumValue = (ActiveSet.Louvers[index].Readings.Reading2 - ReCutLouverToLouverSpec).ToString();
                            BottomMaximumValue = (ActiveSet.Louvers[index].Readings.Reading2 + ReCutLouverToLouverSpec).ToString();
                        }
                    }
                }
                IsEnabledCheckTop = true;
            }
        }

        private string _recutReading1;
        public string RecutReading1
        {
            get => _recutReading1;
            set => SetProperty(ref _recutReading1, value);
        }

        private string _recutReading2;
        public string RecutReading2
        {
            get => _recutReading2;
            set => SetProperty(ref _recutReading2, value);
        }

        private string _reCutLouverSet;
        public string ReCutLouverSet
        {
            get => _reCutLouverSet;
            set => SetProperty(ref _reCutLouverSet, value);
        }

        private string _reCutOrder;
        public string ReCutOrder
        {
            get => _reCutOrder;
            set => SetProperty(ref _reCutOrder, value);
        }

        private string _reCutOrientation = "";
        public string ReCutOrientation
        {
            get => _reCutOrientation;
            set => SetProperty(ref _reCutOrientation, value);
        }

        private Brush _topColor;
        public Brush TopColor
        {
            get => _topColor;
            set => SetProperty(ref _topColor, value);
        }

        private string _topMaximumValue;
        public string TopMaximumValue
        {
            get => _topMaximumValue;
            set => SetProperty(ref _topMaximumValue, value);
        }

        private string _topMinimumValue;
        public string TopMinimumValue
        {
            get => _topMinimumValue;
            set => SetProperty(ref _topMinimumValue, value);
        }

        private string _txtBottomAcceptableReplacement;
        public string TxtBottomAcceptableReplacement
        {
            get => _txtBottomAcceptableReplacement;
            set => SetProperty(ref _txtBottomAcceptableReplacement, value);
        }

        private string _txtTopAcceptableReplacement;
        public string TxtTopAcceptableReplacement
        {
            get => _txtTopAcceptableReplacement;
            set => SetProperty(ref _txtTopAcceptableReplacement, value);
        }
        #endregion

        #region Variables for Admin
        //Variables for Admin
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

        private DateTime _dateTimeToday = DateTime.Now;
        public DateTime DateTimeToday
        {
            get => _dateTimeToday;
            set => SetProperty(ref _dateTimeToday, value);
        }

        private string _salesnumber;
        public string Salesnumber
        {
            get => _salesnumber;
            set => SetProperty(ref _salesnumber, value);
        }

        public string ExcelExportLocation
        {
            get => _globals.ExcelExportLocation;
            set
            {
                SetProperty(ref _globals.ExcelExportLocation, value);
                if (DateRangeStart != null && DateRangeEnd != null && ExcelExportLocation != null)
                {
                    IsEnabledExcelExport = true;
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _passwordToolTip;
        public string PasswordToolTip
        {
            get => _passwordToolTip;
            set => SetProperty(ref _passwordToolTip, value);
        }

        private string _newUserID;
        public string NewUserID
        {
            get => _newUserID;
            set
            {
                if (value != null)
                {
                    if (Regex.IsMatch(value, @"^\d{1,7}$"))
                    {
                        SetProperty(ref _newUserID, value);
                    }
                    if (value.Length == 7)
                    {
                        IsEnabledNewUser = true;
                    }
                    else
                    {
                        IsEnabledNewUser = false;
                    }
                }
                else
                {
                    _newUserID = value;
                };
            }
        }

        #endregion

        #region Observable Collections
        // Observable Collections
        private ObservableCollection<LabelID> _labelIDContent;
        public ObservableCollection<LabelID> LabelIDContent
        {
            get => _labelIDContent;
            set => SetProperty(ref _labelIDContent, value);
        }

        private ObservableCollection<LouverListView> _listViewContent = new ObservableCollection<LouverListView>();
        public ObservableCollection<LouverListView> ListViewContent
        {
            get => _listViewContent;
            set => SetProperty(ref _listViewContent, value);
        }

        private ObservableCollection<ReportListView> _reCutContent;
        public ObservableCollection<ReportListView> ReCutContent
        {
            get => _reCutContent;
            set => SetProperty(ref _reCutContent, value);
        }

        private ObservableCollection<ReportListView> _reportContent = new ObservableCollection<ReportListView>();
        public ObservableCollection<ReportListView> ReportContent
        {
            get => _reportContent;
            set => SetProperty(ref _reportContent, value);
        }
        #endregion

        #region UserMessagePopUp
        //UserMessagePopUp
        private string _txtUserMessage;
        public string TxtUserMessage
        {
            get => _txtUserMessage;
            set { SetProperty(ref _txtUserMessage, value); }
        }
        #endregion

        #region ReportPopUp
        // ReportPopUp
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
        #endregion

        #region UserBadgeInPopUp
        // ReportPopUp
        private string _employeeID;
        public string EmployeeID
        {
            get => _employeeID;
            set
            {
                if (value != null)
                {
                    if (Regex.IsMatch(value, @"^\d{1,7}$"))
                    {
                        SetProperty(ref _employeeID, value);
                    }
                    if (value.Length == 7)
                    {
                        IsEnabledUserID = true;
                    }
                    else
                    {
                        IsEnabledUserID = false;
                    }
                }
                else
                {
                    _employeeID = value;
                }




            }



    }

        private Brush _userIDForeground = Brushes.White;
        public Brush UserIDForeground
        {
            get => _userIDForeground;
            set
            {
                SetProperty(ref _userIDForeground, value);
            }
        }
        #endregion

        #endregion

        #region Commands
        public ICommand MainLoaded { get; set; }
        public ICommand EnterUserID { get; set; }
        public ICommand UpdatePopUp { get; set; }
        public ICommand ChangeLanguage { get; set; }
        public ICommand FilterEnter { get; set; }
        public ICommand Calibrate { get; set; }
        public ICommand CalibrateLaser { get; set; }
        public ICommand ScanLoaded { get; set; }
        public ICommand Barcode1KeyDown { get; set; }
        public ICommand Barcode2KeyDown { get; set; }
        public ICommand ReconnectToDataQ { get; set; }
        public ICommand EnterBarcodes { get; set; }
        public ICommand LouverCountPopUpLoaded { get; set; }
        public ICommand LouverCountOk { get; set; }
        public ICommand CancelOrder { get; set; }
        public ICommand AcqReadingTop { get; set; }
        public ICommand AcqReadingBottom { get; set; }
        public ICommand ReviewLouverReport { get; set; }
        public ICommand RejectSelected { get; set; }
        public ICommand ReworkSet { get; set; }
        public ICommand ReportApproved { get; set; }
        public ICommand SortedLabelsComplete { get; set; }
        public ICommand ReCutLoaded { get; set; }
        public ICommand ReCutBarcode1KeyDown { get; set; }
        public ICommand ReCutBarcode2KeyDown { get; set; }
        public ICommand CancelRecut { get; set; }
        public ICommand SearchOrder { get; set; }
        public ICommand CheckTop { get; set; }
        public ICommand CheckBottom { get; set; }
        public ICommand RejectRecut { get; set; }
        public ICommand CloseReCutPopUp { get; set; }
        public ICommand AdminLogin { get; set; }
        public ICommand AddNewUser { get; set; }
        public ICommand ChangeUser { get; set; }
        public ICommand BrowseForJSONSaveLocation { get; set; }
        public ICommand ExportExcel { get; set; }
        public ICommand ShutDown { get; set; }

        #endregion

        #region CommandImplementation
        public BoundProperities()
        {
            var Mapper = Mappers.Xy<MeasureModel>()
            .X(x => x.ElapsedMilliseconds)
            .Y(x => x.Value);
            LiveCharts.Charting.For<MeasureModel>(Mapper);

            StartUp();

            MainLoaded = new BaseCommand(obj =>
            {
                UpdatePopUp.Execute("UserBadgeIn");
            });

            EnterUserID = new BaseCommand(obj =>
            {
                bool userfound = false;
                if (EmployeeID != null)
                {
                    foreach (var IDs in UserIDs)
                    {
                        if (EmployeeID == IDs)
                        {
                            userfound = true;
                        }
                    }
                }
                if (userfound)
                {
                    UpdatePopUp.Execute("Close");
                }
                else
                {
                    UserIDForeground = Brushes.Red;
                    EmployeeID = null;
                }
            });

            UpdatePopUp = new BaseCommand(obj =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    // Disable the main content and enable scan functionality
                    IsEnabledMain = false;
                    IsEnabledScan = true;

                    // Show the popup and blur the main content
                    VisibilityPopUp = Visibility.Visible;
                    MainContentBlurRadius = 50;

                    // Determine which popup to display based on the input object
                    switch (obj)
                    {
                        case "LouverCount":
                            SelectedPopUp = new Views.PopUps.LouverCountPopUp();
                            break;
                        case "SortedLabelsPopUp":
                            SelectedPopUp = new Views.PopUps.SortedLabelsPopUp();
                            break;
                        case "Calibrate":
                            SelectedPopUp = new Views.PopUps.CalibratePopUp();
                            break;
                        case "CalibrateLaser":
                            SelectedPopUp = new Views.PopUps.CalibrateLaserPopUp();
                            break;
                        case "Message":
                            SelectedPopUp = new Views.PopUps.UserMessagePopUp();
                            break;
                        case "Await":
                            SelectedPopUp = new Views.PopUps.AwaitResultPopUp();
                            break;
                        case "ReCut":
                            SelectedPopUp = new Views.PopUps.ReCutPopUp();
                            break;
                        case "Report":
                            SelectedPopUp = new Views.PopUps.ReportPopUp();
                            break;
                        case "UserBadgeIn":
                            SelectedPopUp = new Views.PopUps.UserBadgeInPopUp();
                            break;
                        case "Close":
                            // Reset the user message and hide the popup
                            TxtUserMessage = "";
                            SelectedPopUp = null;
                            VisibilityPopUp = Visibility.Hidden;

                            // Re-enable the main content and disable scan functionality
                            IsEnabledMain = true;
                            IsEnabledScan = false;

                            // Remove the blur effect from the main content
                            MainContentBlurRadius = 0;
                            break;
                        default:
                            // Handle any unexpected cases
                            break;
                    }
                });
            });

            ChangeLanguage = new BaseCommand(obj =>
            {
                // Set the current culture and UI culture to the specified language
                switch (cultureCode)
                {
                    case "en-US":
                        cultureCode = "es-ES";
                        break;
                    case "es-ES":
                        cultureCode = "en-US";
                        break;
                    default:
                        break;
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);

                // Clear existing resource dictionaries and load the new one for the selected language
                Application.Current.Resources.MergedDictionaries.Clear();
                ResourceDictionary resdict = new ResourceDictionary()
                {
                    Source = new Uri($"/Dictionary-{cultureCode}.xaml", UriKind.Relative)
                };
                Application.Current.Resources.MergedDictionaries.Add(resdict);

                // Enable or disable language options based on the selected language

            });

            FilterEnter = new BaseCommand(obj =>
            {
                // Check if printing unsorted labels is enabled and the selected tab index is 1
                if (IsEnabledPrintUnsortedLabels && SelectedTabIndex == 1)
                {
                    PrintUnsortedLabels();
                    return;
                }

                // Check if the current popup is LouverCount
                if (SelectedPopUp is LouverCountPopUp)
                {
                    LouverCountOk.Execute("");
                    return;
                }

                // Check if the user message requires closing the popup
                if (TxtUserMessage == "Place Unsorted Labels on Louvers" && SelectedPopUp is UserMessagePopUp)
                {
                    UpdatePopUp.Execute("Close");
                    return;
                }

                // Check if acquiring top reading is enabled and the selected tab index is 1
                if (IsEnabledAcquareTop && SelectedTabIndex == 1)
                {
                    AcqReadingTop.Execute("");
                    return;
                }

                // Check if acquiring bottom reading is enabled, no popup is selected, and the selected tab index is 1
                if (IsEnabledAcquireBottom && SelectedPopUp == null && SelectedTabIndex == 1)
                {
                    AcqReadingBottom.Execute("");
                    return;
                }

                // Check if reviewing the louver report is enabled and the selected tab index is 1
                if (IsEnabledReviewReport && SelectedTabIndex == 1)
                {
                    ReviewLouverReport.Execute("");
                    return;
                }

                // Check if the current popup is Report and rework is enabled
                if (SelectedPopUp is ReportPopUp && IsEnabledReworkSet)
                {
                    ReworkSet.Execute("");
                    return;
                }

                // Check if the current popup is Report and approval is enabled
                if (SelectedPopUp is ReportPopUp && IsEnabledApproveSet)
                {
                    PrintSortedLabels();
                    return;
                }

                // Check if the current popup is SortedLabelsPopUp
                if (SelectedPopUp is SortedLabelsPopUp)
                {
                    SortedLabelsComplete.Execute("");
                    return;
                }

                // Check if the current popup is CalibratePopUp
                if (SelectedPopUp is CalibratePopUp)
                {
                    Calibrate.Execute("");
                    return;
                }

                // Check if the current popup is UserMessagePopUp
                if (SelectedPopUp is UserMessagePopUp)
                {
                    UpdatePopUp.Execute("Close");
                    return;
                }

                // Check if the current popup is ReCutPopUp and the selected tab index is 2
                if (SelectedPopUp is ReCutPopUp && SelectedTabIndex == 2)
                {
                    CloseReCutPopUp.Execute("");
                    return;
                }

                // Check if the selected tab index is 3 for potential future admin login functionality
                if (SelectedTabIndex == 3)
                {
                    // AdminLogin.Execute("");
                }
            });

            Calibrate = new BaseCommand(obj =>
            {
                switch (_calibStep)
                {
                    case 1:
                        // Step 1: Show calibration popup with instructions for the top plate
                        UpdatePopUp.Execute("Calibrate");
                        VisibilityCalibImage = Visibility.Visible;
                        CalibTxt = (Application.Current.Resources["Place calibration plate on top of rail"].ToString());
                        CalibImage = PathImage("CalibCheckTop");
                        CalibTxtBoxHint = "";
                        VisibilityCalibRecord = Visibility.Collapsed;
                        _calibStep += 1;
                        break;

                    case 2:
                        // Step 2: Start thread to record data and update UI for bottom plate
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndSetCalibrationFlatAsync();
                            UpdatePopupForBottomPlate();
                            _calibStep += 1;
                        });
                        break;

                    case 3:
                        // Step 3: Start thread to record data and update UI for the highest step
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndSetCalibrationStepAsync();
                            UpdatePopupForHighestStep();
                            _calibStep += 1;
                        });
                        break;

                    case 4:
                        // Step 4: Start thread to check calibration flat and update UI for the lowest step
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndCheckCalFlatAsync();
                            UpdatePopupForLowestStep();
                            _calibStep += 1;
                        });
                        break;

                    case 5:
                        // Step 5: Start thread to check calibration step and handle the result
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndCheckCalStepAsync();
                            HandleCalibrationResult();
                            _calibStep += 1;
                        });
                        break;

                    case 6:
                        // Step 6: Final step to close popup and reset calibration state
                        UpdatePopUp.Execute("Close");
                        _calibStep = 1;

                        if (!_dataQ._cal.Successful)
                        {
                            HandleUnsuccessfulCalibration();
                        }
                        else
                        {
                            HandleSuccessfulCalibration();
                        }
                        break;

                    default:
                        // Handle any unexpected cases
                        break;
                }
            });

            CalibrateLaser = new BaseCommand(obj =>
            {
                switch (_calibStep)
                {
                    case 1:
                        // Step 1: Show calibration popup with instructions for centering the laser
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = (Application.Current.Resources["Place laser centering plate on slide and adjust sensor until red dot is in the cross hair"].ToString());
                        CalibImage = PathImage("LaserCenterJig");
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 2:
                        // Step 2: Show instructions to turn laser to teach mode
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = (Application.Current.Resources["Turn laser to teach mode"].ToString());
                        CalibImage = PathImage("TurnToTeachMode");
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 3:
                        // Step 3: Show instructions to set calibration plate on top of slide
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = (Application.Current.Resources["Set calibration plate on top of slide"].ToString());
                        CalibImage = PathImage("CalibTop");
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 4:
                        // Step 4: Show instructions to press plus on the laser
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = (Application.Current.Resources["Press plus on the laser"].ToString());
                        CalibImage = PathImage("LaserPlusButton");
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 5:
                        // Step 5: Show instructions to set calibration plate on bottom of slide
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = (Application.Current.Resources["Set calibration plate on bottom of slide"].ToString());
                        CalibImage = PathImage("CalibBottom");
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 6:
                        // Step 6: Show instructions to press minus on the laser
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = (Application.Current.Resources["Press minus on laser"].ToString());
                        CalibImage = PathImage("LaserMinusButton");
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 7:
                        // Step 7: Show instructions to turn the dial on laser back to run
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = (Application.Current.Resources["Turn dial on laser back to run"].ToString());
                        CalibImage = PathImage("TurnToRunMode");
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 8:
                        // Step 8: Final step to close popup and reset calibration state

                        UpdatePopUp.Execute("Close");
                        _calibStep = 1;

                        // Check calibration result
                        if (_dataQ.GetSlope() == double.NaN)
                        {
                            IsEnabledCalibrate = true;
                        }
                        else
                        {
                            VisibilitySortSet = Visibility.Visible;
                            IsEnabledReCut = Visibility.Visible;
                            VisibilityAdjustCalib = Visibility.Collapsed;
                            SelectedTabIndex = 1;
                        }
                        break;

                    default:
                        // Handle any unexpected cases
                        break;
                }
            });

            ScanLoaded = new BaseCommand(obj =>
            {
                FocusBarcode1 = true;
            });

            Barcode1KeyDown = new BaseCommand(obj =>
            {
                UpdateValue();

                // Check if the Barcode1 value is correct
                if (Barcode1Correct(Barcode1))
                {
                    // If correct, set focus to Barcode2
                    FocusBarcode2 = true;
                }
                else
                {
                    // If incorrect, clear Barcode1 and set focus back to Barcode1
                    Barcode1 = "";
                    FocusBarcode1 = true;
                }
            });

            Barcode2KeyDown = new BaseCommand(obj =>
            {
                UpdateValue();

                // Check if the Barcode2 value is correct
                if (Barcode2Correct(Barcode2))
                {
                    // If correct, execute the EnterBarcodes command
                    EnterBarcodes.Execute("");
                }
                else
                {
                    // If incorrect, clear Barcode2 and set focus back to Barcode2
                    Barcode2 = "";
                    FocusBarcode2 = true;
                }
            });

            ReconnectToDataQ = new BaseCommand(obj =>
            {
                //CHANGE - make this function actually do something
                ConnectToDataQ();
            });

            EnterBarcodes = new BaseCommand(obj =>
            {
                if (Barcode1 == null || Barcode2 == null || Barcode1 == "" || Barcode2 == "")
                {
                    // If either Barcode1 or Barcode2 is null or empty, show an error message and focus on Barcode1
                    MessageUser(Application.Current.Resources["IncorrectBarcode"].ToString());
                    FocusBarcode1 = true;
                }
                else
                {
                    // Check if the order exists in _allOrders
                    var order = _allOrders.CheckIfOrderExists(new BarcodeSet(Barcode1, Barcode2));
                    if (order != null)
                    {
                        // If the order already exists, show a message and clear the barcodes
                        MessageUser(Application.Current.Resources["Order Already Sorted"].ToString());
                        Barcode1 = "";
                        Barcode2 = "";
                    }
                    else
                    {
                        // If the order does not exist, set the UI elements accordingly
                        IsReadOnlyBarcode = true;
                        // IsEnabledBarcode = false; // Uncomment if this line is needed
                        IsEnabledEnterBarcode = false;
                        IsEnabledCancel = true;
                        UpdatePopUp.Execute("LouverCount");
                    }
                }
            });

            LouverCountPopUpLoaded = new BaseCommand(obj =>
            {
                FocusLouverCount = true;
            });

            LouverCountOk = new BaseCommand(obj =>
            {
                UpdateValue();

                // Check if the Louver Count is valid; if not, return
                if (TxtLouverCount == null || TxtLouverCount == 0) return;

                // Close the popup
                UpdatePopUp.Execute("Close");

                // Create a new order and update the global order count
                var order = _allOrders.CreateOrderAfterScanAndFillAllVariables(
                    new BarcodeSet(Barcode1, Barcode2),
                    Convert.ToInt32(TxtLouverCount)
                );
                order.User = EmployeeID;
                _globals.OrderCount++;

                // Initialize active louver ID and active panel
                ActiveLouverID = 1;
                ActivePanel = order.GetOpeningByLine(order.BarcodeHelper.Line).GetPanel(order.BarcodeHelper.PanelID);
                ActiveSet = ActivePanel.GetSet(order.BarcodeHelper.Set);

                // Update current order details
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

                // Generate recorded louvers and set the selected louver
                ListViewContent = ActiveSet.GenerateRecordedLouvers();
                ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);

                // Execute print unsorted labels command
                PrintUnsortedLabels();

            });

            CancelOrder = new BaseCommand(obj =>
            {
                // Find the order to remove based on the current barcodes
                var orderToRemove = _allOrders.OrdersWithBarcodes.FirstOrDefault(order =>
                    order.BarcodeSet.Equals(new BarcodeSet(Barcode1, Barcode2)));

                // If the order is found, remove it from the list
                if (orderToRemove != null)
                {
                    _allOrders.OrdersWithBarcodes.Remove(orderToRemove);
                }

                // Execute the next louver set command
                NextLouverSet();

                // Disable the cancel button
                IsEnabledCancel = false;
            });

            AcqReadingTop = new BaseCommand(obj =>
            {
                // Start a new thread for data collection
                Thread recordThread = new Thread(() =>
                {
                    // Show awaiting popup
                    UpdatePopUp.Execute("Await");

                    // Collect data and set the reading for the active louver
                    double value = _dataQ.WaitForDataCollection(true).Result;
                    ActiveSet.Louvers[ActiveLouverID - 1].Readings.SetReading1(value);
                    ActiveTopReading = value;

                    // Update UI elements after data collection
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Refresh the ListView with recorded louvers
                        ListViewContent = ActiveSet.GenerateRecordedLouvers();
                        // Set the selected louver in the ListView if possible
                        int currentLouverIndex = ListViewContent.IndexOf(ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID));
                        if (currentLouverIndex != -1 && currentLouverIndex + 1 < ListViewContent.Count)
                        {
                            ListViewSelectedLouver = ListViewContent[currentLouverIndex + 1];
                        }
                    });
                    // Disable the top acquisition button and enable the bottom acquisition button
                    IsEnabledAcquareTop = false;
                    IsEnabledAcquireBottom = true;
                    // Close the awaiting popup
                    UpdatePopUp.Execute("Close");
                });
                recordThread.Start();
            });

            AcqReadingBottom = new BaseCommand(obj =>
            {
                double value;
                Thread recordThread = new Thread(() =>
                {
                    // Show awaiting popup
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        UpdatePopUp.Execute("Await");
                    });

                    // Collect data and calculate the time difference
                    value = _dataQ.WaitForDataCollection(true).Result;
                    TimeSpan difference = ActiveSet.Louvers[ActiveLouverID - 1].Readings.DateReading1 - DateTime.Now;

                    // Check if the time difference and reading difference are within acceptable ranges
                    if (Math.Abs(difference.TotalSeconds) < 2)
                    {
                        // Show message if the time difference is too small
                        MessageUser(Application.Current.Resources["Ensure you record the correct louver side"].ToString());
                        return;
                    }

                    // Check if the reading difference is within acceptable range
                    if (Math.Abs(Convert.ToDouble(ActiveSet.Louvers[ActiveLouverID - 1].Readings.Reading1 - value)) < 0.007)
                    {
                        // Show message if readings are too close together
                        MessageUser(Application.Current.Resources["Readings are too close together"].ToString());
                        return;
                    }

                    // Set the second reading and update the UI
                    ActiveSet.Louvers[ActiveLouverID - 1].Readings.SetReading2(value);
                    ActiveBottomReading = value;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ListViewContent = ActiveSet.GenerateRecordedLouvers();
                        ActiveSet.Louvers[ActiveLouverID - 1].CalcValues(CalculateRejection(Convert.ToDouble(CurLength)));
                        ActiveDeviation = Convert.ToDouble(ActiveSet.Louvers[ActiveLouverID - 1].Deviation);
                        ListViewContent = ActiveSet.GenerateRecordedLouvers();
                        UpdatePopUp.Execute("Close");

                        // Check for the next louver to record readings
                        foreach (var louver in ActiveSet.Louvers)
                        {
                            if (louver.Readings.Reading1 == null && louver.Readings.Reading2 == null)
                            {
                                ActiveLouverID = louver.ID;
                                IsEnabledAcquareTop = true;
                                IsEnabledAcquireBottom = false;
                                ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);
                                return;
                            }
                        }

                        // If all readings are completed, update the UI state
                        IsEnabledAcquareTop = false;
                        IsEnabledAcquireBottom = false;
                        IsEnabledReviewReport = true;
                        ListViewSelectedLouver = null;
                    });
                });
                recordThread.Start();
            });

            ReviewLouverReport = new BaseCommand(obj =>
            {
                // Sort the active set of louvers
                ActiveSet.Sort();

                // Generate the recorded louvers and update the ListView content
                ListViewContent = ActiveSet.GenerateRecordedLouvers();

                // Display the report popup
                UpdatePopUp.Execute("Report");

                // Disable the review report button
                IsEnabledReviewReport = false;
            });

            RejectSelected = new BaseCommand(obj =>
            {
                // Check if ReportSelectedLouver is not null and ActiveSet is initialized.
                if (ReportSelectedLouver != null && ActiveSet != null)
                {
                    // Find the index of the Louver object with the same ID as ReportSelectedLouver.
                    int index = ActiveSet.Louvers.FindIndex(louver => louver.ID == ReportSelectedLouver.LouverID);

                    // If the Louver with the same ID is found, process the rejection.
                    if (index != -1)
                    {
                        // Remove the selected louver from the report content
                        ReportContent.Remove(ReportSelectedLouver);

                        // Set the cause of rejection and move the louver to the rejected collection
                        ActiveSet.Louvers[index].CauseOfRejection = "User Rejected";
                        ActiveSet.RejectedLouvers.Add(ActiveSet.Louvers[index]);
                        ActiveSet.Louvers.Remove(ActiveSet.Louvers[index]);

                        // Add a new louver with the same ID and re-order the louvers
                        ActiveSet.Louvers.Add(new Louver(index + 1));
                        ActiveSet.Louvers = ActiveSet.Louvers.OrderBy(x => x.ID).ToList();
                    }
                }

                // Update UI elements' states
                IsEnabledReworkSet = true;
                IsEnabledApproveSet = false;
                IsEnabledRejectSelectedLouver = false;

                // Check for any remaining failed items in the report content
                foreach (var item in ReportContent)
                {
                    if (item.Status == "FAIL")
                    {
                        ReportSelectedLouver = item;
                        IsEnabledApproveSet = false;
                        break; // Assuming only the first failed item needs to be processed
                    }
                }
            });

            ReworkSet = new BaseCommand(obj =>
            {
                // Update the ListView content with sorted recorded louvers
                ListViewContent = ConvertListToObservableCollection(
                    ActiveSet.GenerateRecordedLouvers().OrderBy(x => x.LouverID).ToList()
                );

                // Enable top reading and disable printing sorted labels
                IsEnabledAcquareTop = true;
                IsEnabledPrintSortedLabels = false;

                // Close the popup
                UpdatePopUp.Execute("Close");

                // Disable the rework set button
                IsEnabledReworkSet = false;

                // Check for any louver that needs readings and set it as the active louver
                foreach (var louver in ActiveSet.Louvers)
                {
                    if (louver.Readings.Reading1 == null && louver.Readings.Reading2 == null)
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
                PrintSortedLabels();
                // Initialize the sorted labels popup
                SortedLabelsPopUpInitialize();
            });

            SortedLabelsComplete = new BaseCommand(obj =>
            {
                // Execute the next louver set command
                NextLouverSet();

                // Close the popup
                UpdatePopUp.Execute("Close");
            });

            ReCutLoaded = new BaseCommand(obj =>
            {
                // Set focus to the Barcode1 input when the ReCut popup is loaded
                ReCutFocusBarcode1 = true;
            });

            ReCutBarcode1KeyDown = new BaseCommand(obj =>
            {
                UpdateValue();

                // Check if the ReCutBarcode1 value is correct
                if (Barcode1Correct(ReCutBarcode1))
                {
                    // If correct, set focus to ReCutBarcode2
                    ReCutFocusBarcode2 = true;
                }
                else
                {
                    // If incorrect, clear ReCutBarcode1 and set focus back to ReCutBarcode1
                    ReCutBarcode1 = "";
                    ReCutFocusBarcode1 = true;
                }
            });

            ReCutBarcode2KeyDown = new BaseCommand(obj =>
            {
                UpdateValue();

                // Check if the ReCutBarcode2 value is correct
                if (Barcode2Correct(ReCutBarcode2))
                {
                    // If correct, execute the SearchOrder command
                    SearchOrder.Execute("");
                }
                else
                {
                    // If incorrect, clear ReCutBarcode2 and set focus back to ReCutBarcode2
                    ReCutBarcode2 = "";
                    ReCutFocusBarcode2 = true;
                }
            });

            CancelRecut = new BaseCommand(obj =>
            {
                // Reset ReCut barcode fields and content
                ReCutBarcode1 = null;
                ReCutBarcode2 = null;
                ReCutContent = null;

                // Update the state of the ReCut cancel button
                IsEnabledReCutCancel = true;
                IsEnabledReCutCancel = false;
            });

            SearchOrder = new BaseCommand(obj =>
            {
                // Check if both ReCutBarcode1 and ReCutBarcode2 are not null
                if (ReCutBarcode1 != null && ReCutBarcode2 != null)
                {
                    // Check if both ReCutBarcode1 and ReCutBarcode2 are not empty
                    if (ReCutBarcode1 != "" && ReCutBarcode2 != "")
                    {
                        // Get the order based on the barcodes
                        var order = _allOrders.GetOrder(new BarcodeSet(ReCutBarcode1, ReCutBarcode2));
                        if (order != null)
                        {
                            // Initialize active panel and set
                            ActivePanel = order.GetOpeningByLine(order.BarcodeHelper.Line).GetPanel(order.BarcodeHelper.PanelID);
                            ActiveSet = ActivePanel.GetSet(order.BarcodeHelper.Set);

                            // Update ReCut details with the order information
                            ReCutBarcode1 = order.BarcodeHelper.BarcodeSet.Barcode1.ToString();
                            ReCutBarcode2 = order.BarcodeHelper.BarcodeSet.Barcode2.ToString();
                            ReCutOrder = order.BarcodeHelper.Order.ToString();
                            ReCutLouverSet = order.BarcodeHelper.Set.ToString();

                            // Generate the report and update ReCut content
                            var report = ActiveSet.GenerateReport(GapSpecRailToLouver, GapSpecLouverToLouver);
                            ReCutContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));

                            // Enable ReCut cancel button
                            IsEnabledReCutCancel = true;
                        }
                        else
                        {
                            // Show message if order is not found
                            MessageUser(Application.Current.Resources["Order not found"].ToString());
                            ReCutBarcode1 = "";
                            ReCutBarcode2 = "";
                        }
                    }
                    else
                    {
                        // Show message if either barcode is empty
                        MessageUser(Application.Current.Resources["Incorrect Barcode"].ToString());
                        FocusBarcode1 = true;
                    }
                }
                else
                {
                    // Show message if either barcode is null
                    MessageUser(Application.Current.Resources["Incorrect Barcode"].ToString());
                    FocusBarcode1 = true;
                }
            });

            CheckTop = new BaseCommand(obj =>
            {
                // Set initial colors to red
                TopColor = Brushes.Red;
                BottomColor = Brushes.Red;

                // Start a new thread for recording
                Thread recordThread = new Thread(() =>
                {
                    // Show awaiting popup
                    UpdatePopUp.Execute("Await");

                    // Collect data and round the value
                    RecutReading1 = Math.Round(_dataQ.WaitForDataCollection(true).Result, 3).ToString();

                    //// Update the UI with the collected data
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    TxtTopAcceptableReplacement = RecutReading1.ToString();

                    //    // Check if the recorded value is within acceptable range
                    //    double recutReadingValue = Convert.ToDouble(RecutReading1);
                    //    if (Math.Abs(recutReadingValue) <= Convert.ToDouble(TopMaximumValue) && Math.Abs(recutReadingValue) >= Convert.ToDouble(TopMinimumValue))
                    //    {
                    //        TopColor = Brushes.Green;
                    //    }
                    //    else
                    //    {
                    //        TopColor = Brushes.Red;
                    //    }

                    //    // Update UI states
                    IsEnabledCheckTop = false;
                    IsEnabledCheckBottom = true;

                    //});
                    // Close the awaiting popup
                    UpdatePopUp.Execute("Close");
                });

                recordThread.Start();
            });

            CheckBottom = new BaseCommand(obj =>
            {
                // Start a new thread for recording
                Thread recordThread = new Thread(() =>
                {
                    // Show awaiting popup
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        UpdatePopUp.Execute("Await");
                    });

                    // Collect data and round the value
                    RecutReading2 = Math.Round(_dataQ.WaitForDataCollection(true).Result, 3).ToString();

                    // Close the awaiting popup
                    UpdatePopUp.Execute("Close");
                    // Update the UI with the collected data


                    TxtBottomAcceptableReplacement = RecutReading2.ToString();
                    TxtTopAcceptableReplacement = RecutReading1.ToString();

                    // Check if the recorded value is within acceptable range
                    double recutReadingValue = Convert.ToDouble(RecutReading2);
                    // bool IsReplacement = false;



                    //Check Top Vs Top
                    if (Math.Abs(Convert.ToDouble(RecutReading1)) <= Convert.ToDouble(TopMaximumValue) && Math.Abs(Convert.ToDouble(RecutReading1)) >= Convert.ToDouble(TopMinimumValue))
                    {
                        TopColor = Brushes.Green;
                        //Check Bottom Vs Bottom
                        if (Math.Abs(Convert.ToDouble(RecutReading2)) <= Convert.ToDouble(BottomMaximumValue) && Math.Abs(Convert.ToDouble(RecutReading2)) >= Convert.ToDouble(BottomMinimumValue))
                        {
                            BottomColor = Brushes.Green;
                            //No flip has passed
                            //IsReplacement = true;
                            ReCutOrientation = "Do Not Flip";
                            BottomColor = Brushes.Green;
                            TxtUserMessage = "Louver is a good replacement";
                            UpdatePopUp.Execute("ReCut");
                        }
                    }
                    else
                    {
                        //Check Top Vs Bottom
                        if (Math.Abs(Convert.ToDouble(RecutReading1)) <= Convert.ToDouble(BottomMaximumValue) && Math.Abs(Convert.ToDouble(RecutReading1)) >= Convert.ToDouble(BottomMinimumValue))
                        {
                            TopColor = Brushes.Green;
                            //Check Bottom Vs Top
                            if (Math.Abs(Convert.ToDouble(RecutReading2)) <= Convert.ToDouble(TopMaximumValue) && Math.Abs(Convert.ToDouble(RecutReading2)) >= Convert.ToDouble(TopMinimumValue))
                            {
                                BottomColor = Brushes.Green;
                                //Flip has passed
                                //IsReplacement = true;
                                ReCutOrientation = "Flip Louver";
                                BottomColor = Brushes.Green;
                                TxtUserMessage = "Louver is a good replacement";
                                UpdatePopUp.Execute("ReCut");
                            }
                        }
                        else
                        {
                            TopColor = Brushes.Red;
                            BottomColor = Brushes.Red;
                            //All checks failed
                            //IsReplacement = false;
                            ReCutOrientation = "Do Not Flip";
                            TxtUserMessage = "Louver is NOT a replacement";
                            VisibilityReCutData = Visibility.Visible;
                            UpdatePopUp.Execute("ReCut");
                        }
                    }
                    // Update UI states
                    IsEnabledCheckTop = true;
                    IsEnabledCheckBottom = false;
                });

                recordThread.Start();
            });

            RejectRecut = new BaseCommand(obj =>
            {
                // Connect to the Zebra printer
                ZebraPrinter _Printer = _zebra.Connect();
                List<Louver> toPrint = new List<Louver>();

                // Find and add the louver to be printed based on its ID
                toPrint.Add(ActiveSet.Louvers[ActiveSet.Louvers.FindIndex(louver => louver.ID == ReCutSelectedLouver.LouverID)]);

                // Print the sorted louver IDs
                _zebra.PrintLouverIDs(_Printer, toPrint);

                // Disconnect from the Zebra printer
                _zebra.Disconnect(_Printer);

                // Close the popup
                UpdatePopUp.Execute("Close");
            });

            CloseReCutPopUp = new BaseCommand(obj =>
            {
                // Close the popup
                UpdatePopUp.Execute("Close");

                // Connect to the Zebra printer
                ZebraPrinter _Printer = _zebra.Connect();
                List<Louver> toPrint = new List<Louver>();

                // Find and add the louver to be printed based on its ID
                toPrint.Add(ActiveSet.Louvers[ActiveSet.Louvers.FindIndex(louver => louver.ID == ReCutSelectedLouver.LouverID)]);

                // Print the sorted louver IDs
                _zebra.PrintSortedLouverIDs(_Printer, toPrint);
                _zebra.Disconnect(_Printer);
                // Reset ReCut-related variables and UI elements
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
                FocusBarcode1 = true;
                ReCutBarcode1 = null;
                ReCutBarcode2 = null;
            });

            BrowseForJSONSaveLocation = new BaseCommand(obj =>
            {
                // Create and configure a new FolderBrowserDialog
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.MyComputer, // Set the initial folder to display
                    Description = "Select a folder for saving the JSON file",
                    ShowNewFolderButton = true // Allow user to create a new folder
                };

                // Show the dialog and check if the user pressed OK
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // User selected a folder and pressed OK
                    ExcelExportLocation = folderBrowserDialog.SelectedPath; // Get the selected folder path
                }
                else
                {
                    // User canceled the operation
                    ExcelExportLocation = null;
                }
            });

            AdminLogin = new BaseCommand(obj =>
            {
                // Update the binding source if the focused element is a TextBox
                UpdateValue();

                // Check if the entered password matches the admin password
                if (Password == AdminPassword)
                {
                    // Hide the password input and show admin controls
                    VisilityPassword = Visibility.Collapsed;
                    VisibilityAdmin = Visibility.Visible;
                    //VisibilityScan = Visibility.Collapsed;
                }
                else
                {
                    // Clear the password and show a tooltip with an error message
                    Password = "";
                    PasswordToolTip = "Incorrect Password";
                }
            });

            AddNewUser = new BaseCommand(obj =>
            {
                UpdateValue();
                if (NewUserID != null)
                {
                    UserIDs.Add(NewUserID);
                    MessageUser(Application.Current.Resources["User ID Added:"].ToString() + NewUserID);

                }
                else
                {
                    MessageUser(Application.Current.Resources["Issue occured trying to add new user"].ToString() + NewUserID);
                }
            });

            ChangeUser = new BaseCommand(obj =>
            {
                EmployeeID = null;
                UpdatePopUp.Execute("UserBadgeIn");
            });

            ExportExcel = new BaseCommand(obj =>
            {
                // Export data to Excel
                ExportToExcel(Path.Combine(ExcelExportLocation, $"LouverSortExport_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.xlsx"), true);


                //CHANGE - check if date range is entered or sales number and export using that

                //CHANGE - add error if report fails
                // Update user message and show the popup
                MessageUser(Application.Current.Resources["Report Generated"].ToString());

                // Reset date range and disable the export button
                DateRangeStart = null;
                DateRangeEnd = null;
                Salesnumber = "";
                IsEnabledExcelExport = false;
            });

            ShutDown = new BaseCommand(obj =>
            {
                // Save the current state to a JSON file
                SaveToJson();

                // Shut down the application
                Application.Current.Shutdown();
            });
        }
        #endregion

        #region Code Behind
        public string PathImage(string imageName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\", imageName);
        }
        public void StartUp()
        {
            ConnectToDataQ();
            //TestComPort();



            //CHANGE - check each file path in the function individually
            //Add messages  if any of the files didn't load in
            if (CheckFile(_jSONSaveLocation + "\\LouverSortData.ini") && CheckFile(_jSONSaveLocation + "\\Globals.ini") && CheckFile(_jSONSaveLocation + "\\DataQ.ini"))
            {
                // LoadFromJson();
            }



            //Make this a function
            if (_dataQ != null)
            {
                if (_dataQ.GetSlope() == 0)
                {
                    IsEnabledCalibrate = true;
                    VisibilitySortSet = Visibility.Collapsed;
                    IsEnabledReCut = Visibility.Collapsed;
                }
                else
                {
                    IsEnabledCalibrate = true;
                    VisibilitySortSet = Visibility.Visible;
                    IsEnabledReCut = Visibility.Visible;
                    SelectedTabIndex = 1;
                }
            }
            else
            {
                IsEnabledCalibrate = true;
                VisibilitySortSet = Visibility.Collapsed;
                IsEnabledReCut = Visibility.Collapsed;
            }




            ////DELETEME
            //VisibilitySortSet = Visibility.Visible;
            //IsEnabledReCut = Visibility.Visible;
        }
        public void NextLouverSet()
        {
            // Stop sorting the current active set and record the stop time
            if (ActiveSet != null)
            {
                ActiveSet.StopSort(DateTime.Now);
            }

            // Reset text fields and variables
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

            // Reset UI element states
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
            IsEnabledCancel = false;
            FocusBarcode1 = true;

            // Check if calibration is needed based on order count
            if (_globals.OrderCount > _globals.CalibratePeriod)
            {
                IsEnabledCalibrate = true;
                VisibilitySortSet = Visibility.Collapsed;
                IsEnabledReCut = Visibility.Collapsed;
                SelectedTabIndex = 0;
            }
        }
        public void PrintSortedLabels()
        {
            List<Louver> toPrint = new List<Louver>();
            ZebraPrinter _Printer = _zebra.Connect();
            // Collect all louver sets from the active panel
            foreach (var set in ActivePanel.GetAllSets())
            {
                toPrint.AddRange(set.GetLouverSet());
            }

            // Print the sorted louver IDs
            _zebra.PrintSortedLouverIDs(_Printer, toPrint);
            _zebra.Disconnect(_Printer);
            // Update the state of various UI elements
            IsEnabledPrintUnsortedLabels = false;
            IsEnabledNextLouverSet = true;
            IsEnabledExitReport = true;
            IsEnabledApproveSet = false;

            // Execute the popup command and initialize the sorted labels popup
            UpdatePopUp.Execute("SortedLabelsPopUp");
            SortedLabelsPopUpInitialize();
        }
        public void PrintUnsortedLabels()
        {
            // Start sorting the active set with the current date and time
            ActiveSet.StartSort(DateTime.Now);

            // Connect to the Zebra printer
            ZebraPrinter _Printer = _zebra.Connect();
            List<Louver> toPrint = new List<Louver>();

            // Collect all louver sets from the active panel
            foreach (var set in ActivePanel.GetAllSets())
            {
                toPrint.AddRange(set.GetLouverSet());
            }

            // Print the louver IDs
            _zebra.PrintLouverIDs(_Printer, toPrint);
            // Disconnect from the Zebra printer
            _zebra.Disconnect(_Printer);

            // Update the state of various UI elements
            IsEnabledPrintUnsortedLabels = false;
            IsEnabledAcquareTop = true;
            IsEnabledAcquireBottom = false;

            // Show a message to place unsorted labels on louvers
            MessageUser(Application.Current.Resources["Place Unsorted Labels on Louvers"].ToString());
        }
        public void MessageUser(string message)
        {
            // Display a message to the user
            TxtUserMessage = message;
            UpdatePopUp.Execute("Message");
        }
        private void StartCalibrationThread(ThreadStart action)
        {
            // Start a new thread for calibration
            Thread recordThread = new Thread(action);
            recordThread.Start();
        }
        private void UpdatePopUpAndAwait()
        {
            // Close the current popup and then await further instructions
            UpdatePopUp.Execute("Close");
            UpdatePopUp.Execute("Await");
        }
        private async Task ConnectAndSetCalibrationFlatAsync()
        {
            // Connect to DataQ device if not already connected and set calibration to flat
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            await _dataQ.SetCalibrationFlatAsync();

            // Close the current popup
            UpdatePopUp.Execute("Close");
        }
        private void UpdatePopupForBottomPlate()
        {
            // Update popup for bottom plate calibration
            CalibTxt = (Application.Current.Resources["Place calibration plate on bottom of slide"].ToString());
            CalibImage = PathImage("CalibBottom");
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");
        }
        private async Task ConnectAndSetCalibrationStepAsync()
        {
            // Connect to DataQ device if not already connected and set calibration to step
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            await _dataQ.SetCalibrationStepAsync();
        }
        private void UpdatePopupForHighestStep()
        {
            // Update popup for highest step calibration
            CalibTxt = (Application.Current.Resources["Place highest step of Louver Sag Gauge on top of rail"].ToString());
            CalibImage = PathImage("CalibTop");
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");
        }
        private async Task ConnectAndCheckCalFlatAsync()
        {
            // Connect to DataQ device if not already connected and check calibration flat
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            await _dataQ.CheckCalFlatAsync();

            // Close the current popup
            UpdatePopUp.Execute("Close");
        }
        private void UpdatePopupForLowestStep()
        {
            // Update popup for lowest step calibration
            CalibTxt = (Application.Current.Resources["Place lowest step of Louver Sag Gauge on top of rail"].ToString());
            CalibImage = PathImage("CalibCheckTop");
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");
        }
        private async Task ConnectAndCheckCalStepAsync()
        {
            // Connect to DataQ device if not already connected and check calibration step with rejection specification
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            await _dataQ.CheckCalStepAsync(CalibrationRejectionSpec);
        }
        private void HandleCalibrationResult()
        {
            // Handle the result of calibration and update the popup text accordingly
            if (_dataQ._cal.Successful)
            {
                CalibTxt = (Application.Current.Resources["Calibration Passed"].ToString());
            }
            else
            {
                CalibTxt = (Application.Current.Resources["Calibration Failed. Repeat Calibration"].ToString());
            }
            VisibilityCalibImage = Visibility.Collapsed;
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");
        }
        private void HandleUnsuccessfulCalibration()
        {
            // Handle the unsuccessful calibration case
            UpdatePopUp.Execute("Close");
            VisibilitySortSet = Visibility.Collapsed;
            IsEnabledReCut = Visibility.Collapsed;
            VisibilityAdjustCalib = Visibility.Collapsed;
            SelectedTabIndex = 0;
        }
        private void HandleSuccessfulCalibration()
        {
            // Handle the successful calibration case
            VisibilitySortSet = Visibility.Visible;
            IsEnabledReCut = Visibility.Visible;
            VisibilityAdjustCalib = Visibility.Collapsed;
            SelectedTabIndex = 1;
        }
        public void UpdateValue()
        {
            // Get the currently focused element as a FrameworkElement
            var focusedElement = Keyboard.FocusedElement as FrameworkElement;

            // If the focused element is a TextBox, update its binding source
            if (focusedElement is TextBox)
            {
                // Get the binding expression for the Text property of the TextBox
                BindingExpression bindingExpression = focusedElement.GetBindingExpression(TextBox.TextProperty);

                // Update the binding source with the current value in the TextBox
                bindingExpression.UpdateSource();
            }
        }
        public void SortedLabelsPopUpInitialize()
        {
            // Create a list of LabelID objects
            List<LabelID> lables = new List<LabelID>();
            ObservableCollection<LabelID> labels = new ObservableCollection<LabelID>();

            // Populate the list with LabelID objects based on ReportContent
            foreach (var louver in ReportContent)
            {
                lables.Add(new LabelID(louver.LouverID, louver.LouverOrder, louver.Orientation));
            }

            // Sort the list by UnsortedID
            lables = lables.OrderBy(louver => louver.UnsortedID).ToList();

            // Clear the ObservableCollection and add the sorted items
            labels.Clear();
            foreach (var item in lables)
            {
                labels.Add(item);
            }

            // Set the LabelIDContent property to the populated ObservableCollection
            LabelIDContent = labels;
        }
        public void ReportInitialize()
        {
            // Generate a report based on the active set and specified gap specification
            var report = ActiveSet.GenerateReport(GapSpecRailToLouver, GapSpecLouverToLouver);

            // Populate ReportContent with sorted report items
            ReportContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));
            IsEnabledApproveSet = true;

            // Check each item in the report for failure status
            foreach (var item in report)
            {
                if (item.Status == "FAIL")
                {
                    // If any item has a status of "FAIL", disable approval and set the selected louver
                    ReportSelectedLouver = item;
                    IsEnabledApproveSet = false;
                }
            }
        }
        public void ConnectToDataQ()
        {
            try
            {
                // Start a new thread to handle DataQ connection
                Thread test = new Thread(async () =>
                {
                    if (_dataQ == null)
                    {
                        try
                        {
                            // Initialize and connect to DataQ
                            _dataQ = new DataQHelper();
                            await _dataQ.StartConnectionAsync();

                            // Update UI elements and start monitoring
                            VisibilityDisconnected = Visibility.Collapsed;
                            _stopwatch.Start();

                            _dataQ.LatestReadingChanged += new EventHandler(DataQNewData);
                            _dataQ.LostConnection += new EventHandler(DataQLostConnection);

                            _dataQ.StartActiveMonitoring();
                        }
                        catch (DataQException ex)
                        {
                            // Handle DataQ connection exception
                            MessageUser(Application.Current.Resources["Disconnect and Reconnect DataQ, restart application"].ToString());
                            throw ex;
                        }
                    }
                    else
                    {
                        // If DataQ is already connected, prompt user to reconnect and restart application
                        MessageUser(Application.Current.Resources["Disconnect and Reconnect DataQ, restart application"].ToString());
                        return;
                    }
                });
                test.Start();
            }
            catch (Exception)
            {
                // Handle general exceptions and prompt user to reconnect and restart application
                MessageUser(Application.Current.Resources["Disconnect and Reconnect DataQ, restart application"].ToString());
                throw;
            }
        }
        public void DataQLostConnection(object sender, EventArgs e)
        {
            // Handle the event of losing connection to DataQ
            Debug.WriteLine("Lost Connection");
        }
        public static bool CheckFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return false;
            }

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
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            // Load and deserialize LouverSortData.ini
            //_allOrders = LoadJsonFile<OrderManager>(_jSONSaveLocation + "LouverSortData.ini", settings);

            // Load and deserialize Globals.ini
            _globals = LoadJsonFile<Globals>(_jSONSaveLocation + "Globals.ini", settings);

            // Load and deserialize DataQ.ini if it exists and is not "null"
            var dataQJson = File.ReadAllText(_jSONSaveLocation + "DataQ.ini");
            if (dataQJson != "null" && _dataQ != null)
            {
                _dataQ._cal = JsonConvert.DeserializeObject<Calibration>(dataQJson, settings);
            }
            else
            {
                _dataQ._cal = new Calibration();
            }
        }
        public void SaveToJson()
        {
            DeleteDataOlderThan90Days();
            var serializer = new JsonSerializer();

            // Serialize and save LouverSortData.ini
            SaveJsonFile(_jSONSaveLocation + "LouverSortData.ini", _allOrders, serializer);

            // Serialize and save Globals.ini
            SaveJsonFile(_jSONSaveLocation + "Globals.ini", _globals, serializer);

            // Serialize and save DataQ.ini
            if (_dataQ != null)
            {
                SaveJsonFile(_jSONSaveLocation + "DataQ.ini", _dataQ._cal, serializer);
            }
        }
        public void DeleteDataOlderThan90Days()
        {
            DateTime cutoffDate = DateTime.Now.AddDays(-90);
            var ordersToRemove = new ObservableCollection<OrderWithBarcode>();

            foreach (var order in _allOrders.OrdersWithBarcodes)
            {
                if (order.Order.Openings.Exists(opening =>
                    opening.Panels.Exists(panel =>
                        panel.Sets.Exists(set => set.DateSortFinished < cutoffDate))))
                {
                    ordersToRemove.Add(order);
                }
            }

            foreach (var orderToRemove in ordersToRemove)
            {
                _allOrders.OrdersWithBarcodes.Remove(orderToRemove);
            }
        }
        private T LoadJsonFile<T>(string filePath, JsonSerializerSettings settings)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
        private void SaveJsonFile<T>(string filePath, T data, JsonSerializer serializer)
        {
            using (var sw = new StreamWriter(filePath))
            using (var writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data);
            }
        }
        public bool IsInSelectedRange(Order order)
        {
            DateTime cutoffDate = DateTime.Now.AddDays(-90);
            foreach (var opening in order.Openings)
            {
                foreach (var panel in opening.Panels)
                {
                    foreach (var set in panel.Sets)
                    {
                        if (set.DateSortFinished < DateRangeEnd || set.DateSortFinished > DateRangeStart)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public async Task ClosingAsync()
        {
            try
            {
                await _dataQ.StopConnection();
                _stopwatch.Stop();
                _stopwatch.Reset();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during closing: " + ex.Message);
            }
        }
        public bool Barcode1Correct(string barcode)
        {
            return !string.IsNullOrEmpty(barcode) && Regex.IsMatch(barcode, Barcode1Regex);
        }
        public bool Barcode2Correct(string barcode)
        {
            return !string.IsNullOrEmpty(barcode) && Regex.IsMatch(barcode, Barcode2Regex);
        }
        public double CalculateRejection(double length)
        {
            return RejectionSpec * (length / 12);
        }
        public ObservableCollection<T> ConvertListToObservableCollection<T>(List<T> list)
        {
            return new ObservableCollection<T>(list);
        }
        public void ExportToExcel(string filename, bool exportByDateRange)
        {
            var ordersToExport = new List<OrderWithBarcode>();

            // Select orders based on the export type
            if (exportByDateRange)
            {
                ordersToExport.AddRange(_allOrders.OrdersWithBarcodes.FindAll(order => IsInSelectedRange(order.Order)));
            }
            else
            {
                ordersToExport.AddRange(_allOrders.OrdersWithBarcodes.FindAll(order => order.Order.BarcodeHelper.Barcode1.Contains(Salesnumber)));
            }

            // Set Excel package license context
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                SummaryExport(package.Workbook.Worksheets.Add($"Sheet{"Summary"}"), ordersToExport);
                int sheetIndex = 1;
                foreach (var order in ordersToExport)
                {
                    FillOrderSheet(package.Workbook.Worksheets.Add($"Sheet{sheetIndex++}"), order);
                }
                BulkExport(package.Workbook.Worksheets.Add($"Sheet{"Raw"}"), ordersToExport);

                if (package.Workbook.Worksheets.Count > 0)
                {
                    package.SaveAs(new FileInfo(filename));
                }
            }
        }
        private void FillOrderSheet(ExcelWorksheet sheet, OrderWithBarcode order)
        {
            sheet.Cells[1, 1].Value = "Barcode 1:";
            sheet.Cells[2, 1].Value = order.BarcodeSet.Barcode1;
            sheet.Cells[1, 2].Value = "Barcode 2:";
            sheet.Cells[2, 2].Value = order.BarcodeSet.Barcode2;
            sheet.Cells[1, 3].Value = "User:";
            sheet.Cells[2, 3].Value = order.Order.User;

            int row = 3;

            foreach (var opening in order.Order.Openings)
            {
                sheet.Cells[row, 1].Value = "Line:";
                sheet.Cells[row + 1, 1].Value = opening.Line;
                sheet.Cells[row, 2].Value = "Model:";
                sheet.Cells[row + 1, 2].Value = opening.ModelNum;
                sheet.Cells[row, 3].Value = "Style:";
                sheet.Cells[row + 1, 3].Value = opening.Style;
                sheet.Cells[row, 4].Value = "Length:";
                sheet.Cells[row + 1, 4].Value = opening.Length;
                sheet.Cells[row, 5].Value = "Width:";
                sheet.Cells[row + 1, 5].Value = opening.Width;

                row += 2;

                foreach (var panel in opening.Panels)
                {
                    sheet.Cells[row, 1].Value = "ID:";
                    sheet.Cells[row + 1, 1].Value = panel.ID;

                    row += 2;

                    foreach (var set in panel.Sets)
                    {
                        sheet.Cells[row, 1].Value = "Louver Count:";
                        sheet.Cells[row + 1, 1].Value = set.LouverCount;
                        sheet.Cells[row, 2].Value = "Date Sort Started:";
                        sheet.Cells[row + 1, 2].Value = set.DateSortStarted;
                        sheet.Cells[row, 3].Value = "Date Sort Finished:";
                        sheet.Cells[row + 1, 3].Value = set.DateSortFinished;

                        row += 2;

                        // Fill louvers details
                        FillLouversDetails(sheet, set.Louvers, ref row);
                        FillRejectedLouversDetails(sheet, set.RejectedLouvers, ref row);
                    }
                }
            }
        }
        private void FillLouversDetails(ExcelWorksheet sheet, List<Louver> louvers, ref int row)
        {
            if (louvers.Count == 0) return;

            sheet.Cells[row, 1].Value = "Louver Details:";
            row++;

            foreach (var louver in louvers)
            {
                sheet.Cells[row, 1].Value = "ID";
                sheet.Cells[row, 2].Value = louver.ID;
                sheet.Cells[row + 1, 1].Value = "Sorted ID";
                sheet.Cells[row + 1, 2].Value = louver.SortedID;
                sheet.Cells[row + 2, 1].Value = "Orientation";
                sheet.Cells[row + 2, 2].Value = louver.Orientation;
                sheet.Cells[row + 3, 1].Value = "Processed";
                sheet.Cells[row + 3, 2].Value = louver.Processed;
                sheet.Cells[row + 4, 1].Value = "Reading Blank Side";
                sheet.Cells[row + 4, 2].Value = louver.Readings.Reading1;
                sheet.Cells[row + 5, 1].Value = "Reading Label Side";
                sheet.Cells[row + 5, 2].Value = louver.Readings.Reading2;
                sheet.Cells[row + 6, 1].Value = "Deviation";
                sheet.Cells[row + 6, 2].Value = louver.Deviation;
                sheet.Cells[row + 7, 1].Value = "Abs Deviation";
                sheet.Cells[row + 7, 2].Value = louver.AbsDeviation;

                row += 8;
            }
        }
        private void FillRejectedLouversDetails(ExcelWorksheet sheet, List<Louver> rejectedLouvers, ref int row)
        {
            if (rejectedLouvers.Count == 0) return;

            sheet.Cells[row, 1].Value = "Rejected Louvers:";
            row++;

            foreach (var louver in rejectedLouvers)
            {
                sheet.Cells[row, 1].Value = "ID";
                sheet.Cells[row, 2].Value = louver.ID;
                sheet.Cells[row + 1, 1].Value = "Sorted ID";
                sheet.Cells[row + 1, 2].Value = louver.SortedID;
                sheet.Cells[row + 2, 1].Value = "Orientation";
                sheet.Cells[row + 2, 2].Value = louver.Orientation;
                sheet.Cells[row + 3, 1].Value = "Processed";
                sheet.Cells[row + 3, 2].Value = louver.Processed;
                sheet.Cells[row + 4, 1].Value = "Reading Blank Side";
                sheet.Cells[row + 4, 2].Value = louver.Readings.Reading1;
                sheet.Cells[row + 5, 1].Value = "Reading Label Side";
                sheet.Cells[row + 5, 2].Value = louver.Readings.Reading2;
                sheet.Cells[row + 6, 1].Value = "Deviation";
                sheet.Cells[row + 6, 2].Value = louver.Deviation;
                sheet.Cells[row + 7, 1].Value = "Abs Deviation";
                sheet.Cells[row + 7, 2].Value = louver.AbsDeviation;
                sheet.Cells[row + 8, 1].Value = "Rejected";
                sheet.Cells[row + 8, 2].Value = louver.Rejected;
                sheet.Cells[row + 9, 1].Value = "Cause of Rejection";
                sheet.Cells[row + 9, 2].Value = louver.CauseOfRejection;

                row += 10;
            }
        }
        private void BulkExport(ExcelWorksheet sheet, List<OrderWithBarcode> order)
        {
            sheet.Cells[1, 1].Value = "Barcode 1:";
            sheet.Cells[1, 2].Value = "Barcode 2:";
            sheet.Cells[1, 3].Value = "User:";
            sheet.Cells[1, 4].Value = "Line:";
            sheet.Cells[1, 5].Value = "Model:";
            sheet.Cells[1, 6].Value = "Style:";
            sheet.Cells[1, 7].Value = "Length:";
            sheet.Cells[1, 8].Value = "Width:";
            sheet.Cells[1, 9].Value = "ID:";
            sheet.Cells[1, 10].Value = "Louver Count:";
            sheet.Cells[1, 11].Value = "Date Sort Started:";
            sheet.Cells[1, 12].Value = "Date Sort Finished:";
            sheet.Cells[1, 13].Value = "Date Reading 1:";
            sheet.Cells[1, 14].Value = "Date Reading 2:";
            sheet.Cells[1, 15].Value = "ID:";
            sheet.Cells[1, 16].Value = "Sorted ID:";
            sheet.Cells[1, 17].Value = "Orientation:";
            sheet.Cells[1, 18].Value = "Processed:";
            sheet.Cells[1, 19].Value = "Reading Unlabled:";
            sheet.Cells[1, 20].Value = "Reading Labeled:";
            sheet.Cells[1, 21].Value = "Deviation:";
            sheet.Cells[1, 22].Value = "Abs Deviation:";
            sheet.Cells[1, 23].Value = "Rejected:";
            sheet.Cells[1, 24].Value = "Cause of Rejection:";

            int i = 2;
            ObservableCollection<Order> orders = new ObservableCollection<Order>();

            foreach (var orderWithBarcode in order)
            {
                orders.Add(orderWithBarcode.Order);
            }
            foreach (var o in orders)
            {
                foreach (var opening in o.Openings)
                {
                    foreach (var panels in opening.Panels)
                    {
                        foreach (var sets in panels.Sets)
                        {
                            foreach (var louver in sets.Louvers)
                            {
                                sheet.Cells[i, 1].Value = o.BarcodeHelper.BarcodeSet.Barcode1;
                                sheet.Cells[i, 2].Value = o.BarcodeHelper.BarcodeSet.Barcode2;
                                sheet.Cells[i, 3].Value = o.User;
                                sheet.Cells[i, 4].Value = opening.Line;
                                sheet.Cells[i, 5].Value = opening.ModelNum;
                                sheet.Cells[i, 6].Value = opening.Style;
                                sheet.Cells[i, 7].Value = opening.Length;
                                sheet.Cells[i, 8].Value = opening.Width;
                                sheet.Cells[i, 9].Value = panels.ID;
                                sheet.Cells[i, 10].Value = sets.LouverCount;
                                sheet.Cells[i, 11].Value = sets.DateSortStarted;
                                sheet.Cells[i, 12].Value = sets.DateSortFinished;
                                sheet.Cells[i, 13].Value = louver.Readings.DateReading1;
                                sheet.Cells[i, 14].Value = louver.Readings.DateReading2;
                                sheet.Cells[i, 15].Value = louver.ID;
                                sheet.Cells[i, 16].Value = louver.SortedID;
                                sheet.Cells[i, 17].Value = louver.Orientation;
                                sheet.Cells[i, 18].Value = louver.Processed;
                                sheet.Cells[i, 19].Value = louver.Readings.Reading1;
                                sheet.Cells[i, 20].Value = louver.Readings.Reading2;
                                sheet.Cells[i, 21].Value = louver.Deviation;
                                sheet.Cells[i, 22].Value = louver.AbsDeviation;
                                sheet.Cells[i, 23].Value = louver.Rejected;
                                sheet.Cells[i, 24].Value = louver.CauseOfRejection;
                                i++;
                            }
                            foreach (var louver in sets.RejectedLouvers)
                            {
                                sheet.Cells[i, 1].Value = o.BarcodeHelper.BarcodeSet.Barcode1;
                                sheet.Cells[i, 2].Value = o.BarcodeHelper.BarcodeSet.Barcode2;
                                sheet.Cells[i, 3].Value = o.User;
                                sheet.Cells[i, 4].Value = opening.Line;
                                sheet.Cells[i, 5].Value = opening.ModelNum;
                                sheet.Cells[i, 6].Value = opening.Style;
                                sheet.Cells[i, 7].Value = opening.Length;
                                sheet.Cells[i, 8].Value = opening.Width;
                                sheet.Cells[i, 9].Value = panels.ID;
                                sheet.Cells[i, 10].Value = sets.LouverCount;
                                sheet.Cells[i, 11].Value = sets.DateSortStarted;
                                sheet.Cells[i, 12].Value = sets.DateSortFinished;
                                sheet.Cells[i, 13].Value = louver.Readings.DateReading1;
                                sheet.Cells[i, 14].Value = louver.Readings.DateReading2;
                                sheet.Cells[i, 15].Value = louver.ID;
                                sheet.Cells[i, 16].Value = louver.SortedID;
                                sheet.Cells[i, 17].Value = louver.Orientation;
                                sheet.Cells[i, 18].Value = louver.Processed;
                                sheet.Cells[i, 19].Value = louver.Readings.Reading1;
                                sheet.Cells[i, 20].Value = louver.Readings.Reading2;
                                sheet.Cells[i, 21].Value = louver.Deviation;
                                sheet.Cells[i, 22].Value = louver.AbsDeviation;
                                sheet.Cells[i, 23].Value = louver.Rejected;
                                sheet.Cells[i, 24].Value = louver.CauseOfRejection;
                                i++;
                            }
                        }
                    }
                }
            }


        }
        public void DataQNewData(object sender, EventArgs e)
        {
            VoltageValues.Add(new MeasureModel
            {
                ElapsedMilliseconds = _stopwatch.Elapsed.TotalSeconds,
                Value = Math.Round(_dataQ.LatestReading, 3)
            });
            //Debug.WriteLine(_DataQ.GetDistance());
            if (VoltageValues.Count > 25)
            {
                VoltageValues.RemoveAt(0);
            }
            //CurrentReading = VoltageValues[VoltageValues.Count].Value.ToString();
        }

        private void SummaryExport(ExcelWorksheet sheet, List<OrderWithBarcode> order)
        {
            sheet.Cells[1, 1].Value = "Total Orders Ran:";
            sheet.Cells[1, 2].Value = "Total Louvers Ran:";
            sheet.Cells[1, 3].Value = "Average Devation:";
            sheet.Cells[1, 4].Value = "Total Rejects:";
            sheet.Cells[1, 5].Value = "Average Time per Order:";
            sheet.Cells[1, 6].Value = "Other Values can be added by request:";

            int i = 0;
            int totallouvers = 0;
            List<double> Devations = new List<double>();
            List<TimeSpan> dateTimes = new List<TimeSpan>();
            sheet.Cells[2, 1].Value = order.Count();
            ObservableCollection<Order> orders = new ObservableCollection<Order>();

            foreach (var orderWithBarcode in order)
            {
                orders.Add(orderWithBarcode.Order);
            }
            foreach (var o in orders)
            {
                foreach (var opening in o.Openings)
                {
                    foreach (var panels in opening.Panels)
                    {
                        foreach (var sets in panels.Sets)
                        {
                            totallouvers += Convert.ToInt16(sets.LouverCount);
                            foreach (var louver in sets.Louvers)
                            {
                                Devations.Add(louver.AbsDeviation);
                                i++;
                            }
                            sheet.Cells[2, 4].Value = sets.RecordedLouvers.Count();
                            dateTimes.Add(sets.DateSortFinished - sets.DateSortStarted);
                            foreach (var louver in sets.RejectedLouvers)
                            {
                                Devations.Add(louver.AbsDeviation);
                                i++;
                            }
                        }
                    }
                }
            }

            sheet.Cells[2, 2].Value = totallouvers;
            sheet.Cells[2, 3].Value = Enumerable.Average(Devations);
            sheet.Cells[2, 5].Value = AverageTimeSpan(dateTimes);
        }
        public static TimeSpan AverageTimeSpan(List<TimeSpan> timeSpans)
        {
            if (timeSpans == null || timeSpans.Count == 0)
            {
                throw new ArgumentException("The list of TimeSpan objects cannot be null or empty.");
            }

            long totalTicks = timeSpans.Sum(ts => ts.Ticks);
            long averageTicks = totalTicks / timeSpans.Count;
            return new TimeSpan(averageTicks);
        }
        #endregion
    }
}

