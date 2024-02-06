using StackExchange.Redis;

namespace API_VietQR.Services.Cache
{
	public interface ICacheServices
	{

		Task<bool> SetHashCacheAsync(string hashKey, string cacheKey, string cacheValue, int database = 1);

		Task<bool> SetCacheAsync(string cacheKey, string cacheValue, int database = 1);

		Task<bool> SetCacheTtlAsync(string cacheKey, string cacheValue, TimeSpan timeToLive, int database = 1);

		bool SetCache(string cacheKey, string cacheValue, int database = 1);

		bool SetCacheTtl(string cacheKey, string cacheValue, TimeSpan timeToLive, int database = 1);

		Task<string> GetHashCacheAsync(string hashKey, string cacheKey, int database = 1);

		Task<string> GetCacheAsync(string cacheKey, int database = 1);

		string GetCache(string cacheKey, int database = 1);

		Task<bool> RemoveHashCacheAsync(string hashKey, string cacheKey, int database = 1);

		Task<bool> RemoveCacheAsync(string cacheKey, int database = 1);
	}

	public class CacheServices : ICacheServices
	{
		private readonly IConnectionMultiplexer _connectionMultiplexer;

		public CacheServices(IConnectionMultiplexer connectionMultiplexer)
		{
			_connectionMultiplexer = connectionMultiplexer;
		}

		public async Task<string> GetHashCacheAsync(string hashKey, string cacheKey, int database = 1)
		{
			var db = _connectionMultiplexer.GetDatabase(database);
			var cachedObject = await db.HashGetAsync(hashKey, cacheKey);
			return cachedObject.IsNullOrEmpty ? string.Empty : cachedObject.ToString();
		}

		public async Task<string> GetCacheAsync(string cacheKey, int database = 1)
		{
			var db = _connectionMultiplexer.GetDatabase(database);
			var cachedObject = await db.StringGetAsync(cacheKey);
			return cachedObject.IsNullOrEmpty ? string.Empty : cachedObject.ToString();
		}

		public async Task<bool> SetCacheTtlAsync(string cacheKey, string cacheValue, TimeSpan timeToLive, int database = 1)
		{
			if (cacheValue == null)
			{
				return false;
			}
			var db = _connectionMultiplexer.GetDatabase(database);

			return await db.StringSetAsync(cacheKey, cacheValue, timeToLive);
		}

		public async Task<bool> SetHashCacheAsync(string hashKey, string cacheKey, string cacheValue, int database = 1)
		{
			if (cacheValue == null)
			{
				return false;
			}
			var db = _connectionMultiplexer.GetDatabase(database);
			return await db.HashSetAsync(hashKey, cacheKey, cacheValue);
		}

		public async Task<bool> SetCacheAsync(string cacheKey, string cacheValue, int database = 1)
		{
			if (cacheValue == null)
			{
				return false;
			}
			var db = _connectionMultiplexer.GetDatabase(database);
			return await db.StringSetAsync(cacheKey, cacheValue);
		}

		public async Task<bool> RemoveHashCacheAsync(string hashKey, string cacheKey, int database = 1)
		{
			var db = _connectionMultiplexer.GetDatabase(database);
			return await db.HashDeleteAsync(hashKey, cacheKey);
		}

		public async Task<bool> RemoveCacheAsync(string cacheKey, int database = 1)
		{
			var db = _connectionMultiplexer.GetDatabase(database);
			return await db.KeyDeleteAsync(cacheKey);
		}

		public bool SetCache(string cacheKey, string cacheValue, int database = 1)
		{
			if (cacheValue == null)
			{
				return false;
			}
			var db = _connectionMultiplexer.GetDatabase(database);
			return db.StringSet(cacheKey, cacheValue);
		}

		public bool SetCacheTtl(string cacheKey, string cacheValue, TimeSpan timeToLive, int database = 1)
		{
			if (cacheValue == null)
			{
				return false;
			}
			var db = _connectionMultiplexer.GetDatabase(database);

			return db.StringSet(cacheKey, cacheValue, timeToLive);
		}

		public string GetCache(string cacheKey, int database = 1)
		{
			var db = _connectionMultiplexer.GetDatabase(database);
			var cachedObject = db.StringGet(cacheKey);
			return cachedObject.IsNullOrEmpty ? string.Empty : cachedObject.ToString();
		}
	}
}
