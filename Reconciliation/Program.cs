using Newtonsoft.Json;
using System;
using System.Linq;

namespace Reconciliation
{
    internal class Program
    {
        //TODO verificare metodi iniziano con maiuscola e capire convenzione esatta variabili
        static void Main(string[] args)
        {
            //capire come passare format
            String format = (args != null && args.Length > 0 && args[0] != null) ? args[0] : "";

            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();

            List<Reconciliation> reconciliations = new List<Reconciliation>();
            IEnumerable<String> mergedCustomersIds = payments.GetAllCustomersIds().Union(purchases.GetAllCustomersIds());

            mergedCustomersIds.ToList().ForEach(customerId =>
            {
                for (int month = 1; month < 13; month++)
                {
                    List<Purchase> monthlyPurchases = purchases.GetByCustomerYearMonth(customerId, 2018, month); // fare variabile anno

                    //Decimal amountDue = monthlyPurchases.Sum(purchase =>
                    //{
                    //  return purchase.ItemIds.Sum(itemId => prices.getPriceByItemId(itemId)); //TODO total in purchase
                    //});

                    Decimal amountDue = 0;
                    monthlyPurchases.ForEach(purchase =>
                    {
                        purchase.ItemIds.ForEach(item =>
                        {
                            amountDue += prices.getPriceByItemId(item);
                        });
                    });

                    reconciliations.Add(new Reconciliation
                    {
                        Customer = customerId,
                        Year = 2018,
                        Month = month,
                        AmountDue = amountDue,
                        AmountPayed = payments.GetAmountByCustomerYearMonth(customerId, 2018, month) //verificare convenzione BY
                    });
                }
            });

            reconciliations = reconciliations.FindAll(rec => rec.Balance != Decimal.Zero).OrderByDescending(rec => Math.Abs(rec.Balance)).ToList();

            ReconciliationPrinter reconciliation;
            switch (format)
            {
                case "json":  
                    reconciliation = new JsonReconciliation();
                    break;
                case "csv":
                    reconciliation = new CsvReconciliation();
                    break;
                case "narrative":
                    reconciliation = new NarrativeReconciliation();
                    break;
                case "webpage":
                    reconciliation = new WebPageReconciliation();
                    break;
                default:
                    reconciliation = new JsonReconciliation();
                    break;
            }
           
            reconciliation.printReconciliation(reconciliations);
        }
    }
}