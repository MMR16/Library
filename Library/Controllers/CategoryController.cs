using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class CategoryController : Controller
    {
       private readonly ApplicationDBContext Db;

        public CategoryController(ApplicationDBContext _Db)
        {
             Db= _Db;
        }

        public IActionResult Index()
        {
            var CatList = Db.Categories.ToList();
            return View(CatList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category Cat)
        {
            if (ModelState.IsValid)
            {
                Db.Categories.Add(Cat);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(Cat);
        }
    }
}
