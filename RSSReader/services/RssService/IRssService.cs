using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace RSSReader.services.RssService
{
    public interface IRssService: IService
    {
        event Action<IRssChannel, IEnumerable<SyndicationItem>> OnServiceUpdated;
        uint CheckPeriod { get; set; }
        IRssChannel AddChannel(Uri uri);
        void RemoveChannel(IRssChannel channel);
        List<IRssChannel> Channels { get; }
        List<SyndicationItem> Items { get; }
    }
}

