namespace API.Dtos
{
    //basket item in the flattened in the desired format for the http request
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductUrl { get; set; }
    }
}