using System.Linq;

namespace Reconciliation
{
    internal class PurchaseRepository
    {
        private int lineIndex = 0;
        private String filePath = Environment.GetEnvironmentVariable("FILESPATH") + "\\Purchases.dat";
        private List<Purchase> purchases = new List<Purchase>();

        public PurchaseRepository()
        {
            getPurchasesFromFile();
        }

        private void getPurchasesFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

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
        }

        private List<String> ExtractItemIds(string[] lines)
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

        internal IEnumerable<IGrouping<Tuple<string, int, int>, Purchase>> groupedByCustomerAndMonth()
        {
           return purchases.GroupBy(purchase => new Tuple<string, int, int>(purchase.CustomerId, purchase.Date.Year, purchase.Date.Month));
        }
    }
}
