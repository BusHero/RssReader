using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace RSSReader.services.RssService
{
    public interface IRssChannel: IService
    {
        event Action<IEnumerable<SyndicationItem>> OnChannelUpdated;

        Uri BaseUri { get; }
        string Description { get; }
        string Id { get; }
        string Title { get; }
        Uri ImageUrl { get; }
        uint UpdatePeriod { get; set; }
        DateTimeOffset LastUpdatedTime { get; }
        List<SyndicationItem> Items { get; }
    }
}
