namespace Reconciliation
{
    internal class NarrativeReconciliation : ReconciliationPrinter
    {
        public void printReconciliation(List<Reconciliation> reconciliations)
        {
            using (StreamWriter writer = new StreamWriter(Environment.GetEnvironmentVariable("FILESPATH") + "\\output\\PaymentsNotMatched.txt"))
            {
                foreach (Reconciliation rec in reconciliations)
                {
                    writer.WriteLine($"Customer {rec.Customer} in {rec.Month}/{rec.Year} has to pay {rec.AmountDue}, has payed {rec.AmountPayed} with a balance of {rec.Balance}");
                }
            }
        }
    }
}