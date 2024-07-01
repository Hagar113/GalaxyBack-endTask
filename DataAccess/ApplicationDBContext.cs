using Microsoft.EntityFrameworkCore;
using Models.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext()
        {

        }
        public ApplicationDBContext(DbContextOptions options):base(options) 
        {
            
        }
        
        public DbSet<Users> users {  get; set; }
    }


}
