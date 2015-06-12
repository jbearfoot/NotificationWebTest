using EPiServer.Editor.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationTest.Models
{
    public class NotificationUser : INotificationUser
    {
        public string DisplayName { get { return UserName; } set { } }

        public string UserName { get; set; }
    }
}
