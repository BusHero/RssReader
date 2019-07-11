using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using RSSReader.services.Notifications;
using RSSReader.services.RssService;
using RSSReader.services.RssService.impl;

namespace RSSReader
{
    public partial class MainWindow
    {
        private IRssService _rssService;
        private readonly NotificationsService _notificationsService = new NotificationsService();

        // ReSharper disable once MemberCanBePrivate.Global
        public ObservableCollection<RssItem> Items { get; } = new ObservableCollection<RssItem>();

        private bool tv8FirstTime = true;
        private bool diezMdFirstTime = true;
        private bool agoraMdFirstTime = true;

        public MainWindow()
        {
            InitializeComponent();
            SetupRssService();
            MySources.Items.Add("TV8.md" );
            MySources.Items.Add("#diez");
            MySources.Items.Add("UNIMEDIA");
            DataContext = this;
            NewsMakerTextBlock.Text = "My Sources";
        }

        // ReSharper disable once MethodTooLong
        private void SetupRssService()
        {
            _rssService = new DefaultRssService(); //TODO Dependency Injection
            _rssService.AddChannel(new Uri("http://tv8.md/feed"));
            _rssService.AddChannel(new Uri("http://diez.md/feed"));
            _rssService.AddChannel(new Uri("https://unimedia.info/ro/rss/all/"));
            // ReSharper disable once ComplexConditionExpression
            _rssService.OnServiceUpdated += (channel, items) =>
            {
                if (channel.Title.Equals("TV8.md") && tv8FirstTime)
                {
                    tv8FirstTime = false;
                    return;
                }

                if (channel.Title.Equals("#diez") && diezMdFirstTime)
                {
                    diezMdFirstTime = false;
                    return;
                }

                if (channel.Title.Contains("UNIMEDIA") && agoraMdFirstTime)
                {
                    agoraMdFirstTime = false;
                    return;
                }
                foreach (var item in items)
                {
                    var notification = SyndicationItemToNotification(item);
                    Dispatcher.Invoke(() => _notificationsService.ShowNotification(notification));
                }
            };
            _rssService.Start();

            Task.Run(() =>
            {
                Task.Delay(2000);
                Dispatcher.Invoke(() => _rssService.Items
                    .ForEach(item => Items.Add(new RssItem(item))));
            });
        }

        private static XmlDocument SyndicationItemToNotification(SyndicationItem item)
        {
            var templateContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);
            var stringElements = templateContent.GetElementsByTagName("text");
            stringElements[0].AppendChild(templateContent.CreateTextNode(item.Title.Text));
            stringElements[1].AppendChild(templateContent.CreateTextNode(item.Summary.Text));
            return templateContent;
        }

        // ReSharper disable once TooManyDeclarations
        private void OnChannelChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Items.Clear();
            var selectedValue = FeedsSources.SelectedValue;
            switch (selectedValue)
            {
                case string value:
                    NewsMakerTextBlock.Text = value;
                    break;
                case TreeViewItem item:
                    NewsMakerTextBlock.Text = item?.Header as string;
                    break;
            }
            if (MySources.Items.Contains(FeedsSources.SelectedItem))
                _rssService.Channels.Find(ch => ch.Title.Contains(NewsMakerTextBlock.Text))
                    ?.Items.ForEach(item => Items.Add(new RssItem(item)));
            else _rssService.Items.ForEach(item => Items.Add(new RssItem(item)));
        }
    }
}
