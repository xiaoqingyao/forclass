using AutoMapper;
using CoursePlatform.Application.Modules;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Tools;
using CoursePlatform.Infrastructure.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Controllers
{
    public class ToolsController : CoursePlatformControllerBase
    {

        private readonly IUnitOfWork<CPDbContext> _unitOfWork;
        private readonly ICachingProvider _cachor;
        private readonly IOptions<DomainOptions> _opt;

        public ToolsController(IAppUser user, IMapper mapper, IUnitOfWork<CPDbContext> uwork, ICachingProvider cachor, IOptions<DomainOptions> opt) : base(user, mapper)
        {
            this._unitOfWork = uwork;
            this._cachor = cachor;
            this._opt = opt;
        }



        /// <summary>
        /// 移除缓存 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("removeCache")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Tools })]
        public Task<bool> RemoveCahce(IdDto dto)
        {
            return this._cachor.Remove(String.Concat(this._opt.Value.CourseCachePrefix, dto.CourseId)); 
        }


        /// <summary>
        /// 系统数据配置 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("syscfg")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Tools })]
        public async Task<ReturnVal<bool>> SysConf(SysConfigDTO dto)
        {
            if (dto.Key != Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"))
            {
                throw new CPValidateExceptions("无权限");
            }

            var reps = this._unitOfWork.GetRepositoryAsync<SysConfigEntity>();

            var entity = await reps.SingleAsync(_ => true);

            if (entity == null)
            {
                entity = new()
                {
                    TagAttr = JsonConvert.SerializeObject(dto.TagAttrs)
                };
            }
            else
            {
                entity.TagAttr = JsonConvert.SerializeObject(dto.TagAttrs);
            }

            if (entity.IndentityId > 0)
            {
                 reps.UpdateAsync(entity);
            }
            else
            {
                await reps.AddAsync(entity);
            }

            this._unitOfWork.SaveChanges();


            return this.RetOk(true);

        }

    }


}
