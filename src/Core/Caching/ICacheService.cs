﻿using System;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace Zord.Core.Caching
{
    public interface ICacheService
    {
        //[return: MaybeNull]
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key, CancellationToken token = default);

        void Refresh(string key);
        Task RefreshAsync(string key, CancellationToken token = default);

        void Remove(string key);
        Task RemoveAsync(string key, CancellationToken token = default);

        void Set<T>(string key, T value, TimeSpan? slidingExpiration = null);
        Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);
    }
}
