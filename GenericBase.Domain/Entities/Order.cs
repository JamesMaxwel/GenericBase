using GenericBase.Domain.Common;
using GenericBase.Domain.Entities.Account;
using System.Collections.ObjectModel;

namespace GenericBase.Domain.Entities
{
    public class Order : Base
    {
        public double Discount { get; set; }
        public string? Comments { get; set; }

        public Guid CashierId { get; set; }
        public virtual User? Cashier { get; set; }

        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public ICollection<OrderItem> OrderDetails { get; set; } = new Collection<OrderItem>();
    }
}
