using Newtonsoft.Json;

namespace Reconciliation
{
    internal class JsonReconciliation : ReconciliationPrinter
    {
        public void printReconciliation(List<Reconciliation> reconciliations)
        {
            var jsonString = JsonConvert.SerializeObject(reconciliations);
            File.WriteAllText(Environment.GetEnvironmentVariable("FILESPATH") + "\\PaymentsNotMatched.json", jsonString); //TODO creare cartella per output
        }
    }
}