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
            IEnumerable<IGrouping<Tuple<string, int, int>, Purchase>> map = purchases.groupedByCustomerAndMonth();

            map.ToList().ForEach(x => { Console.WriteLine(x.Key + " "); });



        }
    }
}