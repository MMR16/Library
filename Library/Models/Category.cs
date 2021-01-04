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
        public int CatId { get; set; }


        [Required(ErrorMessage = "Please Enter Category Name")]
        [Display(Name = "Category Name")]
        [StringLength(50, MinimumLength = 3)]
        public string CatName { get; set; }

        [Required(ErrorMessage ="Please Enter Order Number")]
        [Display(Name = "Display Order")]
        [Range(1,1000)]
        public int DisplayOrder { get; set; }

    }
}
