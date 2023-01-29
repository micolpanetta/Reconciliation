using System.Globalization;
using System.Xml;
using System;

namespace Reconciliation
{
    internal class ItemPriceRepository
    {
        private String filePath = Environment.GetEnvironmentVariable("FILESPATH") + "\\Prices.xml";
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
            //prices.ForEach(Console.WriteLine);
            //Console.WriteLine(prices.ElementAt(0).Price + 1);
        }


        internal Decimal getPriceByItemId(string itemId)
        {
            return prices.Find(item => item.ItemId == itemId).Price;
        }
    }
}
