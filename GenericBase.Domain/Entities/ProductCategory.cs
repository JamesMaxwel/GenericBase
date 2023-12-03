using GenericBase.Domain.Common;


namespace GenericBase.Domain.Entities
{
    public class ProductCategory : Base
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
