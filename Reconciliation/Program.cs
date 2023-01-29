using System;
using System.Linq;

namespace Reconciliation
{
    internal class Program
    {
        //TODO verificare metodi iniziano con maiuscola e capire convenzione esatta variabili
        static void Main(string[] args)
        {
            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();
            
            List<Reconciliation> reconciliations = new List<Reconciliation>();
            
            //TODO tolist here
            IEnumerable<IGrouping<Tuple<string, int, int>, Purchase>> purchasesGroups = purchases.groupedByCustomerAndMonth();

            purchasesGroups.ToList().ForEach(group =>
            {
                //total sum of the purchases in a month 
                Decimal amountDue = group.ToList().Sum(purchase =>
                {
                    //the sum of each item purchased
                    return purchase.ItemIds.Sum(itemId => prices.getPriceByItemId(itemId)); //TODO total in purchase
                });
                //Console.WriteLine(amountDue);
                Reconciliation reconciliation = new Reconciliation
                {
                              //TODO migliorabile?
                    Customer = group.Key.Item1,
                    Year = group.Key.Item2,
                    Month = group.Key.Item3,
                    AmountDue = amountDue,
                    AmountPayed = payments.GetAmountByCustomerYearMonth(group.Key.Item1, group.Key.Item2, group.Key.Item3), //verificare convenzione BY
                };

                reconciliations.Add(reconciliation);
            });

            reconciliations.FindAll(rec => rec.Balance != Decimal.Zero).OrderByDescending(rec => Math.Abs(rec.Balance)).ToList();

        }
    }
}