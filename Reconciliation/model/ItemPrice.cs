namespace Reconciliation
{
    internal class ItemPrice
    {
        private String _itemId;
        private Decimal _price; 

        public String ItemId
        {
            get => _itemId;
            set => _itemId = value;
        }
        public Decimal Price
        {
            get => _price;
            set => _price = value;
        }
        public override string? ToString()
        {
            return "ItemId: " + ItemId + " Price: " + Price;
        }
    }
}
