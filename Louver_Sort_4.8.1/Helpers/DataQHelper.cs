using Dataq.Simple;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using System.Diagnostics;
using System.IO.Ports;
using Dataq.Devices;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace Louver_Sort_4._8._1.Helpers
{
    /// <summary>
    /// Manages Dataq Device operations including connection, configuration, data acquisition, and disconnection.
    /// </summary>
    public class DataQHelper
    {
        private string _outputString; // Stores the latest data received from the device
        public Calibration _cal = new Calibration(); // Calibration object for handling calibration data
        private double _latestReading; // Stores the latest reading received from the device
        public event EventHandler LostConnection;
        public event EventHandler LatestReadingChanged;
        List<double> validReadings = new List<double>();
        public DI155 DI155 = new DI155();
        public DI1100 DI1100 = new DI1100();
        public string DataQModel;
        private CancellationTokenSource cancelRead;
        private bool _dataReceived = false;
        public bool _KeepMonitoring = true;

        public DataQHelper()
        {
            string[] portNames = SerialPort.GetPortNames();

            if (portNames.Length == 0)
            {
                Console.WriteLine("No COM ports found.");
            }
            else
            {
                Console.WriteLine("Available COM ports:");
                foreach (string port in portNames)
                {
                    if (port.Contains("1100"))
                    {
                        DataQModel = "1000";
                    }
                    else if (port.Contains("145") || port.Contains("155"))
                    {
                        DataQModel = "100";
                    }
                }
            }
        }

        #region Switch Between DataQs

        public async Task StartConnectionAsync()
        {
            switch (DataQModel)
            {
                case "100":
                    await DI155.Connect();
                    await DI155.Start();
                    break;

                case "1000":
                    await DI1100.Connect();
                    await DI1100.Start();
                    break;
            }
        }

        public async Task StopConnection()
        {
            switch (DataQModel)
            {
                case "100":
                    await DI155.Stop();
                    await DI155.Disconnect();
                    break;

                case "1000":
                    await DI1100.Stop();
                    await DI1100.Disconnect();
                    break;
            }
        }
        #endregion

        #region Events
        public double LatestReading
        {
            get { return _latestReading; }
            set
            {
                _latestReading = value;
                OnLatestReadingChanged();
            }
        }

        protected virtual void OnLatestReadingChanged()
        {
            LatestReadingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnLostConnection()
        {
            try
            {
                LostConnection?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                HandleDataQException("An error occurred while raising the LostConnection event.", ex);
            }
        }

        private void HandleConnectionError(Exception ex)
        {
            OnLostConnection();
            throw new DataQException("Error connecting to Dataq DI-155 device.", ex);
        }

        private void HandleDataQException(string message, Exception ex)
        {
            throw new DataQException(message, ex);
        }

        #endregion

        #region Calibration

        public async Task SetCalibrationFlatAsync()
        {
            double averageReading = await CollectCalibrationDataAsync(false);
            _cal.FlatReading = averageReading;
        }

        public async Task SetCalibrationStepAsync()
        {
            double averageReading = await CollectCalibrationDataAsync(false);
            _cal.StepReading = averageReading;
            _cal.CalculateLineEquation(Tuple.Create(_cal.FlatReading, 0.0), Tuple.Create(_cal.StepReading, _cal.StepValue));
        }

        public async Task CheckCalFlatAsync()
        {
            double averageReading = await CollectCalibrationDataAsync(true);
            _cal.CheckFlat = averageReading;
        }

        public async Task CheckCalStepAsync(double range)
        {
            double averageReading = await CollectCalibrationDataAsync(true);
            _cal.CheckStep = averageReading;
            _cal.CheckCalibration(range);
        }

        public double GetSlope()
        {
            return _cal?.Slope ?? 0.0;
        }

        private async Task<double> CollectCalibrationDataAsync(bool useCalibration)
        {
            var calAverages = new List<double>
            {
                await WaitForDataCollection(useCalibration)
            };
            return calAverages.Average();
        }

        //public void SetCalibrationFlat()
        //{
        //    List<double> CalAverages = new List<double>();
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    CalAverages.Add(WaitForDataCollection(false).Result);
        //    //}

        //    _cal.FlatReading = CalAverages.Average();
        //}

        //public void SetCalibrationStep()
        //{
        //    List<double> CalAverages = new List<double>();
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    CalAverages.Add(WaitForDataCollection(false).Result);
        //    //}

        //    _cal.StepReading = CalAverages.Average();



        //    _cal.CalculateLineEquation(Tuple.Create(_cal.FlatReading, 0.0), Tuple.Create(_cal.StepReading, _cal.StepValue));
        //}

        //public void CheckCalFlat()
        //{
        //    List<double> CalAverages = new List<double>();
        //    CalAverages.Add(WaitForDataCollection(true).Result);

        //    _cal.CheckFlat = CalAverages.Average();
        //}

        //public void CheckCalStep(double range)
        //{
        //    List<double> CalAverages = new List<double>();
        //    CalAverages.Add(WaitForDataCollection(true).Result);

        //    _cal.CheckStep = CalAverages.Average();

        //    _cal.CheckCalibration(range);
        //}

        //public double GetSlope()
        //{
        //    if (_cal != null)
        //    {
        //        return _cal.Slope;
        //    }
        //    return 0.0;

        //}
        #endregion

        #region Monitoring Functions

        public async void StartActiveMonitoring()
        {
            switch (DataQModel)
            {
                case "100":
                    await StartMonitoringDI155();
                    break;

                case "1000":
                    await StartMonitoringDI1100();
                    break;
            }
        }

        private async Task StartMonitoringDI155()
        {
            DI155.TargetDevice.NewData += PassToMain;

            await Task.Run(async () =>
            {
                while (_KeepMonitoring)
                {
                    await Task.Delay(100);
                }
            });

            DI155.TargetDevice.NewData -= PassToMain;
        }

        private async Task StartMonitoringDI1100()
        {
            cancelRead = new CancellationTokenSource();
            await DI1100.TargetDevice.AcquisitionStartAsync();

            Task taskRead = new Task(async () => await ReadDataDI1100(), cancelRead.Token);
            taskRead.Start();
        }

        private async Task ReadDataDI1100()
        {
            Dataq.Devices.IChannelIn masterChannel = DI1100.TargetDevice.Channels
                .OfType<Dataq.Devices.IChannelIn>()
                .FirstOrDefault();

            while (DI1100.TargetDevice.IsAcquiring)
            {
                try
                {
                    await DI1100.TargetDevice.ReadDataAsync(cancelRead.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (masterChannel?.DataIn.Count == 0) continue;

                string dataString = ExtractDataAsString(DI1100.TargetDevice.Channels);
                LatestReading = Convert.ToDouble(dataString);
                validReadings.Add(LatestReading);

                ClearChannelData(DI1100.TargetDevice.Channels);
            }
            Debug.WriteLine("Stopped" + Environment.NewLine);
        }

        private string ExtractDataAsString(IEnumerable<Dataq.Devices.IChannel> channels)
        {
            return string.Join("", channels
                .OfType<Dataq.Devices.IChannelIn>()
                .Select(ch => ch.DataIn.FirstOrDefault().ToString().Substring(0, 7)));
        }

        private void ClearChannelData(IEnumerable<Dataq.Devices.IChannel> channels)
        {
            foreach (var ch in channels.OfType<Dataq.Devices.IChannelIn>())
            {
                ch.DataIn.Clear();
            }
        }

        private void PassToMain(object sender, EventArgs e)
        {
            if (_cal != null)
            {
                LatestReading = _cal.ConvertToInches(ReadNewData());
            }
        }

        public double ReadNewData()
        {
            switch (DataQModel)
            {
                case "100":
                    return ReadDataFromDI155();

                case "1000":
                    return ReadDataFromDI1100();

                default:
                    throw new NotSupportedException($"Unsupported DataQModel: {DataQModel}");
            }
        }

        private double ReadDataFromDI155()
        {
            int scans = DI155.TargetDevice.NumberOfScansAvailable;
            short channels = (short)DI155.TargetDevice.NumberOfChannelsEnabled;
            var data = new double[scans * channels];
            DI155.TargetDevice.GetInterleavedScaledData(data, 0, scans);

            return Convert.ToDouble(string.Join("", data
                .Select((value, index) => index % channels == 0 ? value.ToString("F4") : "")
                .Where(value => !string.IsNullOrEmpty(value))));
        }

        private double ReadDataFromDI1100()
        {
            return validReadings.LastOrDefault();
        }

        public async Task<double> WaitForDataCollection(bool useCalibration = false)
        {
            _dataReceived = false;
            validReadings.Clear();

            switch (DataQModel)
            {
                case "100":
                    await WaitForDataCollectionDI155();
                    break;

                case "1000":
                    await WaitForDataCollectionDI1100();
                    break;

                default:
                    throw new NotSupportedException($"Unsupported DataQModel: {DataQModel}");
            }

            return useCalibration ? _cal.ConvertToInches(validReadings.Average()) : validReadings.Average();
        }

        private async Task WaitForDataCollectionDI155()
        {
            DI155.TargetDevice.NewData += GetDataHandler;

            int timeoutMilliseconds = 700000000;
            int intervalMilliseconds = 100;
            int elapsedMilliseconds = 0;

            await Task.Run(async () =>
            {
                while (!_dataReceived && elapsedMilliseconds < timeoutMilliseconds)
                {
                    await Task.Delay(intervalMilliseconds);
                    elapsedMilliseconds += intervalMilliseconds;
                }
            });

            DI155.TargetDevice.NewData -= GetDataHandler;
        }

        private async Task WaitForDataCollectionDI1100()
        {
            validReadings.Clear();

            while (validReadings.Count < 3)
            {
                await Task.Delay(100);
            }
        }

        private void GetDataHandler(object sender, EventArgs e)
        {
            double reading = ReadNewData();
            validReadings.Add(reading);

            if (validReadings.Count >= 2)
            {
                _dataReceived = true;
            }
        }




        //public async void StartActiveMonitoring()
        //{
        //    switch (DataQModel)
        //    {
        //        case "100":
        //            DI155.TargetDevice.NewData += PassToMain;

        //            await Task.Run(async () =>
        //            {
        //                while (_KeepMonitoring)
        //                {
        //                    await Task.Delay(100);
        //                }
        //            });

        //            DI155.TargetDevice.NewData -= PassToMain;
        //            break;

        //        case "1000":

        //            // Everything is good, so...
        //            cancelRead = new CancellationTokenSource(); // Create the cancellation token
        //            await DI1100.TargetDevice.AcquisitionStartAsync(); // start acquiring

        //            Task taskRead = new Task(async() =>
        //            {
        //                // Capture the first channel programmed as an input (MasterChannel)
        //                // and use it to track data availability for all input channels
        //                Dataq.Devices.IChannelIn MasterChannel = null;
        //                for (int index = 0; index < DI1100.TargetDevice.Channels.Count; index++)
        //                {
        //                    if (DI1100.TargetDevice.Channels[index] is Dataq.Devices.IChannelIn)
        //                    {
        //                        MasterChannel = (Dataq.Devices.IChannelIn)DI1100.TargetDevice.Channels[index]; // We have our channel
        //                        break;
        //                    }
        //                }

        //                // Keep reading while acquiring data
        //                while (DI1100.TargetDevice.IsAcquiring)
        //                {
        //                    // Read data and catch if cancelled (to exit loop and continue)
        //                    try
        //                    {
        //                        // Throws an error if acquisition has been cancelled
        //                        // Otherwise refreshes the buffer DataIn with new data
        //                        await DI1100.TargetDevice.ReadDataAsync(cancelRead.Token);
        //                    }
        //                    catch (OperationCanceledException)
        //                    {
        //                        // Get here if acquisition cancelled
        //                        break;
        //                    }

        //                    // Get here if acquisition is still active
        //                    if (MasterChannel.DataIn.Count == 0)
        //                    {
        //                        // Get here if no data in the channel buffer
        //                        continue;
        //                    }

        //                    // We have data. Convert it to strings
        //                    string temp = "";
        //                    string temp1 = "";

        //                    for (int index = 0; index < 1; index++) // This is the row (scan) counter
        //                    {
        //                        foreach (var ch in DI1100.TargetDevice.Channels) // This is the column (channel) counter
        //                        {
        //                            // Get a channel value and convert it to a string
        //                            if (ch is Dataq.Devices.IChannelIn channelIn)
        //                            {
        //                                temp1 = channelIn.DataIn[index].ToString();
        //                                // Trim the output to prevent overrunning the display
        //                                if (temp1.Length > 7)
        //                                {
        //                                    temp1 = temp1.Substring(0, 7);
        //                                }
        //                                temp += temp1; // Append the channel value to the output
        //                            }
        //                        }
        //                    }


        //                    // Purge displayed data
        //                    foreach (var ch in DI1100.TargetDevice.Channels)
        //                    {
        //                        if (ch is Dataq.Devices.IChannelIn channelIn)
        //                        {
        //                            channelIn.DataIn.Clear();
        //                        }
        //                    }

        //                    LatestReading = Convert.ToDouble(temp);
        //                    validReadings.Add(LatestReading);
        //                }
        //              Debug.WriteLine("Stopped" + Environment.NewLine);
        //            }, cancelRead.Token);
        //            taskRead.Start();

        //            break;
        //    }
        //}

        //private void PassToMain(object sender, EventArgs e)
        //{
        //    if (_cal != null)
        //    {
        //        LatestReading = _cal.ConvertToInches(ReadNewData());
        //    }
        //}

        //public double ReadNewData()
        //{
        //    string ResponseString = "";
        //    switch (DataQModel)
        //    {
        //        case "100":
        //            int Scans = DI155.TargetDevice.NumberOfScansAvailable;
        //            short Channels = (short)DI155.TargetDevice.NumberOfChannelsEnabled;
        //            var DI_155_Data = new double[(Scans * Channels)]; // will hold all data for the scan


        //            // Attempt to retrieve and process the data
        //            DI155.TargetDevice.GetInterleavedScaledData(DI_155_Data, 0, Scans);

        //            for (int Row = 0; Row < Scans; Row++)
        //            {
        //                ResponseString += DI_155_Data[Row * Channels + 0].ToString("F4");
        //            }
        //            break;

        //        case "1000":
        //            //cancelRead.Cancel();

        //            //// Everything is good, so...
        //            //cancelRead = new CancellationTokenSource(); // Create the cancellation token
        //            //DI1100.TargetDevice.AcquisitionStartAsync(); // start acquiring
        //            //    // Capture the first channel programmed as an input (MasterChannel)
        //            //    // and use it to track data availability for all input channels
        //            //    Dataq.Devices.IChannelIn MasterChannel = null;
        //            //    for (int index = 0; index < DI1100.TargetDevice.Channels.Count; index++)
        //            //    {
        //            //        if (DI1100.TargetDevice.Channels[index] is Dataq.Devices.IChannelIn)
        //            //        {
        //            //            MasterChannel = (Dataq.Devices.IChannelIn)DI1100.TargetDevice.Channels[index]; // We have our channel
        //            //            break;
        //            //        }
        //            //    }

        //            //    // Keep reading while acquiring data
        //            //    while (DI1100.TargetDevice.IsAcquiring && validReadings.Count < 3)
        //            //    {
        //            //        // Read data and catch if cancelled (to exit loop and continue)
        //            //        try
        //            //        {
        //            //            // Throws an error if acquisition has been cancelled
        //            //            // Otherwise refreshes the buffer DataIn with new data
        //            //            DI1100.TargetDevice.ReadDataAsync(cancelRead.Token);
        //            //        }
        //            //        catch (OperationCanceledException)
        //            //        {
        //            //            // Get here if acquisition cancelled
        //            //            break;
        //            //        }



        //            //        // We have data. Convert it to strings
        //            //        string temp1 = "";

        //            //        for (int index = 0; index < 1; index++) // This is the row (scan) counter
        //            //        {
        //            //            foreach (var ch in DI1100.TargetDevice.Channels) // This is the column (channel) counter
        //            //            {
        //            //                // Get a channel value and convert it to a string
        //            //                if (ch is Dataq.Devices.IChannelIn channelIn)
        //            //                {
        //            //                    temp1 = channelIn.DataIn[index].ToString();
        //            //                    // Trim the output to prevent overrunning the display
        //            //                    if (temp1.Length > 7)
        //            //                    {
        //            //                        temp1 = temp1.Substring(0, 7);
        //            //                    }
        //            //                    ResponseString = temp1; // Append the channel value to the output

        //            //                validReadings.Add(Convert.ToDouble(ResponseString));
        //            //                }
        //            //            }
        //            //        }

        //            //        // Purge displayed data
        //            //        foreach (var ch in DI1100.TargetDevice.Channels)
        //            //        {
        //            //            if (ch is Dataq.Devices.IChannelIn channelIn)
        //            //            {
        //            //                channelIn.DataIn.Clear();
        //            //            }
        //            //        }
        //            //    }

        //            break;
        //    }

        //    return Convert.ToDouble(ResponseString);
        //}

        //public async Task<double> WaitForDataCollection(bool UseCalibration = false)
        //{
        //    _dataReceived = false;
        //    validReadings.Clear();

        //    switch (DataQModel)
        //    {
        //        case "100":
        //            DI155.TargetDevice.NewData += GetDataHandler;

        //            // Wait for the task to complete (i.e., for GetData to be called 5 times)
        //            await Task.Run(async () =>
        //            {
        //                int timeoutMilliseconds = 700000000; // Adjust timeout as needed
        //                int intervalMilliseconds = 100; // Adjust interval as needed
        //                int elapsedMilliseconds = 0;

        //                while (!_dataReceived && elapsedMilliseconds < timeoutMilliseconds)
        //                {
        //                    await Task.Delay(intervalMilliseconds);
        //                    elapsedMilliseconds += intervalMilliseconds;
        //                }
        //            });

        //            // Unsubscribe from the event
        //            DI155.TargetDevice.NewData -= GetDataHandler;
        //            //DI_155.NewData += GetDI155Data;
        //            break;

        //        case "1000":

        //            //const double threshold = 0.002; // Adjust this threshold as needed
        //            //double reading = ReadNewData();

        //            validReadings.Clear();

        //            while (validReadings.Count < 3)
        //            {
        //                Thread.Sleep(100);
        //            }

        //            break;
        //    }


        //    // Return the recorded data
        //    if (UseCalibration)
        //    {
        //        //Debug.WriteLine("AVERAGE    " + _cal.ConvertToInches(validReadings.Average()) + "   ");
        //        double value = _cal.ConvertToInches(validReadings.Average());
        //        return value;
        //    }
        //    else
        //    {
        //        //Debug.WriteLine("AVERAGE    " + validReadings.Average() + "   ");
        //        return validReadings.Average();
        //    }


        //    //DI_155.NewData -= GetDI155Data;
        //    // Subscribe to the event
        //}

        //private void GetDataHandler(object sender, EventArgs e)
        //{
        //    //const double threshold = 0.002; // Adjust this threshold as needed
        //    double reading = ReadNewData();

        //    if (validReadings.Count > 0)
        //    {
        //        //if (threshold <= validReadings.Average() - reading)
        //        //{
        //        validReadings.Add(reading);
        //        //Debug.WriteLine("Voltage    " + reading + "   ");
        //        //Debug.WriteLine("Inches    " + _cal.ConvertToInches(reading) + "   ");
        //        //}
        //    }
        //    else
        //    {
        //        validReadings.Add(reading);
        //        //Debug.WriteLine("Voltage    " + reading + "   ");
        //        //Debug.WriteLine("Inches    " + _cal.ConvertToInches(reading) + "   ");
        //    }

        //    if (validReadings.Count >= 2)
        //    {
        //        _dataReceived = true;
        //    }
        //}

        #endregion
    }

    /// <summary>
    /// Implements a mechanism for monitoring the connection status of a DataQ device. It periodically checks
    /// the device's connection state and notifies subscribers if a change occurs. The monitoring starts when
    /// StartMonitoring is called and stops with StopMonitoring. Changes in connection status trigger the
    /// IsConnectedChanged event, allowing external handlers to respond to connectivity changes.
    /// </summary>
    public class DeviceConnectionMonitor
    {
        private readonly DataQHelper _dataQ; // Reference to the associated DataQ device
        public event EventHandler IsConnectedChanged; // Event triggered on connection status change

        private bool _lastIsConnectedState; // Tracks the last known connection state
        private readonly int _pollingInterval = 10; // Time between connection checks in milliseconds
        private System.Threading.Timer _pollingTimer; // Timer for periodic connection checks

        /// <summary>
        /// Initializes a new instance of the DeviceConnectionMonitor class.
        /// </summary>
        /// <param name="dataQ">The DataQ device to monitor.</param>
        public DeviceConnectionMonitor(DataQHelper dataQ)
        {
            _dataQ = dataQ;
        }

        /// <summary>
        /// Begins periodic monitoring of the DataQ device's connection status.
        /// </summary>
        public void StartMonitoring()
        {
            _pollingTimer = new System.Threading.Timer(CheckConnectionStatus, null, 0, _pollingInterval);
        }

        /// <summary>
        /// Stops the ongoing monitoring of the DataQ device's connection status.
        /// </summary>
        public void StopMonitoring()
        {
            _pollingTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            _pollingTimer?.Dispose();
        }

        /// <summary>
        /// Periodically invoked by the timer to check the device's connection status.
        /// If a change is detected, the IsConnectedChanged event is raised.
        /// </summary>
        /// <param name="state">The state object passed to the Timer, not used in this context.</param>
        private void CheckConnectionStatus(object state)
        {
            //bool isConnected = _dataQ.DI155?.IsConnected ?? false;
            //if (_lastIsConnectedState != isConnected)
            //{
            //    _lastIsConnectedState = isConnected;
            //    OnIsConnectedChanged();
            //}
        }

        /// <summary>
        /// Raises the IsConnectedChanged event to notify subscribers of a change in the device's connection status.
        /// </summary>
        protected virtual void OnIsConnectedChanged()
        {
            IsConnectedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Represents exceptions that occur during DataQ device operations.
    /// This custom exception class provides detailed information about errors specifically related to DataQ devices,
    /// such as connection issues, data retrieval problems, or configuration errors.
    /// </summary>
    public class DataQException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataQException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataQException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQException"/> class with a specified error message and a reference
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public DataQException(string message, Exception inner)
            : base(message, inner) { }
    }

    /// <summary>
    /// Class for calibration operations.
    /// </summary>
    public class Calibration
    {
        public double _flatReading;
        public double _stepReading;
        public double _stepValue = 0.75; // Default step value

        // Properties
        public double CheckFlat { get; set; }
        public double CheckStep { get; set; }
        public double CheckStepValue { get; set; } = 0.0393701; // Default check step value
        public bool Successful { get; set; } = false;
        public double Slope { get; set; }
        public double Intercept { get; set; } // Intercept of the linear equation

        // Methods

        /// <summary>
        /// Calculates the slope and intercept of a line passing through two given points.
        /// </summary>
        public void CalculateLineEquation(Tuple<double, double> point1, Tuple<double, double> point2)
        {
            // Extract coordinates of the points
            double x1 = point1.Item1;
            double y1 = point1.Item2;
            double x2 = point2.Item1;
            double y2 = point2.Item2;

            // Calculate slope and intercept of the line
            Slope = (y2 - y1) / (x2 - x1);
            Intercept = y1 - Slope * x1;
        }

        /// <summary>
        /// Converts a voltage reading to inches using the calibrated slope and intercept.
        /// </summary>
        public double ConvertToInches(double voltage)
        {
            return (voltage * Slope) + Intercept;
        }

        /// <summary>
        /// Checks if calibration is successful within a certain range.
        /// </summary>
        public void CheckCalibration(double range)
        {
            if (Math.Abs(CheckFlat - CheckStep) < CheckStepValue + range &&
                Math.Abs(CheckFlat - CheckStep) > CheckStepValue - range)
            {
                Successful = true;
            }
            else
            {
                Successful = false;
            }
        }

        // Properties with encapsulation

        /// <summary>
        /// Flat reading property.
        /// </summary>
        public double FlatReading
        {
            get => _flatReading;
            set => _flatReading = value;
        }

        /// <summary>
        /// Step reading property.
        /// </summary>
        public double StepReading
        {
            get => _stepReading;
            set => _stepReading = value;
        }

        /// <summary>
        /// Step value property.
        /// </summary>
        public double StepValue
        {
            get => _stepValue;
            set => _stepValue = value;
        }
    }


}

