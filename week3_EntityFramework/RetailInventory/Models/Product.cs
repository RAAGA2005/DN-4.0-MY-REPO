namespace RetailInventory.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
