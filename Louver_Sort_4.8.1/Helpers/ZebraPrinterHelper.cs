using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace Louver_Sort_4._8._1.Helpers
{
    /// <summary>
    /// Provides functionalities for connecting to, disconnecting from, and printing to Zebra printers.
    /// </summary>
    internal class ZebraPrinterHelper
    {
        private ZebraPrinter _printer;

        /// <summary>
        /// Attempts to connect to the first discovered Zebra USB printer.
        /// </summary>
        /// <returns>A ZebraPrinter instance if successful, null otherwise.</returns>
        /// <exception cref="ZebraException">Thrown when there is a problem establishing a connection with the printer.</exception>
        public void Connect()
        {
            DiscoveredPrinter discoveredPrinter = UsbDiscoverer.GetZebraUsbPrinters().FirstOrDefault();
            if (discoveredPrinter == null)
                throw new ZebraException("No Zebra USB printers were discovered.");

            try
            {
                Connection connection = discoveredPrinter.GetConnection();
                connection.Open();
                _printer = ZebraPrinterFactory.GetInstance(PrinterLanguage.ZPL, connection);
            }
            catch (ConnectionException ex)
            {
                throw new ZebraException("Failed to connect to Zebra printer.", ex);
            }
        }

        /// <summary>
        /// Closes the connection to a Zebra printer.
        /// </summary>
        /// <exception cref="ZebraException">Thrown if an error occurs while closing the connection.</exception>
        public void Disconnect()
        {
            if (_printer?.Connection == null) return;

            try
            {
                _printer.Connection.Close();
            }
            catch (ConnectionException ex)
            {
                throw new ZebraException("Failed to disconnect from Zebra printer.", ex);
            }
        }

        /// <summary>
        /// Prints Louver IDs on a Zebra printer using ZPL commands.
        /// </summary>
        /// <param name="louvers">A list of Louver instances containing the IDs to print.</param>
        /// <exception cref="PrinterException">Thrown if there is an error during the print operation.</exception>
        public void PrintLouverIDs(List<LouverStructure.Louver> louvers)
        {
            if (!CheckStatus(_printer).IsReady)
                return;

            StringBuilder zplBuilder = new StringBuilder();
            for (int i = 0; i < louvers.Count; i++)
            {
                if (i % 2 == 0) zplBuilder.Append("^XA");

                // Generate ZPL commands for printing Louver IDs
                // Code omitted for brevity...

                if (i % 2 == 1 || i == louvers.Count - 1) zplBuilder.Append("^XZ");
            }
            Print(_printer, zplBuilder.ToString());
            Thread.Sleep(500);
        }

        /// <summary>
        /// Prints sorted Louver IDs on a Zebra printer using ZPL commands.
        /// </summary>
        /// <param name="louvers">A list of Louver instances containing the sorted IDs to print.</param>
        /// <exception cref="PrinterException">Thrown if there is an error during the print operation.</exception>
        public void PrintSortedLouverIDs(List<LouverStructure.Louver> louvers)
        {
            if (!CheckStatus(_printer).IsReady)
                return;

            StringBuilder zplBuilder = new StringBuilder();
            for (int i = 0; i < louvers.Count; i++)
            {
                if (i % 2 == 0) zplBuilder.Append("^XA");

                // Generate ZPL commands for printing sorted Louver IDs
                // Code omitted for brevity...

                if (i % 2 == 1 || i == louvers.Count - 1) zplBuilder.Append("^XZ");
            }
            Print(_printer, zplBuilder.ToString());
            Thread.Sleep(500);
        }

        /// <summary>
        /// Sends a ZPL command string to the specified Zebra printer for printing.
        /// </summary>
        /// <param name="printer">The ZebraPrinter instance to which the ZPL will be sent.</param>
        /// <param name="ZPL">The ZPL command string that defines what will be printed.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided printer argument is null.</exception>
        /// <exception cref="ZebraException">
        /// Thrown if the printer is not ready to print or if an error occurs during the printing process.
        /// </exception>
        public void Print(ZebraPrinter printer, string ZPL)
        {
            if (printer == null)
                throw new ArgumentNullException(nameof(printer));

            var (isReady, statusMessage) = CheckStatus(printer);
            if (!isReady)
            {
                throw new ZebraException(statusMessage);
            }

            try
            {
                printer.Connection.Write(Encoding.UTF8.GetBytes(ZPL));
            }
            catch (ConnectionException ex)
            {
                throw new ZebraException("Failed to print.", ex);
            }
        }

        /// <summary>
        /// Checks the operational status of the specified Zebra printer.
        /// </summary>
        /// <param name="zebraPrinter">The ZebraPrinter instance to check the status of.</param>
        /// <returns>
        /// A tuple containing a boolean indicating whether the printer is ready to print,
        /// and a string providing a message describing the current printer status.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when a null ZebraPrinter instance is passed.</exception>
        /// <exception cref="ZebraException">Thrown if an error occurs while checking the printer status.</exception>
        public (bool IsReady, string Message) CheckStatus(ZebraPrinter zebraPrinter)
        {
            if (zebraPrinter == null)
                throw new ArgumentNullException(nameof(zebraPrinter));

            try
            {
                PrinterStatus printerStatus = zebraPrinter.GetCurrentStatus();

                if (printerStatus.isReadyToPrint)
                {
                    return (true, "Ready to Print");
                }
                else
                {
                    string errorMessage = DeterminePrinterErrorMessage(printerStatus);
                    return (false, errorMessage);
                }
            }
            catch (ConnectionException ex)
            {
                throw new ZebraException("Failed to check printer status.", ex);
            }
        }

        /// <summary>
        /// Determines the error message based on the printer's status.
        /// </summary>
        /// <param name="printerStatus">The current status of the printer.</param>
        /// <returns>A string containing the error message based on the printer's status.</returns>
        private string DeterminePrinterErrorMessage(PrinterStatus printerStatus)
        {
            if (printerStatus.isPaused) return "Printer is paused.";
            if (printerStatus.isHeadOpen) return "Printer head is open.";
            if (printerStatus.isPaperOut) return "Printer is out of paper.";
            if (printerStatus.isRibbonOut) return "Printer is out of ribbon.";
            if (printerStatus.isReceiveBufferFull) return "Receive buffer is full.";
            if (printerStatus.isPartialFormatInProgress) return "Partial format is in progress.";
            if (printerStatus.isHeadCold) return "Printer head is cold.";
            if (printerStatus.isHeadTooHot) return "Printer head is too hot.";
            if (printerStatus.numberOfFormatsInReceiveBuffer > 0) return $"There are {printerStatus.numberOfFormatsInReceiveBuffer} formats in the receive buffer.";
            if (printerStatus.labelsRemainingInBatch > 0) return $"There are {printerStatus.labelsRemainingInBatch} labels remaining in the batch.";
            if (printerStatus.labelLengthInDots > 0) return $"Label length is set to {printerStatus.labelLengthInDots} dots.";

            return "Printer is in an unknown state.";
        }

    }

    /// <summary>
    /// Represents exceptions that occur during Zebra printer operations.
    /// </summary>
    public class ZebraException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ZebraException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ZebraException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ZebraException class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, 
        /// or a null reference if no inner exception is specified.</param>
        public ZebraException(string message, Exception inner) : base(message, inner) { }
    }
}
