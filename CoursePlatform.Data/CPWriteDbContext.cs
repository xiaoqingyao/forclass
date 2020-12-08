using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations;
//using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data
{
    public class CPWriteDbContext : DbContext 
    {
        public CPWriteDbContext(DbContextOptions<CPWriteDbContext> dbContextOptions) : base(dbContextOptions)
        {


        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AllEntity();
           
            modelBuilder.BuildIndexesFromAnnotations();

        }




    }
}
