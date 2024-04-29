using Dataq.Simple;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1.Cms;

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

        public event EventHandler AnalogUpdated; // Event triggered when new analog data is received
        public event EventHandler LostConnection; // Event triggered when connection is lost
        List<double> validReadings = new List<double>();
        double AverageReading;
        public double ReadingResult;

        public string SerialNumber { get; private set; } = "";

        public DataqDevice DI_155 { get; private set; } // The DI-155 device instance

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
                DI_155.NewData += GetDI155Data;
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
            try
            {
                ConfigureScanList();
                DI_155.SampleRatePerChannel = 0.005;
                DI_155.NewDataMinimum = 1; // Minimum number of scans before triggering NewData event

                if (DI_155 == null)
                {
                    throw new DataQException("DI-155 device is not initialized or connected.");
                }

                DI_155.Start();
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
                return Convert.ToDouble(_outputString);
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

        /// <summary>
        /// Handles the NewData event of the DI-155 device, processing received data. It converts the interleaved
        /// data from the device into a formatted string representation. Each piece of data is formatted to four
        /// decimal places and comma-separated. After processing all available scans, it updates a class-level
        /// string with the formatted data and triggers an update event to notify subscribers.
        /// If an error occurs during data retrieval or processing, a DataQException is thrown.
        /// </summary>
        /// <param name="sender">The source of the event, typically the DI-155 device.</param>
        /// <param name="e">The event arguments, not used in this context.</param>
        /// <exception cref="DataQException">Thrown if an error occurs during data retrieval or processing.</exception>
        private void GetDI155Data(object sender, EventArgs e)
        {
            try
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
                    validReadings.Add(Convert.ToDouble(_outputString));
                    OnAnalogUpdated();
                    ResponseString = ""; // reset ResponseString for next iteration


                    if (validReadings.Count() == 5)
                    {
                        AverageReading = validReadings.Average();
                        validReadings.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                // Consider logging, cleaning up, or notifying the user here

                // Throw a custom exception to indicate a problem with data retrieval
                throw new DataQException("An error occurred while retrieving data from the DI-155 device.", ex);
            }
        }




        public double GetLatestData()
        {
            return Convert.ToDouble(_outputString);
        }


        public void RecordAndAverageReadings()
        {
            Thread Record = new Thread(() =>
            {
                 ReadingResult = AverageReadingsFunction();
            });
            Record.Start();
        }

        public double AverageReadingsFunction()
        {
            const int numberOfReadings = 5;
            const double threshold = 0.2; // Adjust this threshold as needed

            List<double> validReadings = new List<double>();

            for (int i = 0; i < numberOfReadings; i++)
            {
                try
                {
                    double reading = GetLatestData(); // Get the recorded reading
                    Thread.Sleep(1000);

                    //Check if the reading is within threshold of the others
                    if (validReadings.Count > 0)
                    {
                        double average = validReadings.Average();
                        double difference = Math.Abs(reading - average);
                        if (difference > threshold * average)
                        {
                            //Reading is vastly different, discard it
                            continue;
                        }
                    }

                    //Reading is valid, add it to the list
                    validReadings.Add(reading);
                }
                catch (DataQException ex)
                {
                    //Handle the exception, log or notify as needed
                    Console.WriteLine("Error recording reading: " + ex.Message);
                }
            }

            //Calculate the average of valid readings
            double averageReading = 0.0;
            if (validReadings.Count > 0)
            {
                averageReading = validReadings.Average();
            }

            return averageReading;
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
}
