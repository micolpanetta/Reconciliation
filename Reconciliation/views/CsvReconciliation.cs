using System.Globalization;

namespace Reconciliation
{
    internal class CsvReconciliation : ReconciliationPrinter
    {
        public void PrintReconciliation(List<Reconciliation> reconciliations)
        {
            using (StreamWriter writer = new StreamWriter(Environment.GetEnvironmentVariable("FILESPATH") + "\\output\\PaymentsNotMatched.csv"))
            {
                foreach (Reconciliation rec in reconciliations)
                {

                    string amountDue = rec.AmountDue.ToString(CultureInfo.InvariantCulture);
                    string amountPayed = rec.AmountPayed.ToString(CultureInfo.InvariantCulture);
                    string balance = rec.Balance.ToString(CultureInfo.InvariantCulture);

                    writer.WriteLine($"{rec.Customer},{rec.Year},{rec.Month},{amountDue},{amountPayed},{balance}");
                }
            }
        }
    }
}