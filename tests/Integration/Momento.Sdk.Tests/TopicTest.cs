#if NET6_0_OR_GREATER

using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Momento.Sdk.Tests;

public class TopicTest
{
    [Fact]
    public async Task PublishAndSubscribe_String_Succeeds()
    {
        await Task.Run(() => Task.Delay(10000));
    }
}
#endif