using System;
using System.Diagnostics;
using System.ServiceModel.Syndication;
using System.Windows.Controls;
using System.Windows.Input;

namespace RSSReader
{
    public partial class RssItem : UserControl
    {
        private readonly SyndicationItem _item;

        public RssItem(SyndicationItem syndicationItem)
        {
            InitializeComponent();
            NewsMakerTextBlock.Text = syndicationItem.SourceFeed.Title.Text;
            TitleTextBlock.Text = syndicationItem.Title.Text;
            DateTextBlock.Text = TimeOffsetToString(syndicationItem.PublishDate);
            DescriptionTextBlock.Text = syndicationItem.Summary.Text;
            _item = syndicationItem;
        }

        private static string TimeOffsetToString(DateTimeOffset date)
        {
            var offset = DateTime.Now - date;
            if (offset.TotalMinutes < 60) return offset.TotalMinutes.ToString("F0") + " minutes ago";
            if (offset.TotalHours < 30) return offset.TotalHours.ToString("F0") + " hours ago";
            return offset.TotalDays.ToString("F0") + " days ago";
        }

        private void RssItem_OnMouseDown(object sender, MouseButtonEventArgs e) => 
            Process.Start(_item.Id);
    }
}
