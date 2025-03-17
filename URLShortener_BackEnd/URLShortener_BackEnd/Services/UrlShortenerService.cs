using MongoDB.Driver;
using URLShortener_BackEnd.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Cryptography;
using StackExchange.Redis;

namespace URLShortener_BackEnd.Services
{
    public class UrlShortenerService
    {
        private readonly IMongoCollection<ShortUrl> _urlsCollection;
        private readonly IDatabase _redisDb;

        public UrlShortenerService(IOptions<MongoDbSetting> mongoDbSettings, IConfiguration config)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _urlsCollection = mongoDatabase.GetCollection<ShortUrl>(mongoDbSettings.Value.CollectionName);

            var redis = ConnectionMultiplexer.Connect($"{config["Redis:Host"]}:{config["Redis:Port"]}");
            _redisDb = redis.GetDatabase();
        }

        public async Task<ShortUrl?> GetByShortCodeAsync(string shortUrl)
        {
            try
            {
                if (_redisDb.Multiplexer.IsConnected)
                {
                    var cachedUrl = await _redisDb.StringGetAsync(shortUrl);
                    if (!string.IsNullOrEmpty(cachedUrl))
                    {
                        return new ShortUrl { ShortCode = shortUrl, OriginalUrl = cachedUrl };
                    }
                }

                else
                {
                    Console.WriteLine("[Redis Warning] Unable to connect Redis, using MongoDB.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis Error] Unable to read data from cache: {ex.Message}");
            }
            var url = await _urlsCollection.Find(u => u.ShortCode == shortUrl).FirstOrDefaultAsync();
            if (url != null)
            {
                try
                {
                    if (_redisDb.Multiplexer.IsConnected)
                    {
                        await _redisDb.StringSetAsync(shortUrl, url.OriginalUrl, TimeSpan.FromHours(1));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Redis Error] Unable to write data to cache: {ex.Message}");
                }
            }
            return url;
        }

        public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl)
        {
            var existingUrl = await _urlsCollection.Find(url => url.OriginalUrl == originalUrl).FirstOrDefaultAsync();
            if (existingUrl != null)
            {
                return existingUrl;
            }

            var shortCode = GenerateShortCode(originalUrl);
            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode
            };

            await _urlsCollection.InsertOneAsync(shortUrl);
            return shortUrl;
        }


        private string GenerateShortCode(string url)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));
            return Convert.ToBase64String(hash)
                .Substring(0, 7)
                .Replace("/", "_")
                .Replace("+", "-");
        }
    }
}



