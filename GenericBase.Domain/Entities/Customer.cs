using GenericBase.Domain.Common;
using System.Collections.ObjectModel;

namespace GenericBase.Domain.Entities
{
    public class Customer : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; } = new Collection<Order>();
    }
}
