using EPiServer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Notification;

namespace EPiNotification.Notifications
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class NotificationsInitalizer : IInitializableModule
    {
        public void Initialize(EPiServer.Framework.Initialization.InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<INotificationPreferenceRegister>().RegisterDefaultPreference("Task", "TaskProvider", (userName) => userName);
        }

        public void Uninitialize(EPiServer.Framework.Initialization.InitializationEngine context)
        {
           
        }
    }
}