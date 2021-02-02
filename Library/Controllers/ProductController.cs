﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Library.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext Db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDBContext _Db, IWebHostEnvironment _webHostEnvironment)
        {
            Db = _Db;
            webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult Index()
        {
            var Products = Db.Products.Include(w => w.Category).ToList();
            return View(Products);
        }

        //Update - Create
        [HttpGet]
        public IActionResult UpdCreate(int? Id)
        {
            //SelectListItem new In .NET Core insted of SelectList & More Better

            ProductViewModel productVM = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = Db.Categories.Select(q => new SelectListItem
                {
                    Text = q.CatName,
                    Value = q.CatId.ToString()

                })
            };

            #region Using ViewBag
            ////SelectListItem new In .NET Core insted of SelectList & More Better
            //// var list = new SelectList(Db.Categories.ToList(), "CatId", "CatName",1);
            //IEnumerable<SelectListItem> CategoryDropDown = Db.Categories.Select(q => new SelectListItem
            //{
            //    Text = q.CatName,
            //    Value = q.CatId.ToString()
            //    //The Seleccted Is In View
            //    //,Selected = q.CatName.Any()
            //}); 
            //ViewBag.CategoryDropDown = CategoryDropDown;
            //// Create
            //if (Id is null)
            //{
            //    //return View(product);
            //    return View();
            //}

            ////Error Page Because Sending Id Not Found In DataBase
            //Product product = Db.Products.Find(Id);
            //if (product is null)
            //{
            //    return NotFound();
            //}

            //return View(product);
            #endregion
            // Create
            if (Id is null)
            {
                //passing productVM for DropDown List
                return View(productVM);
            }

            //Error Page Because Sending Id Not Found In DataBase
            productVM.Product = Db.Products.Find(Id);
            if (productVM.Product is null)
            {
                return NotFound();
            }

            return View(productVM);
        }

        //uploading image in .NET Core
        //very important
        // enable enctype="multipart/form-data" in form
        //inject IWebHostEnvironment
        //use the code below I/O
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdCreate(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
           
            {
                var files = HttpContext.Request.Form.Files;
                string webrootbath = webHostEnvironment.WebRootPath;
                if (productVM.Product.ProId == 0)
                {
                    string upload = webrootbath + WC.ImagePath;
                    //string filename = Path.GetFileName(files[0].FileName); //to get image name
                    string filename = Guid.NewGuid().ToString(); // to create guid image name to avoid duplication errors
                    string extention = Path.GetExtension(files[0].FileName);
                    using (var filestream = new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    productVM.Product.ProImage = filename + extention;
                    Db.Products.Add(productVM.Product);
                    Db.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }


    }
}