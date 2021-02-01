using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class ProType
    {
        [Key]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Please Enter Product Type")]
        [Display(Name = "Product Type")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The Product Type Must Be Between 3 & 30 Letters")]
        public string TypeName { get; set; }
    }
}
