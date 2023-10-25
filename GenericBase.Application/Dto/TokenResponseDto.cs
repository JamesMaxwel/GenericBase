namespace GenericBase.Application.Dto
{
    public class TokenResponseDto
    {
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

        public TokenResponseDto(string at, string rf, int atExpire, int rfExpire)
        {
            AccessToken = new AccessToken() { Value = at, ExpiresIn = atExpire };
            RefreshToken = new RefreshToken() { Value = rf, ExpiresIn = rfExpire };
        }
    }

    public class AccessToken
    {
        public string Value { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }

    public class RefreshToken
    {
        public string Value { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }

}
