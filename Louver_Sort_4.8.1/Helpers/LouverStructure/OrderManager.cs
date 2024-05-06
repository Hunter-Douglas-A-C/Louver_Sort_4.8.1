using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Manages orders and associated operations.
    /// </summary>
    [Serializable]
    public class OrderManager
    {
        // Fields
        private List<OrderWithBarcode> _ordersWithBarcodes = new List<OrderWithBarcode>();

        [JsonProperty("ordersWithBarcodes")]
        public List<OrderWithBarcode> OrdersWithBarcodes { get => _ordersWithBarcodes; set => _ordersWithBarcodes = value; }

        #region Orders

        /// <summary>
        /// Creates a new order or retrieves an existing one based on the provided barcode.
        /// </summary>
        /// <param name="b">The barcode set associated with the order.</param>
        /// <returns>A tuple containing the order and a boolean indicating if it's a new order.</returns>
        public (Order, bool) CreateOrder(BarcodeSet b)
        {
            var testBarcodeHelper = new BarcodeHelper(b);

            // Search for an existing order with matching attributes
            foreach (var orderWithBarcode in OrdersWithBarcodes)
            {
                var existingOrder = orderWithBarcode.Order;
                var existingBarcodeHelper = existingOrder.BarcodeHelper;

                // Compare based on defined "equality" criteria
                if (existingBarcodeHelper.Line == testBarcodeHelper.Line &&
                    existingBarcodeHelper.Style == testBarcodeHelper.Style &&
                    existingBarcodeHelper.Width == testBarcodeHelper.Width &&
                    existingBarcodeHelper.Length == testBarcodeHelper.Length &&
                    existingBarcodeHelper.PanelID == testBarcodeHelper.PanelID &&
                    existingBarcodeHelper.Set == testBarcodeHelper.Set)
                {
                    // Return existing order if it matches the new order's criteria
                    return (existingOrder, false);
                }
            }

            // If no existing order is found, create and add a new order
            var newOrder = new Order(b);
            OrdersWithBarcodes.Add(new OrderWithBarcode(newOrder));

            // Return the new order along with an indicator that it's new
            return (newOrder, true);
        }

        #endregion

        #region Get specific w/ IDs

        /// <summary>
        /// Retrieves an opening from an order by its line number and barcode set.
        /// </summary>
        /// <param name="line">The line number of the opening.</param>
        /// <param name="barcodeSet">The barcode set associated with the order.</param>
        /// <returns>The opening with the specified line number.</returns>
        public Opening GetOpening(int line, BarcodeSet barcodeSet)
        {
            var orderWithBarcode = OrdersWithBarcodes.FirstOrDefault(ob => ob.BarcodeSet.Equals(barcodeSet));
            if (orderWithBarcode != null)
            {
                return orderWithBarcode.Order.Openings.FirstOrDefault(opening => opening.Line == line);
            }

            return null; // or handle the case where the order is not found
        }

        /// <summary>
        /// Retrieves a panel from an opening by its panel ID and line number, associated with the specified barcode set.
        /// </summary>
        /// <param name="panelId">The ID of the panel.</param>
        /// <param name="openingLine">The line number of the opening.</param>
        /// <param name="barcodeSet">The barcode set associated with the order.</param>
        /// <returns>The panel with the specified ID.</returns>
        public Panel GetPanel(double panelId, double openingLine, BarcodeSet barcodeSet)
        {
            var orderWithBarcode = OrdersWithBarcodes.FirstOrDefault(ob => ob.BarcodeSet.Equals(barcodeSet));
            if (orderWithBarcode != null)
            {
                Opening opening = FindOpeningInOrder(orderWithBarcode.Order, openingLine);
                if (opening != null && opening.Panels != null)
                {
                    return opening.Panels.FirstOrDefault(panel => panel.ID == panelId);
                }
            }

            return null; // or handle the case where the panel is not found
        }

        /// <summary>
        /// Retrieves a set from a panel by its set ID, associated with the specified barcode set.
        /// </summary>
        /// <param name="setId">The ID of the set.</param>
        /// <param name="panelId">The ID of the panel.</param>
        /// <param name="openingLine">The line number of the opening.</param>
        /// <param name="barcodeSet">The barcode set associated with the order.</param>
        /// <returns>The set with the specified ID.</returns>
        public Set GetSet(SetID.SetId setId, double panelId, double openingLine, BarcodeSet barcodeSet)
        {
            var orderWithBarcode = OrdersWithBarcodes.FirstOrDefault(ob => ob.BarcodeSet.Equals(barcodeSet));
            if (orderWithBarcode != null)
            {
                Opening opening = FindOpeningInOrder(orderWithBarcode.Order, openingLine);
                if (opening != null)
                {
                    Panel panel = FindPanelInOpening(opening, panelId);
                    if (panel != null)
                    {
                        return panel.Sets.FirstOrDefault(set => set.ID == setId);
                    }
                }
            }

            return null;
        }

        #endregion

        #region Find one level down w/ ID

        /// <summary>
        /// Finds an opening in an order by its line number.
        /// </summary>
        /// <param name="order">The order to search in.</param>
        /// <param name="line">The line number of the opening.</param>
        /// <returns>The opening with the specified line number.</returns>
        public Opening FindOpeningInOrder(Order order, double line)
        {
            if (order != null && order.Openings != null)
            {
                return order.Openings.FirstOrDefault(opening => opening.Line == line);
            }

            return null;
        }

        /// <summary>
        /// Finds a panel in an opening by its ID.
        /// </summary>
        /// <param name="open">The opening to search in.</param>
        /// <param name="id">The ID of the panel.</param>
        /// <returns>The panel with the specified ID.</returns>
        public Panel FindPanelInOpening(Opening open, double id)
        {
            if (open != null && open.Panels != null)
            {
                return open.Panels.FirstOrDefault(panel => panel.ID == id);
            }

            return null;
        }

        /// <summary>
        /// Finds a set in a panel by its ID.
        /// </summary>
        /// <param name="panel">The panel to search in.</param>
        /// <param name="id">The ID of the set.</param>
        /// <returns>The set with the specified ID.</returns>
        public Set FindSetInPanel(Panel panel, SetID.SetId id)
        {
            if (panel != null && panel.Sets != null)
            {
                return panel.Sets.FirstOrDefault(set => set.ID == id);
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Checks if an order exists based on the provided barcode set.
        /// </summary>
        /// <param name="barcodeSet">The barcode set to check.</param>
        /// <returns>The existing order if found; otherwise, null.</returns>
        public Order CheckIfOrderExists(BarcodeSet barcodeSet)
        {
            foreach (var item in OrdersWithBarcodes)
            {
                if (item.BarcodeSet.Barcode1 == barcodeSet.Barcode1 && item.BarcodeSet.Barcode2 == barcodeSet.Barcode2)
                {
                    return item.Order;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates an order after scanning and fills all associated variables.
        /// </summary>
        /// <param name="barcodeSet">The barcode set associated with the order.</param>
        /// <param name="louverCount">The number of louvers to add to the order.</param>
        /// <returns>The created order.</returns>
        public Order CreateOrderAfterScanAndFillAllVariables(BarcodeSet barcodeSet, int louverCount)
        {
            var (order, isNewOrder) = CreateOrder(barcodeSet);
            if (!isNewOrder)
            {
                Debug.WriteLine("An existing order matches the criteria. Skipping the rest of the function.");
                return order; // Skip the rest of the function if an existing order was found.
            }
            else
            {
                var open = order.AddOpening(new Opening(order.BarcodeHelper.Line, order.BarcodeHelper.Style, order.BarcodeHelper.Width, order.BarcodeHelper.Length));
                var pan = open.AddPanel(new Panel(order.BarcodeHelper.PanelID));
                var set = pan.AddSet(new Set(order.BarcodeHelper.Set, louverCount));
                for (int i = 1; i < louverCount + 1; i++)
                {
                    var l = set.AddLouver(new Louver(i));
                    //l.SetReading1(i);
                    //l.SetReading2(i * (i / 1000));
                    //l.CalcValues();
                }

                return order;
            }
        }

        /// <summary>
        /// Retrieves the order associated with the provided barcode set.
        /// </summary>
        /// <param name="barcodeSet">The barcode set associated with the order.</param>
        /// <returns>The order if found; otherwise, null.</returns>
        public Order GetOrder(BarcodeSet barcodeSet)
        {
            foreach (var orderWithBarcode in OrdersWithBarcodes)
            {
                if (orderWithBarcode.BarcodeSet.Equals(barcodeSet))
                {
                    return orderWithBarcode.Order;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves all orders from the specified list of order-with-barcode pairs.
        /// </summary>
        /// <param name="ordersWithBarcodes">The list of order-with-barcode pairs.</param>
        /// <returns>An enumerable collection of orders.</returns>
        public IEnumerable<Order> GetAllOrders(List<OrderWithBarcode> ordersWithBarcodes)
        {
            foreach (var orderWithBarcode in ordersWithBarcodes)
            {
                yield return orderWithBarcode.Order;
            }
        }
    }
}
