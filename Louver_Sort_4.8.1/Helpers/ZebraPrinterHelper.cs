//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Zebra.Sdk.Comm;
//using Zebra.Sdk.Printer;
//using Zebra.Sdk.Printer.Discovery;

//namespace Louver_Sort_4._8._1.Helpers
//{
//    /// <summary>
//    /// Provides functionalities for connecting to, disconnecting from, and printing to Zebra printers.
//    /// </summary>
//    internal class ZebraPrinterHelper
//    {
//        /// <summary>
//        /// Attempts to connect to the first discovered Zebra USB printer.
//        /// </summary>
//        /// <returns>A ZebraPrinter instance if successful, null otherwise.</returns>
//        /// <exception cref="ZebraException">Thrown when there is a problem establishing a connection with the printer.</exception>
//        public ZebraPrinter Connect()
//        {
//            try
//            {
//                DiscoveredPrinter discoveredPrinter = UsbDiscoverer.GetZebraUsbPrinters().FirstOrDefault();
//                if (discoveredPrinter == null) throw new ZebraException("No Zebra USB printers were discovered.");

//                Connection connection = discoveredPrinter.GetConnection();
//                connection.Open();
//                return ZebraPrinterFactory.GetInstance(PrinterLanguage.ZPL, connection);
//            }
//            catch (Exception ex)
//            {
//                // Re-throw any caught exceptions to be handled outside the class.
//                throw new ZebraException($"Failed to connect to Zebra printer: {ex.Message}", ex);
//            }
//        }

//        /// <summary>
//        /// Closes the connection to a Zebra printer.
//        /// </summary>
//        /// <param name="printer">The ZebraPrinter instance to disconnect from.</param>
//        /// <exception cref="ZebraException">Thrown if an error occurs while closing the connection.</exception>
//        public void Disconnect(ZebraPrinter printer)
//        {
//            try
//            {
//                printer.Connection.Close();
//            }
//            catch (ZebraException ex)
//            {
//                // Re-throw any caught exceptions to be handled outside the class.
//                throw new ZebraException($"Failed to disconnect from Zebra printer: {ex.Message}", ex);
//            }
//        }

//        /// <summary>
//        /// Prints Louver IDs on a Zebra printer using ZPL commands.
//        /// </summary>
//        /// <param name="printer">The ZebraPrinter instance to use for printing.</param>
//        /// <param name="louverSet">A list of double values representing Louver IDs to print.</param>
//        /// <exception cref="PrinterException">Thrown if there is an error during the print operation.</exception>
//        public void PrintLouverIDs(ZebraPrinter printer, List<double> louverSet)
//        {
//            try
//            {
//                StringBuilder zplBuilder = new StringBuilder();
//                for (int i = 0; i < louverSet.Count; i++)
//                {
//                    if (i % 2 == 0) zplBuilder.Append("^XA");

//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},40^A0N,40,40^FDLouver ID:^FS");
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 120 : 360)},120^A0N,40,40^FD{louverSet[i]}^FS");

//                    if (i % 2 == 1 || i == louverSet.Count - 1) zplBuilder.Append("^XZ");

//                    // Send ZPL to printer in batches or individually
//                    if (zplBuilder.Length > 0 && (i % 2 == 1 || i == louverSet.Count - 1))
//                    {
//                        Print(printer, zplBuilder.ToString());
//                        zplBuilder.Clear();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                // Re-throw any caught exceptions to be handled outside the class.
//                throw new ZebraException($"Failed to print Louver IDs: {ex.Message}", ex);
//            }
//        }


//        /// <summary>
//        /// Sends a Zebra Programming Language (ZPL) string to the specified Zebra printer for printing.
//        /// </summary>
//        /// <param name="printer">The ZebraPrinter instance to which the ZPL will be sent.</param>
//        /// <param name="ZPL">The ZPL command string that defines what will be printed.</param>
//        /// <exception cref="ArgumentNullException">Thrown if the provided printer argument is null.</exception>
//        /// <exception cref="ZebraException">
//        /// Thrown if the printer is not ready to print. The exception message contains details about the printer's status.
//        /// This exception is also thrown for any errors that occur during the writing process to the printer connection.
//        /// </exception>
//        /// <remarks>
//        /// Before attempting to print, the method checks the status of the printer using the CheckStatus method.
//        /// If the printer is not ready (e.g., if it's paused, out of paper, etc.), the method will throw a ZebraException
//        /// with an appropriate message indicating the reason.
//        /// If the printer is ready, the method sends the ZPL command to the printer for execution.
//        /// </remarks>
//        public void Print(ZebraPrinter printer, string ZPL)
//        {
//            if (printer == null)
//                throw new ArgumentNullException(nameof(printer));

//            // Check the printer's status before attempting to print.
//            var (isReady, statusMessage) = CheckStatus(printer);
//            if (!isReady)
//            {
//                // If the printer is not ready, throw an exception with the status message.
//                throw new ZebraException(statusMessage);
//            }

//            // If the printer is ready, proceed with the printing.
//            try
//            {
//                printer.Connection.Write(Encoding.UTF8.GetBytes(ZPL.ToString()));
//            }
//            catch (ZebraException)
//            {
//                // Rethrow any ZebraExceptions that occur during printing.
//                throw;
//            }
//        }

//        /// <summary>
//        /// Checks the operational status of the specified Zebra printer.
//        /// </summary>
//        /// <param name="zebraPrinter">The ZebraPrinter instance to check the status of.</param>
//        /// <returns>
//        /// A tuple containing a boolean and a string. The boolean indicates whether the printer
//        /// is ready to print (true) or not (false), and the string provides a message describing
//        /// the current state or any issues detected.
//        /// </returns>
//        /// <exception cref="ArgumentNullException">Thrown when a null ZebraPrinter instance is passed.</exception>
//        /// <remarks>
//        /// This method should be used to verify the printer's readiness before attempting a print operation.
//        /// It checks for various statuses such as whether the printer is paused, the head is open, paper or
//        /// ribbon is out, among other conditions. If the printer is ready to print, the method returns true with
//        /// a "Ready to Print" message. Otherwise, it returns false with a descriptive message of the issue.
//        /// </remarks>
//        public (bool IsReady, string Message) CheckStatus(ZebraPrinter zebraPrinter)
//        {
//            if (zebraPrinter == null)
//                throw new ArgumentNullException(nameof(zebraPrinter));

//            PrinterStatus printerStatus = zebraPrinter.GetCurrentStatus();

//            // Assuming that we only proceed if the printer is ready to print.
//            if (printerStatus.isReadyToPrint)
//            {
//                return (true, "Ready to Print");
//            }

//            string errorMessage = printerStatus.isPaused ? "Cannot Print because the printer is paused."
//                : printerStatus.isHeadOpen ? "Cannot Print because the printer head is open."
//                : printerStatus.isPaperOut ? "Cannot Print because the paper is out."
//                : printerStatus.isRibbonOut ? "Cannot Print because the ribbon is out."
//                : printerStatus.isReceiveBufferFull ? "Cannot Print because the receive buffer is full."
//                : printerStatus.isPartialFormatInProgress ? "Cannot Print because a partial format is in progress."
//                : printerStatus.isHeadCold ? "Cannot Print because the print head is cold."
//                : printerStatus.isHeadTooHot ? "Cannot Print because the print head is too hot."
//                : printerStatus.numberOfFormatsInReceiveBuffer > 0 ? $"Cannot Print because there are {printerStatus.numberOfFormatsInReceiveBuffer} formats in the receive buffer."
//                : printerStatus.labelsRemainingInBatch > 0 ? $"Cannot Print because there are {printerStatus.labelsRemainingInBatch} labels remaining in the batch."
//                : printerStatus.labelLengthInDots > 0 ? $"The label length is set to {printerStatus.labelLengthInDots} dots."
//                : "Cannot Print. Unknown error.";

//            // Return a tuple indicating the printer is not ready and providing the corresponding message.
//            return (false, errorMessage);
//        }
//    }

//    /// <summary>
//    /// Represents exceptions that occur during Zebra printer operations. This custom exception class
//    /// is designed to provide detailed information about errors specifically related to Zebra printers,
//    /// such as connection issues, print job failures, or configuration errors.
//    /// </summary>
//    public class ZebraException : Exception
//    {
//        /// <summary>
//        /// Initializes a new instance of the ZebraException class.
//        /// </summary>
//        public ZebraException() { }

//        /// <summary>
//        /// Initializes a new instance of the ZebraException class with a specified error message.
//        /// </summary>
//        /// <param name="message">The message that describes the error.</param>
//        public ZebraException(string message)
//            : base(message) { }

//        /// <summary>
//        /// Initializes a new instance of the ZebraException class with a specified error message and a reference
//        /// to the inner exception that is the cause of this exception.
//        /// </summary>
//        /// <param name="message">The error message that explains the reason for the exception.</param>
//        /// <param name="inner">The exception that is the cause of the current exception, or a null reference
//        /// if no inner exception is specified.</param>
//        public ZebraException(string message, Exception inner)
//            : base(message, inner) { }
//    }
//}







using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <summary>
        /// Attempts to connect to the first discovered Zebra USB printer.
        /// </summary>
        /// <returns>A ZebraPrinter instance if successful, null otherwise.</returns>
        /// <exception cref="ZebraException">Thrown when there is a problem establishing a connection with the printer.</exception>
        public ZebraPrinter Connect()
        {
            DiscoveredPrinter discoveredPrinter = UsbDiscoverer.GetZebraUsbPrinters().FirstOrDefault();
            if (discoveredPrinter == null)
                throw new ZebraException("No Zebra USB printers were discovered.");

            try
            {
                Connection connection = discoveredPrinter.GetConnection();
                connection.Open();
                return ZebraPrinterFactory.GetInstance(PrinterLanguage.ZPL, connection);
            }
            catch (ConnectionException ex)
            {
                throw new ZebraException("Failed to connect to Zebra printer.", ex);
            }
        }

        /// <summary>
        /// Closes the connection to a Zebra printer.
        /// </summary>
        /// <param name="printer">The ZebraPrinter instance to disconnect from.</param>
        /// <exception cref="ZebraException">Thrown if an error occurs while closing the connection.</exception>
        public void Disconnect(ZebraPrinter printer)
        {
            if (printer?.Connection == null) return;

            try
            {
                printer.Connection.Close();
            }
            catch (ConnectionException ex)
            {
                throw new ZebraException("Failed to disconnect from Zebra printer.", ex);
            }
        }

        /// <summary>
        /// Prints Louver IDs on a Zebra printer using ZPL commands.
        /// </summary>
        /// <param name="printer">The ZebraPrinter instance to use for printing.</param>
        /// <param name="louverSet">A list of double values representing Louver IDs to print.</param>
        /// <exception cref="PrinterException">Thrown if there is an error during the print operation.</exception>
        public void PrintLouverIDs(ZebraPrinter printer, List<double> louverSet)
        {
            if (!CheckStatus(printer).IsReady)
                return;

            //string zplData = GenerateZplForLouverIDs(louverSet);
            //Print(printer, zplData);
        }

        /// <summary>
        /// Generates ZPL (Zebra Programming Language) commands for printing Louver IDs.
        /// </summary>
        /// <param name="louverSet">A list of double values representing Louver IDs.</param>
        /// <returns>A string containing the ZPL commands for printing Louver IDs.</returns>
        //private string GenerateZplForLouverIDs(List<double> louverSet)
        //{
        //    var zplBuilder = new StringBuilder();
        //    for (int i = 0; i < louverSet.Count; i++)
        //    {
        //        if (i % 2 == 0) zplBuilder.Append("^XA");

        //        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},40^A0N,40,40^FDLouver ID:^FS");
        //        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 120 : 360)},120^A0N,40,40^FD{louverSet[i]}^FS");

        //        if (i % 2 == 1 || i == louverSet.Count - 1) zplBuilder.Append("^XZ");

        //        // Send ZPL to printer in batches or individually
        //        if (zplBuilder.Length > 0 && (i % 2 == 1 || i == louverSet.Count - 1))
        //        {
        //            return zplBuilder.ToString();
        //            zplBuilder.Clear();
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Sends a Zebra Programming Language (ZPL) string to the specified Zebra printer for printing.
        /// </summary>
        /// <param name="printer">The ZebraPrinter instance to which the ZPL will be sent.</param>
        /// <param name="ZPL">The ZPL command string that defines what will be printed.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided printer argument is null.</exception>
        /// <exception cref="ZebraException">
        /// Thrown if the printer is not ready to print. The exception message contains details about the printer's status.
        /// This exception is also thrown for any errors that occur during the writing process to the printer connection.
        /// </exception>
        /// <remarks>
        /// Before attempting to print, the method checks the status of the printer using the CheckStatus method.
        /// If the printer is not ready (e.g., if it's paused, out of paper, etc.), the method will throw a ZebraException
        /// with an appropriate message indicating the reason.
        /// If the printer is ready, the method sends the ZPL command to the printer for execution.
        /// </remarks>
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
        /// A tuple containing a boolean and a string. The boolean indicates whether the printer
        /// is ready to print (true) or not (false), and the string provides a message describing
        /// the current state or any issues detected.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when a null ZebraPrinter instance is passed.</exception>
        /// <remarks>
        /// This method should be used to verify the printer's readiness before attempting a print operation.
        /// It checks for various statuses such as whether the printer is paused, the head is open, paper or
        /// ribbon is out, among other conditions. If the printer is ready to print, the method returns true with
        /// a "Ready to Print" message
        /// Otherwise, it returns false with a descriptive message of the issue.
        /// </remarks>
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
            // This method returns a specific error message based on the printer's status.
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
/// This custom exception class is designed to provide detailed information 
/// about errors specifically related to Zebra printers, such as connection issues, 
/// print job failures, or configuration errors.
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