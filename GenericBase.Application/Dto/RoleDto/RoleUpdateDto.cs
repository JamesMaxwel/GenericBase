namespace GenericBase.Application.Dto.RoleDto
{
    public class RoleUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Guid> ClaimsIds { get; set; } = new HashSet<Guid>();

    }
}