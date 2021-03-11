using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebNongNghiep.Client.Controllers
{
    public class ClientHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
