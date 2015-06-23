using EPiServer.DataAccess;
using EPiServer.Notification;
using EPiServer.Security;
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
using System.Web.Security;

namespace NotificationTest.Controllers
{
    public class NotificationApiController : ApiController
    {
        private INotifier _notifier;
        private IUserNotificationRepository _userNotifications;
        private static object _lock = new object();

        public NotificationApiController()
        {
            _notifier = ServiceLocator.Current.GetInstance<INotifier>();
            _userNotifications = ServiceLocator.Current.GetInstance<IUserNotificationRepository>();
        }

        [ActionName("messages")]
        public IEnumerable<NotificationMessageViewModel> Get(string userName, int count)
        {
            int totalCount;
            return _userNotifications.GetUserNotifications(new UserNotificationsQuery() { Read = false, User = new NotificationUser(userName) }, 1, count, out totalCount)
                .Select(u => CreateViewModel(u));
        }

        [ActionName("getchannles")]
        public IEnumerable<System.Web.Mvc.SelectListItem> getchannles()
        {
            foreach (var f in ServiceLocator.Current.GetAllInstances<INotificationFormatter>())
            {
                foreach (var c in f.SupportedChannelNames)
                {
                    yield return new System.Web.Mvc.SelectListItem() { Text = c, Value = c };
                }
            }
        }



        [ActionName("markread")]
        [HttpPost]
        public async Task MarkAsRead(int id)
        {
            await _userNotifications.MarkUserNotificationAsReadAsync(id);
        }

        [ActionName("notify")]
        [HttpPost]
        public async Task Notify(NotificationMessageViewModel notificationMessage)
        {
            await _notifier.PostNotificationAsync(new NotificationMessage()
                {
                    Sender = new NotificationUser(notificationMessage.Sender ?? "sender@test.se"),
                    Recipients = new[] { new NotificationUser(notificationMessage.Recivier ?? "receiver@test.se" ) },
                    ChannelName = notificationMessage.Channel ?? "channelName",
                    Content = notificationMessage.Content ?? "content",
                    Subject = notificationMessage.Subject ?? "subject",
                    TypeName = "UserMessage",
                    SendAt = notificationMessage.SendAt
                });
        }

        [HttpPost]
        [ActionName("findusers")]
        public IEnumerable<string> FindUsers(string username)
        {
            string wildcardSymbol = MembershipExtensions.GetWildcardSymbolFromDefaultProvider();
            char[] charactersToTrim = (" " + wildcardSymbol).ToCharArray();

            string userCritera = username ?? "";
            var res = ServiceLocator.Current.GetInstance<QueryableNotificationUserService>().FindAsync(String.IsNullOrEmpty(userCritera) ? wildcardSymbol : wildcardSymbol + userCritera + wildcardSymbol, 0, Int32.MaxValue).Result;
            return res.PagedResult.Select(n => n.UserName);
        }

        [HttpPost]
        [ActionName("createusers")]
        public void CreateUsers(string nrofusers)
        {
            lock (_lock)
            {
                for (int i = 0; i < 10; i++)
                {
                    string name = "User" + new Random().Next(1, 100000);
                    MembershipCreateStatus status;
                    if (Membership.Provider.GetUser(name, false) == null)
                    {
                        Membership.Provider.CreateUser(name, "1234567", name + "@test.se", "q", "a", true, null, out status);
                    }
                };
            }
        }

        [HttpPost]
        [ActionName("task")]
        public void CreateUsers(int id)
        {
            ServiceLocator.Current.GetInstance<TaskDB>().Delete(id);
        }

        private NotificationMessageViewModel CreateViewModel(UserNotificationMessage u)
        {
            return new NotificationMessageViewModel()
            {
                Channel = u.ChannelName,
                Content = u.Content,
                Id = u.Id,
                Recivier = u.Recipient.DisplayName ?? u.Recipient.UserName,
                Saved = u.Notified.ToString("{yyyy/MM/dd H:mm:ss}"),
                Sender = u.Sender.DisplayName ?? u.Sender.UserName,
                Subject = u.Subject
            };
        }
    }
}
