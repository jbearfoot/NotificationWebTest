using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationTest.Models.ViewModels
{

    public class NotificationMessageViewModel
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Recivier { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? SendAt { get; set; }
        public string Channel { get; set; }
        public string Saved { get; set; }
    }
}
