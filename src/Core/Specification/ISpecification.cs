using System;
using System.Linq.Expressions;

namespace Zord.Core.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Expression { get; }
    }
}
