using Reconciliation;
using System.Globalization;

namespace Reconciliation
{
    public class CsvReconciliation : ReconciliationFormatter
    {
        public string FormatReconciliation(List<Reconciliation> reconciliations)
        {
            List<String> formatted = new List<String>();

            foreach (Reconciliation rec in reconciliations)
            {

                string amountDue = rec.AmountDue.ToString(CultureInfo.InvariantCulture);
                string amountPayed = rec.AmountPayed.ToString(CultureInfo.InvariantCulture);
                string balance = rec.Balance.ToString(CultureInfo.InvariantCulture);

                formatted.Add($"{rec.Customer},{rec.Year},{rec.Month},{amountDue},{amountPayed},{balance}");
            }

            return String.Join("\n", formatted.ToArray());
        }

        public string getExtension()
        {
            return ".csv";
        }
    }
}