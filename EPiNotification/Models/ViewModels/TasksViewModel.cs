using EPiServer.Personalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiNotification.Models.ViewModels
{
    public class TasksViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }
        public string UserName { get; set; }
    }
}