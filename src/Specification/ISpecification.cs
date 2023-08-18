using System;
using System.Linq.Expressions;

namespace Zord.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Expression { get; }
    }
}
