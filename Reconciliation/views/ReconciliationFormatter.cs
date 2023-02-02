namespace Reconciliation
{
    public interface ReconciliationFormatter
    {
        string FormatReconciliation(List<Reconciliation> reconciliation);
        string getExtension();
    }
}
