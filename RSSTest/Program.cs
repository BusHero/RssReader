using System;
using System.Xml;

namespace RSSTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://tv8.md/feed/";
            using (XmlReader reader = XmlReader.Create(url))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                Console.WriteLine(feed.Title.Text);
                Console.WriteLine(feed.Links[0].Uri);
                foreach (SyndicationItem item in feed.Items)
                {
                    Console.WriteLine(item.Title.Text);
                }
            }
        }
    }
}
