using CoursePlatform.infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class PlatformUserCreated : ICoursePlatformEvent
    {
        public PlatformUserCreated(string iD, int userid, string name, int schoolId, int sectionId, string schoolName/*, UserPropVal researchGroup*/)
        {
            ID = iD;
            Userid = userid;
            Name = name;
            SchoolId = schoolId;
            SectionId = sectionId;
            SchoolName = schoolName;
            //ResearchGroup = researchGroup;

        }


        public string ID { get; }
        public int Userid { get; }
        public string Name { get; }
        public int SchoolId { get; }
        public int SectionId { get; }
        public string SchoolName { get; }
        public UserPropVal ResearchGroup { get; }
    }
}
