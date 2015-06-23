using EPiNotification.Models.ViewModels;
using EPiServer.DataAccess;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPiNotification.Controllers
{
    public class TaskController : Controller
    {
        private TaskDB _taskRepository;

        public TaskController()
        {
            _taskRepository = ServiceLocator.Current.GetInstance<TaskDB>();
        }

        public ActionResult Index()
        {
            return View(new TasksViewModel 
            { 
                UserName = ControllerContext.HttpContext.User.Identity.Name,
                Tasks = _taskRepository.List(ControllerContext.HttpContext.User.Identity.Name)
            });
        }
	}
}