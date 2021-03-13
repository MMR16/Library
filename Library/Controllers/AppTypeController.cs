using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class AppTypeController : Controller
    {
        private readonly ApplicationDBContext Db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AppTypeController(ApplicationDBContext _Db, IWebHostEnvironment _webHostEnvironment)
        {
            Db = _Db;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var Types =await Db.AppTypes.ToListAsync();
            return View(Types);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppType T)
        {
            if (ModelState.IsValid)
            {
                Db.Add(T);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(T);
        }

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id is null)
            {
                return BadRequest();
            }
            var id = Db.AppTypes.Any(q => q.TypeId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var AppType = Db.AppTypes.Find(Id);
            return View(AppType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppType T)
        {
            if (ModelState.IsValid)
            {
                Db.Update(T);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(T);
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id is null)
            {
                return BadRequest();
            }
            var id = Db.AppTypes.Any(q => q.TypeId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var AppType = Db.AppTypes.Find(Id);
            return View(AppType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteType(int? Id)
        {
            if (Id is null)
            {
                return BadRequest();
            }
            var id = Db.AppTypes.Any(q => q.TypeId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var AppType = Db.AppTypes.Find(Id);

            var pro = Db.Products.Where(q => q.TypeId == AppType.TypeId).ToList();
            if (pro is not null)
            {
                foreach (var item in pro)
                {
                    var ImageName = item.ProImage.ToString();
                    string OldImage = webHostEnvironment.WebRootPath + WC.ImagePath + ImageName;
                    System.IO.File.Delete(OldImage);
                }
            }

            Db.AppTypes.Remove(AppType);
            Db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
