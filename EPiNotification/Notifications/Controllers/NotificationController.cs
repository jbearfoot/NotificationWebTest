using EPiServer.Notification;
using EPiServer.ServiceLocation;
using NotificationTest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NotificationTest.Controllers
{
    public class NotificationController : Controller
    {
        private IEnumerable<string> _channelNames;

        public NotificationController()
        {
            _channelNames = ServiceLocator.Current.GetAllInstances<INotificationFormatter>().SelectMany(p => p.SupportedChannelNames).Distinct();
        }

        public ActionResult Index()
        {
            return View(new NotificationViewModel() { Channels = _channelNames, UserName = HttpContext.User.Identity.Name });
        }

    }
}
