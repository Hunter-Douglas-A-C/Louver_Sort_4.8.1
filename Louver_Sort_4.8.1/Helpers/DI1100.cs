using Lextm.SharpSnmpLib.Security;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.VisualBasic;

namespace Louver_Sort_4._8._1.Helpers
{
    public class DI1100
    {
        public Dataq.Devices.DI1100.Device TargetDevice;
        private Task taskRead;
        private CancellationTokenSource cancelRead;


        public async Task Connect()
        {
            IReadOnlyList<Dataq.Devices.IDevice> AllDevices =
               await Dataq.Misc.Discovery.ByModelAsync(typeof(Dataq.Devices.DI1100.Device));
            if (AllDevices.Count > 0)
            {
                TargetDevice = (Dataq.Devices.DI1100.Device)AllDevices[0];
                await TargetDevice.ConnectAsync();
                await TargetDevice.AcquisitionStopAsync();
                await TargetDevice.QueryDeviceAsync();
            }
            else
            {
                //error
            }
        }

        public async Task Disconnect()
        {

        }

        public async Task Start()
        {
            Dataq.Devices.DI1100.AnalogVoltageIn AnalogChan = (Dataq.Devices.DI1100.AnalogVoltageIn)TargetDevice.ChannelFactory(typeof(Dataq.Devices.DI1100.AnalogVoltageIn), 1); 
            TargetDevice.SetSampleRateOnChannels(2000);

            if (cancelRead != null)
            {
                // get here if an acquisition process is in progress and we've been commanded to stop
                cancelRead.Cancel(); // cancel the read process
                cancelRead = null;
                await taskRead; // wait for the read process to complete
                taskRead = null;
                await TargetDevice.AcquisitionStopAsync(); // stop the device from acquiring 
            }
            else
            {
                TargetDevice.SetSampleRateOnChannels(2000);
                try
                {
                    await TargetDevice.InitializeAsync(); // configure the device as defined. Errors if no channels are enabled
                }
                catch (Exception ex)
                {
                    // Detect if no channels are enabled, and bail if so. 
                    MessageBox.Show("Please enable at least one analog channel or digital port.", "No Enabled Channels", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public async Task Stop()
        {

        }
    }
}



