namespace Auth_Shared.Helpers
{
    public class JwtSettings
    {
        public AccessTokenSettings AccessTokenSettings { get; set; }
        public RefreshTokenSettings RefreshTokenSettings { get; set; }
    }

    public class AccessTokenSettings : Shared.Helpers.AccessTokenSettings
    {
        public long LifeTimeInMinutes { get; set; }
    }

    public class RefreshTokenSettings
    {
        public int Length { get; set; }
        public int LifeTimeInDays { get; set; }
    }
}
