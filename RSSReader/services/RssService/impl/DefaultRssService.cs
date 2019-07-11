using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace RSSReader.services.RssService.impl
{
    internal class DefaultRssService : IRssService
    {
        private const uint DefaultCheckPeriod = 1000 * 60;

        public event Action<IRssChannel, IEnumerable<SyndicationItem>> OnServiceUpdated;

        public bool IsRunning { get; private set; }

        public void Start()
        {
            Channels.ForEach(channel => channel.Start());
            IsRunning = true;
        }

        public void Stop()
        {
            Channels.ForEach(channel => channel.Stop());
            IsRunning = false;
        }

        public uint CheckPeriod { get; set; } = DefaultCheckPeriod;

        public IRssChannel AddChannel(Uri uri)
        {
            var channel = new DefaultRssChannel(uri);
            Channels.Add(channel);
            channel.UpdatePeriod = CheckPeriod;
            channel.OnChannelUpdated += items => OnServiceUpdated?.Invoke(channel, items);
            if (IsRunning) channel.Start();
            return channel;
        }

        public void RemoveChannel(IRssChannel channel)
        {
            channel.Stop();
            Channels.Remove(channel);
        }

        public List<IRssChannel> Channels { get; } = new List<IRssChannel>();

        
        class DescendedDateComparer : IComparer<DateTimeOffset>
        {
            public int Compare(DateTimeOffset x, DateTimeOffset y)
            {
                if (x > y) return -1;
                else if (x < y) return 1;
                return 0;
            }
        }

        public List<SyndicationItem> Items
        {
            get
            {
                var items = new SortedList<DateTimeOffset, SyndicationItem>(new DescendedDateComparer());
                Channels.ForEach(
                    channel => channel.Items.ForEach(
                        item => items.Add(item.PublishDate, item)));
                return items.Values.ToList();
            }
        }

    }
}