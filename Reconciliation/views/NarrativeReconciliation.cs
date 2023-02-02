using System.Globalization;

namespace Reconciliation
{
    public class NarrativeReconciliation : ReconciliationFormatter
    {
        public string FormatReconciliation(List<Reconciliation> reconciliations)
        {
            List<String> formatted = new List<String>();

            foreach (Reconciliation rec in reconciliations)
            {
                string amountDue = rec.AmountDue.ToString(CultureInfo.InvariantCulture);
                string amountPayed = rec.AmountPayed.ToString(CultureInfo.InvariantCulture);
                string balance = rec.Balance.ToString(CultureInfo.InvariantCulture);

                formatted.Add($"Customer {rec.Customer} in {rec.Month}/{rec.Year} has to pay {amountDue}, has payed {amountPayed} with a balance of {balance}");
            }
            return String.Join( "\n", formatted.ToArray() );
        }

        public string getExtension()
        {
            return ".txt";
        }
    }
}