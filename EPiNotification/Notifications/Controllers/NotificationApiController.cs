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
        public IEnumerable<NotificationMessageViewModel> Get(string userName, int count)
        {
            int totalCount;
            //return _notifier.GetUserNotifications(new UserNotificationsQuery() { Read = false, User = new NotificationUser() { UserName = userName } }, 1, count, out totalCount);

            return new[]{ new NotificationMessageViewModel(){
                Content = "this is some text",
                Subject = "A message",
                Saved = DateTime.Now.AddHours(-2).ToString("{yyyy/MM/dd H:mm:ss}"),
                Id = 1,
                Sender = "kalle"
                }
                , new NotificationMessageViewModel(){
                Content = "this is some other text",
                Subject = "Another message",
                Saved = DateTime.Now.AddHours(-3).ToString("{yyyy/MM/dd H:mm:ss}"),
                Id = 12,
                Sender =  "bertil"
                }
            };
        }

        [ActionName("markread")]
        public async Task MarkAsRead(int id)
        {
            await _notifier.MarkAsReadAsync(id);
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
