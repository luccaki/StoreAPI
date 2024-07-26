namespace StoreAPI.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public long StoreId { get; set; }
    }
}