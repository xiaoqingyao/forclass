using CoursePlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoursePlatform.Data
{
    public static class Builders
    {


        public static void AllEntity(this ModelBuilder mb)
        {
            mb.Entity<CourseEntity>().ToTable("B_Course");
            mb.Entity<TagsEntity>().ToTable("B_Course_Tags");
            mb.Entity<QuoteDsEntity>().ToTable("B_Course_DS");
            mb.Entity<CollabratorEntity>().ToTable("B_Course_Collabrator");
            mb.Entity<PartnerEntity>().ToTable("B_Partner");//.HasKey(p => new { p.ResourceId, p.Type });
            mb.Entity<CourseListedEntity>().ToTable("B_Course_Listed");

            mb.Entity<PlatformUserEntity>().ToTable("U_PlatformUser");

            mb.Entity<PlatformUserJoinedCourseEntity>(b => {

                b.Property(p => p.JoinType).HasDefaultValue(1);
                b.ToTable("U_platformUser_Course");
                
            });


            mb.Entity<SysConfigEntity>().ToTable("B_SystemConfig");


            mb.Entity<CourseReviewLogEntity>().ToTable("B_Course_Audit_Log");

            mb.Entity<GroupRecommendEntity>().ToTable("B_Course_Group_Recommend");
        }

    }
}
