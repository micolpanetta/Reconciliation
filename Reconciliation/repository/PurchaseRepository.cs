using System.Linq;

namespace Reconciliation
{
    public class PurchaseRepository
    {
        private int lineIndex = 0;
        private String filePath = ".\\files\\input\\Purchases.dat";
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

        internal List<string> GetAllCustomersIds()
        {
            return purchases.Select(purchase => purchase.CustomerId).ToList();
        }

        internal List<Purchase> GetByCustomerYearMonth(string Customer, int Year, int Month)
        {
            return purchases.FindAll(purchase => purchase.CustomerId == Customer && purchase.Date.Year == Year && purchase.Date.Month == Month);
        }
    }
}