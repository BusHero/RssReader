using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;

namespace RSSConsoleTest
{
    internal class DefaultRssChannelListener : IRssChannelListener
    {
        public event ChannelUpdatedHandler ChannelUpdated;

        public string Url { get; }
        public uint UpdateInterval { get; } = 1000 * 60 * 10;
        private Timer _timer;
        private SyndicationItem _lastItem;

        public DefaultRssChannelListener(string url)
        {
            Url = url;
        }

        public void Start()
        {
            _timer = new Timer(CheckRssFeed, null, 0, UpdateInterval);
        }

        private void CheckRssFeed(object e)
        {
            Console.WriteLine(@"Tick");
            using var reader = XmlReader.Create(Url);
            var feed = SyndicationFeed.Load(reader);
            var items = feed.Items.ToList();
            if (_lastItem == null)
            {
                ChannelUpdated?.Invoke(items);
                _lastItem = items.First();
            }
            else if (!items.First().Title.Text.Equals(_lastItem.Title.Text))
            {
                var syndicationItems = items.GetRange(0, items.IndexOf(_lastItem));
                ChannelUpdated?.Invoke(syndicationItems);
                _lastItem = items.First();
            }
        }

        public void Stop()
        {
            _timer.Dispose();
        }

    }
}
