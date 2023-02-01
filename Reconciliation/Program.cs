using NDesk.Options;

namespace Reconciliation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //setting default values for the file format and the year of the reconciliation, or setting values passed as args
            Dictionary<string, object> options = ParseArgs(args);
            string format = (string) options["format"];
            int year = (int) options["year"];

            //reading input from files and transform it in objects
            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();

            List<Reconciliation> reconciliations = new List<Reconciliation>();
            //merging the customers ids from payments and purchases
            IEnumerable<String> mergedCustomersIds = payments.GetAllCustomersIds().Union(purchases.GetAllCustomersIds());

            //creating the reconciliations list, not a month is missed 
            mergedCustomersIds.ToList().ForEach(customerId =>
            {
                for (int month = 1; month < 13; month++)
                {
                    List<Purchase> monthlyPurchases = purchases.GetByCustomerYearMonth(customerId, year, month);

                    Decimal amountDue = 0;
                    monthlyPurchases.ForEach(purchase =>
                    {
                        purchase.ItemIds.ForEach(item =>
                        {
                            amountDue += prices.GetPriceByItemId(item);
                        });
                    });

                    reconciliations.Add(new Reconciliation
                    {
                        Customer = customerId,
                        Year = year,
                        Month = month,
                        AmountDue = amountDue,
                        AmountPayed = payments.GetAmountByCustomerYearMonth(customerId, year, month) //todo verificare convenzione BY
                    });
                }
            });

            reconciliations = reconciliations.FindAll(rec => rec.Balance != Decimal.Zero).OrderByDescending(rec => Math.Abs(rec.Balance)).ToList();

            //chosing a view
            ReconciliationPrinter reconciliation;
            switch (format)
            {
                case "json":
                    reconciliation = new JsonReconciliation();
                    break;
                case "csv":
                    reconciliation = new CsvReconciliation();
                    break;
                case "narrative":
                    reconciliation = new NarrativeReconciliation();
                    break;
                case "webpage":
                    reconciliation = new WebPageReconciliation();
                    break;
                default:
                    reconciliation = new JsonReconciliation();
                    break;
            }

            reconciliation.PrintReconciliation(reconciliations);
        }

        private static Dictionary<string, object> ParseArgs(string[] args)
        {
            string format = "json";
            int year = 2018;
            bool help = false;
            OptionSet options = new OptionSet() {
               { "format=", "The file format of the output. \nYou can choose between json, csv, narrative and webpage.", value => format = value },
               { "year=", "The year of the reconciliation.", (int value) => year = value },
               { "help", value => help = value != null }
            };
            try
            {
                options.Parse(args);
                if (help)
                {
                    options.WriteOptionDescriptions(Console.Out);
                    Environment.Exit(0);
                }
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            return new Dictionary<string, object>() {{ "format", format }, { "year", year }};
        }
    }
}