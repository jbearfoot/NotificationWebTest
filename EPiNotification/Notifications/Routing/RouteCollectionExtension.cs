using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Mvc;

namespace NotificationTest.Routing
{
    public static class RouteCollectionExtension
    {
        public static void RegisterNotificationRoutes(this RouteCollection routes)
        {
            routes.Add(new WebSocketRoute("websockets/notification"));
            routes.MapRoute(
                name: "mvc",
                url: "{controller}/{action}",
                defaults: new { action = "index" });

            GlobalConfiguration.Configure(config => 
                {
                    config.MapHttpAttributeRoutes();

                    config.Routes.MapHttpRoute(
                             name: "DefaultApi",
                             routeTemplate: "api/{controller}/{id}",
                             defaults: new { id = RouteParameter.Optional }
                     );

                    config.Routes.MapHttpRoute(
                        name: "ControllerAndAction",
                        routeTemplate: "api/{controller}/{action}/{id}/{startIndex}",
                        defaults: new { startIndex = 0 }
                    );
                });
                              
        }
    }
}
