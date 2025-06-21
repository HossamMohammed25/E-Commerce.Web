using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string CachKey);
        Task SetKey(string CachKey,string CachValue,TimeSpan TimeToLive);
    }
}
