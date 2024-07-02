using Dataq.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers
{
    public class DI145
    {
        private DataqDevice[] DataqDeviceArray; // Array containing all connected devices
        public DataqDevice TargetDevice; // Primary device we'll work with
        private double[] CollectedData = new double[11]; // Array to hold acquired data
        private string[] ChannelConfig = new string[11]; // Array to retain the channel configuration

        public void Connect()
        {
            // Discover connected devices
            DataqDeviceArray = Discovery.DiscoverAllDevices();

            // Use only the first device in the discovery array
            // Bail if the first array position is empty
            if (DataqDeviceArray.Length == 0)
            {
                Console.WriteLine("No compatible DATAQ Instruments devices found.");
                Environment.Exit(1);
            }
            else
            {
                TargetDevice = DataqDeviceArray[0]; // Assign DataqDevice DI_145 to the first device in the array and...
                TargetDevice.Connect(); // ...connect to it
            }

            // Display model-specific information
            Console.WriteLine($"Model: {TargetDevice.Model}");
            Console.WriteLine($"Serial Number: {TargetDevice.SerialNumber}");
            Console.WriteLine($"COM Port: {TargetDevice.ComPort}");

            // Bail if the connected device is not model DI-145
            if (TargetDevice.Model != "DI-145")
            {
                Console.WriteLine($"This program works only with DATAQ Instruments model DI-145. Incompatible Device ({TargetDevice.Model})");
                Environment.Exit(1);
            }

            // Load up ChannelConfig list with a default analog channel configuration
            for (int channel = 0; channel <= 3; channel++)
            {
                ChannelConfig[channel] = "On"; // Enable all four analog input channels
            }

            // Now define the default settings for the digital inputs
            ChannelConfig[8] = "Off"; // Turn off both discrete input bits
        }

        public async Task Disconnect()
        {
            
        }

        public void Start()
        {
            // This method configures the DI-145 scan list
            var range = new Dataq.Range<double>(); // Holds the FSR of the rate channel. Analog ranges are fixed at ±10 Vfs

            // Configure the DI-145's scan list for analog channels
            // Do the following for each of the four analog channels
            for (int channel = 0; channel <= 3; channel++)
            {
                // First disable the channel
                TargetDevice.ChannelArray[channel].Enabled = false;

                // Now configure the channel as defined by array ChannelConfig
                if (ChannelConfig[channel] != "Off")
                {
                    // Not OFF so must be on
                    TargetDevice.ChannelArray[channel].Enabled = true; // Enable the channel for acquisition
                }
            }

            // Now set up the digital inputs
            if (ChannelConfig[8] != "Off")
            {
                // Not OFF so must be on, so enable
                TargetDevice.ChannelArray[8].Enabled = true;
            }

            // Display the actual sampling rate per channel
            //Console.WriteLine($"Sampling Rate: {DI_145.SampleRatePerChannel:F3}");

            TargetDevice.NewDataMinimum = 1; // Set the number of scans to acquire before the NewData event fires
            TargetDevice.Start(); // Start scanning
        }

        public async Task Stop()
        {
            // Stop the DI-145
            TargetDevice.Stop();
        }
    }
}
