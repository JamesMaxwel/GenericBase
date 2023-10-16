using GenericBase.Domain.Interfaces;

namespace GenericBase.Domain.Common
{
    public class Base : IBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
    }
}
