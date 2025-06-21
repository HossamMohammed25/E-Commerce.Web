using Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
         readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string CachKey)
        {
            var CacheValue = await _database.StringGetAsync(CachKey);
            return CacheValue.IsNullOrEmpty ? null : CacheValue.ToString();
        }

        public async Task SetKey(string CachKey, string CachValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CachKey, CachValue, TimeToLive); 
        }
    }
}
