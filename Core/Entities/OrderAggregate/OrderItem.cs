namespace Core.Entities.OrderAggregate
{
    //entity framework format of the ordered item 
    public class OrderItem : BaseEntity
    {
        //required by entity framework
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity, string productUrl)
        {      
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
            ProductUrl = productUrl;
        }

        public ProductItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductUrl { get; set; }
    }
}