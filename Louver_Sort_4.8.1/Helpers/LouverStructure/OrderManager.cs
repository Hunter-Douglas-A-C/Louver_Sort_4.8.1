using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static Louver_Sort_4._8._1.Helpers.LouverStructure.SetID;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class OrderManager
    {
        public Dictionary<BarcodeSet, Order> ordersByBarcode = new Dictionary<BarcodeSet, Order>();


        #region Orders

        public Order GetOrCreateOrder(BarcodeSet b)
        {
            // Check if the order already exists.
            if (ordersByBarcode.TryGetValue(b, out var existingOrder))
            {
                // Return the existing order.
                return existingOrder;
            }

            // If not, create a new order and add to the dictionary.
            var newOrder = new Order(b);
            ordersByBarcode[b] = newOrder;

            return newOrder;
        }


        #endregion


        #region Get all of class with barcodeset


        public IEnumerable<Opening> GetAllOpenings(BarcodeSet barcodeSet)
        {
            var order = GetOrCreateOrder(barcodeSet);
            return order.Openings;
        }

        public IEnumerable<Panel> GetAllPanels(BarcodeSet barcodeSet)
        {
            var order = GetOrCreateOrder(barcodeSet);
            return order.Openings.SelectMany(opening => opening.Panels);
        }

        public IEnumerable<Set> GetAllSets(BarcodeSet barcodeSet)
        {
            var order = GetOrCreateOrder(barcodeSet);
            return order.Openings.SelectMany(opening => opening.Panels)
                                 .SelectMany(panel => panel.Sets);
        }

        public IEnumerable<Louver> GetAllLouvers(BarcodeSet barcodeSet)
        {
            var order = GetOrCreateOrder(barcodeSet);
            return order.Openings.SelectMany(opening => opening.Panels)
                                 .SelectMany(panel => panel.Sets)
                                 .SelectMany(set => set.Louvers);
        }



        #endregion

        #region Add to order

        public void AddOpening(BarcodeSet barcodeSet, Opening o)
        {
            var order = GetOrCreateOrder(barcodeSet);
            order.AddOpening(o);
        }

        public void AddPanel(BarcodeSet barcodeSet, Panel p)
        {
            var order = GetOrCreateOrder(barcodeSet);
            order.Openings[1].AddPanel(p);
        }

        public void AddSet(BarcodeSet barcodeSet, Set s)
        {
            var order = GetOrCreateOrder(barcodeSet);
            order.Openings[1].Panels[1].AddSet(s);
        }

        public void AddLouver(BarcodeSet barcodeSet, Louver l)
        {
            var order = GetOrCreateOrder(barcodeSet);
            order.Openings[1].Panels[1].Sets[1].AddLouver(l);
        }

        public void AddLouvers(BarcodeSet barcodeSet, List<Louver> l)
        {
            var order = GetOrCreateOrder(barcodeSet);
            order.Openings[1].Panels[1].Sets[1].AddLouvers(l);
        }

        #endregion

        #region Get specific w/ IDs

        public Opening GetOpening(double line, BarcodeSet barcodeSet)
        {

            // Find the order using the barcode key
            if (ordersByBarcode.TryGetValue(barcodeSet, out Order order))
            {
                // Look for the opening with the matching line number within this order
                return order.Openings.FirstOrDefault(opening => opening.Line == line);
            }

            // If the order doesn't exist or no opening with the given line is found
            return null;
        }


        public Panel GetPanel(string PanelID, double Openingline, BarcodeSet barcodeSet)
        {

            // Find the order using the barcode key
            if (ordersByBarcode.TryGetValue(barcodeSet, out Order order))
            {
                Opening o = FindOpeningInOrder(order, Openingline);
                // Check if the order is not null and has openings
                if (o != null && o.Panels != null)
                {
                    // Use LINQ to find the opening with the matching ID
                    return o.Panels.FirstOrDefault(panel => panel.ID == PanelID);
                }
            }

            // If the order doesn't exist or no opening with the given line is found
            return null;
        }


        public Set GetSet(SetID.SetId SetID, string PanelID, double OpeningLine, BarcodeSet barcodeSet)
        {

            // Find the order using the barcode key
            if (ordersByBarcode.TryGetValue(barcodeSet, out Order order))
            {
                Opening o = FindOpeningInOrder(order, OpeningLine);
                Panel p = FindPanelInOpening(o, PanelID);
                // Check if the order is not null and has openings
                if (o != null && o.Panels != null)
                {
                    if (p != null && p.Sets != null)
                    {
                        // Use LINQ to find the opening with the matching ID
                        return p.Sets.FirstOrDefault(set => set.ID == SetID);
                    }
                }
            }

            // If the order doesn't exist or no opening with the given line is found
            return null;
        }
        #endregion

        #region Find one level down w/ ID

        public Opening FindOpeningInOrder(Order order, double line)
        {
            // Check if the order is not null and has openings
            if (order != null && order.Openings != null)
            {
                // Use LINQ to find the opening with the matching ID
                return order.Openings.FirstOrDefault(opening => opening.Line == line);
            }

            // Return null if no opening with the given ID is found or if the order is null
            return null;
        }

        public Panel FindPanelInOpening(Opening open, string ID)
        {
            // Check if the order is not null and has openings
            if (open != null && open.Panels != null)
            {
                // Use LINQ to find the opening with the matching ID
                return open.Panels.FirstOrDefault(panel => panel.ID == ID);
            }

            // Return null if no opening with the given ID is found or if the order is null
            return null;
        }

        public Set FindSetInPanel(Panel panel, SetID.SetId ID)
        {
            // Check if the order is not null and has openings
            if (panel != null && panel.Sets != null)
            {
                // Use LINQ to find the opening with the matching ID
                return panel.Sets.FirstOrDefault(set => set.ID == ID);
            }

            // Return null if no opening with the given ID is found or if the order is null
            return null;
        }

        #endregion

        public Order CreateOrderAfterScanAndFillAllVariables(BarcodeSet barcodeSet, int LouverCount)
        {
            var order = GetOrCreateOrder(barcodeSet);
            order.AddOpening(new Opening(order.BarcodeHelper.Line, Opening.LouverModels.MSL01, order.BarcodeHelper.Style, order.BarcodeHelper.Width, order.BarcodeHelper.Length));
            GetOpening(order.BarcodeHelper.Line, barcodeSet).AddPanel(new Panel(order.BarcodeHelper.PanelID));
            GetPanel(order.BarcodeHelper.PanelID, order.BarcodeHelper.Line, barcodeSet).AddSet(new Set(order.BarcodeHelper.Set, LouverCount));
            for (int i = 1; i < LouverCount+1; i++)
            {
                GetSet(order.BarcodeHelper.Set, order.BarcodeHelper.PanelID, order.BarcodeHelper.Line, barcodeSet).AddLouver(new Louver(i));
            }

            return order;
        }


















































    }
}
