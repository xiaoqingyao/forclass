using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlatform.Domain.Core.CourseAggregate
{
    public enum CourseStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft = 0x01,


        Review = Draft << 1,

        Accept = Review << 1,

        Reject = Accept << 1,

        Listed = Reject << 1,

        School = Listed << 1,

        Region = School << 1,

        UnListed = Region << 1,


        SchoolReview = Review | School,
        SchoolAccept = Accept | School,
        SchoolShelvse = Listed | School,



        RegiogDefault = Draft | Region,
        RegionReview = Review | Region,
        RegionAccept = Accept | Region,
        RegionListed = Listed | Region,
        RegionReject = Reject | Region ,
        RegionUnlisted = Region | UnListed

    }



    //}
    //public enum CourseRegionStatus
    //{
    //    /// <summary>
    //    /// 草稿
    //    /// </summary>
    //    [Description("草稿")]
    //    Draft = 0x01,


    //    RegionReview = Draft << 1,

    //    Accept = RegionReview << 1,

    //    Reject = Accept << 1,

    //    Shelvse = Reject << 1


    //}



}
