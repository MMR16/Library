using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library.ViewModels;

namespace Library.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext Db;

        public ProductController(ApplicationDBContext _Db)
        {
            Db = _Db;
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

            ProductViewModel productVM= new ProductViewModel()
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

        [HttpPost]
        public IActionResult UpdCreate()
        {
            return View();
        }


    }
}
