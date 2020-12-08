using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Core.ValueFactory;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.CourseAggregate
{

    public interface ICourseLoader
    {
        Task<bool> Delete(string courseId, int operaoterId);
        Task<Course> LoaderAsync(string id);
        Task<bool> Save(Course course);
    }

    public class CourseLoader : ICourseLoader
    {

        private readonly IToolbox toolbox;
        private readonly IValueFactory _vf;

        public CourseLoader(IToolbox toolbox, IValueFactory vf, string id)
        {
            this.toolbox = toolbox;
            _vf = vf;
            this.id = id;
        }

        private string cacheKey(string id) => String.Concat(this.toolbox.Options.Value.CourseCachePrefix, id);



        private string id;



        public async Task<bool> Delete(string courseId, int operaoterId)
        {
            var course = await this.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息不能为空{courseId}");

            course.Delete(operaoterId);

            await this.toolbox.Cachor.Remove(this.cacheKey(courseId));

            await this.toolbox.Sender.SendAsync(course.Events);

            return true;
        }


        public async Task<bool> Save(Course course)
        {
            bool rev = await this.toolbox.Cachor.SetAsync(this.cacheKey(course.ID), course);
            if (rev)
            {
                await this.toolbox.Sender.SendAsync(course.Events);
            }

            return rev;

        }



        public async Task<Course> LoaderAsync(string id)
        {

            this.id = id;


            var obj = await this.toolbox.Cachor.Get<Course>(cacheKey(id), async () =>
            {

                var entity = await this.toolbox.Reader.GetRepositoryAsync<CourseEntity>()
                        .SingleAsync(C => C.ID == id);

                var rev = this.toolbox.Mapper.Map<Course>(entity);

                if (rev == null)
                {
                    return null;
                }

                rev.Region = new UserPropVal
                {
                    Code = entity.RegionCode,
                    Name = entity.RegionName

                };
                rev.School = new UserPropVal
                {
                    Name = entity.SchoolName,
                    Code = entity.SchoolCode

                };
                rev.Creator = new UserPropVal
                {
                    Code = entity.CreatorCode,
                    Name = entity.CreatorName
                };

                //标签
                var tag = await this.toolbox.Reader.GetRepositoryAsync<TagsEntity>()
                            .Query(t => t.CourseId == id);

                rev.Tag = this.TagFromEntity(tag);





                //引用的学程
                var ds = await this.toolbox.Reader.GetRepositoryAsync<QuoteDsEntity>()
                        .Query(q => q.CourseId == id);

                rev.DsItems = this.DsFromEntity(ds);


                return rev;

            });


            if (obj is null)
            {
                return null;
            }



            this._vf.Register(obj, obj => obj.CollaboratorLib, LoadCollabrator);



            return obj;
        }


        // 加载协作人
        private async Task<CollabratorVal> LoadCollabrator()
        {
            var collabrators = await this.toolbox.Reader.GetRepositoryAsync<CollabratorEntity>()
                              .Query(c => c.CourseId == id);

            if (collabrators.NoData() is false)
            {

                var rev = new CollabratorVal();
                foreach (var item in collabrators)
                {
                    rev.Add(new(item.ObjId));
                }
                return rev;
            }

            return null;
        }





        private IList<QuoteDSVal> DsFromEntity(IList<QuoteDsEntity> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            var rev = new List<QuoteDSVal>();

            foreach (var item in entities)
            {
                rev.Add(new QuoteDSVal
                {
                    IsOpen = item.IsOpen,
                    CatalogId = item.CatalogId,
                    DsId = item.DsId,
                    DsName = item.DsName,
                    OperatorId = item.OperatorId,
                    OperatorName = item.OperatorName,
                    SortVal = item.SortVal,
                    IsOriginal = item.IsOriginal,
                    IsShared = item.IsShared
                });
            }
            return rev;
        }

        private TagVal TagFromEntity(IList<TagsEntity> entity)
        {

            if (entity == null || entity.Count == 0)
            {
                return null;
            }

            var tagVal = new TagVal
            {
                Items = new List<TagItem>()
            };


            foreach (var item in entity)
            {

                if (String.IsNullOrEmpty(item.TypeName))
                {
                    tagVal.Name = item.Name;
                    tagVal.Id = item.AssetId;
                    continue;
                }
                tagVal.Items.Add(new TagItem
                {
                    Name = item.Name,
                    TypeName = item.TypeName
                });

            }

            return tagVal;
        }



    }
}
