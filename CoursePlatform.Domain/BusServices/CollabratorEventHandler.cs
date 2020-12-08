using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Events.Events;
using DotNetCore.CAP;
using EasyCaching.Core.Interceptor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper.Internal;
using System.Threading.Tasks;
using CoursePlatform.Events.Events.Collabrator;

namespace CoursePlatform.Domain.BusServices
{
    public class CollabratorEventHandler : EventHandlerBase, ICapSubscribe
    {
        public CollabratorEventHandler(IUnitOfWork<CPWriteDbContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }



        [CapSubscribe(nameof(CollaboratorAdded))]
        public async Task AddAsync(CollaboratorAdded @event)
        {

            var items = @event.Items.Select(i =>
            {
                var rev = this._mapper.Map<CollabratorEntity>(i);
                rev.CourseId = @event.CourseId;
                return rev;
            });


            await this._unitOfWork.GetRepositoryAsync<CollabratorEntity>()
                  .AddAsync(items);


            this._unitOfWork.SaveChanges();

        }

        [CapSubscribe(nameof(CollabratorReplaced))]
        public async Task ReplaceAsync(CollabratorReplaced @event)
        {

            await this._unitOfWork.GetRepositoryAsync<CollabratorEntity>()
                .DelAsync(c => c.CourseId == @event.CourseId);

            if (@event.Items is not null and { Count: > 0 })
            {


                var items = @event.Items.Select(i =>
                {
                    var rev = this._mapper.Map<CollabratorEntity>(i);
                    rev.CourseId = @event.CourseId;
                    return rev;
                });

                await this._unitOfWork.GetRepositoryAsync<CollabratorEntity>()
                      .AddAsync(items);

            }

            this._unitOfWork.SaveChanges();

        }





        [CapSubscribe(nameof(CollabratorAllDeleted))]
        public async Task CollabratorAllDeletedAction(CollabratorAllDeleted @event)
        {

            await this._unitOfWork.GetRepositoryAsync<CollabratorEntity>()
    .DelAsync(c => c.CourseId == @event.CourseId);

            this._unitOfWork.SaveChanges();

        }




    }
}
