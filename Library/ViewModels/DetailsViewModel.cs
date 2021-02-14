using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class DetailsViewModel
    {
        public Product product { get; set; }
        public bool ExistsInCard { get; set; }
    }
}
