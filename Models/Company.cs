namespace StoreAPI.Models
{
    public class Company
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public ICollection<Store> Stores { get; set; }
    }
}