namespace CoursePlatform.Data.EFProvider
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T> where T :EntityBase 
    {

    }
}