using Library.Data;
using Library.Models;
using Library.Utility;
using Library.ViewModels;
using Microsoft.AspNetCore.Http;
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
                products = Db.Products.Include(q => q.Category).Include(w => w.AppType).ToList(),
                categories = Db.Categories.Include(q => q.Products).ToList()
            };
            return View(home);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var exist = Db.Products.Any(q => q.ProId == id);
            if (exist)
            {
                //retriving items from session th check if the product is in the cart or not
                List<ShoppingCart> ShopppingCartList = new List<ShoppingCart>();
                var ItemsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessinCart);
                if (ItemsList is not null && ItemsList.Count() > 0)
                {
                    ShopppingCartList = ItemsList;
                }
                //to get all product details & setting ExistsInCard to false to allow user adding it in shopping cart
                var product = new DetailsViewModel()
                {
                    product = Db.Products.Include(q => q.Category).Include(a => a.AppType).FirstOrDefault(q => q.ProId == id),
                    ExistsInCard = false
                };
                //check if the item is in the list retriving from session so the boolean ExistsInCard will be true 
                //else it will be false
                foreach (var item in ShopppingCartList)
                {
                    if (item.ProductId == id)
                    {
                        product.ExistsInCard = true;
                    }
                    //else
                    //{
                    //    product.ExistsInCard = false;
                    //}
                }
                return View(product);
            }
            return NotFound();
        }

        //!important
        //using session 
        [HttpPost]
        [ActionName(nameof(Details))]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPost(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var exist = Db.Products.Any(q => q.ProId == id);
            if (exist)
            {
                List<ShoppingCart> ShopppingCartList = new List<ShoppingCart>();
                //Get items in session
                var ItemsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessinCart);
                //if session is empty We skip Adding the item
                //get session items if exists (old items if session already had them) 
                if (ItemsList is not null && ItemsList.Count() > 0)
                {
                    ShopppingCartList = ItemsList;
                }
                //adding new item to session
                //it is alist so we can add more than one item
                //if there is any old items it will add new items with them
                ShopppingCartList.Add(new ShoppingCart() { ProductId = id });
                //setting session with key WC.SessinCart & new value whitch is the list ShopppingCartList with new items
                //Update session with new list
                HttpContext.Session.Set(WC.SessinCart, ShopppingCartList);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        //RemoveFromCart
        //!session
        [HttpGet]
        public IActionResult RemoveFromCart(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var exist = Db.Products.Any(q => q.ProId == id);
            if (exist)
            {
                //retriving items from session th check if the product is in the cart or not
                List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
                var ItemsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessinCart);
                if (ItemsList is not null && ItemsList.Count() > 0)
                {
                    shoppingCartList = ItemsList;
                }
                //get the item to delete
                var ItemToRemove = shoppingCartList.FirstOrDefault(q => q.ProductId == id);
                //removing it from List
                if (ItemToRemove is not null)
                {
                    shoppingCartList.Remove(ItemToRemove);
                }
                //Update session with new list
                HttpContext.Session.Set(WC.SessinCart, shoppingCartList);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
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
