using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Library.Data
{

    //EntityFrameworkCore
    //EntityFrameworkCore.sqlserver

    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }

        public virtual DbSet<Category> Categories { set; get; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProType> ProTypes { get; set; }


    }
}
