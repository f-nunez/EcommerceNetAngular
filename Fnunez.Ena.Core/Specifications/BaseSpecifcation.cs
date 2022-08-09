using System.Linq.Expressions;

namespace Fnunez.Ena.Core.Specifications;

public class BaseSpecifcation<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public bool IsPagingEnabled { get; private set; }
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public int Skip { get; private set; }
    public int Take { get; private set; }

    public BaseSpecifcation()
        {
        }

    public BaseSpecifcation(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<T, object>> expression)
    {
        Includes.Add(expression);
    }

    protected void AddOrderBy(Expression<Func<T, object>> expression)
    {
        OrderBy = expression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> expression)
    {
        OrderByDescending = expression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}