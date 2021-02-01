using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class ProTypeController : Controller
    {
        private readonly ApplicationDBContext Db;

        public ProTypeController(ApplicationDBContext _Db)
        {
            Db = _Db;
        }

        public IActionResult Index()
        {
            var Types = Db.ProTypes.ToList();
            return View(Types);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProType T)
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
            var id = Db.ProTypes.Any(q => q.TypeId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var PType = Db.ProTypes.Find(Id);
            return View(PType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProType T)
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
            var id = Db.ProTypes.Any(q => q.TypeId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var PType = Db.ProTypes.Find(Id);
            return View(PType);
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
            var id = Db.ProTypes.Any(q => q.TypeId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var PType =Db.ProTypes.Find(Id);
            Db.ProTypes.Remove(PType);
            Db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
