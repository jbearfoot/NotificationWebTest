using EPiServer.Editor.Notification;
using EPiServer.ServiceLocation;
using Newtonsoft.Json;
using NotificationTest.Models;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace NotificationTest.HttpHandlers
{
  

    public class WebSocketHandler : IHttpHandler
    {
       
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                var hub = new NotificationHub(ServiceLocator.Current.GetInstance<INotifier>());
                context.AcceptWebSocketRequest(hub.ProcessWebSocketRequest);
            }
        }
    }

    public class NotificationHub
    {
        private INotifier _notifier;
        AspNetWebSocketContext _webSocketContext;

        public NotificationHub(INotifier notifier)
        {
            _notifier = notifier;
            _notifier.MessageSaved += MessageSaved;
        }

        internal async Task ProcessWebSocketRequest(AspNetWebSocketContext webSocketContext)
        {
            _webSocketContext = webSocketContext;
            await SendCurrentNumberForUser(new NotificationUser() { UserName = _webSocketContext.User.Identity.Name });
            try
            {
                var inputBuffer = new ArraySegment<byte>(new byte[1024]);
                while (true)
                {
                    var result = await _webSocketContext.WebSocket.ReceiveAsync(inputBuffer, CancellationToken.None);
                    if (_webSocketContext.WebSocket.State != WebSocketState.Open)
                    {
                        Close();
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Close();
            }
        }

        private async void MessageSaved(object sender, NotificationEventArgs e)
        {
            if (_webSocketContext.WebSocket.State != WebSocketState.Open)
            {
                await Close();
                return;
            }

            var hubUser = e.NotificationMessage.Recipients.FirstOrDefault(u => u.UserName.Equals(_webSocketContext.User.Identity.Name, StringComparison.OrdinalIgnoreCase));
            if (hubUser != null)
            {
                await SendCurrentNumberForUser(hubUser);
            }
        }

        private async Task SendCurrentNumberForUser(INotificationUser user)
        {
            var anonymus = new { Count = _notifier.GetUserNotificationsCount(new UserNotificationsQuery() { User = user, Read = false }) };

            var json = JsonConvert.SerializeObject(anonymus);
            var output = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            await _webSocketContext.WebSocket.SendAsync(output, WebSocketMessageType.Text, true, CancellationToken.None);
        }


        public async Task Close()
        {
            _notifier.MessageSaved -= MessageSaved;
            if (_webSocketContext.WebSocket.State == WebSocketState.Open)
            {
                await _webSocketContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
            }
        }
    }
}
