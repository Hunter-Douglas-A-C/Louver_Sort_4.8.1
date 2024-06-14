using Dataq.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers
{
    public class DI155
    {
        private DataqDevice[] DataqDeviceArray; // Array of all connected Dataq devices
        private readonly string[] ChannelConfig = new string[12]; // Configuration for each channel
        public string SerialNumber { get; private set; } = "";
        public DataqDevice TargetDevice { get; private set; }

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
                TargetDevice = DataqDeviceArray.FirstOrDefault();
                TargetDevice?.Connect();

                // Check if connection is successful and device model matches
                if (TargetDevice?.IsConnected != true)
                {
                    throw new DataQException("Failed to connect to DI-155 device or model mismatch.");
                }

                // Store the serial number of the connected device
                SerialNumber = TargetDevice.SerialNumber;
                // Configure default channel settings
                ConfigureDefaultChannelSettings();
            }
            catch (Exception ex)
            {
                HandleDataQException(ex.Message,ex);
            }
        }

        public async Task Start()
        {
            try
            {
                // Configure scan list and start data acquisition
                ConfigureScanList();
                TargetDevice.SampleRatePerChannel = 0.005;
                TargetDevice.NewDataMinimum = 1;
                TargetDevice.Start();
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to start the Dataq DI-155 device.", ex);
            }
        }

        public async Task Stop()
        {
            try
            {
                // Stop data acquisition
                TargetDevice.Stop();
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to stop the Dataq DI-155 device.", ex);
            }
        }

        public async Task Disconnect()
        {
            try
            {
                // Disconnect from the device
                TargetDevice.Stop();
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to disconnect the Dataq DI-155 device.", ex);
            }
        }

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

        private void ConfigureScanList()
        {
            try
            {
                var Range = new Dataq.Range<double>();
                for (int Channel = 0; Channel < 4; Channel++)
                {
                    TargetDevice.ChannelArray[Channel].Enabled = ChannelConfig[Channel] != "Off";
                    if (TargetDevice.ChannelArray[Channel].Enabled)
                    {
                        Range.Maximum = Convert.ToDouble(ChannelConfig[Channel]);
                        TargetDevice.ChannelArray[Channel].InputRange = Range;
                    }
                }
                ConfigureDigitalInputChannels(Range);
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to configure the scan list.", ex);
            }
        }

        private void ConfigureDigitalInputChannels(Dataq.Range<double> Range)
        {
            try
            {
                for (int Channel = 8; Channel <= 10; Channel++)
                {
                    TargetDevice.ChannelArray[Channel].Enabled = ChannelConfig[Channel] != "Off";
                    if (TargetDevice.ChannelArray[Channel].Enabled && Channel == 9)
                    {
                        Range.Maximum = Convert.ToDouble(ChannelConfig[Channel]);
                        TargetDevice.ChannelArray[Channel].InputRange = Range;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleDataQException("Failed to configure digital input channels.", ex);
            }
        }

        private void HandleDataQException(string message, Exception ex)
        {
            throw new DataQException(message, ex);
        }
    }
}
