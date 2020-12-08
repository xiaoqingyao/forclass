using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Events;
using CoursePlatform.Events.Events;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.BusServices
{
    public class QuoteDsEventHandler : ICapSubscribe 
    {
        private readonly IUnitOfWork<CPWriteDbContext> _unitOfWork;
        private readonly IMapper _mapper;

        public QuoteDsEventHandler(IUnitOfWork<CPWriteDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [CapSubscribe(nameof(CourseDSQuoted))]
        public async Task DsQouted(CourseDSQuoted @event)
        {

            var res = this._unitOfWork.GetRepositoryAsync<QuoteDsEntity>();

            foreach (var item in @event.Items)
            {
                var entityItem = this._mapper.Map<QuoteDsEntity>(item);
                await res.AddAsync(entityItem);
            }
            this._unitOfWork.SaveChanges();
        }


        [CapSubscribe(nameof(CourseDSSorted))]
        public async Task DsSorted(CourseDSSorted @event)
        {

            var res = this._unitOfWork.GetRepositoryAsync<QuoteDsEntity>();

            foreach (var item in @event.Items)
            {
                await res.UpdateBach(q => q.CatalogId == item.CatalogId && q.CourseId == item.CourseId && q.DsId == item.DsId, q => new QuoteDsEntity
                {
                    SortVal = item.SortVal,
                    UpdateTime = DateTime.Now
                });
            }
            this._unitOfWork.SaveChanges();
        }

        [CapSubscribe(nameof(CourseDsStatusUpdated))]
        public async Task DsStatusUpdated(CourseDsStatusUpdated @event)
        {
            await this._unitOfWork.GetRepositoryAsync<QuoteDsEntity>()
                    .UpdateBach(q => q.DsId == @event.DsId && q.CourseId == @event.CourseId && q.CatalogId == @event.CatalogId, q => new QuoteDsEntity
                    {
                         IsOpen = @event.IsOpen,
                         UpdateTime = DateTime.Now
                    });

            this._unitOfWork.SaveChanges();
        }


        [CapSubscribe(nameof(CourseDsDeleted))]
        public async Task DsDeleted(CourseDsDeleted @event)
        {
            await this._unitOfWork.GetRepositoryAsync<QuoteDsEntity>()
                  .DelAsync(q => q.DsId == @event.DsId && q.CatalogId == @event.CatalogId && q.CourseId == @event.CourseId);
            this._unitOfWork.SaveChanges();
        }
    }
}
