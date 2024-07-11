
using System;
using Microsoft.AspNetCore.Mvc;

namespace shopapp.webui.Controllers {
    public class HomeController: Controller {
        // localhost:5000/home
        public IActionResult Index() {
            int hour = DateTime.Now.Hour;

            string message = hour > 12 ? "Good Afternoon" : "Good Morning";

            ViewBag.Greeting = message;
            ViewBag.UserName = "John Doe";
            return View();
        }

        public IActionResult About() {
            return View();
        }

        public IActionResult Contact() {
            return View("MyView");
        }
    }
}