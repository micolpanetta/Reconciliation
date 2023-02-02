using NDesk.Options;

namespace Reconciliation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //setting default values for file format, year of the reconciliation and output path or setting values passed as args
            Dictionary<string, object> options = ParseArgs(args);
            string format = (string) options["format"];
            int year = (int) options["year"];
            string path = (string) options["path"];

            //reading input from files and transforming it into objects
            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();

            Reconciliator reconciliator = new Reconciliator(purchases, prices, payments);
            List<Reconciliation> reconciliations = reconciliator.reconciliate(year); 

            //chosing a view
            ReconciliationFormatter formatter;
            switch (format)
            {
                case "json":
                    formatter = new JsonReconciliation();
                    break;
                case "csv":
                    formatter = new CsvReconciliation();
                    break;
                case "narrative":
                    formatter = new NarrativeReconciliation();
                    break;
                case "webpage":
                    formatter = new WebPageReconciliation();
                    break;
                default:
                    formatter = new JsonReconciliation();
                    break;
            }

            String output = formatter.FormatReconciliation(reconciliations);
            
            //writing into a file
            File.WriteAllText(Path.Join(path, "PaymentsNotMatched" + formatter.getExtension()), output);

        }

        //console options
        private static Dictionary<string, object> ParseArgs(string[] args)
        {
            string format = "json";
            int year = 2018;
            string path = ".\\";
            bool help = false;
            OptionSet options = new OptionSet() {
               { "format=", "The file format of the output. \nYou can choose between json, csv, narrative and webpage.", value => format = value },
               { "year=", "The year of the reconciliation.", (int value) => year = value },
               { "path=", "The path where the output file will be saved. Default is the same path as the .exe file.", value => path = value },
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
            return new Dictionary<string, object>() { { "format", format }, { "year", year }, { "path", path } };
        }
    }
}