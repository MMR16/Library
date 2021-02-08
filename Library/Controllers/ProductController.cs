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
            void CateList()
            {
                productVM.CategorySelectList = Db.Categories.Select(q => new SelectListItem
                {
                    Text = q.CatName,
                    Value = q.CatId.ToString()
                });
            }
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                //Function to CategoryListItem
                //create
                if (productVM.Product.ProId == 0)
                {
                    if (files.Count is not 1)
                    {
                        ViewBag.ImageError = "Please Upload one Image only don't think you'r so smart";
                        CateList();
                        return View(productVM);
                    }
                    else
                    {
                        string extention = Path.GetExtension(files[0].FileName);
                        if (Path.HasExtension(extention))
                        {
                            //for vaildate image by the type "image"
                            //better than extentions beccause i onky know about 5 extention of images
                            string image_type = string.Join("", files[0].ContentType.Take(5)).ToLower();

                            if (image_type == "image")
                            {
                                string webrootbath = webHostEnvironment.WebRootPath;
                                string upload = webrootbath + WC.ImagePath;
                                //string filename = Path.GetFileName(files[0].FileName); //to get uploaded image name
                                string filename = Guid.NewGuid().ToString(); // to create guid image name to avoid duplication errors
                                using (var filestream = new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                                {
                                    files[0].CopyTo(filestream);
                                }
                                productVM.Product.ProImage = filename + extention;
                                Db.Products.Add(productVM.Product);
                                Db.SaveChanges();
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        ViewBag.ImageError = "Please Upload Proper Image";
                        CateList();
                        return View(productVM);
                    }

                }
                //edit
                else
                {
                    //disable tracking //new feature
                    var proDB = Db.Products.AsNoTracking().FirstOrDefault(q => q.ProId == productVM.Product.ProId);
                    if (files.Count > 1)
                    {
                        ViewBag.ImageError = "Please Upload one Image only don't think you'r so smart";
                        CateList();
                        return View(productVM);
                    }
                    else if (files.Count is 1)
                    {
                        string extention = Path.GetExtension(files[0].FileName);
                        if (Path.HasExtension(extention))
                        {
                            string image_type = string.Join("", files[0].ContentType.Take(5)).ToLower();

                            if (image_type == "image")
                            {
                                string webrootbath = webHostEnvironment.WebRootPath;
                                string upload = webrootbath + WC.ImagePath;

                                string newfile = Guid.NewGuid().ToString(); // to create guid image name to avoid duplication errors

                                var oldfile = Path.Combine(upload, proDB.ProImage);
                                //deleting old file
                                if (System.IO.File.Exists(oldfile))
                                {
                                    System.IO.File.Delete(oldfile);
                                }
                                //addinf new file
                                using (var filestream = new FileStream(Path.Combine(upload, newfile + extention), FileMode.Create))
                                {
                                    files[0].CopyTo(filestream);
                                }
                                //changing file name in database
                                productVM.Product.ProImage = newfile + extention;
                            }
                            else
                            {
                                ViewBag.ImageError = "Please Upload Proper Image";
                                CateList();
                                return View(productVM);
                            }
                        }
                        else
                        {
                            ViewBag.ImageError = "Please Upload Proper Image";
                            CateList();
                            return View(productVM);
                        }
                    }

                    else
                    {
                        productVM.Product.ProImage = proDB.ProImage;
                    }
                    Db.Products.Update(productVM.Product);
                    Db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            CateList();
            return View(productVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var pro = Db.Products.Any(q => q.ProId == id);
            if (pro)
            {
                var product = Db.Products.Include(q => q.Category).FirstOrDefault(q => q.ProId == id);
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public IActionResult DeleteConfirm(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var pro = Db.Products.Any(q => q.ProId == id);
            if (pro)
            {
                var product = Db.Products.Find(id);
                //deleting Product image
                //getting image
                var ImageName = product.ProImage.ToString();
                string OldImage = webHostEnvironment.WebRootPath + WC.ImagePath + ImageName;
                System.IO.File.Delete(OldImage);

                //deleting from database
                Db.Remove(product);
                Db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}

