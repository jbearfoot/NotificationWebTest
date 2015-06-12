using System;
using System.Web.Mvc;
using NotificationTest.Routing;
using System.Web.Routing;

namespace EPiNotification
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RouteTable.Routes.RegisterNotificationRoutes();
            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }
    }
}