using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Required(ErrorMessage = "You Must Enter Product Name")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Product Name Must Be Between 3 & 50 Letters")]
        [Display(Name = "Name")]
        public string ProName { get; set; }

        [StringLength(maximumLength: int.MaxValue, MinimumLength = 5, ErrorMessage = "Description Must Be More Than 5 Letters")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        public string ProDescription { get; set; }

        [StringLength(maximumLength:80, MinimumLength = 10, ErrorMessage = "Description Must be between 10 ~ 80 Letters")]
        [DataType(DataType.Text)]
        [DisplayName("Short Description")]
        [Required(ErrorMessage ="Please Write Short Description Of Product")]
        public string ProShortDescription { get; set; }

        [Range(1, 100_000, ErrorMessage = "Price Range Must Be 1 ~ 100,000")]
        [Display(Name = "Price")]
        public double ProPrice { get; set; }

        [DisplayName("Image")]
        public string ProImage { get; set; }

        [Required(ErrorMessage = "Please Choose Currency")]
        [Display(Name = "Currency")]
        public Currency Currencies { get; set; }

        //ForeignKey
        [ForeignKey("Category")]
        [Required(ErrorMessage = "Please Choose A Category")]
        [Display(Name = "Category")]
        public int CatId { get; set; }

        [ForeignKey("AppType")]
        [Required(ErrorMessage = "Please Choose App Type")]
        [Display(Name = "App Type")]
        public int TypeId { get; set; }

        //Navigation Property
        public virtual Category Category { get; set; }
        public virtual AppType AppType { get; set; }

    }
}
