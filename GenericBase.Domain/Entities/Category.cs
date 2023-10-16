using GenericBase.Domain.Common;
using GenericBase.Domain.Enums;
using System.Collections.ObjectModel;


namespace GenericBase.Domain.Entities
{
    public class Category : Base
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public CategoryTypeEnum Type { get; set; }
        public ICollection<Product> Products { get; set; } = new Collection<Product>();
    }
}
