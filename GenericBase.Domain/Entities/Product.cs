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
        public virtual ProductCategory? Category { get; set; }

        public virtual ICollection<Product> Children { get; set; } = new Collection<Product>();
        public virtual ICollection<OrderItem> OrderDetails { get; set; } = new Collection<OrderItem>();
    }
}
