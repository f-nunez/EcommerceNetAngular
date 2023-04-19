using Fnunez.Ena.Core.Entities;

namespace Fnunez.Ena.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> CompleteAsync();
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
}