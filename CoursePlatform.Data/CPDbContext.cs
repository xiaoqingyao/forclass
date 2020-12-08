using CoursePlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations;

namespace CoursePlatform.Data
{
    public class CPDbContext : DbContext
    {


        public const string CONN = "Conn";

        public const string WriteCONN = "WriteConn";



        public readonly ILoggerFactory loggerFactory;


        public CPDbContext()
        {

//#if DEBUG
//            this.loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
//#endif
        }


        public CPDbContext(DbContextOptions<CPDbContext> dbContextOptions) : base(dbContextOptions)
        {


        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

//#if DEBUG
//            optionsBuilder.UseLoggerFactory(this.loggerFactory).EnableSensitiveDataLogging();
//#endif
            //optionsBuilder.EnableSensitiveDataLogging();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.AllEntity();

            //modelBuilder.BuildIndexesFromAnnotations();

        }



    }
}
