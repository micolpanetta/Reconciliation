using Newtonsoft.Json;

namespace Reconciliation
{
    internal class Payment
    {
        [JsonProperty(PropertyName = "Customer")]
        private String _customerId = default!;

        private int _year;
        private int _month;
        private Decimal _amount;

        public String CustomerId
        {
            get => _customerId;
            set => _customerId = value;
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
        public Decimal Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public override string? ToString()
        {
            return "CustomerId: " + CustomerId + " Year: " + Year + "Month: " + Month + " Amount: " + Amount;
        }
    }
}