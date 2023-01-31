using System.Globalization;
using System.Xml;
using System;

namespace Reconciliation
{
    internal class ItemPriceRepository
    {
        private String filePath = Environment.GetEnvironmentVariable("FILESPATH") + "\\input\\Prices.xml";
        private List<ItemPrice> prices = new List<ItemPrice>();

        public ItemPriceRepository()
        {
            getPricesFile();
        }

        private void getPricesFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode itemPriceNodelist = doc.DocumentElement.SelectSingleNode("/ItemPricesRoot/ItemPricesList");

            foreach (XmlNode itemPriceNode in itemPriceNodelist)
            {

                String item = itemPriceNode.ChildNodes[0].InnerText;
                String price = itemPriceNode.ChildNodes[1].InnerText;

                ItemPrice itemPrice = new ItemPrice
                {
                    ItemId = item,
                    Price = Decimal.Parse(price, CultureInfo.InvariantCulture),
                };

                prices.Add(itemPrice);

            }
        }


        internal Decimal GetPriceByItemId(string ItemId)
        {
            return prices.Find(item => item.ItemId == ItemId).Price;
        }
    }
}
