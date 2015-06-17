using EPiServer.Editor.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiNotification.Notifications.NotificationFormatter
{
    [EPiServer.ServiceLocation.ServiceConfiguration(typeof(INotificationFormatter))]
    public class DefaultNotificationFormatter : INotificationFormatter
    {
        static  String[] _supportedChannelNames = new string[]{"EmailChannelName", "LoggerChannelName"};

        public IEnumerable<FormatterNotificationMessage> FormatMessages(IEnumerable<FormatterNotificationMessage> notifications, string sender, string recipient, NotificationFormat format, string notificationChannelName = null)
        {
             return notifications;
        }

        public string FormatterName
        {
            get { return "Default"; }
        }

        public IEnumerable<NotificationFormat> GetSupportedFormats(string notificationChannelName)
        {
            return new NotificationFormat[] { new NotificationFormat() { FormatType = NotificationFormatType.Html } };
        }

        public IEnumerable<string> SupportedChannelNames
        {
            get 
            { 
                return _supportedChannelNames;
            }
        }
    }
}