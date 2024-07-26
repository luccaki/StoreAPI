namespace StoreAPI.Models
{
    public class Store
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}