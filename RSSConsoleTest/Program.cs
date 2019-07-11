using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;

namespace RSSConsoleTest
{
    internal class Program
    {
        private static void Main()
        {
//            var rssChannelListener = new DefaultRssChannelListener("http://tv8.md/feed");
//            rssChannelListener.ChannelUpdated += OnChannelUpdated;
//            rssChannelListener.Start();
            var notificationsService = new NotificationsService();
//            notificationsService.ShowNotification();
            Console.ReadKey();
        }

        private static void OnChannelUpdated(IEnumerable<SyndicationItem> syndicationItems)
        {
            foreach (var syndicationItem in syndicationItems) Console.WriteLine(syndicationItem.Title.Text);
        }
    }
}
