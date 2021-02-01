using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Category
    {
        [Key]
        [Required(ErrorMessage ="Please Choose A Category")]
        public int CatId { get; set; }

        [Required(ErrorMessage = "Please Enter Category Name")]
        [Display(Name = "Category Name")]
        [StringLength(50, MinimumLength = 3,ErrorMessage ="Category Name Must Be Between 3 & 50 Letters")]
        public string CatName { get; set; }

        [Required(ErrorMessage ="Please Enter Order Number")]
        [Display(Name = "Display Order")]
        [Range(1,int.MaxValue,ErrorMessage ="Order Must Be Greater Than Zero")]
        public int DisplayOrder { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}
