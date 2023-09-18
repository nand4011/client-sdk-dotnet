#if NET6_0_OR_GREATER

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Momento.Sdk.Tests;

public class TopicTest : IClassFixture<CacheClientFixture>, IClassFixture<TopicClientFixture>
{
    private readonly string cacheName;
    private readonly ITopicClient topicClient;

    public TopicTest(CacheClientFixture cacheFixture, TopicClientFixture topicFixture)
    {
        topicClient = topicFixture.Client;
        cacheName = cacheFixture.CacheName;
    }

    [Fact(Timeout = 5000)]
    public async Task PublishAndSubscribe_String_Succeeds()
    {
        const string topicName = "topic_string";
        var valuesToSend = new List<string>
        {
            "one",
            "two",
            "three",
            "four",
            "five"
        };

        var produceCancellation = new CancellationTokenSource();
        produceCancellation.CancelAfter(2000);

        // we don't need to put this on a different thread
        var consumeTask = ConsumeMessages(topicName, produceCancellation.Token);
        await Task.Delay(500);

        await ProduceMessages(topicName, valuesToSend);
        await Task.Delay(500);

        produceCancellation.Cancel();

        var consumedMessages = await consumeTask;
        Assert.Equal(valuesToSend.Count, consumedMessages.Count);
        for (var i = 0; i < valuesToSend.Count; ++i)
        {
            Assert.Equal(((TopicMessage.Text)consumedMessages[i]).Value, valuesToSend[i]);
        }
    }

    private async Task ProduceMessages(string topicName, List<string> valuesToSend)
    {
        foreach (var value in valuesToSend)
        {
            var publishResponse = await topicClient.PublishAsync(cacheName, topicName, value);
            switch (publishResponse)
            {
                case TopicPublishResponse.Success:
                    await Task.Delay(100);
                    break;
                default:
                    throw new Exception("publish error");
            }
        }
    }

    private async Task<List<TopicMessage>> ConsumeMessages(string topicName, CancellationToken token)
    {
        var subscribeResponse = await topicClient.SubscribeAsync(cacheName, topicName);
        switch (subscribeResponse)
        {
            case TopicSubscribeResponse.Subscription subscription:
                var cancellableSubscription = subscription.WithCancellation(token);
                var receivedSet = new List<TopicMessage>();
                await foreach (var message in cancellableSubscription)
                {
                    switch (message)
                    {
                        case TopicMessage.Binary:
                        case TopicMessage.Text:
                            receivedSet.Add(message);
                            break;
                        default:
                            throw new Exception("bad message received");
                    }
                }

                subscription.Dispose();
                return receivedSet;
            default:
                throw new Exception("subscription error");
        }
    }
}
#endif