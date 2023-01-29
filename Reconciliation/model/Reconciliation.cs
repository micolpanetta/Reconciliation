namespace Reconciliation
{
    internal class Reconciliation
    {

        private String _customer;
        private int _year;
        private int _month;
        private Decimal _amountDue;
        private Decimal _amountPayed;

        public String Customer
        {
            get => _customer;
            set => _customer = value;
        }
        public int Year
        {
            get => _year;
            set => _year = value;
        }
        public int Month
        {
            get => _month;
            set => _month = value;
        }
        public Decimal AmountDue
        {
            get => _amountDue;
            set => _amountDue = value;
        }

        public Decimal AmountPayed
        {
            get => _amountPayed;
            set => _amountPayed = value;
        }

        public Decimal Balance
        {
            get => AmountDue - AmountPayed;
        }

        public override string? ToString()
        {
            return "CustomerId: " + Customer + " Year: " + Year + " Month: " + Month + " AmountDue: " + AmountDue
                + " AmountPayed: " + AmountPayed + " Balance: " + Balance;
        }
    }
}
