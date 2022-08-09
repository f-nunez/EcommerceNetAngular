using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Specifications;

namespace Fnunez.Ena.Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<int> CountAsync(ISpecification<T> specification);
    Task<T> GetByIdAsync(int id);
    Task<T> GetFirstOrDefaultAsync(ISpecification<T> specification);
    Task<IReadOnlyList<T>> GetListAllAsync();
    Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> specification);
}