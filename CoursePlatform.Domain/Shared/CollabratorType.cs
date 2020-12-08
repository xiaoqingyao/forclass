namespace CoursePlatform.Domain.Queries.Share
{


    /// <summary>
    /// 查询方式 1： 学校 2: 教研组
    /// </summary>
    public enum CollabratorType
    {
        School = 0x01,

        Community = School << 1

    }
}
