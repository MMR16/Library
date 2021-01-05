using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Product
    {
        [Key]
        public int ProId { get; set; }

        [Required(ErrorMessage ="You Must Enter Product Name")]
        [StringLength(maximumLength:50,MinimumLength =3,ErrorMessageResourceName = "Product Name Must Be Between 3 & 50 Letters")]
        public string ProName { get; set; }

        [StringLength(maximumLength:int.MaxValue,MinimumLength =5,ErrorMessage ="Description Must Be More Than 5 Letters")]
        [DataType(DataType.MultilineText)]
        public string ProDescription { get; set; }

        [Range(1,100_000,ErrorMessage ="Price Range Must Be 1 ~ 100,000")]
        public double  ProPrice { get; set; }

        public string ProImage { get; set; }

        [ForeignKey("Category")]
        public int CatId { get; set; }

        public virtual Category Category { get; set; }
    }
}
