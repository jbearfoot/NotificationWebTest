using EPiServer.Editor.Notification;
using EPiServer.ServiceLocation;
using NotificationTest.Models;
using NotificationTest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace NotificationTest.Controllers
{
    public class NotificationApiController : ApiController
    {
        private INotifier _notifier;

        public NotificationApiController()
        {
            _notifier = ServiceLocator.Current.GetInstance<INotifier>();
        }

        [ActionName("messages")]
        public IEnumerable<UserNotificationMessage> Get(string userName, int count)
        {
            int totalCount;
            return _notifier.GetUserNotifications(new UserNotificationsQuery() { Read = false, User = new NotificationUser() { UserName = userName } }, 1, count, out totalCount);
        }

        [ActionName("notify")]
        public async Task Post([ModelBinder] NotificationMessageViewModel notificationMessage)
        {
            await _notifier.NotifyAsync(new NotificationMessage()
                {
                    Sender = new NotificationUser { UserName = notificationMessage.Sender},
                    Recipients = new[] { new NotificationUser { UserName = notificationMessage.Recivier} },
                    ChannelName = notificationMessage.Channel,
                    Content = notificationMessage.Content,
                    Subject = notificationMessage.Subject,
                    TypeName = "UserMessage"
                });
        }
    }
}
