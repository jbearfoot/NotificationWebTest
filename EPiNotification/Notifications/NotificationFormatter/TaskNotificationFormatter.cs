using EPiServer.Notification;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiNotification.Notifications.NotificationFormatter
{
    [ServiceConfiguration(typeof(INotificationFormatter))]
    public class TaskNotificationFormatter : INotificationFormatter
    {
        public IEnumerable<FormatterNotificationMessage> FormatMessages(IEnumerable<FormatterNotificationMessage> notifications, string sender, string recipient, NotificationFormat format, string notificationChannelName = null)
        {
            return notifications;
        }

        public string FormatterName
        {
            get { return "TaskFormatter"; }
        }

        public IEnumerable<string> SupportedChannelNames
        {
            get { return new[] { "Task" }; }
        }
    }
}