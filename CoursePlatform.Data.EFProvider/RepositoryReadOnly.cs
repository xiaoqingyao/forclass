using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Data.EFProvider
{
    public class RepositoryReadOnly<T> : BaseRepository<T>, IRepositoryReadOnly<T> where T :EntityBase 
    {
        public RepositoryReadOnly(DbContext context) : base(context)
        {
        }
    }
}