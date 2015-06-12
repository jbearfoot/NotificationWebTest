using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationTest.Models.ViewModels
{
    public class NotificationViewModel
    {
        public IEnumerable<string> Channels { get; set; }
        public string UserName { get; set; }
    }
}
