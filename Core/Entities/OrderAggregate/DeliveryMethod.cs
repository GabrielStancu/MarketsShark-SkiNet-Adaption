namespace Core.Entities.OrderAggregate
{
    //the option of the user for the delivery
    public class DeliveryMethod : BaseEntity
    {
        public string ShortName {get; set; }
        public string DeliveryTime {get; set; }
        public string Description {get; set; }
        public decimal Price {get; set; }
    }
}