using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.ViewModels;
using Library.Models;


namespace Library.ViewModels
{
    public class ProductUserViewModel
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<Product> productList { get; set; }


        public ProductUserViewModel()
        {
            productList = new List<Product>();
        }
    }
}
