using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexte
{
    class ApplicationDbContexte: DbContext
    {
        public ApplicationDbContexte(DbContextOptions<ApplicationDbContexte> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //TODO : Configure your entity mappings here if needed
        }
        // Define your DbSets (tables) here
        public DbSet<Product> Products { get; set; }
    }

}
