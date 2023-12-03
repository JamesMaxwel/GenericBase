namespace GenericBase.Application.Dto.PermissionDto
{
    public class PermissionResponseDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

}
