namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountCredentialUpdateDto
    {
        public IEnumerable<Guid> RolesIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> ClaimsIds { get; set; } = new List<Guid>();
    }
}
