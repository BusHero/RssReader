using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace RSSConsoleTest
{
    public delegate void ChannelUpdatedHandler(List<SyndicationItem> syndicationFeeds);

    public interface IRssChannelListener
    {
        event ChannelUpdatedHandler ChannelUpdated;

        void Start();
        void Stop();
    }

}
