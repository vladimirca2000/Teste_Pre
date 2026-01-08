using MeuTeste.Domain.Interfaces.Services;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace MeuTeste.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _db = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _db.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                {
                    _logger.LogInformation($"Cache miss para chave: {key}");
                    return default;
                }

                var deserialized = JsonSerializer.Deserialize<T>(value.ToString());
                _logger.LogInformation($"Cache hit para chave: {key}");
                return deserialized;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter valor do cache para chave: {key}");
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var serialized = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, serialized, expiration);
                _logger.LogInformation($"Valor armazenado no cache para chave: {key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao armazenar valor no cache para chave: {key}");
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
                _logger.LogInformation($"Chave removida do cache: {key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover chave do cache: {key}");
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _db.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao verificar existência da chave no cache: {key}");
                return false;
            }
        }
    }
}
