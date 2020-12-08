namespace CoursePlatform.Data.EFProvider
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : EntityBase,new();
        IRepositoryAsync<T> GetRepositoryAsync<T>() where T : EntityBase, new();
        IRepositoryReadOnly<T> GetReadOnlyRepository<T>() where T : EntityBase, new();
    }
}