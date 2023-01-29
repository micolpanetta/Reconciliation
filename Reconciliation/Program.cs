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
            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();

            List<Reconciliation> reconciliations = new List<Reconciliation>();
            List<String> paymentsCustomers = payments.GetAllCustomersIds();
            List<String> purchasesCustomers = purchases.GetAllCustomersIds();

            IEnumerable<String> customersIds = paymentsCustomers.Union(purchasesCustomers);

            customersIds.ToList().ForEach(id =>
            {

                for (int i = 1; i < 13; i++)
                {

                    reconciliations.Add(new Reconciliation
                    {
                        Customer = id,
                        Year = 2018,
                        Month = i,
                    });


                }

            });

            //TODO tolist here
            IEnumerable<IGrouping<Tuple<string, int, int>, Purchase>> purchasesGroups = purchases.groupedByCustomerAndMonth();

            reconciliations.ToList().ForEach(rec =>
            {
                IGrouping<Tuple<string, int, int>, Purchase> group = purchasesGroups.ToList().Find(g => g.Key.Item1 == rec.Customer && g.Key.Item2 == rec.Year && g.Key.Item3 == rec.Month);
                //total sum of the purchases in a month 
                Decimal amountDue = group == null ? 0 : group.ToList().Sum(purchase =>
                {
                    //the sum of each item purchased
                    return purchase.ItemIds.Sum(itemId => prices.getPriceByItemId(itemId)); //TODO total in purchase
                });
               //Console.WriteLine(amountDue);

                rec.AmountDue = amountDue;
                //TODO migliorabile?
                rec.AmountPayed = payments.GetAmountByCustomerYearMonth(rec.Customer, rec.Year, rec.Month);  //verificare convenzione BY

            });

            reconciliations = reconciliations.FindAll(rec => rec.Balance != Decimal.Zero).OrderByDescending(rec => Math.Abs(rec.Balance)).ToList();

            Console.WriteLine(JsonConvert.SerializeObject(reconciliations));

        }
    }
}