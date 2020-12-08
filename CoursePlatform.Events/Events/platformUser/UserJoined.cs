using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class UserJoined : ICoursePlatformEvent
    {

        public string PlatUserId { get; set; }


        public int UserId { get; set; }


        public string CourseId { get; set; }

        public int CreatorId { get; set; }


    }
}
