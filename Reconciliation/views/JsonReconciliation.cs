using Newtonsoft.Json;

namespace Reconciliation
{
    public class JsonReconciliation : ReconciliationFormatter
    {
        public string FormatReconciliation(List<Reconciliation> reconciliations)
        {
            return JsonConvert.SerializeObject(reconciliations);
        }

        public string getExtension()
        {
            return ".json";
        }
    }
}