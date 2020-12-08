using AutoMapper;
using CoursePlatform.Application.Modules;
using CoursePlatform.Domain.Queries.Catalog;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Controllers
{
    public class CatalogController : CoursePlatformControllerBase
    {


        private readonly IBindQuery _bq;

        public CatalogController(IAppUser user, IMapper mapper, IBindQuery bq) : base(user, mapper)
        {
            this._bq = bq;
        }





        /// <summary>
        /// 查询目录，及目录引用的学程数 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury})]
        public Task<ReturnVal<IList<Asset.ApiDTO.CatalogResult>>> Get(QueryParam dto)
        {
            var rev = this._bq.GetAsync(dto);

            return RetOkAsync(rev);
        }




    }
}
