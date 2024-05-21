﻿using System;
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
using Zebra.Sdk.Printer;
using OfficeOpenXml;
using System.Windows.Media;
using Louver_Sort_4._8._1.Views.PopUps;




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
        //Class Instances
        private DataQHelper _dataQ;
        private ZebraPrinterHelper _zebra = new ZebraPrinterHelper();
        public OrderManager _allOrders = new OrderManager();
        public Globals _globals = new Globals();

        //Generic
        string Barcode1Regex = @"^\d{16}P\d$";
        string Barcode2Regex = @"^(PNL[1-9])\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$";
        string EmptyRegex = @"^$";
        private string _jSONSaveLocation = AppDomain.CurrentDomain.BaseDirectory;

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

        public double GapSpec
        {
            get => _globals.GapSpec;
            set => SetProperty(ref _globals.GapSpec, value);
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

        // User Control Views
        private UserControl _selectedPopUp;
        public UserControl SelectedPopUp
        {
            get => _selectedPopUp;
            set => SetProperty(ref _selectedPopUp, value);
        }

        // IsEnabled
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

        private bool _isEnabledMain;
        public bool IsEnabledMain
        {
            get => _isEnabledMain;
            set => SetProperty(ref _isEnabledMain, value);
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

        // IsReadOnly
        private bool _isReadOnlyBarcode;
        public bool IsReadOnlyBarcode
        {
            get => _isReadOnlyBarcode;
            set => SetProperty(ref _isReadOnlyBarcode, value);
        }

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
            get => _activeBottomReading;
            set => SetProperty(ref _activeBottomReading, value);
        }

        private double _activeDeviation;
        public double ActiveDeviation
        {
            get => _activeDeviation;
            set => SetProperty(ref _activeDeviation, value);
        }

        private double _activeTopReading;
        public double ActiveTopReading
        {
            get => _activeTopReading;
            set => SetProperty(ref _activeTopReading, value);
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
                    SetProperty(ref _txtLouverCount, value);
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

                    //ZebraPrinter _Printer = _zebra.Connect();
                    List<Louver> toPrint = new List<Louver> { ActiveSet.Louvers[index] };
                    _zebra.PrintSortedLouverIDs(toPrint);
                    //_zebra.Disconnect(_Printer);

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


        //UserMessagePopUp
        private string _txtUserMessage;
        public string TxtUserMessage
        {
            get => _txtUserMessage;
            set { SetProperty(ref _txtUserMessage, value); }
        }

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

        #region Commands
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
        public ICommand BrowseForJSONSaveLocation { get; set; }
        public ICommand ExportExcel { get; set; }
        public ICommand ShutDown { get; set; }

        #endregion

        #region CommandImplementation
        public BoundProperities()
        {
            ConnectToDataQ();

            //CHANGE - check each file path in the function individually
            //Add messages  if any of the files didn't load in
            if (CheckFile(_jSONSaveLocation + "\\LouverSortData.ini") && CheckFile(_jSONSaveLocation + "\\Globals.ini") && CheckFile(_jSONSaveLocation + "\\DataQ.ini"))
            {
                LoadFromJson();
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

            var Mapper = Mappers.Xy<MeasureModel>()
            .X(x => x.ElapsedMilliseconds)
            .Y(x => x.Value);
            LiveCharts.Charting.For<MeasureModel>(Mapper);


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
                string cultureCode = obj.ToString();
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
                switch (cultureCode)
                {
                    case "en-US":
                        EnUSEnabled = false;
                        EsESEnabled = true;
                        break;
                    case "es-ES":
                        EnUSEnabled = true;
                        EsESEnabled = false;
                        break;
                    default:
                        break;
                }
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
                        CalibTxt = "Place calibration plate on top of rail";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\CalibTop.jpg";
                        CalibTxtBoxHint = "";
                        VisibilityCalibRecord = Visibility.Collapsed;
                        _calibStep += 1;
                        break;

                    case 2:
                        // Step 2: Start thread to record data and update UI for bottom plate
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndSetCalibrationFlat();
                            UpdatePopupForBottomPlate();
                            _calibStep += 1;
                        });
                        break;

                    case 3:
                        // Step 3: Start thread to record data and update UI for the highest step
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndSetCalibrationStep();
                            UpdatePopupForHighestStep();
                            _calibStep += 1;
                        });
                        break;

                    case 4:
                        // Step 4: Start thread to check calibration flat and update UI for the lowest step
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndCheckCalFlat();
                            UpdatePopupForLowestStep();
                            _calibStep += 1;
                        });
                        break;

                    case 5:
                        // Step 5: Start thread to check calibration step and handle the result
                        StartCalibrationThread(() =>
                        {
                            UpdatePopUpAndAwait();
                            ConnectAndCheckCalStep();
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
                        CalibTxt = "Place laser centering plate on slide and adjust sensor until red dot is in the cross hair";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\1.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 2:
                        // Step 2: Show instructions to turn laser to teach mode
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = "Turn laser to teach mode";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\2.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 3:
                        // Step 3: Show instructions to set calibration plate on top of slide
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = "Set calibration plate on top of slide";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\3.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 4:
                        // Step 4: Show instructions to press plus on the laser
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = "Press plus on the laser";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\2.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 5:
                        // Step 5: Show instructions to set calibration plate on bottom of slide
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = "Set calibration plate on bottom of slide";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\4.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 6:
                        // Step 6: Show instructions to press minus on the laser
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = "Press minus on laser";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\2.png";
                        VisibilityCalibImage = Visibility.Visible;
                        _calibStep += 1;
                        break;

                    case 7:
                        // Step 7: Show instructions to turn the dial on laser back to run
                        UpdatePopUp.Execute("CalibrateLaser");
                        CalibTxt = "Turn dial on laser back to run";
                        CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\5.png";
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
                    MessageUser("Incorrect Barcode");
                    FocusBarcode1 = true;
                }
                else
                {
                    // Check if the order exists in _allOrders
                    var order = _allOrders.CheckIfOrderExists(new BarcodeSet(Barcode1, Barcode2));
                    if (order != null)
                    {
                        // If the order already exists, show a message and clear the barcodes
                        MessageUser("Order Already Sorted");
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
                        MessageUser("Ensure you record the correct louver side");
                        return;
                    }

                    // Check if the reading difference is within acceptable range
                    if (Math.Abs(Convert.ToDouble(ActiveSet.Louvers[ActiveLouverID - 1].Readings.Reading1 - value)) < 0.007)
                    {
                        // Show message if readings are too close together
                        MessageUser("Readings are too close together");
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
                            var report = ActiveSet.GenerateReport(GapSpec);
                            ReCutContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));

                            // Enable ReCut cancel button
                            IsEnabledReCutCancel = true;
                        }
                        else
                        {
                            // Show message if order is not found
                            MessageUser("Order not found");
                            ReCutBarcode1 = "";
                            ReCutBarcode2 = "";
                        }
                    }
                    else
                    {
                        // Show message if either barcode is empty
                        MessageUser("Incorrect Barcode");
                        FocusBarcode1 = true;
                    }
                }
                else
                {
                    // Show message if either barcode is null
                    MessageUser("Incorrect Barcode");
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

                    // Update the UI with the collected data
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TxtTopAcceptableReplacement = RecutReading1.ToString();

                        // Check if the recorded value is within acceptable range
                        double recutReadingValue = Convert.ToDouble(RecutReading1);
                        if (Math.Abs(recutReadingValue) <= Convert.ToDouble(TopMaximumValue) && Math.Abs(recutReadingValue) >= Convert.ToDouble(TopMinimumValue))
                        {
                            TopColor = Brushes.Green;
                        }
                        else
                        {
                            TopColor = Brushes.Red;
                        }

                        // Update UI states
                        IsEnabledCheckTop = false;
                        IsEnabledCheckBottom = true;

                    });
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

                    // Check if the recorded value is within acceptable range
                    double recutReadingValue = Convert.ToDouble(RecutReading2);
                    if (Math.Abs(recutReadingValue) <= Convert.ToDouble(BottomMaximumValue) && Math.Abs(recutReadingValue) >= Convert.ToDouble(BottomMinimumValue))
                    {
                        VisibilityReCutData = Visibility.Visible;
                        BottomColor = Brushes.Green;
                        TxtUserMessage = "Louver is a good replacement";
                        UpdatePopUp.Execute("ReCut");

                        // Determine orientation message
                        if (ActiveSet.Louvers.FirstOrDefault(l => l.ID == ReCutSelectedLouver.LouverOrder).Orientation == true)
                        {
                            ReCutOrientation = "Flip";
                        }
                        else
                        {
                            ReCutOrientation = "Don't Flip";
                        }
                    }
                    else
                    {
                        BottomColor = Brushes.Red;
                        TxtUserMessage = "Louver is NOT a replacement";
                        VisibilityReCutData = Visibility.Visible;
                        UpdatePopUp.Execute("ReCut");
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
                //ZebraPrinter _Printer = _zebra.Connect();
                List<Louver> toPrint = new List<Louver>();

                // Find and add the louver to be printed based on its ID
                toPrint.Add(ActiveSet.Louvers[ActiveSet.Louvers.FindIndex(louver => louver.ID == ReCutSelectedLouver.LouverID)]);

                // Print the sorted louver IDs
                _zebra.PrintSortedLouverIDs(toPrint);

                // Disconnect from the Zebra printer
                //_zebra.Disconnect(_Printer);

                // Close the popup
                UpdatePopUp.Execute("Close");
            });


            CloseReCutPopUp = new BaseCommand(obj =>
            {
                // Close the popup
                UpdatePopUp.Execute("Close");

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



            ExportExcel = new BaseCommand(obj =>
            {
                // Export data to Excel
                ExportToExcel(Path.Combine(ExcelExportLocation, $"LouverSortExport_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.xlsx"), true);


                //CHANGE - check if date range is entered or sales number and export using that

                //CHANGE - add error if report fails
                // Update user message and show the popup
                MessageUser("Report Generated");

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

        #region Code Behind














































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

            // Collect all louver sets from the active panel
            foreach (var set in ActivePanel.GetAllSets())
            {
                toPrint.AddRange(set.GetLouverSet());
            }

            // Print the sorted louver IDs
            _zebra.PrintSortedLouverIDs(toPrint);

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
            //ZebraPrinter _Printer = _zebra.Connect();
            List<Louver> toPrint = new List<Louver>();

            // Collect all louver sets from the active panel
            foreach (var set in ActivePanel.GetAllSets())
            {
                toPrint.AddRange(set.GetLouverSet());
            }

            // Print the louver IDs
            _zebra.PrintLouverIDs(toPrint);
            // Disconnect from the Zebra printer
            //_zebra.Disconnect(_Printer);


            // Update the state of various UI elements
            IsEnabledPrintUnsortedLabels = false;
            IsEnabledAcquareTop = true;
            IsEnabledAcquireBottom = false;

            // Show a message to place unsorted labels on louvers
            MessageUser("Place Unsorted Labels on Louvers");
        }

        public void MessageUser(string message)
        {

            TxtUserMessage = message;
            UpdatePopUp.Execute("Message");


        }

        private void StartCalibrationThread(ThreadStart action)
        {
            Thread recordThread = new Thread(action);
            recordThread.Start();
        }

        private void UpdatePopUpAndAwait()
        {
            UpdatePopUp.Execute("Close");
            UpdatePopUp.Execute("Await");

        }

        private void ConnectAndSetCalibrationFlat()
        {
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            _dataQ.SetCalibrationFlat();

            UpdatePopUp.Execute("Close");

        }

        private void UpdatePopupForBottomPlate()
        {
            CalibTxt = "Place calibration plate on bottom of slide";
            CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\CalibBottom.jpg";
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");
        }

        private void ConnectAndSetCalibrationStep()
        {
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            _dataQ.SetCalibrationStep();
        }

        private void UpdatePopupForHighestStep()
        {

            CalibTxt = "Place highest step of Louver Sag Guage on top of rail";
            CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\CalibTop.jpg";
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");

        }
        private void ConnectAndCheckCalFlat()
        {
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            _dataQ.CheckCalFlat();

            UpdatePopUp.Execute("Close");

        }

        private void UpdatePopupForLowestStep()
        {

            CalibTxt = "Place lowest step of Louver Sag Guage on top of rail";
            CalibImage = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\CalibTop.jpg";
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");

        }

        private void ConnectAndCheckCalStep()
        {
            if (_dataQ == null)
            {
                ConnectToDataQ();
            }
            _dataQ.CheckCalStep(CalibrationRejectionSpec);
        }

        private void HandleCalibrationResult()
        {

            if (_dataQ._cal.Successful)
            {
                CalibTxt = "Calibration Passed";
            }
            else
            {
                CalibTxt = "Calibration Failed. Repeat Calibration";
            }
            VisibilityCalibImage = Visibility.Collapsed;
            CalibTxtBoxHint = "";
            VisibilityCalibRecord = Visibility.Collapsed;
            UpdatePopUp.Execute("Calibrate");

        }

        private void HandleUnsuccessfulCalibration()
        {
            UpdatePopUp.Execute("Close");
            VisibilitySortSet = Visibility.Collapsed;
            IsEnabledReCut = Visibility.Collapsed;
            VisibilityAdjustCalib = Visibility.Collapsed;
            SelectedTabIndex = 0;
        }

        private void HandleSuccessfulCalibration()
        {
            VisibilitySortSet = Visibility.Visible;
            IsEnabledReCut = Visibility.Visible;
            VisibilityAdjustCalib = Visibility.Collapsed;
            SelectedTabIndex = 1;
        }

        public void UpdateValue()
        {
            var focusedElement = Keyboard.FocusedElement as FrameworkElement;

            // If the focused element is a TextBox, update its binding source
            if (focusedElement is TextBox)
            {
                BindingExpression bindingExpression = focusedElement.GetBindingExpression(TextBox.TextProperty);
                bindingExpression.UpdateSource();
            }
        }

        //View Intialize
        public void SortedLabelsPopUpInitialize()
        {
            List<LabelID> lables = new List<LabelID>();
            ObservableCollection<LabelID> labels = new ObservableCollection<LabelID>();
            foreach (var louver in ReportContent)
            {
                lables.Add(new LabelID(louver.LouverID, louver.LouverOrder, louver.Orientation));
            }
            lables = lables.OrderBy(louver => louver.UnsortedID).ToList();
            labels.Clear();
            foreach (var item in lables)
            {
                labels.Add(item);
            }
            LabelIDContent = labels;
        }

        public void ReportInitialize()
        {
            var report = ActiveSet.GenerateReport(GapSpec);
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

        //DataQ
        public void ConnectToDataQ()
        {
            try
            {
                Thread test = new Thread(async () =>
                {
                    if (_dataQ == null)
                    {
                        try
                        {
                            _dataQ = new DataQHelper();
                            await _dataQ.Connect();
                            _dataQ.Start();

                            VisibilityDisconnected = Visibility.Collapsed;
                            _stopwatch.Start();

                            //_dataQ.AnalogUpdated += new EventHandler(DataQNewData);
                            _dataQ.LostConnection += new EventHandler(DataQLostConnection);

                            _dataQ.StartActiveMonitoring();
                        }
                        catch (DataQException ex)
                        {

                            MessageUser("Disconnect and Reconnect DataQ, restart application");
                            throw ex;
                        }

                    }
                    else
                    {
                        MessageUser("Disconnect and Reconnect DataQ, restart application");
                        return;
                    }





                });
                test.Start();
            }
            catch (Exception)
            {

                MessageUser("Disconnect and Reconnect DataQ, restart application");


                throw;
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
            string json = File.ReadAllText(_jSONSaveLocation + "LouverSortData.ini");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            _allOrders = JsonConvert.DeserializeObject<OrderManager>(json, settings);

            json = File.ReadAllText(_jSONSaveLocation + "Globals.ini");

            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            _globals = JsonConvert.DeserializeObject<Globals>(json, settings);

            json = File.ReadAllText(_jSONSaveLocation + "DataQ.ini");

            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            //UNCOMMENT
            if (json != "null")
            {
                if (_dataQ != null)
                {
                    _dataQ._cal = JsonConvert.DeserializeObject<Calibration>(json, settings);
                }
            }
            else
            {
                _dataQ._cal = new Calibration();
            }

        }

        public void SaveToJson()
        {
            DeleteDataOlderThan90Days();
            //// Serialize to JSONF
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(_jSONSaveLocation + "LouverSortData.ini"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                if (_allOrders != null)
                {
                    serializer.Serialize(writer, _allOrders);
                }
            }
            using (StreamWriter sw = new StreamWriter(_jSONSaveLocation + "Globals.ini"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                if (_globals != null)
                {

                    serializer.Serialize(writer, _globals);
                }
            }
            using (StreamWriter sw = new StreamWriter(_jSONSaveLocation + "DataQ.ini"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                if (_dataQ != null)
                {
                    serializer.Serialize(writer, _dataQ._cal);
                }
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

        public void ExportToExcel(string filename, bool ExportType)
        {
            List<OrderWithBarcode> OrdersToExport = new List<OrderWithBarcode>();
            //Exporting with DateRange
            if (ExportType)
            {
                foreach (var order in _allOrders.OrdersWithBarcodes)
                {
                    if (IsInSelectedRange(order.Order))
                    {
                        OrdersToExport.Add(order);
                    }
                }
            }
            //Exporting with Sales Number
            else
            {
                foreach (var order in _allOrders.OrdersWithBarcodes)
                {
                    if (order.Order.BarcodeHelper.Barcode1.Contains(Salesnumber))
                    {
                        OrdersToExport.Add(order);
                    }
                }
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var l = 1;
                foreach (var order in OrdersToExport)
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
                                    sheet.Cells[8, i].Value = louver.Readings.Reading1.ToString();
                                    sheet.Cells[9, i].Value = louver.Readings.Reading2.ToString();
                                    sheet.Cells[10, i].Value = louver.Deviation.ToString();
                                    sheet.Cells[11, i].Value = louver.AbsDeviation.ToString();

                                    i++;
                                }
                                foreach (var louver in sets.RejectedLouvers)
                                {
                                    sheet.Cells[3, i].Value = "Rejected Louver";
                                    sheet.Cells[4, i].Value = louver.ID.ToString();
                                    sheet.Cells[5, i].Value = louver.SortedID.ToString();
                                    sheet.Cells[6, i].Value = louver.Orientation.ToString();
                                    sheet.Cells[7, i].Value = louver.Processed.ToString();
                                    sheet.Cells[8, i].Value = louver.Readings.Reading1.ToString();
                                    sheet.Cells[9, i].Value = louver.Readings.Reading2.ToString();
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
                if (package.Workbook.Worksheets.Count > 0)
                {
                    package.SaveAs(new FileInfo(filename));
                }
            }

        }

        public void Closing()
        {
            try
            {
                _dataQ.Stop();
                _dataQ.Disconnect();
                _stopwatch.Stop();
                _stopwatch.Reset();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Barcode1Correct(string barcode)
        {
            if (barcode != null)
            {
                if (barcode != "")
                {
                    if (Regex.IsMatch(barcode, Barcode1Regex))
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

        public bool Barcode2Correct(string barcode)
        {
            if (barcode != null)
            {
                if (barcode != "")
                {
                    if (Regex.IsMatch(barcode, Barcode2Regex))
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

        public double CalculateRejection(double Length)
        {
            return RejectionSpec * (Convert.ToDouble(Length) / 12);
        }

        public ObservableCollection<T> ConvertListToObservableCollection<T>(List<T> list)
        {
            return new ObservableCollection<T>(list);
        }
        #endregion
    }
}
#endregion