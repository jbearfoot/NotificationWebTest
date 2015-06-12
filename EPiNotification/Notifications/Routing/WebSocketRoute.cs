using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace NotificationTest.Routing
{
    public class WebSocketRoute : RouteBase
    {
        private string _path;

        public WebSocketRoute(string path)
        {
            _path = path;
        }

        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        {
            
            if (httpContext.Request.FilePath.Substring(1).Equals(_path, StringComparison.OrdinalIgnoreCase))
            {
                return new RouteData(this, new WebSocketRouteHandler());
            }
            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
