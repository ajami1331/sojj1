﻿using Microsoft.Extensions.Caching.Memory;
using Sojj.Services.Contracts;
using System.IO.Compression;

namespace Sojj.Services;

public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> logger;
    private readonly IConfiguration configuration;
    private readonly string cacheLocation;
    private readonly IMemoryCache memoryCache;

    public CacheService(ILogger<CacheService> logger, IConfiguration configuration, IMemoryCache memoryCache)
    {
        this.logger = logger;
        this.configuration = configuration;
        cacheLocation = this.configuration["JudgeService:CacheLocation"] ?? throw new ArgumentNullException("CacheLocation");
        this.memoryCache = memoryCache;
    }

    public async Task<int> GetCacheUpdateTimeAsync()
    {
        logger.LogInformation("Get cache update time");
        string fileName = Path.Combine(cacheLocation, "cache.txt");
        Directory.CreateDirectory(cacheLocation);
        if (!File.Exists(fileName))
        {
            await File.WriteAllTextAsync(fileName, "0");
            return 0;
        }
        string content = await File.ReadAllTextAsync(fileName);
        if (int.TryParse(content, out int lastUpdateAt))
        {
            logger.LogInformation("Cache last update at {lastUpdateAt}", lastUpdateAt);
            return lastUpdateAt;
        }

        return 0;
    }

    public Task InvalidateCacheAsync()
    {
        logger.LogInformation("Invalidate cache");
        Directory.CreateDirectory(cacheLocation);
        Directory.Delete(cacheLocation, true);
        return Task.CompletedTask;
    }

    public Task InvalidateCacheAsync(string domainId, string problemId)
    {
        logger.LogInformation("Invalidate cache {domainId}-{problemId}", domainId, problemId);
        string path = Path.Combine(cacheLocation, domainId, problemId);
        Directory.CreateDirectory(path);
        Directory.Delete(path, true);
        this.memoryCache.Remove($"{domainId}-{problemId}");
        return Task.CompletedTask;
    }

    public void WriteCache(ZipArchive zipData, string domainId, string problemId)
    {
        logger.LogInformation("Write cache for problem {problemId} in domain {domainId}", problemId, domainId);
        string path = Path.Combine(cacheLocation, domainId, problemId);
        logger.LogInformation("Write cache to {path}", path);
        Directory.CreateDirectory(path);
        zipData.ExtractToDirectory(path, false);
        logger.LogInformation("Write cache to {path} completed", path);
    }

	public Task UpdateCacheTimeAsync(int unixTimestamp)
	{
		return File.WriteAllTextAsync(Path.Combine(cacheLocation, "cache.txt"), unixTimestamp.ToString());
	}
}
