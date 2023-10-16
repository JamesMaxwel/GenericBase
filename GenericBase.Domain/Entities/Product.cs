using GenericBase.Domain.Common;
using System.Collections.ObjectModel;


namespace GenericBase.Domain.Entities
{
    public class Product : Base
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool IsActive { get; set; }
        public bool IsDiscontinued { get; set; }

        public Guid? ParentId { get; set; }
        public virtual Product? Parent { get; set; }

        public Guid? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public ICollection<Product> Children { get; set; } = new Collection<Product>();
        public ICollection<OrderItem> OrderDetails { get; set; } = new Collection<OrderItem>();
    }
}
