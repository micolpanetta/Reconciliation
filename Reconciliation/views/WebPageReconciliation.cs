using Scriban;
using Scriban.Runtime;

namespace Reconciliation
{
    public class WebPageReconciliation : ReconciliationFormatter
    {
        public string FormatReconciliation(List<Reconciliation> reconciliations)
        {
            var html = @"
            <!DOCTYPE html>
            <html>
            <style> table, th, td {
                border:1px solid black;
            }
            </style>
                    
                <head>
                    <title>Reconciliation Webpage</title>
                </head>

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

            var template = Template.Parse(html);
            return template.Render(new { reconciliations = reconciliations }, new MemberRenamerDelegate(member => member.Name));

        }

        public string getExtension()
        {
            return ".html";
        }
    }
}