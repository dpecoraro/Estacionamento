namespace GreenVille.Domain
{
    public class Appsettings
    {
        public ConnectionDetail ConnectionDetail { get; set; }

        public JWTSettings JwtSettings { get; set; }
    }

    public class ConnectionDetail
    {
        public string DataSource { get; set; }

        public string Schema { get; set; }
    }

    public class JWTSettings
    {
        public string securityKey { get; set; }

        public string validIssuer { get; set; }

        public string validAudience { get; set; }

        public int expiryInMinutes { get; set; }
    }
}
