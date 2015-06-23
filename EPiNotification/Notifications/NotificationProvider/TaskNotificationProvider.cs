using EPiServer.Notification;
using EPiServer.Personalization;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiNotification.Notifications.NotificationProvider
{
    [ServiceConfiguration(typeof(INotificationProvider))]
    public class TaskNotificationProvider : INotificationProvider
    {

        public string ProviderName
        {
            get { return "TaskProvider"; }
        }

        public NotificationFormat GetProviderFormat()
        {
            return new NotificationFormat() { IsUserNotification = false, SupportsHtml = true };
        }


        public void Send(IEnumerable<ProviderNotificationMessage> messages, Action<ProviderNotificationMessage> succeededAction, Action<ProviderNotificationMessage, Exception> failedAction)
        {
            foreach (var message in messages)
            {
                try
                { 
                    foreach (var recepient in message.RecipientAddresses)
                    {
                        var task = new Task()
                        {
                            AssignedTo = recepient,
                            Description = message.Content,
                            Owner = message.SenderAddress,
                            Subject = message.Subject
                        };
                        task.Save();
                        succeededAction(message);
                    }
                }catch(Exception ex)
                {
                    failedAction(message, ex);
                }

            }
        }

    }
}