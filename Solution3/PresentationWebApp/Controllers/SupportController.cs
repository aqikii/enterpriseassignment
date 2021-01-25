using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PresentationWebApp.Controllers
{
    public class SupportController : Controller
    {
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string email, string query)
        {

            if (string.IsNullOrEmpty(query))
            TempData["warning"] = "type in some question";
            else
            TempData["feedback"] = "Thank you for getting in touch with us. We will answer back asap";

            return View();
        }
    }
}