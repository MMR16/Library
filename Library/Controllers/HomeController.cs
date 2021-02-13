using Library.Data;
using Library.Models;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext Db;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext _DB)
        {
            _logger = logger;
            Db = _DB;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel home = new HomeViewModel()
            {
                products = Db.Products.Include(q=>q.Category).Include(w=>w.AppType).ToList(),
                categories = Db.Categories.Include(q => q.Products).ToList()
            };
            return View(home);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
