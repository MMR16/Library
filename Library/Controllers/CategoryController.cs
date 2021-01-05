﻿using Library.Data;
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

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id is null)
            {
                return BadRequest();
            }
            bool id = Db.Categories.Any(q => q.CatId == Id);
             if (id is false)
            {
                return NotFound();
            }
            var Obj = Db.Categories.Find(Id);
            return View(Obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category Cat)
        {
            if (ModelState.IsValid)
            {
                Db.Categories.Update(Cat);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(Cat);
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id is null)
            {
                return BadRequest();
            }
            bool id = Db.Categories.Any(q => q.CatId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var Obj = Db.Categories.Find(Id);
            return View(Obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteCat(int? Id)
        {
            if (Id is null)
            {
                return BadRequest();
            }
            bool id = Db.Categories.Any(q => q.CatId == Id);
            if (id is false)
            {
                return NotFound();
            }
            var Obj = Db.Categories.Find(Id);
            Db.Categories.Remove(Obj);
            Db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
