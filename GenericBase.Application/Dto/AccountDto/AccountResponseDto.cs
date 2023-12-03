namespace GenericBase.Application.Dto.AccountDto
{
    public class AccountResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsEnabled { get; set; }

    }
}
