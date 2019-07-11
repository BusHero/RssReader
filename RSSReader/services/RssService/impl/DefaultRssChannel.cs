using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;

namespace RSSReader.services.RssService.impl
{
    public class DefaultRssChannel: IRssChannel
    {
        public event Action<IEnumerable<SyndicationItem>> OnChannelUpdated;
        public Uri BaseUri { get; }
        public string Description { get; private set; }
        public string Id { get; private set; }
        public string Title { get; private set; }
        public Uri ImageUrl { get; private set; }
        public uint UpdatePeriod { get; set; }

        public DateTimeOffset LastUpdatedTime { get; private set; }
        public List<SyndicationItem> Items { get; } = new List<SyndicationItem>();

        private Timer _timer;

        public DefaultRssChannel(Uri uri) => BaseUri = uri;

        public bool IsRunning { get; private set; }
        public void Start()
        {
            _timer ??= new Timer(CheckRssFeed, null, 0, UpdatePeriod);
            IsRunning = true;
        }

        private void CheckRssFeed(object states)
        {
            using var reader = XmlReader.Create(BaseUri.AbsoluteUri);
            var feed = SyndicationFeed.Load(reader);
            var items = feed.Items.ToList(); 
            items.ForEach(item => item.SourceFeed = feed);
            if (Description == null)
            {
                SetupChannelProperties(feed);
                Items.AddRange(items);
                OnChannelUpdated?.Invoke(Items);
            }
            else if (!LastUpdatedTime.EqualsExact(feed.LastUpdatedTime))
            {
                var oldFirstId = Items.First();
                var array = items
                    .TakeWhile(item => !item.Id.Equals(oldFirstId.Id))
                    .ToArray();
                Items.InsertRange(0, array);
                OnChannelUpdated?.Invoke(array);
            }
            LastUpdatedTime = feed.LastUpdatedTime;
        }

        private void SetupChannelProperties(SyndicationFeed feed)
        {
            Id = feed.Id;
            Title = feed.Title.Text;
            ImageUrl = feed.ImageUrl;
            Description = feed.Description.Text;
        }

        public void Stop()
        {
            _timer?.Dispose();
            IsRunning = false;
        }
    }
}
