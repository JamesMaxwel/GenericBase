using GenericBase.Domain.Common;

namespace GenericBase.Domain.Entities
{
    public class OrderItem : Base
    {
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
