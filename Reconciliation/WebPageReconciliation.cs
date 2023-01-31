using Scriban;

namespace Reconciliation
{
    internal class WebPageReconciliation : ReconciliationPrinter
    {
        public void printReconciliation(List<Reconciliation> reconciliations)
        {
            using (StreamWriter writer = new StreamWriter(Environment.GetEnvironmentVariable("FILESPATH") + "\\PaymentsNotMatched.html"))
            {
                var html = @"
                <!DOCTYPE html>
                <html>
                <style> table, th, td {
                  border:1px solid black;
                }
                </style>
                    <body>

                        <h2>Reconciliations</h2>

                        <table style=""width:100%"">
                          <tr>
                            <th>Customer</th>
                            <th>Year</th>
                            <th>Month</th>
                            <th>AmountDue</th>
                            <th>AmountPayed</th>
                            <th>Balance</th>
                          </tr>
                        {{- for reconciliation in reconciliations }}
                          <tr>
                            <td> {{ reconciliation.Customer }} </td>
                            <td> {{ reconciliation.Year }} </td>
                            <td> {{ reconciliation.Month }} </td>
                            <td> {{ reconciliation.AmountDue }} </td>
                            <td> {{ reconciliation.AmountPayed }} </td>
                            <td> {{ reconciliation.Balance }} </td>
                          </tr>
                        {{- end }}
                        </table>
                    
                    </body>
                </html>
                ";

                var tpl = Template.Parse(html);
                var res = tpl.Render(new { reconciliations = reconciliations });
                writer.WriteLine(res);
            }
        }
    }
}
