using System;
using System.Linq.Expressions;

namespace Zord.Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Selector { get; }
    }
}
