using Reconciliation;

namespace ReconciliationTest
{
    public class ProgramTest
    {
        [Fact]
        public void InputTest()
        {
            string[] args = { "", "" };
            Reconciliation.Program.Main(args);
        }

        [Fact]
        public void ReconciliatorTest()
        {
            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();

            Reconciliator reconciliator = new Reconciliator(purchases, prices, payments);
            List<Reconciliation.Reconciliation> actual = reconciliator.reconciliate(2018);

            Assert.Equal(9, actual.Count);

        }

        [Fact]
        public void EmptyReconciliatorTest()
        {
            PurchaseRepository purchases = new PurchaseRepository();
            ItemPriceRepository prices = new ItemPriceRepository();
            PaymentRepository payments = new PaymentRepository();

            Reconciliator reconciliator = new Reconciliator(purchases, prices, payments);
            List<Reconciliation.Reconciliation> actual = reconciliator.reconciliate(2019);

            Assert.Empty(actual);
        }

        [Fact]
        public void FormatReconciliationTest()
        {
            String output = "000012,2018,3,10,11,-1\n000013,2018,4,2,3,-1";
            List<Reconciliation.Reconciliation> reconciliations = new List<Reconciliation.Reconciliation>();
            reconciliations.Add(new Reconciliation.Reconciliation
            {
                Customer = "000012",
                Year = 2018,
                Month = 3,
                AmountDue = new decimal(10),
                AmountPayed = new decimal(11.00),
            });
            reconciliations.Add(new Reconciliation.Reconciliation
            {
                Customer = "000013",
                Year = 2018,
                Month = 4,
                AmountDue = new decimal(2.00),
                AmountPayed = new decimal(3),
            });
            ReconciliationFormatter formatter = new CsvReconciliation();
            string formatted = formatter.FormatReconciliation(reconciliations);

            Assert.Equal(output, formatted);

            output = "[{\"Customer\":\"000012\",\"Year\":2018,\"Month\":3,\"AmountDue\":10.0,\"AmountPayed\":11.0,\"Balance\":-1.0},{\"Customer\":\"000013\",\"Year\":2018,\"Month\":4,\"AmountDue\":2.0,\"AmountPayed\":3.0,\"Balance\":-1.0}]";
            formatter = new JsonReconciliation();
            formatted = formatter.FormatReconciliation(reconciliations);
            Console.WriteLine(formatted);
            Assert.Equal(output, formatted);

            output = "Customer 000012 in 3/2018 has to pay 10, has payed 11 with a balance of -1\nCustomer 000013 in 4/2018 has to pay 2, has payed 3 with a balance of -1";
            formatter = new NarrativeReconciliation();
            formatted = formatter.FormatReconciliation(reconciliations);
            Console.WriteLine(formatted);
            Assert.Equal(output, formatted);

        }
    }
}