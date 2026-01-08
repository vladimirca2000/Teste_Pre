namespace MeuTeste.Presentation.Configuration
{
    public class RateLimitConfiguration
    {
        public int RequestsPerMinute { get; set; } = 60;
        public int RequestsPerHour { get; set; } = 1000;
        public bool EnableRateLimiting { get; set; } = true;
        public List<string> WhitelistedIPs { get; set; } = new();
    }
}
