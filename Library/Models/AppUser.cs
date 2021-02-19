using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(15,MinimumLength =2,ErrorMessage = "First Name Must Be Between 2~15 Letters")]
        public string UserFName { get; set; }

        [StringLength(15, MinimumLength = 2, ErrorMessage = "Last Name Must Be Between 2~15 Letters")]
        public string UserLName { get; set; }

        public string UserImage { get; set; }

  
    }
}
