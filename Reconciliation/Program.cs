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
            String format = "csv";

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
            if (format == "json")
            {
                reconciliation = new JsonReconciliation();
            }
            else if (format == "csv")
            {
                reconciliation = new CsvReconciliation();
            }
            else if (format == "narrative")
            {
                reconciliation = new NarrativeReconciliation();
            }
            else 
            {
                reconciliation = new JsonReconciliation();
            }
            
            reconciliation.printReconciliation(reconciliations);
        }
    }
}