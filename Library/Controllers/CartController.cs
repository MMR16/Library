using Library.Data;
using Library.Models;
using Library.Utility;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        readonly ApplicationDBContext db;

        //No Need to Bind Property AS parameter It Will READ At Entire Class
        [BindProperty]
        public ProductUserViewModel ProductUserVM { get; set; }

        public CartController(ApplicationDBContext _db)
        {
            db = _db;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            var session = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessinCart);
            //list
            //var session = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessinCart);
            if (session is not null && session.Count() > 0)
            {
                //expilicit converting to list
                ShoppingCartList = session.ToList();

            }
            List<int?> porductInCart = ShoppingCartList.Select(q => q.ProductId).ToList();
            IEnumerable<Product> productList = db.Products.Where(q => porductInCart.Contains(q.ProId));
            return View(productList);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var product = db.Products.Any(q => q.ProId == id);
            if (product)
            {
                List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
                var session = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessinCart);
                if (session is not null && session.Count() > 0)
                {
                    ShoppingCartList = session.ToList();
                }
                ShoppingCartList.Remove(ShoppingCartList.FirstOrDefault(q => q.ProductId == id));
                HttpContext.Session.Set(WC.SessinCart, ShoppingCartList);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Index))]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        [HttpGet]
        public IActionResult Summary()
        {
            //Get User Id
            //var claimIdentity = User.Identity as ClaimsIdentity;
            //var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            //or
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            var session = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessinCart);
            if (session is not null && session.Count() > 0)
            {
                ShoppingCartList = session.ToList();

            }
            List<int?> porductInCart = ShoppingCartList.Select(q => q.ProductId).ToList();
            IEnumerable<Product> productList = db.Products.Where(q => porductInCart.Contains(q.ProId));

            ProductUserVM = new ProductUserViewModel()
            {
                AppUser = db.AppUsers.FirstOrDefault(q=>q.Id == UserId)
            };


            return View(ProductUserVM);

        }
    }
}
