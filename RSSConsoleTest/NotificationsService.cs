using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace RSSConsoleTest
{
    public class NotificationsService
    {
        private const string AppId = "RSS_CONSOLE_TEST";
        private readonly ToastNotifier _toastNotifier;

        public NotificationsService()
        {
            _toastNotifier = ToastNotificationManager.CreateToastNotifier(AppId);
        }

        public void ShowNotification(XmlDocument xmlNotification) => _toastNotifier.Show(new ToastNotification(xmlNotification));
    }
}
