namespace Reconciliation
{
    internal class Purchase
    {
        private String _customerId = default!;
        private DateTime _date;
        private List<String> _itemIds = default!;
        
        public String CustomerId
        {
            get => _customerId;
            set => _customerId = value;
        }
        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }
        public List<String> ItemIds
        {
            get => _itemIds;
            set => _itemIds = value;
        }

        public override string? ToString()
        {
            return "CustomerId: " + CustomerId + " Date: " + Date + " ItemIds: " + string.Join(",", ItemIds);
        }
    }
}