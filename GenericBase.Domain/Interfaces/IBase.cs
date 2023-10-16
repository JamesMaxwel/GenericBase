namespace GenericBase.Domain.Interfaces
{
    public interface IBase
    {
        public Guid Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
    }
}
