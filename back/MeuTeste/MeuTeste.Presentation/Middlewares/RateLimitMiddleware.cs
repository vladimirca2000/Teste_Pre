namespace MeuTeste.Presentation.Middlewares
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitMiddleware> _logger;
        private readonly Dictionary<string, List<DateTime>> _requestHistory = new();
        private readonly int _requestsPerMinute;
        private readonly object _lockObject = new();

        public RateLimitMiddleware(RequestDelegate next, ILogger<RateLimitMiddleware> logger, int requestsPerMinute = 60)
        {
            _next = next;
            _logger = logger;
            _requestsPerMinute = requestsPerMinute;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            lock (_lockObject)
            {
                if (!_requestHistory.ContainsKey(clientIp))
                {
                    _requestHistory[clientIp] = new List<DateTime>();
                }

                var now = DateTime.UtcNow;
                var oneMinuteAgo = now.AddMinutes(-1);

                // Remover requisições antigas
                _requestHistory[clientIp] = _requestHistory[clientIp]
                    .Where(t => t > oneMinuteAgo)
                    .ToList();

                // Verificar limite
                if (_requestHistory[clientIp].Count >= _requestsPerMinute)
                {
                    _logger.LogWarning($"Rate limit excedido para IP: {clientIp}");
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var problem = new
                    {
                        status = 429,
                        title = "Too Many Requests",
                        detail = "Você excedeu o número de requisições permitidas. Tente novamente mais tarde.",
                        instance = context.Request.Path
                    };
                    return;
                }

                _requestHistory[clientIp].Add(now);
            }

            await _next(context);
        }
    }
}
