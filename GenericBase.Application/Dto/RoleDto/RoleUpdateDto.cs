namespace GenericBase.Application.Dto.RoleDto
{
    public class RoleUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> ClaimsIds { get; set; }

    }
}