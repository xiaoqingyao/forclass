using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Validators;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Permission
{
    public interface IPermission
    {
        bool IsSchoolAuditor { get; }
        bool IsRegionAuditor { get; }
        bool IsOwner { get; }
        bool EnableEdit { get; }
        bool IsCollabrator { get; }

        Task<bool> IsRegionAuditorAsync(int id);

        Task<bool> IsSchoolAuditorAsync(int id);
        IList<CourseOperationItem> PersonalEnableOperation();
        Task<IList<CourseOperationItem>> RegionEnableOperationAsync();
        Task<IList<CourseOperationItem>> SchoolEnableOperationAsync();
        void SetValidateParameter(ValidateParameter param);
    }



    public class PermissionServices : IPermission
    {
        private readonly IAppUser _appUser;

        private readonly IOptions<DomainOptions> _options;

        public PermissionServices(IAppUser appUser, IOptions<DomainOptions> options)
        {
            _appUser = appUser;
            _options = options;
        }

        public bool IsSchoolAuditor
        {
            get
            {
                if (this._appUser.Roles.NoData())
                {
                    return false;
                }
                return this._appUser.Roles.Any(r => this._options.Value.SchoolAuditor.Contains(r.Code));
            }
        }

        public bool IsRegionAuditor
        {
            get
            {
                if (this._appUser.Roles.NoData())
                {
                    return false;
                }
                return this._appUser.Roles.Any(r => this._options.Value.RegionAuditor.Contains(r.Code));
            }
        }

        public async System.Threading.Tasks.Task<bool> IsRegionAuditorAsync(int id)
        {
            if (this.IsRegionAuditor is false)
            {
                return false;
            }
            var re = await this._appUser.Get(AppUserFlagData.OrgRegion);
            return re.Code == id;
        }


        public async System.Threading.Tasks.Task<bool> IsSchoolAuditorAsync(int id)
        {
            if (this.IsSchoolAuditor is false)
            {
                return false;
            }
            var re = await this._appUser.Get(AppUserFlagData.OrgSchool);
            return re.Code == id;
        }

        private ValidateParameter _param;

        public void SetValidateParameter(ValidateParameter param)
        {
            this._param = param;
        }

        //public bool EnableDelete => this._appUser.UserId > 0 && this._course.Creator.Code == this._appUser.UserId && (this._course.Status == CourseStatus.Draft || this._course.Status == CourseStatus.Reject);



        public async Task<IList<CourseOperationItem>> RegionEnableOperationAsync()

           
        {

            //await Task.CompletedTask;


            var re = await this._appUser.GetRegion();

            if (this.IsRegionAuditor == false || this._param.RegionId != re.Code)
            {
                return null;
            }

  
            //默认或拒绝
            if (this._param.RegionStatus == CourseStatus.RegiogDefault || this._param.RegionStatus == CourseStatus.RegionReject)
            {
                return null;
            }

            IList<CourseOperationItem> items = new List<CourseOperationItem>();

            //审核

            if (this._param.RegionStatus == CourseStatus.RegionReview)
            {
                items.Add(this._options.Value.CourseOperation["RegionPass"]);
                items.Add(this._options.Value.CourseOperation["RegionReject"]);
               
            }

            if (this._param.RegionStatus == CourseStatus.RegionAccept || this._param.RegionStatus == CourseStatus.RegionUnlisted)
            {
                items.Add(this._options.Value.CourseOperation["RegionListed"]);
                items.Add(this._options.Value.CourseOperation["RegionReject"]);
            }

            if (this._param.RegionStatus == CourseStatus.RegionListed)
            {
                items.Add(this._options.Value.CourseOperation["RegionListRemove"]);
            }



            return items;
           
        }


        public async Task<IList<CourseOperationItem>> SchoolEnableOperationAsync()
        {
            var sc = await this._appUser.GetSchool();

            //await Task.CompletedTask;

            if (this.IsSchoolAuditor == false || this._param.SchoolId != sc.Code)
            {
                return null;
            }


            //默认或拒绝
            if (this._param.SchoolStatus == CourseStatus.Draft )
            {
                return null;
            }

            IList<CourseOperationItem> items = new List<CourseOperationItem>();

            //审核

            if (this._param.SchoolStatus == CourseStatus.Review)
            {
                items.Add(this._options.Value.CourseOperation["SchoolPass"]);
                items.Add(this._options.Value.CourseOperation["SchoolReject"]);

            }

            if (this._param.SchoolStatus == CourseStatus.Reject)
            {
                
                items.Add(this._options.Value.CourseOperation["SchoolPass"]);
            }

            if (this._param.SchoolStatus == CourseStatus.Accept || this._param.SchoolStatus == CourseStatus.UnListed)
            {
                items.Add(this._options.Value.CourseOperation["SchoolListed"]);

                //如果区域状态为默认或拒绝
                if (this._param.RegionStatus is CourseStatus.RegiogDefault or CourseStatus.RegionReject)
                {
                    items.Add(this._options.Value.CourseOperation["SchoolReject"]);

                }

                if (this._param.RegionStatus is CourseStatus.RegiogDefault or  CourseStatus.RegionReject)
                {
                    items.Add(this._options.Value.CourseOperation["RegionReview"]);

                }
            }

            if (this._param.RegionStatus is CourseStatus.RegionReview)
            {
                items.Add(this._options.Value.CourseOperation["RegionReviewCancel"]);
            }

            if (this._param.SchoolStatus == CourseStatus.Listed)
            {
                items.Add(this._options.Value.CourseOperation["SchoolListRemove"]);

                //区域提审
                if (this._param.RegionStatus is CourseStatus.RegiogDefault or CourseStatus.RegionReject)
                {
                    items.Add(this._options.Value.CourseOperation["RegionReview"]);

                }

            }

            return items;

        }


        public bool IsOwner => this._appUser.UserId == this._param.Creator;


       
        public bool EnableEdit => (this.IsOwner is true || this.IsCollabrator is true) && this._param.SchoolStatus == CourseStatus.Draft;


        public IList<CourseOperationItem> PersonalEnableOperation()
        {
            if (this.IsOwner is false)
            {
                return null;
            }

            if (this._param.SchoolStatus == CourseStatus.Review)
            {
                return new List<CourseOperationItem> { 
                    this._options.Value.CourseOperation["SchoolReviewCancel"] 
                };
            }

            if (this._param.SchoolStatus != CourseStatus.Draft && this._param.SchoolStatus != CourseStatus.Reject)
            {
                return null;
            }

            return new List<CourseOperationItem>
            {
                this._options.Value.CourseOperation["SchoolReview"],
                this._options.Value.CourseOperation["Del"]

            };
        }

        public bool IsCollabrator => this._param.Collabrator == null ? false : this._param.Collabrator.Any(c => c == this._appUser.UserId);

    }
}
