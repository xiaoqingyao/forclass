using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries
{
    public class ConstData
    {
        public const string Tag_Create = "创建";
        public const string Tag_Persional_Create = "个人创建";
        public const string Tag_Collabrator_Create = "协作创建";
        public const string Tag_Course = "课程";
        public const string Tag_Source = "出处";
        public const string Tag_Grade = "年级";
        public const string Tag_Subject = "学科";
        public const string Tag_Version = "版本";

        public const char Filter_CourseQuery = 'c';
        public const char Filter_CourseAudit = 'a';
        public const char Filter_Learning = 's';
        public const char Filter_SchoolOrg = 's';
        public const char Filter_RegionOrg = 'c';
        public const char Filter_All = '0';
    }
}
