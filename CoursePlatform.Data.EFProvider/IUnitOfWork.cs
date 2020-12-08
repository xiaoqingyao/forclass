using System;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Data.EFProvider
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase,new();
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : EntityBase,new();
        IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : EntityBase,new();

        int SaveChanges();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
