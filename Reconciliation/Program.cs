using System.Globalization;
using System.Xml;

namespace Reconciliation
{
    internal class Program
    {
        static readonly string rootFolder = "C:\\Temp\\Files";
        static readonly string paymentsFile = "C:\\Temp\\Files\\Payments.json";
        static readonly string pricesFile = "C:\\Temp\\Files\\Prices.xml";
        static readonly string purchasesFile = "C:\\Temp\\Files\\Purchases.dat";
        static int lineIndex = 0;

        static void Main(string[] args)
        {
            List<Purchase> purchases = handlePurchasesFile();
            List<ItemPrice> prices = handlePricesFile();
        }


        private static List<ItemPrice> handlePricesFile()
        {
            List<ItemPrice> prices = new List<ItemPrice>();

            XmlDocument doc = new XmlDocument();
            doc.Load(pricesFile);

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
            prices.ForEach(Console.WriteLine);
            //Console.WriteLine(prices.ElementAt(0).Price + 1);
            return prices;

        }

        private static List<Purchase> handlePurchasesFile()
        {
            List<Purchase> purchases = new List<Purchase>();

            if (File.Exists(purchasesFile))
            {
                string[] lines = File.ReadAllLines(purchasesFile);

                while (lineIndex < lines.Length)
                {
                    Purchase purchase = new Purchase
                    {
                        CustomerId = lines[lineIndex].Substring(4),
                        Date = ExtractDate(lines[lineIndex + 1]),
                        ItemIds = ExtractItemIds(lines),
                    };

                    purchases.Add(purchase);
                }
                //purchases.ForEach(Console.WriteLine);
            }
            return purchases;
        }

        private static List<String> ExtractItemIds(string[] lines)
        {
            List<String> ItemIds = new List<String>();

            lineIndex += 2;

            while (lineIndex < lines.Length && lines[lineIndex].StartsWith("ITEM"))
            {
                ItemIds.Add(lines[lineIndex].Substring(4));
                lineIndex++;
            }

            return ItemIds;
        }

        private static DateTime ExtractDate(String date)
        {
            int day = int.Parse(date.Substring(4, 2));
            int month = int.Parse(date.Substring(6, 2));
            int year = int.Parse(date.Substring(8, 4));

            return new DateTime(year, month, day);

        }
    }
}