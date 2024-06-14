//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using Zebra.Sdk.Comm;
//using Zebra.Sdk.Printer;
//using Zebra.Sdk.Printer.Discovery;
//using System.Threading;


//namespace Louver_Sort_4._8._1.Helpers
//{
//    /// <summary>
//    /// Provides functionalities for connecting to, disconnecting from, and printing to Zebra printers.
//    /// </summary>
//    internal class ZebraPrinterHelper
//    {
//        private string _ZPLSpacerLabel = "^XA  \r\n^FD  \r\n^XZ";

//        /// <summary>
//        /// Attempts to connect to the first discovered Zebra USB printer.
//        /// </summary>
//        /// <returns>A ZebraPrinter instance if successful, null otherwise.</returns>
//        /// <exception cref="ZebraException">Thrown when there is a problem establishing a connection with the printer.</exception>
//        public ZebraPrinter Connect()
//        {
//            DiscoveredPrinter discoveredPrinter = UsbDiscoverer.GetZebraUsbPrinters().FirstOrDefault();
//            if (discoveredPrinter == null)
//                throw new ZebraException("No Zebra USB printers were discovered.");

//            try
//            {
//                Connection connection = discoveredPrinter.GetConnection();
//                connection.Open();
//                return ZebraPrinterFactory.GetInstance(PrinterLanguage.ZPL, connection);
//            }
//            catch (ConnectionException ex)
//            {
//                throw new ZebraException("Failed to connect to Zebra printer.", ex);
//            }
//        }

//        /// <summary>
//        /// Closes the connection to a Zebra printer.
//        /// </summary>
//        /// <param name="printer">The ZebraPrinter instance to disconnect from.</param>
//        /// <exception cref="ZebraException">Thrown if an error occurs while closing the connection.</exception>
//        public void Disconnect(ZebraPrinter printer)
//        {
//            if (printer?.Connection == null) return;

//            try
//            {
//                printer.Connection.Close();
//            }
//            catch (ConnectionException ex)
//            {
//                throw new ZebraException("Failed to disconnect from Zebra printer.", ex);
//            }
//        }

//        /// <summary>
//        /// Prints Louver IDs on a Zebra printer using ZPL commands.
//        /// </summary>
//        /// <param name="printer">The ZebraPrinter instance to use for printing.</param>
//        /// <param name="louverSet">A list of double values representing Louver IDs to print.</param>
//        /// <exception cref="PrinterException">Thrown if there is an error during the print operation.</exception>
//        public void PrintLouverIDs(ZebraPrinter printer, List<Louver_Sort_4._8._1.Helpers.LouverStructure.Louver> louvers)
//        {
//            if (!CheckStatus(printer).IsReady)
//                return;

//            StringBuilder zplBuilder = new StringBuilder();
//            for (int i = 0; i < louvers.Count; i++)
//            {
//                if (i % 2 == 0) zplBuilder.Append("^XA");

//                zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},80^A0N,40,40^FDUnsorted:^FS");
//                zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 180 : 420)},80^A0N,40,40^FD{louvers[i].ID}^FS");
//                if (i % 2 == 1 || i == louvers.Count - 1) zplBuilder.Append("^XZ");
//            }
//            Print(printer, zplBuilder.ToString());
//            Thread.Sleep(500);
//        }


//        public void PrintSortedLouverIDs(ZebraPrinter printer, List<Louver_Sort_4._8._1.Helpers.LouverStructure.Louver> louvers)
//        {
//            if (!CheckStatus(printer).IsReady)
//                return;

//            StringBuilder zplBuilder = new StringBuilder();
//            for (int i = 0; i < louvers.Count; i++)
//            {
//                if (i % 2 == 0) zplBuilder.Append("^XA");



//                if (louvers[i].Orientation)
//                {
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},60^A0N,40,40^FDUnsorted:^FS");
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 180 : 420)},60^A0N,40,40^FD{louvers[i].ID}^FS");
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},100^A0N,40,40^FDSorted:^FS");
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 160 : 380)},100^A0N,40,40^FD{louvers[i].SortedID}^FS");
//                }
//                else
//                {
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},60^A0N,40,40^FDUnsorted:^FS");
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 180 : 420)},60^A0N,40,40^FD{louvers[i].ID}^FS");
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},100^A0N,40,40^FDSorted:^FS");
//                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 160 : 380)},100^A0N,40,40^FD{louvers[i].SortedID} F^FS");
//                }


//                if (i % 2 == 1 || i == louvers.Count - 1) zplBuilder.Append("^XZ");
//            }
//            Print(printer, zplBuilder.ToString());
//            Thread.Sleep(500);
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

//            var (isReady, statusMessage) = CheckStatus(printer);
//            if (!isReady)
//            {
//                throw new ZebraException(statusMessage);
//            }

//            try
//            {
//                printer.Connection.Write(Encoding.UTF8.GetBytes(ZPL));
//            }
//            catch (ConnectionException ex)
//            {
//                throw new ZebraException("Failed to print.", ex);
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
//        /// a "Ready to Print" message
//        /// Otherwise, it returns false with a descriptive message of the issue.
//        /// </remarks>
//        public (bool IsReady, string Message) CheckStatus(ZebraPrinter zebraPrinter)
//        {
//            if (zebraPrinter == null)
//                throw new ArgumentNullException(nameof(zebraPrinter));

//            try
//            {
//                PrinterStatus printerStatus = zebraPrinter.GetCurrentStatus();

//                if (printerStatus.isReadyToPrint)
//                {
//                    return (true, "Ready to Print");
//                }
//                else
//                {
//                    string errorMessage = DeterminePrinterErrorMessage(printerStatus);
//                    return (false, errorMessage);
//                }
//            }
//            catch (ConnectionException ex)
//            {
//                throw new ZebraException("Failed to check printer status.", ex);
//            }
//        }

//        /// <summary>
//        /// Determines the error message based on the printer's status.
//        /// </summary>
//        /// <param name="printerStatus">The current status of the printer.</param>
//        /// <returns>A string containing the error message based on the printer's status.</returns>
//        private string DeterminePrinterErrorMessage(PrinterStatus printerStatus)
//        {
//            // This method returns a specific error message based on the printer's status.
//            if (printerStatus.isPaused) return "Printer is paused.";
//            if (printerStatus.isHeadOpen) return "Printer head is open.";
//            if (printerStatus.isPaperOut) return "Printer is out of paper.";
//            if (printerStatus.isRibbonOut) return "Printer is out of ribbon.";
//            if (printerStatus.isReceiveBufferFull) return "Receive buffer is full.";
//            if (printerStatus.isPartialFormatInProgress) return "Partial format is in progress.";
//            if (printerStatus.isHeadCold) return "Printer head is cold.";
//            if (printerStatus.isHeadTooHot) return "Printer head is too hot.";
//            if (printerStatus.numberOfFormatsInReceiveBuffer > 0) return $"There are {printerStatus.numberOfFormatsInReceiveBuffer} formats in the receive buffer.";
//            if (printerStatus.labelsRemainingInBatch > 0) return $"There are {printerStatus.labelsRemainingInBatch} labels remaining in the batch.";
//            if (printerStatus.labelLengthInDots > 0) return $"Label length is set to {printerStatus.labelLengthInDots} dots.";

//            return "Printer is in an unknown state.";
//        }
//    }

//    /// <summary>
//    /// Represents exceptions that occur during Zebra printer operations. 
//    /// This custom exception class is designed to provide detailed information 
//    /// about errors specifically related to Zebra printers, such as connection issues, 
//    /// print job failures, or configuration errors.
//    /// </summary>
//    public class ZebraException : Exception
//    {
//        /// <summary>
//        /// Initializes a new instance of the ZebraException class with a specified error message.
//        /// </summary>
//        /// <param name="message">The message that describes the error.</param>
//        public ZebraException(string message) : base(message) { }

//        /// <summary>
//        /// Initializes a new instance of the ZebraException class with a specified error message 
//        /// and a reference to the inner exception that is the cause of this exception.
//        /// </summary>
//        /// <param name="message">The error message that explains the reason for the exception.</param>
//        /// <param name="inner">The exception that is the cause of the current exception, 
//        /// or a null reference if no inner exception is specified.</param>
//        public ZebraException(string message, Exception inner) : base(message, inner) { }
//    }
//}


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;
using System.Threading;
using System.IO;

namespace Louver_Sort_4._8._1.Helpers
{
    /// <summary>
    /// Provides functionalities for connecting to, disconnecting from, and printing to Zebra printers.
    /// </summary>
    internal class ZebraPrinterHelper
    {
        private string _ZPLSpacerLabel = "^XA  \r\n^FD  \r\n^XZ";

        /// <summary>
        /// Attempts to connect to the first discovered Zebra USB printer.
        /// </summary>
        /// <returns>A ZebraPrinter instance if successful, null otherwise.</returns>
        /// <exception cref="ZebraException">Thrown when there is a problem establishing a connection with the printer.</exception>
        public ZebraPrinter Connect()
        {
            try
            {
                DiscoveredPrinter discoveredPrinter = UsbDiscoverer.GetZebraUsbPrinters().FirstOrDefault();
                if (discoveredPrinter == null)
                {
                    throw new ZebraException("No Zebra USB printers were discovered.");
                }

                Connection connection = discoveredPrinter.GetConnection();
                connection.Open();
                return ZebraPrinterFactory.GetInstance(PrinterLanguage.ZPL, connection);
            }
            catch (ConnectionException ex)
            {
                throw new ZebraException("Failed to connect to Zebra printer.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ZebraException("Unauthorized access during printer connection.", ex);
            }
            catch (IOException ex)
            {
                throw new ZebraException("I/O error during printer connection.", ex);
            }
            catch (Exception ex)
            {
                throw new ZebraException("An unexpected error occurred during printer connection.", ex);
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
            catch (UnauthorizedAccessException ex)
            {
                throw new ZebraException("Unauthorized access during printer disconnection.", ex);
            }
            catch (IOException ex)
            {
                throw new ZebraException("I/O error during printer disconnection.", ex);
            }
            catch (Exception ex)
            {
                throw new ZebraException("An unexpected error occurred during printer disconnection.", ex);
            }
        }

        /// <summary>
        /// Prints Louver IDs on a Zebra printer using ZPL commands.
        /// </summary>
        /// <param name="printer">The ZebraPrinter instance to use for printing.</param>
        /// <param name="louvers">A list of louver structures to print.</param>
        /// <exception cref="PrinterException">Thrown if there is an error during the print operation.</exception>
        public void PrintLouverIDs(ZebraPrinter printer, List<Louver_Sort_4._8._1.Helpers.LouverStructure.Louver> louvers)
        {
            try
            {
                if (!CheckStatus(printer).IsReady)
                    throw new ZebraException("Printer is not ready.");

                StringBuilder zplBuilder = new StringBuilder();
                for (int i = 0; i < louvers.Count; i++)
                {
                    if (i % 2 == 0) zplBuilder.Append("^XA");

                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},80^A0N,40,40^FDUnsorted:^FS");
                    zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 180 : 420)},80^A0N,40,40^FD{louvers[i].ID}^FS");
                    if (i % 2 == 1 || i == louvers.Count - 1) zplBuilder.Append("^XZ");
                }
                Print(printer, zplBuilder.ToString());
                Thread.Sleep(500);
            }
            catch (ArgumentNullException ex)
            {
                throw new ZebraException("Printer is not specified.", ex);
            }
            catch (ZebraException ex)
            {
                throw new ZebraException("Failed to print louver IDs.", ex);
            }
            catch (Exception ex)
            {
                throw new ZebraException("An unexpected error occurred while printing louver IDs.", ex);
            }
        }

        public void PrintSortedLouverIDs(ZebraPrinter printer, List<Louver_Sort_4._8._1.Helpers.LouverStructure.Louver> louvers)
        {
            try
            {
                if (!CheckStatus(printer).IsReady)
                    throw new ZebraException("Printer is not ready.");

                StringBuilder zplBuilder = new StringBuilder();
                for (int i = 0; i < louvers.Count; i++)
                {
                    if (i % 2 == 0) zplBuilder.Append("^XA");

                    if (louvers[i].Orientation)
                    {
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},60^A0N,40,40^FDUnsorted:^FS");
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 180 : 420)},60^A0N,40,40^FD{louvers[i].ID}^FS");
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},100^A0N,40,40^FDSorted:^FS");
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 160 : 380)},100^A0N,40,40^FD{louvers[i].SortedID}^FS");
                    }
                    else
                    {
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},60^A0N,40,40^FDUnsorted:^FS");
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 180 : 420)},60^A0N,40,40^FD{louvers[i].ID}^FS");
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 20 : 260)},100^A0N,40,40^FDSorted:^FS");
                        zplBuilder.AppendLine($"^FO{(i % 2 == 0 ? 160 : 380)},100^A0N,40,40^FD{louvers[i].SortedID} F^FS");
                    }

                    if (i % 2 == 1 || i == louvers.Count - 1) zplBuilder.Append("^XZ");
                }
                Print(printer, zplBuilder.ToString());
                Thread.Sleep(500);
            }
            catch (ArgumentNullException ex)
            {
                throw new ZebraException("Printer is not specified.", ex);
            }
            catch (ZebraException ex)
            {
                throw new ZebraException("Failed to print sorted louver IDs.", ex);
            }
            catch (Exception ex)
            {
                throw new ZebraException("An unexpected error occurred while printing sorted louver IDs.", ex);
            }
        }

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
            catch (IOException ex)
            {
                throw new ZebraException("I/O error during printing.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ZebraException("Unauthorized access during printing.", ex);
            }
            catch (Exception ex)
            {
                throw new ZebraException("An unexpected error occurred during printing.", ex);
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
            catch (IOException ex)
            {
                throw new ZebraException("I/O error while checking printer status.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ZebraException("Unauthorized access while checking printer status.", ex);
            }
            catch (Exception ex)
            {
                throw new ZebraException("An unexpected error occurred while checking printer status.", ex);
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
