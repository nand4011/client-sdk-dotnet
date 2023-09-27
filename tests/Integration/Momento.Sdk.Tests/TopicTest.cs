#if NET6_0_OR_GREATER

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Momento.Sdk.Tests;

public class TopicTest : IClassFixture<CacheClientFixture>
{
    private readonly string cacheName;

    public TopicTest(CacheClientFixture cacheFixture)
    {
        cacheName = cacheFixture.CacheName;
    }

    [Fact(Timeout = 5000)]
    public async Task PublishAndSubscribe_String_Succeeds()
    {
        await Task.Run(() => Task.Delay(10000));
    }
}
#endif