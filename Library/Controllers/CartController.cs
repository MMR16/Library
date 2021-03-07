using Library.Data;
using Library.Models;
using Library.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class CartController : Controller
    {
        readonly ApplicationDBContext db;
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
            if (session is not null && session.Count()>0)
            {
                //expilicit converting to list
                ShoppingCartList = session.ToList();

            }
            List<int?> porductInCart = ShoppingCartList.Select(q => q.ProductId).ToList();
            IEnumerable<Product> productList = db.Products.Where(q => porductInCart.Contains(q.ProId));
            return View(productList);
        }
    }
}
