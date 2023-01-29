using System;

namespace Reconciliation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();

            //TODO tolist here
            IEnumerable<IGrouping<Tuple<string, int, int>, Purchase>> purchasesGroups = purchases.groupedByCustomerAndMonth();

            purchasesGroups.ToList().ForEach(group =>
            {
                //total sum of the purchases in a month 
                Decimal amountDue = group.ToList().Sum(purchase =>
                {
                    //the sum of each item purchased
                    return purchase.ItemIds.Sum(itemId => prices.getPriceByItemId(itemId));
                });
                Console.WriteLine(amountDue);
            });



        }
    }
}