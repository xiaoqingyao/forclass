using CoursePlatform.Data.EFProvider.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Data
{
    public static class DataEntentions
    {
        public const string IsDebug = "IsDebug";

        public static IServiceCollection AddDbSorte(this IServiceCollection servcies, string connString, string writeConnStr, bool isDebug)
        {

            servcies.AddDbContext<CPDbContext>(opts =>
            {
                opts.UseSqlServer(connString);
            })
                .AddDbContext<CPWriteDbContext>(opts =>
                {
                    opts.UseSqlServer(writeConnStr);
                })
                .AddUnitOfWork<CPDbContext, CPWriteDbContext>();




            return servcies;
        }




    }
}
