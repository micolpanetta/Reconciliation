namespace Reconciliation
{
    internal class Purchase
    {
        private String _customerId;
        private DateTime _date;
        private List<String> _itemIds;
        
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

        //public override string ToString()
        //{
        //    return GetType().GetProperties()
        //        .Select(info => (info.Name, Value: info.GetValue(this, null) ?? "(null)"))
        //        .Aggregate(
        //            new StringBuilder(),
        //            (sb, pair) => sb.AppendLine($"{pair.Name}: {pair.Value}"),
        //            sb => sb.ToString());
        //}
    }

}