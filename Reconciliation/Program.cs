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
            if (File.Exists(purchasesFile))
            {
                string[] lines = File.ReadAllLines(purchasesFile);
                
                List<Purchase> purchases = new List<Purchase>();
                
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
                purchases.ForEach(Console.WriteLine);
            }
        }

        private static List<String> ExtractItemIds(string[] lines)
        {
            List<String> ItemIds = new List<String>();

            lineIndex += 2;

            while (lineIndex < lines.Length && lines[lineIndex].StartsWith("ITEM")) {
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