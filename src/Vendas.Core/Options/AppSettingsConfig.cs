namespace Vendas.Core.Options
{
    public class AppSettingsConfig
    {
        public string Secret { get; set; }
        public string ApiKey { get; set; }
        public int TokenExpiration { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string BaseUrl { get; set; }
    }
}
