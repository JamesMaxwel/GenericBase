namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountCredentialUpdateDto
    {
        public IEnumerable<Guid> RolesIds { get; set; } = [];
        public IEnumerable<Guid> ClaimsIds { get; set; } = [];
    }
}
