﻿using System.IO.Compression;

namespace Sojj.Services.Contracts;

public interface ICacheService
{
    Task InvalidateCacheAsync();

    Task InvalidateCacheAsync(string domainId, string problemId);

    Task<int> GetCacheUpdateTimeAsync();

    Task WriteCacheAsync(ZipArchive zipData, string domainId, string problemId, int unixTimestamp);
}
