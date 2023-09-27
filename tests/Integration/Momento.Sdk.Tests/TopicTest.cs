#if NET6_0_OR_GREATER

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Momento.Sdk.Tests;

public class TopicTest
{
    public TopicTest()
    {
    }

    [Fact(Timeout = 60000)]
    public async Task PublishAndSubscribe_String_Succeeds()
    {
        await Task.Run(() => Task.Delay(100000));
    }
}
#endif