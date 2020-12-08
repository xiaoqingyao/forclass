using CoursePlatform.Data.Entities;
using CoursePlatform.infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.PartnerAggregate
{
    public interface IPartnerService
    {
        Task<bool> CourselistRemove(string courseId, int objId, PartnerType type);
        Task<bool> CourseToList(int creator,string courseId, string name, int objId, int parentId);
        Task<Partner> GetOrCreate(int objId, string name, int parentId);
    }

    public class PartnerService : IPartnerService
    {
        private readonly IToolbox _tools;

        public PartnerService(IToolbox tools)
        {
            _tools = tools;
        }

        #region Internal function...
        private string Key(int objId, PartnerType type) => String.Concat(this._tools.Options.Value.PartnerCachePrefix, objId, "_", (int)type);
        private Task<Partner> LoaderAsync(int objId, PartnerType type)
        {
            var key = this.Key(objId, type);

            var obj = this._tools.Cachor.Get(key, async () =>
            {
                var entity = await this._tools.Reader.GetRepositoryAsync<PartnerEntity>()
                           .SingleAsync(p => p.ResourceId == objId && p.Type == (int)type);
                if (entity is null)
                {
                    return null;
                }

                Partner p = this._tools.Mapper.Map<Partner>(entity);
                return p;

            });
            return obj;
        }

        private async Task Save(Partner p)
        {
            await this._tools.Cachor.SetAsync(this.Key(p.ResourceId, p.Type), p);
            await this._tools.Sender.SendAsync(p.Events);

        }
        #endregion



        #region Get partner if null create a new...
        public async Task<Partner> GetOrCreate(int objId, string name, int parentId)
        {
            PartnerType type = parentId == 0 ? PartnerType.Region : PartnerType.School;

            Partner p = await this.LoaderAsync(objId, type);
            if (p is not null)
            {
                return p;
            }
            p = new()
            {
                ParentId = parentId
            };
            p.Apply(this._tools.DomainSetter[DomainSetter.ID]);
            p.Create(objId, name, type, parentId);
            await this.Save(p);
            return p;

        }
        #endregion



        #region 上架课程...
        public async Task<bool> CourseToList(int creator,string courseId, string name, int objId, int parentId)
        {
            var p = await this.GetOrCreate(objId, name, parentId);

            var ret = p.AddToList(courseId, creator);

            if (ret is false)
            {
                return false;
            }

            await this.Save(p);

            return true;
        }
        #endregion



        #region 下架课程


        public async Task<bool> CourselistRemove(string courseId, int objId, PartnerType type)
        {
            var p = await this.LoaderAsync(objId, type);
            Prosecutor.NotNull(p, "合伙信息为空");

            bool ret = p.Unlisted(courseId);

            if (ret is false)
            {
                return false;
            }
            await this.Save(p);

            return true;

        }


        #endregion


    }
}
