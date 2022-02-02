﻿using System;
using System.Threading.Tasks;
using Xunit;
using MomentoSdk;
using MomentoSdk.Responses;

namespace MomentoIntegrationTest
{
    public class CacheTest
    {
        private String authKey = Environment.GetEnvironmentVariable("TEST_AUTH_TOKEN");
        private String cacheName = Environment.GetEnvironmentVariable("TEST_CACHE_NAME");
        [Fact]
        public void HappyPath()
        {
            String cacheKey = "some cache key";
            String cacheValue = "some cache value";
            uint defaultTtlSeconds = 10;
            Momento momento = new Momento(authKey);
            MomentoCache cache = momento.GetOrCreateCache(cacheName, defaultTtlSeconds);
            cache.Set(cacheKey, cacheValue, defaultTtlSeconds);
            CacheGetResponse result = cache.Get(cacheKey);
            String stringResult = result.String();
            Assert.Equal(cacheValue, stringResult);
        }

        [Fact]
        public async void HappyPathExpiredTtl()
        {
            String cacheKey = "some cache key";
            String cacheValue = "some cache value";
            uint defaultTtlSeconds = 10;
            Momento momento = new Momento(authKey);
            MomentoCache cache = momento.GetOrCreateCache(cacheName, defaultTtlSeconds);
            cache.Set(cacheKey, cacheValue, 1);
            await Task.Delay(1100);
            CacheGetResponse result = cache.Get(cacheKey);
            Assert.Equal(CacheGetStatus.MISS, result.Status);
        }

        [Fact]
        public async void HappyPathAsync()
        {
            String cacheKey = "async cache key";
            String cacheValue = "async cache value";
            uint defaultTtlSeconds = 10;
            Momento momento = new Momento(authKey);
            MomentoCache cache = momento.GetOrCreateCache(cacheName, defaultTtlSeconds);
            await cache.SetAsync(cacheKey, cacheValue, defaultTtlSeconds);
            CacheGetResponse result = await cache.GetAsync(cacheKey);
            Assert.Equal(CacheGetStatus.HIT, result.Status);
            Assert.Equal(cacheValue, result.String());
        }

        [Fact]
        public void HappyPathMiss()
        {
            uint defaultTtlSeconds = 10;
            Momento momento = new Momento(authKey);
            MomentoCache cache = momento.GetOrCreateCache(cacheName, defaultTtlSeconds);
            CacheGetResponse result = cache.Get(Guid.NewGuid().ToString());
            Assert.Equal(CacheGetStatus.MISS, result.Status);
            Assert.Null(result.String());
            Assert.Null(result.Bytes());
        }
    }
}
