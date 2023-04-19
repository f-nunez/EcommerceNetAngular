using System.Collections;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Interfaces;

namespace Fnunez.Ena.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreDbContext _dbContext;
    private Hashtable _repositories;

    public UnitOfWork(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CompleteAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories == null)
            _repositories = new Hashtable();

        string type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            Type repositoryType = typeof(GenericRepository<>);

            object repositoryInstance = Activator
                .CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<TEntity>)_repositories[type];
    }
}