using Reconciliation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reconciliation
{
    public class Reconciliator
    {
        private PurchaseRepository purchases;
        private ItemPriceRepository prices;
        private PaymentRepository payments;

        public Reconciliator(PurchaseRepository purchaseRepository, ItemPriceRepository itemPriceRepository, PaymentRepository paymentRepository)
        {
            this.purchases = purchaseRepository;
            this.prices = itemPriceRepository;
            this.payments = paymentRepository;
        }

        public List<Reconciliation> reconciliate(int year)
        {
            List<Reconciliation> reconciliations = new List<Reconciliation>();
            //merging the customers ids from payments and purchases
            IEnumerable<String> mergedCustomersIds = payments.GetAllCustomersIds().Union(purchases.GetAllCustomersIds());

            //creating the reconciliations list, not a month is missed 
            mergedCustomersIds.ToList().ForEach(customerId =>
            {
                for (int month = 1; month < 13; month++)
                {
                    List<Purchase> monthlyPurchases = purchases.GetByCustomerYearMonth(customerId, year, month);

                    Decimal amountDue = 0;
                    monthlyPurchases.ForEach(purchase =>
                    {
                        purchase.ItemIds.ForEach(item =>
                        {
                            amountDue += prices.GetPriceByItemId(item);
                        });
                    });

                    reconciliations.Add(new Reconciliation
                    {
                        Customer = customerId,
                        Year = year,
                        Month = month,
                        AmountDue = amountDue,
                        AmountPayed = payments.GetAmountByCustomerYearMonth(customerId, year, month)
                    });
                }
            });

            return reconciliations.FindAll(rec => rec.Balance != Decimal.Zero).OrderByDescending(rec => Math.Abs(rec.Balance)).ToList();
        }
    }
}