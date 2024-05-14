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

namespace Louver_Sort_4._8._1.Helpers
{
    /// <summary>
    /// Manages Dataq Device operations including connection, configuration, data acquisition, and disconnection.
    /// </summary>
    public class DataQHelper
    {
        private DataqDevice[] DataqDeviceArray; // Array of all connected Dataq devices
        private string _outputString; // Stores the latest data received from the device
        private readonly string[] ChannelConfig = new string[12]; // Configuration for each channel
        public Calibration _cal = new Calibration();

        public event EventHandler AnalogUpdated; // Event triggered when new analog data is received
        public event EventHandler LostConnection; // Event triggered when connection is lost
        List<double> validReadings = new List<double>();
        double AverageReading;


        public event EventHandler LatestReadingChanged;

        private double _latestReading;
        public double LatestReading
        {
            get
            {
                return _latestReading;
            }
            set
            {
                _latestReading = value;
                OnLatestReadingChanged();
            }
        }

        protected virtual void OnLatestReadingChanged()
        {
            // Check if there are subscribers to the event
            LatestReadingChanged?.Invoke(this, EventArgs.Empty);
        }

        public string SerialNumber { get; private set; } = "";

        public DataqDevice DI_155 { get; private set; } // The DI-155 device instance

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

        public double GetSlope()
        {
            if (_cal != null)
            {
                return _cal.Slope;
            }
            return 0.0;
        }

        public void SetSlope(double Slope)
        {
            _cal.Slope = Slope;
        }





        /// <summary>
        /// Attempts to connect to a single Dataq DI-155 device and configures it for use. Throws a DataQException if the connection fails, no device is found, or multiple devices are connected.
        /// </summary>
        /// <exception cref="DataQException">Thrown when the connection process fails or encounters an error.</exception>
        public void Connect()
        {
            try
            {
                DataqDeviceArray = Discovery.DiscoverAllDevices();
                if (DataqDeviceArray.Length != 1)
                {
                    throw new DataQException("No devices found or multiple devices connected.");
                }

                DI_155 = DataqDeviceArray.FirstOrDefault();
                DI_155?.Connect();

                if (DI_155?.IsConnected != true || DI_155.Model != "DI-155")
                {
                    throw new DataQException("Failed to connect to DI-155 device or model mismatch.");
                }

                SerialNumber = DI_155.SerialNumber;
                ConfigureDefaultChannelSettings();
                //DI_155.NewData += GetDI155Data;
            }
            catch (Exception ex)
            {
                // Optionally, log the exception or perform other internal error handling here

                // Notify subscribers about the lost connection
                OnLostConnection();

                // Rethrow a custom exception to be handled by the caller
                throw new DataQException("Error connecting to Dataq DI-155 device.", ex);
            }
        }

        /// <summary>
        /// Starts data acquisition with predefined settings on the DI-155 device. Throws a DataQException if the device is not initialized or fails to start.
        /// </summary>
        /// <exception cref="DataQException">Thrown when starting the data acquisition process fails.</exception>
        public void Start()
        {
            int timeoutMilliseconds = 1000;
            try
            {
                ConfigureScanList();
                DI_155.SampleRatePerChannel = 0.005;
                DI_155.NewDataMinimum = 1; // Minimum number of scans before triggering NewData event

                if (DI_155 == null)
                {
                    throw new DataQException("DI-155 device is not initialized or connected.");
                }

                // Create a CancellationTokenSource with the specified timeout
                using (var cancellationTokenSource = new CancellationTokenSource(timeoutMilliseconds))
                {
                    // Create a Task to execute the Start method
                    var startTask = Task.Run(() => DI_155.Start(), cancellationTokenSource.Token);

                    // Wait for the task to complete or the timeout to occur
                    if (!startTask.Wait(timeoutMilliseconds))
                    {
                        // If the task didn't complete within the timeout, cancel it and throw a DataQException
                        cancellationTokenSource.Cancel();
                        throw new DataQException("Start operation timed out.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception, notify the user, or perform other appropriate error handling here.

                // Rethrow a custom exception to be handled by the caller.
                throw new DataQException("Failed to start the Dataq DI-155 device.", ex);
            }
        }

        /// <summary>
        /// Stops data acquisition on the DI-155 device. Throws a DataQException if the device is not initialized or the stop operation fails.
        /// </summary>
        /// <exception cref="DataQException">Thrown when stopping the device fails.</exception>
        public void Stop()
        {
            try
            {
                if (DI_155 == null)
                {
                    throw new DataQException("DI-155 device is not initialized or connected.");
                }

                DI_155.Stop();
            }
            catch (Exception ex)
            {
                // Perform any necessary cleanup or logging before rethrowing the exception.

                // If you have a logger, you might want to log the exception here.
                // Logger.LogError(ex, "Failed to stop the Dataq DI-155 device.");

                // Rethrow the exception to notify the caller that stopping the device has failed.
                throw new DataQException("Failed to stop the Dataq DI-155 device.", ex);
            }
        }

        /// <summary>
        /// Disconnects the DI-155 device and releases any associated resources. Throws a DataQException if the device is not initialized or disconnection fails.
        /// </summary>
        /// <exception cref="DataQException">Thrown when disconnecting the device fails.</exception>
        public void Disconnect()
        {
            try
            {
                if (DI_155 == null)
                {
                    throw new DataQException("DI-155 device is not initialized or connected.");
                }

                DI_155.Disconnect();
            }
            catch (Exception ex)
            {
                // Handle the error, such as logging it, or perform any necessary cleanup here.

                // If you have a logger, you might want to log the exception.
                // Logger.LogError(ex, "Failed to disconnect the Dataq DI-155 device.");

                // Rethrow a custom exception to inform the caller of the disconnection failure.
                throw new DataQException("Failed to disconnect the Dataq DI-155 device.", ex);
            }
        }

        /// <summary>
        /// Parses the last received data string into a double representing distance. Throws a DataParsingException if the string cannot be parsed.
        /// </summary>
        /// <returns>The parsed distance as a double.</returns>
        /// <exception cref="DataParsingException">Thrown when parsing the data string fails.</exception>
        public double GetDistance()
        {
            try
            {
                //if (!string.IsNullOrEmpty(_outputString))
                //{
                //    string pattern = ",";
                //    string replacement = "";
                //    // Using InvariantCulture to ensure consistent parsing regardless of system settings.
                //    return Convert.ToDouble(Regex.Replace(_outputString, pattern, replacement), CultureInfo.InvariantCulture);
                //}
                //// Consider whether returning 0 is appropriate for all scenarios.
                //// It might be better to throw an exception if _outputString is null or empty.
                return Math.Round(Convert.ToDouble(_outputString), 2);
            }
            catch (FormatException ex)
            {
                // Handle the format exception if the string is not in a valid format.
                throw new DataQException("Failed to parse distance from the output string.", ex);
            }
            catch (OverflowException ex)
            {
                // Handle cases where the number is too large or too small for a double.
                throw new DataQException("The number in the output string is too large or too small to fit in a double.", ex);
            }
            // Other specific exceptions can be caught and handled here if necessary.
        }


        #region Private Methods
        /// <summary>
        /// Notifies subscribers of new analog data availability. Catches and rethrows exceptions as a DataQException.
        /// </summary>
        /// <exception cref="DataQException">Thrown when notifying subscribers fails.</exception>
        private void OnAnalogUpdated()
        {
            try
            {
                AnalogUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // Log the exception, if necessary
                // Logger.LogError(ex, "Error occurred in OnAnalogUpdated event invocation.");

                // Wrap and throw a custom exception to be handled by the caller of OnAnalogUpdated
                throw new DataQException("An error occurred while raising the AnalogUpdated event.", ex);
            }
        }

        /// <summary>
        /// Notifies subscribers of a lost connection with the DI-155 device. Catches and rethrows exceptions as a DataQException.
        /// </summary>
        /// <exception cref="DataQException">Thrown when notifying subscribers fails.</exception>
        private void OnLostConnection()
        {
            try
            {
                LostConnection?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // Log the exception, if necessary
                // Logger.LogError(ex, "Error occurred in OnAnalogUpdated event invocation.");

                // Wrap and throw a custom exception to be handled by the caller of OnAnalogUpdated
                throw new DataQException("An error occurred while raising the AnalogUpdated event.", ex);
            }
        }

        /// <summary>
        /// Configures default channel settings for the DI-155 device based on predefined configurations. Throws a DataQException on failure.
        /// </summary>
        /// <exception cref="DataQException">Thrown when configuring channels fails.</exception>
        private void ConfigureDefaultChannelSettings()
        {
            try
            {
                // Analog channels configuration
                for (int i = 0; i < 4; i++)
                {
                    ChannelConfig[i] = "10"; // Set all analog channels to the ±10 V full scale range
                }
                // ChannelConfig[4-7] are unused
                // Digital inputs configuration
                for (int i = 8; i <= 10; i++)
                {
                    ChannelConfig[i] = "Off"; // Default: Off
                }
            }
            catch (Exception ex)
            {
                // Handle the exception internally if needed (e.g., logging)

                // Then, throw a custom exception to indicate a configuration error
                throw new DataQException("Failed to configure default channel settings.", ex);
            }
        }

        /// <summary>
        /// Configures the scan list of the DI-155 device based on current channel configurations. Throws a DataQException on failure.
        /// </summary>
        /// <exception cref="DataQException">Thrown when configuring the scan list fails.</exception>
        private void ConfigureScanList()
        {
            try
            {
                var Range = new Dataq.Range<double>();

                // Configure analog channels
                for (int Channel = 0; Channel < 4; Channel++)
                {
                    DI_155.ChannelArray[Channel].Enabled = ChannelConfig[Channel] != "Off";
                    if (DI_155.ChannelArray[Channel].Enabled)
                    {
                        Range.Maximum = Convert.ToDouble(ChannelConfig[Channel]); // Potential FormatException
                        DI_155.ChannelArray[Channel].InputRange = Range;
                    }
                }

                // Configure digital input channels
                ConfigureDigitalInputChannels(Range);
            }
            catch (Exception ex)
            {
                // Handle specific known exceptions here if necessary, such as FormatException

                // Rethrow a custom exception to indicate a scan list configuration error
                throw new DataQException("Failed to configure the scan list.", ex);
            }
        }

        /// <summary>
        /// Configures digital input channels of the DI-155 device according to the ChannelConfig array. Throws a DataQException on failure.
        /// </summary>
        /// <param name="Range">The range object used for setting the input range of channels.</param>
        /// <exception cref="DataQException">Thrown when configuring digital input channels fails.</exception>
        private void ConfigureDigitalInputChannels(Dataq.Range<double> Range)
        {
            try
            {
                for (int Channel = 8; Channel <= 10; Channel++)
                {
                    DI_155.ChannelArray[Channel].Enabled = ChannelConfig[Channel] != "Off";
                    if (DI_155.ChannelArray[Channel].Enabled && Channel == 9)
                    {
                        // This conversion might throw a FormatException if ChannelConfig[Channel] is not a valid double
                        Range.Maximum = Convert.ToDouble(ChannelConfig[Channel]);
                        DI_155.ChannelArray[Channel].InputRange = Range;
                    }
                }
            }
            catch (Exception ex)
            {
                // Catch and wrap any exception in a DigitalInputConfigurationException
                throw new DataQException("Failed to configure digital input channels.", ex);
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
                OnAnalogUpdated();
                ResponseString = ""; // reset ResponseString for next iteration


            }
            return Math.Round(Convert.ToDouble(_outputString), 2);
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
                double value = Math.Round(_cal.ConvertToInches(validReadings.Average()), 2);
                return value;
            }
            else
            {
                Debug.WriteLine("AVERAGE    " + validReadings.Average() + "   ");
                return Math.Round(validReadings.Average(), 2);
            }
        }

        private void GetDataHandler(object sender, EventArgs e)
        {
            //const double threshold = 0.002; // Adjust this threshold as needed
            double reading = Math.Round(ReadNewData(), 2);

            if (validReadings.Count > 0)
            {
                //if (threshold <= validReadings.Average() - reading)
                //{
                validReadings.Add(reading);
                Debug.WriteLine("    " + reading + "   ");
                //}
            }
            else
            {
                validReadings.Add(reading);
                Debug.WriteLine("    " + reading + "   ");
            }

            if (validReadings.Count >= 5)
            {
                _dataReceived = true;
            }
        }














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
            LatestReading = Math.Round(_cal.ConvertToInches(ReadNewData()), 2);

            }
        }


        #endregion
    }

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
    /// Represents exceptions that occur during DataQ device operations. This custom exception class
    /// is designed to provide detailed information about errors specifically related to DataQ devices,
    /// such as connection issues, data retrieval problems, or configuration errors.
    /// </summary>
    public class DataQException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DataQException class.
        /// </summary>
        public DataQException() { }

        /// <summary>
        /// Initializes a new instance of the DataQException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataQException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the DataQException class with a specified error message and a reference
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public DataQException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class Calibration
    {
        private double _flatReading;
        private double _stepReading;
        //private double _stepValue = 0.0393701;  // This is the distance difference related to the step reading, set appropriately.
        private double _stepValue = 0.75;


        //private double _slope = double.NaN; // Initialize slope to NaN.
        private double _slope; // Initialize slope to NaN.



        public double FlatReading
        {
            get => _flatReading;
            set
            {
                _flatReading = value;
            }
        }

        public double StepReading
        {
            get => _stepReading;
            set
            {
                _stepReading = value;
            }
        }

        public double StepValue
        {
            get => _stepValue;
            set
            {
                _stepValue = value;
            }
        }

        public double Slope
        {
            get => _slope;
            set
            {
                _slope = value;
            }
        }

        public double intercept; // Intercept of the linear equation

        public void CalculateLineEquation(Tuple<double, double> point1, Tuple<double, double> point2)
        {
            // Extract coordinates of the points
            double x1 = point1.Item1;
            double y1 = point1.Item2;
            double x2 = point2.Item1;
            double y2 = point2.Item2;

            // Calculate slope and intercept of the line
            Slope = (y2 - y1) / (x2 - x1);
            intercept = y1 - Slope * x1;
        }

        public double ConvertToInches(double voltage)
        {
            return (voltage * Slope) + intercept;
        }





    }

}

