namespace API.Dtos
{
    //reduced number of fields for returning over response to client s
    public class ProductToReturnDto
    {
        public int Id {get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }      
        public string PictureUrl { get; set; }      
        public string ProductType { get; set; }    
        public string ProductBrand { get; set; }  
        public string ProductUrl { get; set; }
    }
}