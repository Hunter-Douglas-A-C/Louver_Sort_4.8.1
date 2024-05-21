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
using Dataq.Channels;

namespace Louver_Sort_4._8._1.Helpers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Manages Dataq Device operations including connection, configuration, data acquisition, and disconnection.
    /// </summary>
    public class DataQHelper
    {
        private DataqDevice[] DataqDeviceArray; // Array of all connected Dataq devices
        private string _outputString; // Stores the latest data received from the device
        private readonly string[] ChannelConfig = new string[12]; // Configuration for each channel
        public Calibration _cal = new Calibration(); // Calibration object for handling calibration data
        private double _latestReading; // Stores the latest reading received from the device

        // Events for notifying subscribers of analog data updates and lost connections
        //public event EventHandler AnalogUpdated;
        public event EventHandler LostConnection;
        public event EventHandler LatestReadingChanged;
        List<double> validReadings = new List<double>();

        // Property for accessing the latest reading
        public double LatestReading
        {
            get { return _latestReading; }
            set
            {
                _latestReading = value;
                OnLatestReadingChanged();
            }
        }

        // Property for storing the serial number of the connected device
        public string SerialNumber { get; private set; } = "";

        // Property for accessing the connected Dataq device instance
        public DataqDevice DI_155 { get; private set; }


        // Method for connecting to the Dataq device asynchronously
        public async Task Connect()
        {
            try
            {
                // Discover all connected Dataq devices
                DataqDeviceArray = await Task.Run(() => Discovery.DiscoverAllDevices());
                // Check if exactly one device is found
                if (DataqDeviceArray.Length != 1)
                {
                    throw new DataQException("No devices found or multiple devices connected.");
                }

                // Get the first device found and connect to it
                DI_155 = DataqDeviceArray.FirstOrDefault();
                DI_155?.Connect();

                // Check if connection is successful and device model matches
                if (DI_155?.IsConnected != true || DI_155.Model != "DI-155")
                {
                    throw new DataQException("Failed to connect to DI-155 device or model mismatch.");
                }

                // Store the serial number of the connected device
                SerialNumber = DI_155.SerialNumber;
                // Configure default channel settings
                ConfigureDefaultChannelSettings();
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
        }

        // Method for starting data acquisition asynchronously
        public void Start()
        {
            try
            {
                // Configure scan list and start data acquisition
                ConfigureScanList();
                DI_155.SampleRatePerChannel = 0.005;
                DI_155.NewDataMinimum = 1;
                DI_155.Start();
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to start the Dataq DI-155 device.", ex);
            }
        }

        // Method for stopping data acquisition asynchronously
        public void Stop()
        {
            try
            {
                // Stop data acquisition
                DI_155.Stop();
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to stop the Dataq DI-155 device.", ex);
            }
        }

        // Method for disconnecting from the device asynchronously
        public void Disconnect()
        {
            try
            {
                // Disconnect from the device
                DI_155.Stop();
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to disconnect the Dataq DI-155 device.", ex);
            }
        }

        #region Private Methods

        // Method for handling connection errors
        private void HandleConnectionError(Exception ex)
        {
            OnLostConnection();
            throw new DataQException("Error connecting to Dataq DI-155 device.", ex);
        }

        // Method for handling DataQ exceptions
        private void HandleDataQException(string message, Exception ex)
        {
            throw new DataQException(message, ex);
        }

        // Event handler for triggering the LatestReadingChanged event
        protected virtual void OnLatestReadingChanged()
        {
            LatestReadingChanged?.Invoke(this, EventArgs.Empty);
        }

        // Event handler for triggering the LostConnection event
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

        // Method for configuring default channel settings
        private void ConfigureDefaultChannelSettings()
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    ChannelConfig[i] = "10";
                }
                for (int i = 8; i <= 10; i++)
                {
                    ChannelConfig[i] = "Off";
                }
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to configure default channel settings.", ex);
            }
        }

        // Method for configuring the scan list
        private void ConfigureScanList()
        {
            try
            {
                var Range = new Dataq.Range<double>();
                for (int Channel = 0; Channel < 4; Channel++)
                {
                    DI_155.ChannelArray[Channel].Enabled = ChannelConfig[Channel] != "Off";
                    if (DI_155.ChannelArray[Channel].Enabled)
                    {
                        Range.Maximum = Convert.ToDouble(ChannelConfig[Channel]);
                        DI_155.ChannelArray[Channel].InputRange = Range;
                    }
                }
                ConfigureDigitalInputChannels(Range);
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to configure the scan list.", ex);
            }
        }

        // Method for configuring digital input channels
        private void ConfigureDigitalInputChannels(Dataq.Range<double> Range)
        {
            try
            {
                for (int Channel = 8; Channel <= 10; Channel++)
                {
                    DI_155.ChannelArray[Channel].Enabled = ChannelConfig[Channel] != "Off";
                    if (DI_155.ChannelArray[Channel].Enabled && Channel == 9)
                    {
                        Range.Maximum = Convert.ToDouble(ChannelConfig[Channel]);
                        DI_155.ChannelArray[Channel].InputRange = Range;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to configure digital input channels.", ex);
            }
        }

        #endregion

        public bool _KeepMonitoring = true;
        public async void StartActiveMonitoring()
        {
            DI_155.NewData += PassToMain;

            await Task.Run(async () =>
            {
                while (_KeepMonitoring)
                {
                    await Task.Delay(100);
                }
            });

            DI_155.NewData -= PassToMain;
        }

        private void PassToMain(object sender, EventArgs e)
        {
            if (_cal != null)
            {
                LatestReading = _cal.ConvertToInches(ReadNewData());
                Debug.WriteLine("Live Data In volts: " + ReadNewData());
                Debug.WriteLine("Live Data In in: " + LatestReading);

            }
        }


        private readonly List<double> _recordedData = new List<double>();
        private bool _dataReceived = false;

        public double ReadNewData()
        {
            int Scans = DI_155.NumberOfScansAvailable;
            short Channels = (short)DI_155.NumberOfChannelsEnabled;
            var DI_155_Data = new double[(Scans * Channels)]; // will hold all data for the scan
            string ResponseString = "";

            // Attempt to retrieve and process the data
            DI_155.GetInterleavedScaledData(DI_155_Data, 0, Scans);

            for (int Row = 0; Row < Scans; Row++)
            {

                ResponseString += DI_155_Data[Row * Channels + 0].ToString("F4");

                _outputString = ResponseString;
                //OnAnalogUpdated();
                ResponseString = ""; // reset ResponseString for next iteration


            }
            return Convert.ToDouble(_outputString);
        }








        public async Task<double> WaitForDataCollection(bool UseCalibration = false)
        {
            _dataReceived = false;
            validReadings.Clear();



            //DI_155.NewData -= GetDI155Data;
            // Subscribe to the event
            DI_155.NewData += GetDataHandler;



            // Wait for the task to complete (i.e., for GetData to be called 5 times)

            await Task.Run(async () =>
            {
                int timeoutMilliseconds = 700000000; // Adjust timeout as needed
                int intervalMilliseconds = 100; // Adjust interval as needed
                int elapsedMilliseconds = 0;

                while (!_dataReceived && elapsedMilliseconds < timeoutMilliseconds)
                {
                    await Task.Delay(intervalMilliseconds);
                    elapsedMilliseconds += intervalMilliseconds;
                }
            });

            // Unsubscribe from the event
            DI_155.NewData -= GetDataHandler;
            //DI_155.NewData += GetDI155Data;

            // Return the recorded data
            if (UseCalibration)
            {

                Debug.WriteLine("AVERAGE    " + _cal.ConvertToInches(validReadings.Average()) + "   ");
                double value = _cal.ConvertToInches(validReadings.Average());
                return value;
            }
            else
            {
                Debug.WriteLine("AVERAGE    " + validReadings.Average() + "   ");
                return validReadings.Average();
            }
        }

        private void GetDataHandler(object sender, EventArgs e)
        {
            //const double threshold = 0.002; // Adjust this threshold as needed
            double reading = ReadNewData();

            if (validReadings.Count > 0)
            {
                //if (threshold <= validReadings.Average() - reading)
                //{
                validReadings.Add(reading);
                Debug.WriteLine("Voltage    " + reading + "   ");
                Debug.WriteLine("Inches    " + _cal.ConvertToInches(reading) + "   ");
                //}
            }
            else
            {
                validReadings.Add(reading);
                Debug.WriteLine("Voltage    " + reading + "   ");
                Debug.WriteLine("Inches    " + _cal.ConvertToInches(reading) + "   ");
            }

            if (validReadings.Count >= 2)
            {
                _dataReceived = true;
            }
        }

        public void SetCalibrationFlat()
        {
            List<double> CalAverages = new List<double>();
            //for (int i = 0; i < 2; i++)
            //{
            CalAverages.Add(WaitForDataCollection(false).Result);
            //}

            _cal.FlatReading = CalAverages.Average();
        }

        public void SetCalibrationStep()
        {
            List<double> CalAverages = new List<double>();
            //for (int i = 0; i < 2; i++)
            //{
            CalAverages.Add(WaitForDataCollection(false).Result);
            //}

            _cal.StepReading = CalAverages.Average();



            _cal.CalculateLineEquation(Tuple.Create(_cal.FlatReading, 0.0), Tuple.Create(_cal.StepReading, _cal.StepValue));
        }

        public void CheckCalFlat()
        {
            List<double> CalAverages = new List<double>();
            CalAverages.Add(WaitForDataCollection(true).Result);

            _cal.CheckFlat = CalAverages.Average();
        }

        public void CheckCalStep(double range)
        {
            List<double> CalAverages = new List<double>();
            CalAverages.Add(WaitForDataCollection(true).Result);

            _cal.CheckStep = CalAverages.Average();

            _cal.CheckCalibration(range);
        }




        public double GetSlope()
        {
            if (_cal != null)
            {
                return _cal.Slope;
            }
            return 0.0;
        }
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
        private Timer _pollingTimer; // Timer for periodic connection checks

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
            _pollingTimer = new Timer(CheckConnectionStatus, null, 0, _pollingInterval);
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
            bool isConnected = _dataQ.DI_155?.IsConnected ?? false;
            if (_lastIsConnectedState != isConnected)
            {
                _lastIsConnectedState = isConnected;
                OnIsConnectedChanged();
            }
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
        private double _flatReading;
        private double _stepReading;
        private double _stepValue = 0.75; // Default step value

        // Properties
        public double CheckFlat { get; set; }
        public double CheckStep { get; set; }
        public double CheckStepValue { get; set; } = 0.0393701; // Default check step value
        public bool Successful { get; private set; } = false;
        public double Slope { get; private set; }
        public double Intercept { get; private set; } // Intercept of the linear equation

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

