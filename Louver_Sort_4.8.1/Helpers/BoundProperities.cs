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
using Newtonsoft.Json.Linq;
using Zebra.Sdk.Printer;
using Louver_Sort_4._8._1.Views.PopUps;
using System.Reflection.Emit;


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

        private int recordingstaken = 0;
        private DataQHelper _DataQ;
        private ZebraPrinterHelper _zebra = new ZebraPrinterHelper();
        private UserControl _SelectedPopUp;
        private UserControl _SelectedView;
        private Visibility _DisconnectedEnabled = Visibility.Visible;
        private Stopwatch stopwatch = new Stopwatch();
        private ChartValues<MeasureModel> _VoltageValues;
        private bool _en_USEnabled;
        private bool _es_ESEnabled;
        private string _Barcode1 = "1018652406000001L1";
        private string _Barcode2 = "PNL1/LXL/L4.5/L30.5188/LT";
        //private string _Barcode1;
        //private string _Barcode2;
        public OrderManager AllOrders = new OrderManager();
        private int _ActiveLouverId;
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Panel _ActivePanel;
        public Louver_Sort_4._8._1.Helpers.LouverStructure.Set _ActiveSet;
        public double _ActiveTopReading;
        public double _ActiveBottomReading;
        private string _CurOrder;
        private string _CurLine;
        private string _CurUnit;
        private string _CurPanelID;
        private string _CurLouverSet;
        private bool _CurXL;
        private string _CurWidth;
        private string _CurLength;
        private string _CurBarcode1;
        private string _CurBarcode2;
        private bool _Acquare1Enabled;
        private bool _Acquare2Enabled;
        private bool _PrintLouverIDLabelsEnabled;
        private bool _SortLouverSetEnabled;
        private bool _ReviewLouverReportEnabled;
        private bool _PrintLouverSortedLabels;
        private bool _NextLouverSetEnabled;
        private string _CurrentReading;
        private ObservableCollection<LouverListView> _ListViewContent = new ObservableCollection<LouverListView>();
        private Visibility _popUpVisible;
        private bool _mainEnabled;
        private int? _txtLouverCount = 5;
        private int _MainContentBlurRadius;
        private ObservableCollection<ReportListView> _ReportContent = new ObservableCollection<ReportListView>();
        private double _ActiveDeviation;
        private bool _focusBarcode1;
        private bool _focusBarcode2;
        private bool _focusLouverCount;
        private ObservableCollection<ReportListView> _ReCutContent;
        private ObservableCollection<LabelID> _LabelIDContent;
        private ReportListView _ReCutSelectedLouver;
        private bool _ApproveSetEnabled = true;
        private string _ReCutOrder;
        private string _ReCutLine;
        private string _ReCutUnit;
        private string _ReCutPanelID;
        private string _ReCutLouverSet;
        private bool _ReCutXL;
        private string _ReCutWidth;
        private string _ReCutLength;
        private string _ReCutBarcode1;
        private string _ReCutBarcode2;
        private ReportListView _ReportSelectedLouver;
        private LouverListView _ListViewSelectedLouver;
        private bool _RejectSelectedLouverEnabled = false;
        private bool _ReworkSetEnabled = false;
        private string _RecutReading1;
        private string _RecutReading2;
        private bool _CheckValueEnabled = false;
        private string _TxtTopAcceptableReplacement;
        private string _TxtBottomAcceptableReplacement;
        private bool _IsReadOnlyBarcode;
        private bool _IsEnabledBarcode = true;
        private Visibility _VisilitySettingsPassword = Visibility.Visible;
        private Visibility _VisibilitySettings = Visibility.Collapsed;
        private Visibility _VisilityReportingPassword = Visibility.Visible;
        private Visibility _VisibilityReporting = Visibility.Collapsed;
        private string _PasswordSettings;
        private string _PasswordReporting;
        private string _SettingsPasswordToolTip = "Enter your password";
        private string _ReportingPasswordToolTip = "Enter your password";
        private bool _IsEnabledExitReport = false;
        private int _ListViewSelectedIndex = 0;

        #endregion

        #region Public Properities

        public int ListViewSelectedIndex
        {
            get => _ListViewSelectedIndex;
            set { SetProperty(ref _ListViewSelectedIndex, value); }
        }

        public bool IsEnabledExitReport
        {
            get => _IsEnabledExitReport;
            set { SetProperty(ref _IsEnabledExitReport, value); }
        }

        public string SettingsPasswordToolTip
        {
            get => _SettingsPasswordToolTip;
            set { SetProperty(ref _SettingsPasswordToolTip, value); }
        }

        public string ReportingPasswordToolTip
        {
            get => _ReportingPasswordToolTip;
            set { SetProperty(ref _ReportingPasswordToolTip, value); }
        }

        public string PasswordSettings
        {
            get => _PasswordSettings;
            set
            {

                    SetProperty(ref _PasswordSettings, value);


            }
        }

        public string PasswordReporting
        {
            get => _PasswordReporting;
            set
            {

                    SetProperty(ref _PasswordReporting, value);

              

            }
        }

        public Visibility VisilitySettingsPassword
        {
            get => _VisilitySettingsPassword;
            set
            {
                SetProperty(ref _VisilitySettingsPassword, value);
            }
        }

        public Visibility VisibilitySettings
        {
            get => _VisibilitySettings;
            set
            {
                SetProperty(ref _VisibilitySettings, value);
            }
        }

        public Visibility VisilityReportingPassword
        {
            get => _VisilityReportingPassword;
            set
            {
                SetProperty(ref _VisilityReportingPassword, value);
            }
        }

        public Visibility VisibilityReporting
        {
            get => _VisibilityReporting;
            set
            {
                SetProperty(ref _VisibilityReporting, value);
            }
        }

        public bool IsReadOnlyBarcode
        {
            get => _IsReadOnlyBarcode;
            set
            {
                SetProperty(ref _IsReadOnlyBarcode, value);
            }

        }

        public bool IsEnabledBarcode
        {
            get => _IsEnabledBarcode;
            set { SetProperty(ref _IsEnabledBarcode, value); }
        }

        public bool ApproveSetEnabled
        {
            get => _ApproveSetEnabled;
            set { SetProperty(ref _ApproveSetEnabled, value); }
        }

        public string TxtTopAcceptableReplacement
        {
            get => _TxtTopAcceptableReplacement;
            set { SetProperty(ref _TxtTopAcceptableReplacement, value); }
        }

        public string TxtBottomAcceptableReplacement
        {
            get => _TxtBottomAcceptableReplacement;
            set { SetProperty(ref _TxtBottomAcceptableReplacement, value); }
        }

        public bool CheckValueEnabled
        {
            get => _CheckValueEnabled;
            set { SetProperty(ref _CheckValueEnabled, value); }
        }

        public string RecutReading1
        {
            get => _RecutReading1;
            set { SetProperty(ref _RecutReading1, value); }
        }

        public string RecutReading2
        {
            get => _RecutReading2;
            set { SetProperty(ref _RecutReading2, value); }
        }

        public ReportListView ReCutSelectedLouver
        {
            get => _ReCutSelectedLouver;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _ReCutSelectedLouver, value);
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

        public ObservableCollection<ReportListView> ReCutContent
        {
            get => _ReCutContent;
            set { SetProperty(ref _ReCutContent, value); }
        }

        public ObservableCollection<LabelID> LabelIDContent
        {
            get => _LabelIDContent;
            set { SetProperty(ref _LabelIDContent, value); }
        }

        public bool ReworkSetEnabled
        {
            get => _ReworkSetEnabled;
            set { SetProperty(ref _ReworkSetEnabled, value); }
        }

        public bool RejectSelectedLouverEnabled
        {
            get => _RejectSelectedLouverEnabled;
            set { SetProperty(ref _RejectSelectedLouverEnabled, value); }
        }

        public ReportListView ReportSelectedLouver
        {
            get => _ReportSelectedLouver;
            set
            {
                SetProperty(ref _ReportSelectedLouver, value);
                if (_ReportSelectedLouver != null)
                {
                    RejectSelectedLouverEnabled = true;
                }
            }
        }

        public LouverListView ListViewSelectedLouver
        {
            get => _ListViewSelectedLouver;
            set
            {
                SetProperty(ref _ListViewSelectedLouver, value);
                if (_ListViewSelectedLouver != null)
                {
                    ActiveLouverID = _ListViewSelectedLouver.LouverID;

                }
            }
        }

        public string ReCutOrder
        {
            get => _ReCutOrder;
            set { SetProperty(ref _ReCutOrder, value); }
        }

        public string ReCutLine
        {
            get => _ReCutLine;
            set { SetProperty(ref _ReCutLine, value); }
        }

        public string ReCutUnit
        {
            get => _ReCutUnit;
            set { SetProperty(ref _ReCutUnit, value); }
        }

        public string ReCutPanelID
        {
            get => _ReCutPanelID;
            set { SetProperty(ref _ReCutPanelID, value); }
        }

        public string ReCutLouverSet
        {
            get => _ReCutLouverSet;
            set { SetProperty(ref _ReCutLouverSet, value); }
        }

        public bool ReCutXL
        {
            get => _ReCutXL;
            set { SetProperty(ref _ReCutXL, value); }
        }

        public string ReCutWidth
        {
            get => _ReCutWidth;
            set { SetProperty(ref _ReCutWidth, value); }
        }

        public string ReCutLength
        {
            get => _ReCutLength;
            set { SetProperty(ref _ReCutLength, value); }
        }

        public string ReCutBarcode1
        {
            get => _ReCutBarcode1;
            set { SetProperty(ref _ReCutBarcode1, value); }
        }

        public string ReCutBarcode2
        {
            get => _ReCutBarcode2;
            set { SetProperty(ref _ReCutBarcode2, value); }
        }

        public bool FocusBarcode1
        {
            get => _focusBarcode1;
            set { SetProperty(ref _focusBarcode1, value); }
        }

        public bool FocusBarcode2
        {
            get => _focusBarcode2;
            set { SetProperty(ref _focusBarcode2, value); }
        }


        public UserControl SelectedPopUp { get => _SelectedPopUp; set => SetProperty(ref _SelectedPopUp, value); }
        public UserControl SelectedView { get => _SelectedView; set => SetProperty(ref _SelectedView, value); }

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
        public bool en_USEnabled
        {
            get => _en_USEnabled;
            set { SetProperty(ref _en_USEnabled, value); }
        }


        public bool es_ESEnabled
        {
            get => _es_ESEnabled;
            set { SetProperty(ref _es_ESEnabled, value); }
        }



        public string Barcode1
        {
            get => _Barcode1;
            set
            {
                if (value == null)
                {
                    SetProperty(ref _Barcode1, "");
                }
                else
                {
                    if (Regex.IsMatch(value, @"^\d{16}L\d$"))
                    {
                        SetProperty(ref _Barcode1, value);
                    }
                    else if (Regex.IsMatch(value, @"^$"))
                    {
                        SetProperty(ref _Barcode1, value);
                    }
                }


            }
        }


        public string Barcode2
        {
            get => _Barcode2;
            set
            {
                if (value == null)
                {
                    SetProperty(ref _Barcode2, "");
                }
                else
                {
                    if (Regex.IsMatch(value, @"^(PNL[1-9])\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$"))
                    {
                        SetProperty(ref _Barcode2, value);
                    }
                    else if (Regex.IsMatch(value, @"^$"))
                    {
                        SetProperty(ref _Barcode2, value);
                    }
                }

            }
        }

        public int ActiveLouverID
        {
            get => _ActiveLouverId;
            set
            {
                SetProperty(ref _ActiveLouverId, value);
            }
        }

        public Louver_Sort_4._8._1.Helpers.LouverStructure.Panel ActivePanel
        {
            get => _ActivePanel;
            set { SetProperty(ref _ActivePanel, value); }
        }

        public Louver_Sort_4._8._1.Helpers.LouverStructure.Set ActiveSet
        {
            get => _ActiveSet;
            set { SetProperty(ref _ActiveSet, value); }
        }

        public double ActiveTopReading
        {
            get => _ActiveTopReading;
            set { SetProperty(ref _ActiveTopReading, value); }
        }

        public double ActiveBottomReading
        {
            get => _ActiveBottomReading;
            set { SetProperty(ref _ActiveBottomReading, value); }
        }

        public string CurOrder
        {
            get => _CurOrder;
            set { SetProperty(ref _CurOrder, value); }
        }

        public string CurLine
        {
            get => _CurLine;
            set { SetProperty(ref _CurLine, value); }
        }

        public string CurUnit
        {
            get => _CurUnit;
            set { SetProperty(ref _CurUnit, value); }
        }

        public string CurPanelID
        {
            get => _CurPanelID;
            set { SetProperty(ref _CurPanelID, value); }
        }

        public string CurLouverSet
        {
            get => _CurLouverSet;
            set { SetProperty(ref _CurLouverSet, value); }
        }

        public bool CurXL
        {
            get => _CurXL;
            set { SetProperty(ref _CurXL, value); }
        }

        public string CurWidth
        {
            get => _CurWidth;
            set { SetProperty(ref _CurWidth, value); }
        }

        public string CurLength
        {
            get => _CurLength;
            set { SetProperty(ref _CurLength, value); }
        }

        public string CurBarcode1
        {
            get => _CurBarcode1;
            set { SetProperty(ref _CurBarcode1, value); }
        }

        public string CurBarcode2
        {
            get => _CurBarcode2;
            set { SetProperty(ref _CurBarcode2, value); }
        }

        public bool Acquare1Enabled
        {
            get => _Acquare1Enabled;
            set { SetProperty(ref _Acquare1Enabled, value); }
        }

        public bool Acquare2Enabled
        {
            get => _Acquare2Enabled;
            set { SetProperty(ref _Acquare2Enabled, value); }
        }

        public ObservableCollection<LouverListView> ListViewContent
        {
            get => _ListViewContent;
            set { SetProperty(ref _ListViewContent, value); }
        }

        public string CurrentReading
        {
            get => _CurrentReading;
            set { SetProperty(ref _CurrentReading, value); }
        }

        public bool PrintLouverIDLabelsEnabled
        {
            get => _PrintLouverIDLabelsEnabled;
            set { SetProperty(ref _PrintLouverIDLabelsEnabled, value); }
        }

        public bool SortLouverSetEnabled
        {
            get => _SortLouverSetEnabled;
            set { SetProperty(ref _SortLouverSetEnabled, value); }
        }

        public bool ReviewLouverReportEnabled
        {
            get => _ReviewLouverReportEnabled;
            set { SetProperty(ref _ReviewLouverReportEnabled, value); }
        }

        public bool PrintLouverSortedLabels
        {
            get => _PrintLouverSortedLabels;
            set { SetProperty(ref _PrintLouverSortedLabels, value); }
        }

        public bool NextLouverSetEnabled
        {
            get => _NextLouverSetEnabled;
            set { SetProperty(ref _NextLouverSetEnabled, value); }
        }

        public Visibility PopUpVisible
        {
            get => _popUpVisible;
            set { SetProperty(ref _popUpVisible, value); }
        }

        public bool MainEnabled
        {
            get => _mainEnabled;
            set { SetProperty(ref _mainEnabled, value); }
        }

        public int? TxtLouverCount
        {
            get => _txtLouverCount;
            set
            {
                SetProperty(ref _txtLouverCount, value);
            }
        }

        public int MainContentBlurRadius
        {
            get => _MainContentBlurRadius;
            set { SetProperty(ref _MainContentBlurRadius, value); }
        }

        public ObservableCollection<ReportListView> ReportContent
        {
            get => _ReportContent;
            set { SetProperty(ref _ReportContent, value); }
        }

        public double ActiveDeviation
        {
            get => _ActiveDeviation;
            set { SetProperty(ref _ActiveDeviation, value); }
        }

        public bool FocusLouverCount
        {
            get => _focusLouverCount;
            set { SetProperty(ref _focusLouverCount, value); }
        }


        #endregion

        #region Commands

        public ICommand UpdateView { get; set; }
        public ICommand ChangeLanguage { get; set; }
        public ICommand PrintStartingLabels { get; set; }
        public ICommand AcqReading1 { get; set; }
        public ICommand AcqReading2 { get; set; }
        public ICommand SortActiveSet { get; set; }
        public ICommand PrintSortedLabels { get; set; }
        public ICommand NextLouverSet { get; set; }
        public ICommand ReconnectToDataQ { get; set; }
        public ICommand ReviewLouverReport { get; set; }
        public ICommand EnterBarcodes { get; set; }
        public ICommand UpdatePopUp { get; set; }
        public ICommand LouverCountOk { get; set; }
        public ICommand ScanLoaded { get; set; }
        public ICommand Barcode1KeyDown { get; set; }
        public ICommand FilterEnter { get; set; }
        public ICommand EnterLouverCount { get; set; }
        public ICommand ReportApproved { get; set; }
        public ICommand ShutDown { get; set; }
        public ICommand SearchOrder { get; set; }
        public ICommand ClosePopUp { get; set; }
        public ICommand RejectSelected { get; set; }
        public ICommand ReworkSet { get; set; }
        public ICommand CheckTop { get; set; }
        public ICommand CheckBottom { get; set; }
        public ICommand PasswordEnter { get; set; }
        public ICommand LostFocusSettings { get; set; }
        public ICommand LostFocusReporting { get; set; }
        public ICommand NOTHING { get; set; }
        public ICommand SortedLabelsComplete { get; set; }
        #endregion




        private ItemsControl _ItemsToShowInCanvas;
        public ItemsControl ItemsToShowInCanvas { get => _ItemsToShowInCanvas; set => SetProperty(ref _ItemsToShowInCanvas, value); }


        public BoundProperities()
        {
            //string json = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\json.txt");

            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            //AllOrders = JsonConvert.DeserializeObject<OrderManager>(json, settings);


            var Mapper = Mappers.Xy<MeasureModel>()
            .X(x => x.ElapsedMilliseconds)
            .Y(x => x.Value);
            LiveCharts.Charting.For<MeasureModel>(Mapper);

            ScanLoaded = new BaseCommand(obj =>
            {
                FocusBarcode1 = true;
            });


            SortedLabelsComplete = new BaseCommand(obj =>
            {
                NextLouverSet.Execute("");
                UpdateView.Execute("Scan");
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

            PasswordEnter = new BaseCommand(obj =>
            {
                switch (obj)
                {
                    case "Settings":
                        if (PasswordSettings == "Admin")
                        {
                            VisilitySettingsPassword = Visibility.Collapsed;
                            VisibilitySettings = Visibility.Visible;
                        }
                        else
                        {
                            SettingsPasswordToolTip = "Incorrect Password";
                        }
                        break;
                    case "Reporting":
                        if (PasswordReporting == "Admin")
                        {
                            VisilityReportingPassword = Visibility.Collapsed;
                            VisibilityReporting = Visibility.Visible;
                        }
                        else
                        {
                            ReportingPasswordToolTip = "Incorrect Password";
                        }
                        break;
                }
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


            ReworkSet = new BaseCommand(obj =>
            {
                ListViewContent = ActiveSet.GenerateRecordedLouvers();
                Acquare1Enabled = true;
                PrintLouverSortedLabels = false;

                UpdateView.Execute("Scan");
                ReworkSetEnabled = false;
                foreach (var louver in ActiveSet.Louvers)
                {
                    if (louver.Reading1 == 0 && louver.Reading2 == 0)
                    {
                        ActiveLouverID = louver.ID;
                        Acquare1Enabled = true;
                        Acquare2Enabled = false;
                        ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);
                        return;
                    }
                }
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

                ReworkSetEnabled = true;
                ApproveSetEnabled = false;
                RejectSelectedLouverEnabled = false;
            });

            Barcode1KeyDown = new BaseCommand(obj =>
            {
                FocusBarcode2 = true;
            });

            ClosePopUp = new BaseCommand(obj =>
            {
                PopUpVisible = Visibility.Hidden;
                MainEnabled = true;
                SelectedPopUp = null;
                MainContentBlurRadius = 0;
            });

            FilterEnter = new BaseCommand(obj =>
            {
                //if (PrintLouverIDLabelsEnabled)
                //{
                //    PrintStartingLabels.Execute("");
                //}
                //else if (Acquare1Enabled)
                //{
                //    AcqReading1.Execute("");
                //}
                //else if (Acquare2Enabled)
                //{
                //    AcqReading2.Execute("");
                //}
                //else if (SortLouverSetEnabled)
                //{
                //    SortActiveSet.Execute("");
                //}
                //else if (ReviewLouverReportEnabled)
                //{
                //    ReviewLouverReport.Execute("");
                //}
                //else if (PrintLouverSortedLabels)
                //{
                //    PrintSortedLabels.Execute("");
                //}
                //else if (NextLouverSetEnabled)
                //{
                //    NextLouverSet.Execute("");
                //}
                //else if (SelectedView is Report)
                //{
                //    ReportApproved.Execute("");
                //}
                //else if (SelectedPopUp is Views.PopUps.LouverCount)
                //{
                //    LouverCountOk.Execute("");
                //}
            });

            UpdateView = new BaseCommand(obj =>
            {
                PopUpVisible = Visibility.Hidden;
                MainEnabled = true;
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
                    default:
                        break;
                }
            });

            UpdatePopUp = new BaseCommand(obj =>
            {
                MainEnabled = false;
                PopUpVisible = Visibility.Visible;
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

            EnterBarcodes = new BaseCommand(obj =>
            {
                if (Barcode1 != "" && Barcode2 != "")
                {
                    var o = AllOrders.CheckIfOrderExists(new BarcodeSet(Barcode1, Barcode2));
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
                var order = AllOrders.CreateOrderAfterScanAndFillAllVariables(new BarcodeSet(Barcode1, Barcode2), Convert.ToInt32(TxtLouverCount));
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

                PrintLouverIDLabelsEnabled = true;

                ListViewContent = ActiveSet.GenerateRecordedLouvers();
                ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);
            });

            PrintStartingLabels = new BaseCommand(obj =>
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


                PrintLouverIDLabelsEnabled = false;
                Acquare1Enabled = true;
                Acquare2Enabled = false;
            });

            AcqReading1 = new BaseCommand(obj =>
            {
                Random random = new Random();

                // Generate a random integer between -1000 and 1000
                int randomInt = random.Next(-1000, 1001);

                // Divide the random integer by 1000 to get increments of 0.001
                //double value = randomInt / 1000.0;
                double value = _DataQ.RecordAndAverageReadings();
                ActiveSet.Louvers[ActiveLouverID].SetReading1(value);
                ActiveTopReading = value;

                ListViewContent = ActiveSet.GenerateRecordedLouvers();
                Acquare1Enabled = false;
                Acquare2Enabled = true;
                ListViewSelectedLouver = ListViewContent[(ListViewContent.IndexOf(ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID))+1)];
            });

            AcqReading2 = new BaseCommand(obj =>
            {
                Random random = new Random();

                // Generate a random integer between -1000 and 1000
                int randomInt = random.Next(-1000, 1001);

                // Divide the random integer by 1000 to get increments of 0.001
                //double value = randomInt / 1000.0;
                double value = _DataQ.RecordAndAverageReadings();
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
                        Acquare1Enabled = true;
                        Acquare2Enabled = false;
                        ListViewSelectedLouver = ListViewContent.FirstOrDefault(x => x.LouverID == ActiveLouverID);
                        return;
                    }
                }
                Acquare1Enabled = false;
                Acquare2Enabled = false;
                ReviewLouverReportEnabled = true;
                ListViewSelectedLouver = null;
            });

            ReviewLouverReport = new BaseCommand(obj =>
            {
                ActiveSet.Sort();
                ListViewContent = null;
                ListViewContent = ActiveSet.GenerateRecordedLouvers();
                UpdateView.Execute("Report");
                ReviewLouverReportEnabled = false;
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

                PrintLouverSortedLabels = false;
                NextLouverSetEnabled = true;
                IsEnabledExitReport = true;
                ApproveSetEnabled = false;


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

                PrintLouverIDLabelsEnabled = false;
                Acquare1Enabled = false;
                Acquare2Enabled = false;
                SortLouverSetEnabled = false;
                ReviewLouverReportEnabled = false;
                PrintLouverIDLabelsEnabled = false;
                NextLouverSetEnabled = false;
                ActiveDeviation = 0;


                FocusBarcode1 = true;
            });

            ReconnectToDataQ = new BaseCommand(obj =>
            {
                ConnectToDataQ();
            });

            ReportApproved = new BaseCommand(obj =>
            {
                //UpdateView.Execute("Scan");
                //PrintLouverSortedLabels = true;
                //NextLouverSet.Execute("");
                SortedLabelsPopUpInitialize();
            });

            ShutDown = new BaseCommand(obj =>
            {


                //// Serialize to JSON
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\json.txt"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, AllOrders);
                }










                //// Serialize the object to JSON
                //string j = JsonConvert.SerializeObject(AllOrders, Formatting.Indented);

                //// Specify the file path where you want to write the JSON
                //string filePath = System.IO.Directory.GetCurrentDirectory() + @"\json.txt";

                //// Write the JSON string to the file
                //File.WriteAllText(filePath, j);


                Console.WriteLine("JSON exported and saved to file.");

                Application.Current.Shutdown();


            });

            SearchOrder = new BaseCommand(obj =>
            {
                var order = AllOrders.GetOrder(new BarcodeSet(Barcode1, Barcode2));
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

        }

        #region Code Behind



        public void SortedLabelsPopUpInitialize()
        {
            ObservableCollection<LabelID> lables = new ObservableCollection<LabelID>();
            foreach (var louver in ReportContent)
            {
                lables.Add( new LabelID (louver.LouverID, louver.LouverOrder, louver.Orientation));
            }
            LabelIDContent = lables;
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
                readings.Add(_DataQ.GetDistance());
                readings.Add(_DataQ.GetDistance());


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

        public void Closing()
        {
            try
            {
                _DataQ.Stop();
                _DataQ.Disconnect();
                stopwatch.Stop();
                stopwatch.Reset();
                DisconnectedEnabled = Visibility.Visible;
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool runonce = false;

        public void ScanInitialize()
        {
            if (!runonce)
            {
                ConnectToDataQ();
                PrintLouverIDLabelsEnabled = false;
                Acquare1Enabled = false;
                Acquare2Enabled = false;
                SortLouverSetEnabled = false;
                ReviewLouverReportEnabled = false;
                PrintLouverIDLabelsEnabled = false;
                NextLouverSetEnabled = false;
                runonce = true;
            }
        }

        public void ConnectToDataQ()
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

        public void MenuInitialize()
        {

        }

        public void ReportInitialize()
        {
            var report = ActiveSet.GenerateReport();
            ReportContent = new ObservableCollection<ReportListView>(report.OrderBy(r => r.LouverOrder));
            ApproveSetEnabled = true;
        }

        public void ReCutInitialize()
        {

        }

        public void DataQNewData(object sender, EventArgs e)
        {
            VoltageValues.Add(new MeasureModel
            {
                ElapsedMilliseconds = stopwatch.Elapsed.TotalSeconds,
                Value = _DataQ.GetDistance()
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

        public void CreateOrderFromBarCodeSet()
        {

        }

        public void LouverCountPopUpLoaded()
        {
            FocusLouverCount = true;
        }

        #endregion


    }
}













//PRINT ALL IN ORDERMANAGER TO DEBUG
//foreach (var order in AllOrders.GetAllOrders())
//{
//    Debug.WriteLine($"Order Barcode1: {order.BarcodeHelper.Barcode.Barcode1}, Barcode2: {order.BarcodeHelper.Barcode.Barcode2}");

//    foreach (var opening in order.Openings)
//    {
//        Debug.WriteLine($"\tOpening Line: {opening.Line}, ModelNum: {opening.ModelNum}, Style: {opening.Style}, Width: {opening.Width}, Length: {opening.Length}");

//        foreach (var panel in opening.Panels)
//        {
//            Debug.WriteLine($"\t\tPanel ID: {panel.ID}");

//            foreach (var set in panel.Sets)
//            {
//                Debug.WriteLine($"\t\t\tSet ID: {set.ID}, Date Sort Started: {set.DateSortStarted}, Date Sort Finished: {set.DateSortFinished}, Louver Count: {set.LouverCount}");

//                foreach (var louver in set.Louvers)
//                {
//                    Debug.WriteLine($"\t\t\t\tLouver ID: {louver.ID}, Processed: {louver.Processed}, Warp: {louver.Warp}, Rejected: {louver.Rejected}, Cause of Rejection: {louver.CauseOfRejection}");
//                }
//            }
//        }
//    }
//}
//Debug.WriteLine("\n\n\n\n\n\n\n\n\n\n\n");